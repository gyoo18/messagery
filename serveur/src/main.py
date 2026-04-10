import json
import re
import requests
from time import time
from http.server import ThreadingHTTPServer, BaseHTTPRequestHandler
from base64 import b64encode, b64decode
from datetime import datetime

class État:
    jetons_actifs : list[str] = []

class ServeurHTTP(BaseHTTPRequestHandler):

    def __init__(self, *args, **kwargs):
        self.session_jeton : str = ""
        self.session : tuple[str,str,int] = () # (nom_id,mdp,date_connection_unix)
        super().__init__(*args,**kwargs)
  
# ===========
#   Entrées
# ===========

    def do_GET(self):

        réponse : dict = None
        match(self.path):
            case "/connection" :                 réponse = self.connecter()
            case "/synchronisation-connection" : réponse = self.synchronisation_connection()
            case _:
                self.renvoyer_erreur(404,"Ce noeud n'existe pas")
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
            case "/deconnection" :       réponse = self.déconnection()
            case "/conversation" :       réponse = self.créer_conversation()
            case "/invitation" :         réponse = self.envoyer_invitation()
            case "/invitation-relais" :  réponse = self.recevoir_invitation()
            case "/message" :            réponse = self.envoyer_message()
            case "/message-relais" :     réponse = self.recevoir_message()
            case "/synchronisation" :    réponse = self.synchronisation()
            case _:
                self.renvoyer_erreur(404,"Ce noeud n'existe pas")
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

    def connecter(self) -> dict:
        """
        BaseHTTPRequestHandler.connection est une variable déjà définie.
        """
        # TODO vérifier les identifiants dans la base de donnée
        # TODO modifier l'information de connection dans la base de donnée
        self.session_jeton = b64encode((self.session[0]+':'+self.session[1]+':'+str(self.session[2])).encode()).decode("utf8")
        État.jetons_actifs.append(self.session_jeton)
        return {
            "accepté":True, 
            "jeton": self.session_jeton
        }

    def déconnection(self) -> dict:
        if not self.est_client_autorisé():
            self.renvoyer_erreur(401,"Soit vous n'êtes pas connectés, soit votre session est échue.")
            return

        # TODO vérifier les identifiants dans la base de donnée
        # TODO modifier l'information de connection dans la base de donnée
        État.jetons_actifs.remove(self.session_jeton) # Obtenu dans self.est_autorisé()
        return {}

    def synchronisation_connection(self) -> dict:
        if not self.est_client_autorisé():
            self.renvoyer_erreur(401,"Soit vous n'êtes pas connectés, soit votre session est échue.")
            return

        # TODO récupérer les contacts dans la base de donnée
        # TODO récupérer les conversations dans la base de donnée
        # TODO récupérer les conversations non-lues dans la base de donnée
        return {
            "contacts":[],
            "conversations":[],
            "conversations-non-lues":[]
        }

    def synchronisation(self) -> dict:
        if not self.est_client_autorisé():
            self.renvoyer_erreur(401,"Soit vous n'êtes pas connectés, soit votre session est échue.")
            return

        modifications : dict
        try:
            taille = self.headers.get("Content-Length")
            if not taille or not taille.isdigit():
                self.send_response(422,"L'en-tête 'Content-Length' doit être assigné et être un entier.")
                return

            modifications = json.loads(self.rfile.read(int(taille)))
        except Exception as e:
            print("Données mal formées")
            self.renvoyer_erreur(400,"Les données reçues ne peuvent être interprétées comme un json.")
            return None
        
        if ( ("conversations-lues" not in modifications) or 
            ("nouvelles-conversations" not in modifications) or 
            ("conversations-effacées" not in modifications)):
            print("JSON mal formé")
            self.renvoyer_erreur(422,"Les données reçues ne sont pas bien formattées.")
            return None
        
        # TODO modifier la base de donnée en fonction des données reçues
        # TODO obtenir les nouvelles conversations
        # TODO obtenir les nouveaux messages
        return {
            "nouvelles-conversations":[],
            "nouveaux-messages":[]
        }

    def créer_conversation(self) -> dict:
        if not self.est_client_autorisé():
            self.renvoyer_erreur(401,"Soit vous n'êtes pas connectés, soit votre session est échue.")
            return

        infos : dict = None
        try:
            taille = self.headers.get("Content-Length")
            if not taille or not taille.isdigit():
                self.send_response(422,"L'en-tête 'Content-Length' doit être assigné et être un entier.")
                return

            infos = json.loads(self.rfile.read(int(taille)))
        except Exception as e:
            print("Données mal formées")
            self.renvoyer_erreur(400,"Les données reçues ne peuvent être interprétées comme un json.")
            return None
        
        if "contacts" not in infos:
            print("JSON mal formé")
            self.renvoyer_erreur(422,"Les données reçues ne sont pas bien formattées.")
            return None
        
        contacts : list[str] = infos["contacts"]
        
        # Retirer les contacts invalides
        len_o = len(contacts)
        for i in range(len(contacts)):
            if not self.est_id_valide(contacts[len_o-i-1]):
                contacts.pop(len_o-i-1)
        
        if len(contacts) == 0:
            print("Aucun contact valide")
            return {"accepté":False}

        conversation_id = round(time())

        # TODO vérifier si une conversation avec ces contacts existe déjà

        # Collecter les serveurs
        serveurs = []
        for contact in contacts:
            mots = contact.split("@")
            if mots[1] not in serveurs:
                serveurs.append(mots[1])
        
        # Envoyer l'invitation aux serveurs
        for serveur in serveurs:
            try:
                r = requests.post(
                    url="http://"+serveur+"/invitation-relais",
                    json={
                        "conversation":conversation_id,
                        "contacts":contacts,
                        "messages":[]
                    })
                if not r.json()["accepté"] :
                    print("Erreur dans le relais de l'invitation")
                    self.renvoyer_erreur(502,"Le serveur d'un contact n'a pas accepté l'invitation.")
                    return None
            except Exception as e:
                print("Erreur dans le relais de l'invitation")
                self.renvoyer_erreur(502,"Le serveur d'un contact n'a pas pus être rejoint.")
                return None
        
        # TODO mettre à jour la base de données

        return {"accepté":True,"conversation":conversation_id}

    def envoyer_invitation(self) -> dict:
        if not self.est_client_autorisé():
            self.renvoyer_erreur(401,"Soit vous n'êtes pas connectés, soit votre session est échue.")
            return

        infos : dict = None
        try:
            taille = self.headers.get("Content-Length")
            if not taille or not taille.isdigit():
                self.send_response(422,"L'en-tête 'Content-Length' doit être assigné et être un entier.")
                return

            infos = json.loads(self.rfile.read(int(taille)))
        except Exception as e:
            print("Données mal formées")
            self.renvoyer_erreur(400,"Les données reçues ne peuvent être interprétées comme un json.")
            return None
        
        if "conversation" not in infos or "contacts" not in infos:
            print("JSON mal formé")
            self.renvoyer_erreur(422,"Les données reçues ne sont pas bien formattées.")
            return None
        
        conversation_id : int = infos["conversation"]
        contacts : list[str] = infos["contacts"]

        # TODO vérifier que la conversation existe
        
        # Retirer les contacts invalides
        len_o = len(contacts)
        for i in range(len(contacts)):
            if not self.est_id_valide(contacts[len_o-i-1]):
                contacts.pop(len_o-i-1)
        
        if len(contacts) == 0:
            print("Aucun contact valide")
            return {"accepté":False}

        # TODO vérifier si les contacts sont déjà dans la conversation

        # Collecter les serveurs
        serveurs = []
        for contact in contacts:
            mots = contact.split("@")
            if mots[1] not in serveurs:
                serveurs.append(mots[1])
        
        # Envoyer l'invitation aux serveurs
        for serveur in serveurs:
            try:
                r = requests.post(
                    url="http://"+serveur+"/invitation-relais",
                    json={
                        "conversation":conversation_id,
                        "contacts":contacts,
                        "messages":[]
                    })
                if not r.json()["accepté"] :
                    print("Erreur dans le relais de l'invitation")
                    self.renvoyer_erreur(502,"Le serveur d'un contact n'a pas accepté l'invitation.")
                    return None
            except Exception as e:
                print("Erreur dans le relais de l'invitation")
                self.renvoyer_erreur(502,"Le serveur d'un contact n'a pas pus être rejoint.")
                return None
        
        # TODO mettre à jour la base de données
        
        return {"accepté":True}

    def recevoir_invitation(self) -> dict:
        if not self.est_serveur_autorisé():
            self.renvoyer_erreur(401,"Votre serveur n'est pas autorisé à communiquer avec nous. Veuillez contacter l'administrateur de ce serveur pour l'ajouter à la liste d'autorisation.")
            return
        
        infos : dict = None
        try:
            taille = self.headers.get("Content-Length")
            if not taille or not taille.isdigit():
                self.send_response(422,"L'en-tête 'Content-Length' doit être assigné et être un entier.")
                return

            infos = json.loads(self.rfile.read(int(taille)))
        except Exception as e:
            print("Données mal formées")
            self.renvoyer_erreur(400,"Les données reçues ne peuvent être interprétées comme un json.")
            return None
        
        if "conversation" not in infos or "contacts" not in infos or "messages" not in infos:
            print("JSON mal formé")
            self.renvoyer_erreur(422,"Les données reçues ne sont pas bien formattées.")
            return None
        
        conversation : int = infos["conversation"]
        contacts : list[str] = infos["contacts"]
        messages : list[dict[str,str]] = infos["messages"]

        # TODO vérifier si la conversation existe déjà
        
        # Retirer les contacts invalides
        len_o = len(contacts)
        for i in range(len(contacts)):
            if not self.est_id_valide(contacts[len_o-i-1]):
                contacts.pop(len_o-i-1)
        
        if len(contacts) == 0:
            print("Aucun contact valide")
            return {"accepté":False}
        
        # TODO mettre à jour la base de données

        return {"accepté":True}

    def envoyer_message(self) -> dict:
        if not self.est_client_autorisé():
            self.renvoyer_erreur(401,"Soit vous n'êtes pas connectés, soit votre session est échue.")
            return

        infos : dict = None
        try:
            taille = self.headers.get("Content-Length")
            if not taille or not taille.isdigit():
                self.send_response(422,"L'en-tête 'Content-Length' doit être assigné et être un entier.")
                return

            infos = json.loads(self.rfile.read(int(taille)))
        except Exception as e:
            print("Données mal formées")
            self.renvoyer_erreur(400,"Les données reçues ne peuvent être interprétées comme un json.")
            return None
        
        if "conversation" not in infos or "message" not in infos:
            print("JSON mal formé")
            self.renvoyer_erreur(422,"Les données reçues ne sont pas bien formattées.")
            return None
        
        conversation : int = infos["conversation"]
        message : str = infos["message"]
        date : str = datetime.today().strftime("%Y-%m-%dT%H:%M%SZ") # Format ISO 8601

        # TODO vérifier que la conversation existe
        # TODO modifier la base de donnée
        
        contacts = [] # TODO récupérer les contacts de la conversation

        # Récolter les serveurs concernés
        serveurs = []
        for contact in contacts:
            serveur = contact.split("@")[1]
            if serveur not in serveurs:
                serveurs.append(serveur)

        # Relayer l'invitation aux serveurs
        for serveur in serveurs:
            try:
                requests.post(
                    url="http://"+serveur+"/message-relais",
                    json={
                        "conversation":conversation,
                        "contact":self.session[0]+"@"+self.headers.get("Host"),
                        "date":date,
                        "message":message
                    }
                )
            except Exception as e:
                print(e)
        
        return {}
    
    def recevoir_message(self) -> dict:
        if not self.est_serveur_autorisé():
            self.renvoyer_erreur(401,"Votre serveur n'est pas autorisé à communiquer avec nous. Veuillez contacter l'administrateur de ce serveur pour l'ajouter à la liste d'autorisation.")
            return

        infos : dict = None
        try:
            taille = self.headers.get("Content-Length")
            if not taille or not taille.isdigit():
                self.send_response(422,"L'en-tête 'Content-Length' doit être assigné et être un entier.")
                return

            infos = json.loads(self.rfile.read(int(taille)))
        except Exception as e:
            print("Données mal formées")
            self.renvoyer_erreur(400,"Les données reçues ne peuvent être interprétées comme un json.")
            return None
        
        if "conversation" not in infos or "message" not in infos:
            print("JSON mal formé")
            self.renvoyer_erreur(422,"Les données reçues ne sont pas bien formattées.")
            return None

        conversation : int = infos["conversation"]
        contact : str = infos["contact"]
        message : str = infos["message"]
        date : str = infos["date"] # Format ISO 8601

        # TODO vérifier que la conversation existe
        # TODO modifier la base de donnée

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

        if mots[0] == "Basic":
            identifiants : list[str] = None
            try:
                identifiants = b64decode(mots[1].encode()).decode("utf8").split(":")
                if len(identifiants) != 2:
                    print("Identifiants mal formée")
                    return False
            except Exception:
                print("Identifiants mal formée")
                return False
            
            # TODO interroger la base de donnée sur les identifiants
            self.session = (identifiants[0],identifiants[1],round(time()))
            return True
        
        elif mots[0] == "Bearer":
            if not mots[1] in État.jetons_actifs:
                print("Session échue")
                return False

            identifiants : list[str] = None
            try:
                identifiants = b64decode(mots[1].encode()).decode("utf8").split(":")
                if len(identifiants) != 3:
                    print("Identifiants mal formée")
                    return False
            except Exception:
                print("Identifiants mal formée")
                return False
            
            # TODO interroger la base de donnée sur les identifiants
            self.session = (identifiants[0],identifiants[1],identifiants[2])
            self.session_jeton = mots[1]
            return True
        
        print(mots[0])
        print("La méthode d'authentification n'est pas reconnue")
        return False

    def est_serveur_autorisé(self) -> bool:
        # TODO Vérifier si la communication vient d'un serveur autorisé.
        if self.client_address[0] == self.headers.get("Host").split(':')[0]:
            return True
        return True # TMP

    def renvoyer_erreur(self, code:int, msg:str) -> None:
        self.send_response(code)
        self.send_header("Content-Type","application/json")
        self.end_headers()

        msg_json = {"erreur":code,"message":msg}
        self.wfile.write(json.dumps(msg_json).encode())

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
            r"^[a-z0-9]([\w-]*[a-z0-9])?@[a-z0-9]([\w-]*[a-z0-9])?(\.[a-z0-9]([\w-]*[a-z0-9])?)+(:[0-9]{2,5})?$",
            id.lower() ) is not None


def main():
    port = ThreadingHTTPServer(('', 8000), ServeurHTTP)
    port.serve_forever()

if __name__ == "__main__":
    main()