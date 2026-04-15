import os
import json
import re
import requests
import mysql.connector
from time import time
from http.server import ThreadingHTTPServer, BaseHTTPRequestHandler
from base64 import b64encode, b64decode
from datetime import datetime, timedelta

class État:
    serveur_nom = os.getenv("SERVEUR_NOM","nul")
    serveur_mdp = os.getenv("SERVEUR_MDP","nul")

    requêtes_confirmations_attentes : list[int] = []

    jetons_actifs : list[str] = []

class ServeurHTTP(BaseHTTPRequestHandler):

    def __init__(self, *args, **kwargs):
        self.session_jeton : str = ""
        self.session : tuple[str,str,int] = () # (nom_id,mdp,date_connection_unix)
        self.sql_connection = mysql.connector.connect(
                host="localhost",
                user=os.getenv("MYSQL_USER","messagery"),
                password=os.getenv("MYSQL_PASSWORD","z2zAKZuE"),
                database=os.getenv("MYSQL_DATABASE","messagery")
            )
        self.sql = self.sql_connection.cursor()
        super().__init__(*args,**kwargs)
        self.sql_connection.close()
  
# ===========
#   Entrées
# ===========

    def do_GET(self):

        réponse : dict = None
        match(self.path):
            case "/connection" :                 réponse = self.connecter()
            case "/synchronisation-connection" : réponse = self.synchronisation_connection()
            case "/authentification-serveur" :   réponse = self.authentification_serveur()
            case _:
                self.send_error(404,"Ce noeud n'existe pas")
                return
        
        if not réponse:
            return

        self.send_response(200)
        self.send_header("Content-Type","application/json")
        self.end_headers()

        self.wfile.write(json.dumps(réponse).encode())

    def do_POST(self):
        
        réponse : dict = None
        match(self.path):
            case "/inscription" :        réponse = self.inscription()
            case "/deconnection" :       réponse = self.déconnection()
            case "/conversation" :       réponse = self.créer_conversation()
            case "/invitation" :         réponse = self.envoyer_invitation()
            case "/invitation-relais" :  réponse = self.recevoir_invitation()
            case "/message" :            réponse = self.envoyer_message()
            case "/message-relais" :     réponse = self.recevoir_message()
            case "/synchronisation" :    réponse = self.synchronisation()
            case _:
                self.send_error(404,"Ce noeud n'existe pas")
                return

        if réponse is None:
            return

        self.send_response(200)
        self.send_header("Content-Type","application/json")
        self.end_headers()

        self.wfile.write(json.dumps(réponse).encode())

# ===========
#   NOEUDS
# ===========

    def authentification_serveur(self) -> dict:
        # Ce noeud est appelé en réponse à une requête inter-serveur et 
        # sert à confirmer l'identité de l'appelant
        autorisation = self.headers.get("Authorization")

        if not autorisation or not isinstance(autorisation, str) or len(autorisation) == 0:
            self.send_error(400,"Authorisation mal formée")
            return None

        mots = autorisation.split(" ")
        if len(mots) != 2:
            self.send_error(400,"Authorisation mal formée")
            return None
        
        if mots[0] != "Basic":
            self.send_error(400,"La méthode d'authentification n'est pas reconnue.")
            return None

        identifiants = b64decode(mots[1].encode()).decode("utf8").split(':')

        if (
            identifiants[0] != État.serveur_nom or 
            identifiants[1] != État.serveur_nom or
            int(identifiants[2]) not in État.requêtes_confirmations_attentes
            ):
            self.send_error(401,"La requête n'émane pas de ce serveur")
            return None
        
        return {"jeton":autorisation}

    def inscription(self) -> dict:
        infos : dict
        try:
            taille = self.headers.get("Content-Length")
            if not taille or not taille.isdigit():
                self.send_error(422,"L'en-tête 'Content-Length' doit être assigné et être un entier.")
                return

            infos = json.loads(self.rfile.read(int(taille)))
        except Exception as e:
            print("Données mal formées")
            self.send_error(400,"Les données reçues ne peuvent être interprétées comme un json.")
            return None

        if ( ("nom_id" not in infos) or 
            ("nom_affichage" not in infos) or
            ("mot_de_passe" not in infos)):
            print("JSON mal formé")
            self.send_error(422,"Les données reçues ne sont pas bien formattées.")
            return None
        
        infos["nom_id"] = infos["nom_id"].lower()
        # Valider le nom d'utilisateur
        if not self.est_id_valide(infos["nom_id"]+"@"+self.headers.get("Host")):
            self.send_error(422,"L'identifiant est invalide")
            return None

        # Vérifier si qqun existe déjà
        self.sql.execute("SELECT 1 FROM Utilisateurs WHERE nom_id=%s",(infos["nom_id"],))
        if len(self.sql.fetchall()) != 0:
            self.send_error(400,"Un utilisateur possède déjà cet identifiant.")
            return None

        self.sql.execute("SELECT MAX(utilisateur_id) FROM Utilisateurs")
        uid = self.sql.fetchall()[0][0]
        if uid is None:
            uid = 0
        uid += 1
        
        self.sql.execute(
            "INSERT INTO Utilisateurs (utilisateur_id, nom_affichage, nom_id, mot_de_passe)" +
            "VALUES (%s,%s,%s,%s)",
            (uid,infos["nom_affichage"].lower(),infos["nom_id"],infos["mot_de_passe"]))
        self.sql_connection.commit()

        return {"accepté":True}

    def connecter(self) -> dict:
        """
        BaseHTTPRequestHandler.connection est une variable déjà définie.
        """
        autorisation = self.headers.get("Authorization")

        if not autorisation or not isinstance(autorisation, str) or len(autorisation) == 0:
            print("Authorisation mal formée")
            return False

        mots = autorisation.split(" ")
        if len(mots) != 2:
            print("Authorisation mal formée")
            return False

        if mots[0] != "Basic":
            self.send_error(422,"Mode d'authentification attendu : 'Basic'")
            return None

        identifiants : list[str] = None
        try:
            identifiants = b64decode(mots[1].encode()).decode("utf8").split(":")
            if len(identifiants) != 2:
                print("Identifiants mal formée")
                return False
        except Exception:
            print("Identifiants mal formée")
            return False
        
        self.session = (identifiants[0],identifiants[1],round(time()))

        # Vérifier les identifiants dans la base de donnée
        self.sql.execute(
            "SELECT utilisateur_id, date_connection, date_dernière_interaction "+
            "FROM Utilisateurs "+
            "WHERE nom_id=%s AND mot_de_passe=%s",
            (identifiants[0],identifiants[1])
        )

        résultat : list[tuple[int,datetime,datetime]] = self.sql.fetchall()
        if len(résultat) != 1:
            self.send_error(401,"Les identifiants n'ont pas pus être vérifiés.")
            return None

        if résultat[0][1] is not None and datetime.now() - résultat[0][2] < timedelta(hours=3) :
            self.send_error(401,"Vous êtes déjà connecté. Veuillez vous déconnecter avant de réessayer.")
            return None

        # Modifier l'information de connection dans la base de donnée
        self.sql.execute(
            "UPDATE Utilisateurs "+
            "SET date_connection=%s, date_dernière_interaction=%s "+
            "WHERE utilisateur_id=%s",
            (datetime.now(),datetime.now(),résultat[0][0])
        )

        self.sql_connection.commit()

        self.session_jeton = b64encode((self.session[0]+':'+self.session[1]+':'+str(self.session[2])).encode()).decode("utf8")
        État.jetons_actifs.append(self.session_jeton)
        return {
            "accepté":True, 
            "jeton": self.session_jeton
        }

    def déconnection(self) -> dict:
        if not self.est_client_autorisé():
            self.send_error(401,"Soit vous n'êtes pas connectés, soit votre session est échue.")
            return

        # Modifier l'information de connection dans la base de donnée
        self.sql.execute(
            "UPDATE Utilisateurs "+
            "SET date_connection=NULL, date_dernière_interaction=NULL "+
            "WHERE nom_id=%s AND mot_de_passe=%s",
            (self.session[0],self.session[1])
        )
        self.sql_connection.commit()

        État.jetons_actifs.remove(self.session_jeton) # Obtenu dans self.est_autorisé()
        return {}

    def synchronisation_connection(self) -> dict:
        if not self.est_client_autorisé():
            self.send_error(401,"Soit vous n'êtes pas connectés, soit votre session est échue.")
            return
        self.enregistrer_temps_interaction()

        # Récupérer les contacts dans la base de donnée
        contacts : list[dict[str:str]] = []

        self.sql.execute("SELECT nom_id, nom_affichage, serveur_id, est_local FROM Contacts")
        résultats = self.sql.fetchall()
        for r in résultats:
            contacts.append({
                "ID":r[0]+"@"+(r[2] if not r[3] else self.headers.get("Host")),
                "nom":r[1]
            })

        # Récupérer les conversations dans la base de donnée
        self.sql.execute(
            "SELECT c.conversation_id FROM Conversations AS c "+
            "INNER JOIN ConversationsContacts AS cc ON c.conversation_id=cc.conversation_id "+
            "INNER JOIN Contacts AS c2 ON c2.contact_id=cc.contact_id "+
            "WHERE c2.nom_id=%s AND c2.est_local=True",
            (self.session[0],)
        )
        conversations_ids = self.sql.fetchall()
        
        conversations : list[dict[str:any]] = []
        for id in conversations_ids:
            # Contacts impliqués
            self.sql.execute(
                "SELECT ct.nom_id, s.url, ct.est_local "+
                "FROM Contacts AS ct "+
                "LEFT JOIN ServeursAutorisés AS s ON ct.serveur_id=s.serveur_id "+
                "INNER JOIN ConversationsContacts AS cc ON ct.contact_id=cc.contact_id "+
                "WHERE cc.conversation_id=%s",
                (id[0],)
            )
            cv_contacts_sql : list[tuple[str,str,bool]] = self.sql.fetchall()
            cv_contacts : list[str] = []
            for c in cv_contacts_sql:
                cv_contacts.append(c[0]+"@"+(c[1] if not c[2] else self.headers.get("Host")))
            
            # Messages
            self.sql.execute(
                "SELECT c.nom_id, s.url, c.est_local, m.date, m.message "+
                "FROM Contacts AS c "+
                "LEFT JOIN ServeursAutorisés AS s ON c.serveur_id=s.serveur_id "+
                "INNER JOIN Messages AS m ON c.contact_id=m.contact_id "+
                "WHERE m.conversation_id=%s "+
                "ORDER BY m.date ASC",
                (id[0],)
            )
            cv_messages_sql :list[tuple[str,str,bool,datetime,str]] = self.sql.fetchall()
            cv_messages : list[dict[str:any]] = []
            for m in cv_messages_sql:
                cv_messages.append({
                    "contact":m[0]+"@"+(m[1] if not m[2] else self.headers.get("Host")),
                    "date":m[3].strftime("%Y-%m-%dT%H:%M:%SZ"),
                    "message":m[4]
                })
            
            # Conversation
            conversations.append({
                "ID":id[0],
                "contacts":cv_contacts,
                "messages":cv_messages
            })

        # Récupérer les conversations non-lues dans la base de donnée
        self.sql.execute(
            "SELECT DISTINCT m.conversation_id "+
            "FROM Messages AS m "+
            "INNER JOIN MessagesNonLus AS mnl ON m.message_id=mnl.message_id "+
            "INNER JOIN Utilisateurs AS u ON u.utilisateur_id=mnl.message_id "+
            "WHERE u.nom_id=%s",
            (self.session[0],)
        )
        conversations_non_lues_sql : list[tuple[int]] = self.sql.fetchall()
        conversations_non_lues : list[int] = []
        for c in conversations_non_lues_sql:
            conversations_non_lues.append(c[0])

        return {
            "contacts":contacts,
            "conversations":conversations,
            "conversations-non-lues":conversations_non_lues
        }

    def synchronisation(self) -> dict:
        # BUG TODO #1 Un contact s'ajoutant à une conversation n'est pas communiqué au client s'il est connecté.
        if not self.est_client_autorisé():
            self.send_error(401,"Soit vous n'êtes pas connectés, soit votre session est échue.")
            return
        self.enregistrer_temps_interaction()

        modifications : dict
        try:
            taille = self.headers.get("Content-Length")
            if not taille or not taille.isdigit():
                self.send_error(422,"L'en-tête 'Content-Length' doit être assigné et être un entier.")
                return

            modifications = json.loads(self.rfile.read(int(taille)))
        except Exception as e:
            print("Données mal formées")
            self.send_error(400,"Les données reçues ne peuvent être interprétées comme un json.")
            return None
        
        if ( ("conversations-lues" not in modifications) or 
            ("conversations-effacées" not in modifications)):
            print("JSON mal formé")
            self.send_error(422,"Les données reçues ne sont pas bien formattées.")
            return None
        
        # Modifier la base de donnée en fonction des données reçues
        # Conversations lues
        if len(modifications["conversations-lues"]) != 0:
            params = modifications["conversations-lues"].copy()
            params.append(self.session[0])
            self.sql.execute(
                "DELETE mnl FROM MessagesNonLus AS mnl "+
                "INNER JOIN Messages AS m ON m.message_id=mnl.message_id "+
                "INNER JOIN Utilisateurs AS u ON mnl.utilisateur_id=u.utilisateur_id "+
                ("WHERE m.conversation_id IN (%s) " % ','.join(['%s']*len(modifications["conversations-lues"])))+ 
                "AND u.nom_id=%s",
                tuple(params)
            )
        
        # Conversations effacées
        # BUG TODO #2 Cette information n'est pas communiquée aux autres serveurs
        if len(modifications["conversations-effacées"]) != 0:
            for cid in modifications["conversations-effacées"]:
                # Effacer le lien entre l'utilisateur et la conversation
                self.sql.execute(
                    "DELETE cc FROM ConversationsContacts AS cc "+
                    "INNER JOIN Utilisateurs AS u ON cc.utilisateur_id=u.utilisateur_id "+
                    "WHERE u.nom_id=%s",
                    (self.session[0],)
                )
                # Effacer les conversations non lues
                self.sql.execute(
                    "DELETE cnl FROM ConversationsNonLues AS cnl "+
                    "INNER JOIN Utilisateurs AS u ON cnl.utilisateur_id=u.utilisateur_id "+
                    "WHERE cnl.conversation_id=%s AND u.nom_id=%s",
                    (cid,self.session[0])
                )
                # Effacer les invitations
                self.sql.execute(
                    "DELETE i FROM Invitations AS i "+
                    "INNER JOIN Utilisateurs AS u ON i.utilisateur_id=u.utilisateur_id "+
                    "WHERE i.conversation_id=%s AND u.utilisateur_id=%s",
                    (cid[0],self.session[0])
                )

                # Effacer la conversation s'il ne reste plus personne dedans
                self.sql.execute(
                    "SELECT c.conversation_id "+
                    "FROM Conversations AS c "+
                    "LEFT JOIN ConversationsContacts AS cc ON c.conversation_id=cc.conversation_id "+
                    "WHERE cc.conversation_id IS NULL"
                )
                conversations_orphelines : list[tuple[int]] = self.sql.fetchall()
                for cid in conversations_orphelines:
                    self.sql.execute(
                        "DELETE c, m FROM Conversations AS c "+
                        "INNER JOIN Messages AS m ON c.conversation_id=m.conversation_id "+
                        "WHERE c.conversation_id=%s",
                        (cid,)
                )

        # Obtenir les nouvelles conversations
        self.sql.execute(
            "SELECT i.conversation_id "+
            "FROM Invitations AS i "+
            "INNER JOIN Utilisateurs AS u ON u.utilisateur_id=i.utilisateur_id "+
            "WHERE u.nom_id=%s",
            (self.session[0],)
        )
        nouvelles_conversations_sql : list[tuple[int]] = self.sql.fetchall()
        nouvelles_conversations : list[int] = []
        for c in nouvelles_conversations_sql:
            nouvelles_conversations.append(c[0])
        #Effacer les invitations
        self.sql.execute(
            "DELETE i FROM Invitations AS i "+
            "INNER JOIN Utilisateurs AS u ON i.utilisateur_id=u.utilisateur_id "+
            "WHERE u.nom_id=%s",
            (self.session[0],)
        )

        # Obtenir les nouveaux messages
        self.sql.execute(
            "SELECT m.conversation_id, c.nom_id, s.url, c.est_local, m.date, m.message "+
            "FROM MessagesNonLus AS mnl "+
            "INNER JOIN Utilisateurs AS u ON mnl.utilisateur_id=u.utilisateur_id "+
            "INNER JOIN Messages AS m ON mnl.message_id=m.message_id "+
            "INNER JOIN Contacts AS c ON m.contact_id=c.contact_id "+
            "LEFT JOIN ServeursAutorisés AS s on c.serveur_id=s.serveur_id "+
            "WHERE u.nom_id=%s",
            (self.session[0],)
        )
        messages_non_lus_sql : list[tuple[int,str,str,bool,datetime,str]] = self.sql.fetchall()
        messages_non_lus : list[dict[str:any]] = []
        for m in messages_non_lus_sql:
            messages_non_lus.append({
                "conversation":m[0],
                "contact":m[1]+"@"+(m[2] if not m[3] else self.headers.get("Host")),
                "date":m[4].strftime("%Y-%m-%dT%H:%M:%SZ"),
                "message":m[5]
            })
        # Effacer les messages non lus
        self.sql.execute(
            "DELETE mnl FROM MessagesNonLus AS mnl "+
            "INNER JOIN Utilisateurs AS u ON mnl.utilisateur_id=u.utilisateur_id "+
            "WHERE u.nom_id=%s",
            (self.session[0],)
        )

        self.sql_connection.commit()
        return {
            "nouvelles-conversations":nouvelles_conversations,
            "nouveaux-messages":messages_non_lus
        }

    def créer_conversation(self) -> dict:
        if not self.est_client_autorisé():
            self.send_error(401,"Soit vous n'êtes pas connectés, soit votre session est échue.")
            return

        infos : dict = None
        try:
            taille = self.headers.get("Content-Length")
            if not taille or not taille.isdigit():
                self.send_error(422,"L'en-tête 'Content-Length' doit être assigné et être un entier.")
                return

            infos = json.loads(self.rfile.read(int(taille)))
        except Exception as e:
            print("Données mal formées")
            self.send_error(400,"Les données reçues ne peuvent être interprétées comme un json.")
            return None
        
        if "contacts" not in infos:
            print("JSON mal formé")
            self.send_error(422,"Les données reçues ne sont pas bien formattées.")
            return None
        
        self.enregistrer_temps_interaction()

        contacts : list[str] = infos["contacts"]
        
        # Retirer les contacts invalides et le contact à l'origine de la requête.
        len_o = len(contacts)
        for i in range(len(contacts)):
            nom_id, serveur = contacts[len_o-i-1].split('@')
            if (
                not self.est_id_valide(contacts[len_o-i-1]) or 
                (nom_id == self.session[0] and serveur == self.headers.get("Host"))
                ):
                contacts.pop(len_o-i-1)
        
        if len(contacts) == 0:
            print("Aucun contact valide")
            return {"accepté":False}

        conversation_id = round(time())

        # Vérifier si une conversation avec ces contacts existe déjà
        self.sql.execute(
           ("WITH D(contact_id) AS (VALUES %s) "+
            "SELECT cc.conversation_id "+
            "FROM ConversationsContacts AS cc "+
            "LEFT JOIN D ON cc.contact_id=D.contact_id "+
            "WHERE D.contact_id IS NOT NULL "+
            "GROUP BY cc.conversation_id "+
            "HAVING COUNT(cc.contact_id)=(SELECT COUNT(*) FROM D)") % ','.join(["ROW(%s)"]*len(contacts)),
            tuple(contacts)
        )
        if len(self.sql.fetchall()) != 0:
            print("La conversation existe déjà.")
            return {"accepté":False,"conversation":None}

        # Mettre à jour la base de données
        self.sql.execute(
            "INSERT INTO Conversations VALUES (%s)",
            (conversation_id,)
        )
        self.sql.execute(
            "SELECT c.contact_id "+
            "FROM Contacts AS c "+
            "INNER JOIN Utilisateurs AS u ON c.utilisateur_id=u.utilisateur_id "+
            "WHERE u.nom_id=%s",
            (self.session[0],)
        )
        contact_id = self.sql.fetchall()[0][0]
        self.sql.execute(
            "INSERT INTO ConversationsContacts (contact_id, conversation_id) "+
            "VALUES (%s,%s)",
            (contact_id,conversation_id)
        )

        self.sql_connection.commit()
        
        # Collecter les serveurs
        serveurs = []
        for contact in contacts:
            mots = contact.split("@")
            if mots[1] not in serveurs:
                serveurs.append(mots[1])
        
        # Envoyer l'invitation aux serveurs
        for serveur in serveurs:
            try:
                id_requête = round(time())
                État.requêtes_confirmations_attentes.append(id_requête)
                r = requests.post(
                    url="http://"+serveur+"/invitation-relais",
                    json={
                        "conversation":conversation_id,
                        "contacts":contacts,
                        "messages":[]
                    },
                    headers={
                        "Authorization":"Basic "+b64encode((État.serveur_nom+":"+État.serveur_mdp+":"+str(id_requête)).encode()).decode("utf8")
                    }
                )
                if not r.json()["accepté"] :
                    print("Erreur dans le relais de l'invitation")
                    self.send_error(502,"Le serveur d'un contact n'a pas accepté l'invitation.")
                    return None
            except Exception as e:
                print("Erreur dans le relais de l'invitation")
                self.send_error(502,"Le serveur d'un contact n'a pas pus être rejoint.")
                return None

        return {"accepté":True,"conversation":conversation_id}

    def envoyer_invitation(self) -> dict:
        if not self.est_client_autorisé():
            self.send_error(401,"Soit vous n'êtes pas connectés, soit votre session est échue.")
            return

        infos : dict = None
        try:
            taille = self.headers.get("Content-Length")
            if not taille or not taille.isdigit():
                self.send_error(422,"L'en-tête 'Content-Length' doit être assigné et être un entier.")
                return

            infos = json.loads(self.rfile.read(int(taille)))
        except Exception as e:
            print("Données mal formées")
            self.send_error(400,"Les données reçues ne peuvent être interprétées comme un json.")
            return None
        
        if "conversation" not in infos or "contacts" not in infos:
            print("JSON mal formé")
            self.send_error(422,"Les données reçues ne sont pas bien formattées.")
            return None
        
        self.enregistrer_temps_interaction()

        conversation_id : int = infos["conversation"]
        contacts : list[str] = infos["contacts"]

        self.sql.execute(
            "SELECT * FROM Conversations WHERE conversation_id=%s",
            (conversation_id,)
        )
        if len(self.sql.fetchall()) == 0:
            self.send_error(400,"La conversation n'existe pas.")
            return None
        
        # Retirer les contacts invalides
        len_o = len(contacts)
        for i in range(len(contacts)):
            if not self.est_id_valide(contacts[len_o-i-1]):
                contacts.pop(len_o-i-1)
        
        if len(contacts) == 0:
            print("Aucun contact valide")
            return {"accepté":False}

        # Vérifier si les contacts sont déjà dans la conversation
        self.sql.execute(
            "SELECT c.nom_id, c.serveur_id, c.est_local "+
            "FROM Contacts AS c "+
            "INNER JOIN ConversationsContacts AS cc ON c.contact_id=cc.contact_id "+
            "WHERE cc.conversation_id=%s",
            (conversation_id,)
        )
        contacts_existants_sql : list[tuple[str,str,bool]] = self.sql.fetchall()
        contacts_existants : list[tuple[str,str]] = []
        for c in contacts_existants_sql:
            contacts_existants.append(c[0] + "@" + (c[1] if not c[2] else self.headers.get("Host")))
        
        len_o = len(contacts)
        for i in range(len(contacts)):
            if contacts[len_o-i-1] in contacts_existants:
                contacts.pop(len_o-i-1)
        
        if len(contacts) == 0:
            return {"accepté":True}

        self.sql_connection.commit()
        # Collecter les serveurs
        serveurs = []
        for contact in contacts:
            mots = contact.split("@")
            if mots[1] not in serveurs:
                serveurs.append(mots[1])
        
        # Envoyer l'invitation aux serveurs
        for serveur in serveurs:
            try:
                id_requête = round(time())
                État.requêtes_confirmations_attentes.append(id_requête)
                r = requests.post(
                    url="http://"+serveur+"/invitation-relais",
                    json={
                        "conversation":conversation_id,
                        "contacts":contacts,
                        "messages":[]
                    },
                    headers={"Authorization":"Basic "+b64encode((État.serveur_nom+":"+État.serveur_mdp+":"+str(id_requête)).encode()).decode("utf8")}
                )
                if not r.json()["accepté"] :
                    print("Erreur dans le relais de l'invitation")
                    self.send_error(502,"Le serveur d'un contact n'a pas accepté l'invitation.")
                    return None
            except Exception as e:
                print("Erreur dans le relais de l'invitation")
                self.send_error(502,"Le serveur d'un contact n'a pas pus être rejoint.")
                return None
        
        return {"accepté":True}

    def recevoir_invitation(self) -> dict:
        if not self.est_serveur_autorisé():
            self.send_error(401,"Votre serveur n'est pas autorisé à communiquer avec nous. Veuillez contacter l'administrateur de ce serveur pour l'ajouter à la liste d'autorisation.")
            return
        
        infos : dict = None
        try:
            taille = self.headers.get("Content-Length")
            if not taille or not taille.isdigit():
                self.send_error(422,"L'en-tête 'Content-Length' doit être assigné et être un entier.")
                return

            infos = json.loads(self.rfile.read(int(taille)))
        except Exception as e:
            print("Données mal formées")
            self.send_error(400,"Les données reçues ne peuvent être interprétées comme un json.")
            return None
        
        if "conversation" not in infos or "contacts" not in infos or "messages" not in infos:
            print("JSON mal formé")
            self.send_error(422,"Les données reçues ne sont pas bien formattées.")
            return None
        
        conversation : int = infos["conversation"]
        contacts : list[str] = infos["contacts"]
        messages : list[dict[str:str]] = infos["messages"]

        # Vérifier la validité de chaque contact
        len_o = len(contacts)
        for i in range(len(contacts)):
            if not self.est_id_valide(contacts[len_o-i-1]):
                contacts.pop(contacts[len_o-i-1])
        if len(contacts) == 0:
            return {"accepté":False}

        # Obtenir les contact_id de chaque contact
        contacts_ids :dict[str:int] = {}
        for c in contacts:
            nom_id, serveur = c.split("@")
            est_local = serveur == self.headers.get("Host")
            self.sql.execute(
                "SELECT contact_id FROM Contacts AS c "+
                "LEFT JOIN ServeursAutorisés AS s ON c.serveur_id=s.serveur_id "
                "WHERE c.nom_id=%s AND "+("c.est_local=1" if est_local else "s.url=%s"),
                ((nom_id,) if est_local else (nom_id,serveur))
            )
            res = self.sql.fetchall()
            if len(res) == 0:
                print("L'un des contacts nous est inconnu.")
                continue
            contacts_ids[c] = res[0][0]
        
        # Obtenir la liste des contacts figurant d'utilisateurs
        self.sql.execute(
           ("WITH D(contact_id) AS (VALUES %s) "+
            "SELECT c.utilisateur_id, c.contact_id "+
            "FROM Contacts AS c "+
            "INNER JOIN D ON c.contact_id=D.contact_id "+
            "WHERE c.est_local=True") % ','.join(['ROW(%s)']*len(contacts_ids)),
            tuple(contacts_ids.values())
        )
        utilisateurs_sql : list[tuple[int]] = self.sql.fetchall()
        contacts_utilisateurs : list[int] = []
        utilisateurs : list[int] = []
        for u in utilisateurs_sql:
            utilisateurs.append(u[0])
            contacts_utilisateurs.append(u[1])

        # Créer la conversation si elle n'existe pas encore
        self.sql.execute(
            "SELECT 1 FROM Conversations WHERE conversation_id=%s",
            (conversation,)
        )
        res = self.sql.fetchall()
        if len(res) == 0 and len(utilisateurs) == 0:
            # La conversation n'invite aucun nouvel utilisateur et 
            # ne concerne aucune de nos discussions: elle ne nous concerne pas.
            return {"accepté":False}
        elif len(res) == 0:
            self.sql.execute(
                "INSERT INTO Conversations VALUES (%s)",
                (conversation,)
            )

            # Assembler la requête pour insérer les messages
            self.sql.execute("SELECT MAX(message_id) FROM Messages")
            message_id = self.sql.fetchall()[0][0]
            if message_id is None:
                message_id = 0
            message_id += 1
            requête = "INSERT INTO Messages (date, message) VALUES " + ','.join(["(%s,%s,%s,%s,%s)"]*len(messages))

            # Assembler les paramètres
            requête_params = []
            for m in messages:
                if m[0] not in contacts_ids:
                    continue
                message_id += 1
                requête_params.append(message_id)
                requête_params.append(conversation)
                self.sql.execute()
                requête_params.append(contacts_ids[m[0]])
                requête_params.append(m[1])
                requête_params.append(m[2])
            
            self.sql.execute(requête,tuple(requête_params))
        else:
            # La conversation existe, il faut donc y ajouter les contacts invités
            
            # Obtenir la liste des contacts déjà présents dans la discussion
            self.sql.execute(
                "SELECT contact_id FROM ConversationsContacts WHERE conversation_id=%s",
                (conversation,)
            )
            contacts_présents_sql : list[tuple[int]] = self.sql.fetchall()
            contacts_présents : list[int] = []
            for c in contacts_présents_sql:
                contacts_présents.append(c[0])
            
            requête = "INSERT INTO ConversationsContacts (contact_id, conversation_id) VALUES "

            requête_params : list[int] = []
            contacts_à_ajouter : list[int] = []
            for c in contacts:
                if contacts_ids[c] in contacts_présents:
                    continue
                contacts_à_ajouter.append(c)
                requête_params.append(contacts_ids[c])
                requête_params.append(conversation)
            
            if len(contacts_à_ajouter) != 0:
                requête += ','.join(["(%s,%s)"]*len(contacts_à_ajouter))
                self.sql.execute(requête,tuple(requête_params))
        
        # Ajouter les invitations et les contacts à la conversation
        requête = "INSERT INTO Invitations (conversation_id, utilisateur_id) VALUES " + ','.join(["(%s,%s)"]*len(utilisateurs))
        params : list[int] = []
        for u in utilisateurs:
            params.append(conversation)
            params.append(u)
        self.sql.execute(requête,params)

        self.sql_connection.commit()
        return {"accepté":True}

    def envoyer_message(self) -> dict:
        if not self.est_client_autorisé():
            self.send_error(401,"Soit vous n'êtes pas connectés, soit votre session est échue.")
            return

        infos : dict = None
        try:
            taille = self.headers.get("Content-Length")
            if not taille or not taille.isdigit():
                self.send_error(422,"L'en-tête 'Content-Length' doit être assigné et être un entier.")
                return

            infos = json.loads(self.rfile.read(int(taille)))
        except Exception as e:
            print("Données mal formées")
            self.send_error(400,"Les données reçues ne peuvent être interprétées comme un json.")
            return None
        
        if "conversation" not in infos or "message" not in infos:
            print("JSON mal formé")
            self.send_error(422,"Les données reçues ne sont pas bien formattées.")
            return None

        self.enregistrer_temps_interaction()

        conversation : int = infos["conversation"]
        message : str = infos["message"]
        date : str = datetime.today() # Format ISO 8601

        # Vérifier que la conversation existe
        self.sql.execute(
            "SELECT 1 FROM Conversations WHERE conversation_id=%s",
            (conversation,)
        )
        if len(self.sql.fetchall()) == 0:
            self.send_error(400,"Cette conversation n'existe pas.")
            return None

        # Insérer le message dans la base de données
        # ID du message
        self.sql.execute("SELECT MAX(message_id) FROM Messages")
        message_id : int = self.sql.fetchall()[0][0]
        if message_id is None: # Il pourrait ne pas y avoir de messages
            message_id = 0
        message_id += 1
        
        # ID du contact
        self.sql.execute(
            "SELECT contact_id "+
            "FROM Contacts AS c "+
            "WHERE nom_id=%s AND est_local=True",
            (self.session[0],)
        )
        contact_id : int = self.sql.fetchall()[0][0]

        self.sql.execute(
            "INSERT INTO Messages (message_id, conversation_id, contact_id, date, message) "+
            "VALUES (%s,%s,%s,%s,%s)",
            (message_id,conversation,contact_id,date,message)
        )
        
        # Récupérer les contacts de la conversation
        self.sql.execute(
            "SELECT c.nom_id, s.url, c.est_local "+
            "FROM Contacts AS c "+
            "INNER JOIN ConversationsContacts AS cc ON c.contact_id=cc.contact_id "+
            "LEFT JOIN ServeursAutorisés AS s ON s.serveur_id=c.serveur_id "+
            "WHERE cc.conversation_id=%s",
            (conversation,)
        )
        contacts_sql : list[tuple[str,str,bool]] = self.sql.fetchall()
        contacts : list[str] = []
        for c in contacts_sql:
            contacts.append(c[0]+"@"+(c[1] if not c[2] else self.headers.get("Host")))
        
        # Ajouter le message non lu pour les utilisateurs de ce serveur
        self.sql.execute(
            "SELECT u.utilisateur_id "+
            "FROM Utilisateurs AS u "+
            "INNER JOIN Contacts AS c ON u.utilisateur_id=c.utilisateur_id "+
            "INNER JOIN ConversationsContacts AS cc ON c.contact_id=cc.contact_id "+
            "WHERE cc.conversation_id=%s AND u.nom_id!=%s",
            (conversation,self.session[0])
        )
        utilisateurs_sql : list[tuple[int]] = self.sql.fetchall()
        utilisateurs : list[int] = []
        for u in utilisateurs_sql:
            utilisateurs.append(u[0])

        if len(utilisateurs) != 0:
            requête = "INSERT INTO MessagesNonLus (utilisateur_id, message_id) VALUES " + ','.join(["(%s,%s)"*len(utilisateurs)])
            params : list = []
            for u in utilisateurs:
                params.append(u)
                params.append(message_id)
            self.sql.execute(requête,params)

        self.sql_connection.commit()

        # Récolter les serveurs concernés
        serveurs = []
        for contact in contacts:
            serveur = contact.split("@")[1]
            if serveur not in serveurs and serveur != self.headers.get("Host"):
                serveurs.append(serveur)

        # Relayer l'invitation aux serveurs
        for serveur in serveurs:
            try:
                id_requête = round(time())
                État.requêtes_confirmations_attentes.append(id_requête)
                requests.post(
                    url="http://"+serveur+"/message-relais",
                    json={
                        "conversation":conversation,
                        "contact":self.session[0]+"@"+self.headers.get("Host"),
                        "date":date,
                        "message":message
                    },
                    headers={"Authorization":"Basic "+b64encode((État.serveur_nom + ":" + État.serveur_mdp+":"+str(id_requête)).encode()).decode("utf8")}
                )
            except Exception as e:
                print(e)
        
        return {}
    
    def recevoir_message(self) -> dict:
        if not self.est_serveur_autorisé():
            self.send_error(401,"Votre serveur n'est pas autorisé à communiquer avec nous. Veuillez contacter l'administrateur de ce serveur pour l'ajouter à la liste d'autorisation.")
            return

        infos : dict = None
        try:
            taille = self.headers.get("Content-Length")
            if not taille or not taille.isdigit():
                self.send_error(422,"L'en-tête 'Content-Length' doit être assigné et être un entier.")
                return

            infos = json.loads(self.rfile.read(int(taille)))
        except Exception as e:
            print("Données mal formées")
            self.send_error(400,"Les données reçues ne peuvent être interprétées comme un json.")
            return None
        
        if "conversation" not in infos or "message" not in infos:
            print("JSON mal formé")
            self.send_error(422,"Les données reçues ne sont pas bien formattées.")
            return None

        conversation : int = infos["conversation"]
        contact : str = infos["contact"]
        message : str = infos["message"]
        date : str = infos["date"] # Format ISO 8601

        # Vérifier que la conversation existe
        self.sql.execute(
            "SELECT 1 FROM Conversation WHERE conversation_id=%s",
            (conversation,)
        )
        if len(self.sql.fetchall()) == 0:
            self.send_error(400,"Cette conversation n'existe pas.")
            return None

        # Vérifier que le contact est valide et existe
        if not self.est_id_valide(contact):
            self.send_error(400,"Le contact est invalide")
            return None
        
        contact_nom, contact_serveur = contact.split("@")
        self.sql.execute(
            "SELECT c.contact_id "+
            "FROM Contacts AS c "+
            "LEFT JOIN ServeursAutorisés AS s ON c.serveur_id=s.serveur_id "+
            "WHERE c.nom_id=%s AND s.url=%s",
            (contact_nom,contact_serveur)
        )
        contact_id_sql : list[tuple[str,str]] = self.sql.fetchall()
        if len(contact_id_sql) == 0:
            self.send_error(400,"Ce contact nous est inconnus.")
            return None
        contact_id = contact_id_sql[0][0]

        # Obtenir message_id
        self.sql.execute("SELECT MAX(message_id) FROM Messages")
        message_id : int = self.sql.fetchall()[0][0]
        if message_id is None: # Il pourrait ne pas y avoir de messages.
            message_id = 0
        message_id += 1

        # Insérer le nouveau message
        self.sql.execute(
            "INSERT INTO Messages (date, message) "+
            "VALUES (%s,%s,%s,%s,%s)",
            (message_id, conversation, contact_id, date, message, contact_id, date, message)
        )

        # Insérer les messages nons lus

        # Obtenir les utilisateurs concernés
        self.sql.execute(
            "SELECT c.utilisateur_id "+
            "FROM Conversations AS c "+
            "INNER JOIN ConversationsContacts AS cc ON c.contact_id=cc.contact_id "+
            "WHERE cc.conversation_id=%s"
        )
        utilisateurs_sql : list[tuple[int]] = self.sql.fetchall()
        utilisateurs : list[int] = []
        for u in utilisateurs_sql:
            utilisateurs.append(u[0])

        requête = "INSERT INTO MessagesNonLus (message_id, utilisateur_id) VALUES " + ','.join(["(%s,%s)"]*len(utilisateurs))
        params : list = []
        for u in utilisateurs:
            params.append(message_id)
            params.append(u)
        self.sql.execute(requête,params)

        self.sql_connection.commit()
        return {}

# ===============
#   Utilitaires
# ===============

    def est_client_autorisé(self) -> bool:
        # Vérifier la validité de la connection d'un client
        autorisation = self.headers.get("Authorization")

        if not autorisation or not isinstance(autorisation, str) or len(autorisation) == 0:
            print("Authorisation mal formée")
            return False

        mots = autorisation.split(" ")
        if len(mots) != 2:
            print("Authorisation mal formée")
            return False

        if mots[0] != "Bearer":
            print("La méthode d'authentification n'est pas reconnue")
            return False

        if mots[1] not in État.jetons_actifs:
            print("Session échue")
            return False

        identifiants : list[str] = None
        try:
            identifiants = b64decode(mots[1].encode()).decode("utf8").split(":")
            if len(identifiants) != 3 or not identifiants[2].isdigit():
                print("Identifiants mal formée")
                return False
            identifiants[2] = int(identifiants[2])
        except Exception:
            print("Identifiants mal formée")
            return False
        
        # Interroger la base de donnée sur les identifiants
        self.sql.execute(
            "SELECT 1 "+
            "FROM Utilisateurs "+
            "WHERE nom_id=%s AND mot_de_passe=%s AND date_connection<=DATE_ADD(NOW(), INTERVAL 3 SECOND) AND DATE_ADD(date_dernière_interaction, INTERVAL 3 HOUR) > NOW()",
            (identifiants[0],identifiants[1])
        )
        if len(self.sql.fetchall()) == 0:
            print("Identifiants invalides.")
            État.jetons_actifs.remove(mots[1])
            return False

        self.session = (identifiants[0],identifiants[1],identifiants[2])
        self.session_jeton = mots[1]
        return True

    def est_serveur_autorisé(self) -> bool:
        # Vérifier si la communication vient d'un serveur autorisé.
        autorisation = self.headers.get("Authorization")

        if not autorisation or not isinstance(autorisation, str) or len(autorisation) == 0:
            print("Authorisation mal formée")
            return False

        mots = autorisation.split(" ")
        if len(mots) != 2:
            print("Authorisation mal formée")
            return False
        
        if mots[0] != "Basic":
            print("La méthode d'authentification n'est pas reconnue.")
            return False

        identifiants = b64decode(mots[1].encode()).decode("utf8").split(':')

        # Pour vérifier qu'il s'agit bien du serveur qu'il prétend être, nous
        # allons le contacter avec les identifiants fournits à travers l'URL 
        # que nous possédons. Il devrait répondre avec les mêmes identifiants.

        # Si la requête provient de nous, autoriser
        if identifiants[0] == État.serveur_nom and identifiants[1] == État.serveur_mdp and int(identifiants[2]) in État.requêtes_confirmations_attentes:
            return True

        # Obtenir le serveur correspondant
        self.sql.execute(
            "SELECT url "+
            "FROM ServeursAutorisés "+
            "WHERE nom_id=%s AND mot_de_passe=%s",
            (identifiants[0],identifiants[1])
        )
        res : list[tuple[str]] = self.sql.fetchall()
        if len(res) == 0:
            print("Aucun serveur autorisé trouvé")
            return False
        serveur_url = res[0][0]

        try:
            r = requests.get(
                url=serveur_url+"/authentification-serveur",
                headers={"Authorization":"Basic "+autorisation}
            )
            if r.status_code != 200:
                print("La vérification a échoué : "+str(r.status_code)+", "+r.reason)
                return False
            if json.loads(r.content.decode("utf8"))["jeton"] != autorisation:
                print("La vérification a échoué")
                return False
        except Exception as e:
            print(e)
            return False
        
        return True

    def est_id_valide(self, id : str) -> bool:
        # Format des identifiants : <nom>@<domaine1>.<domaine2>[.<domaine3>...](:<port>)?
        # <nom> peut contenir [a-z0-9_-] (la casse n'est pas discriminée), mais ne peut 
        #   ni commencer, ni finir par [_|-]
        # <domaine> est une liste de noms [a-z0-9_-] séparés par des points : 
        #   ex. : domaine.qc.ca
        # <port> est un entier optionnel.
        # Chaque nom doit commencer et se terminer par [a-zA-Z0-9]
        # Le domaine ne peut ni commencer ni se terminer par un point
        # Il doit y avoir au moins deux noms dans le domaine
        #
        # Le regex est essentiellement composé de trois fois le groupe
        #   [a-z0-9]([\w-]*[a-z0-9])?
        # Répartis comme ceci : <grp>@<grp>(\.<grp>)+
        # Auquel on ajoute le port : (:[0-9]{2,5})?
        return re.match(
            r"^[a-z0-9]([\w-]*[a-z0-9])?@([a-z0-9]([\w-]*[a-z0-9])?(\.[a-z0-9]([\w-]*[a-z0-9])?)+|localhost)(:[0-9]{2,5})?$",
            id.lower() ) is not None

    def enregistrer_temps_interaction(self):
        """
        Enregistre dans la base de donnée la date de cette interaction
        pour déconnecter après 3h d'innactivité
        """
        self.sql.execute(
            "UPDATE Utilisateurs SET date_dernière_interaction=NOW() WHERE nom_id=%s",
            (self.session[0],)
        )
        self.sql_connection.commit()

def main():
    port = ThreadingHTTPServer(('', 8000), ServeurHTTP)
    port.serve_forever()

if __name__ == "__main__":
    main()