# Messagery

Application de messagerie interne client-serveur en Visual Basic pour le cours de Développement d'Application en Visual Basic.

## Maquette

![page principale](./README/maquette1.svg)
![page de création de discussion](./README/maquette2.svg)

## Fonctionnalités

- Se connecter à un compte stocké sur un serveur
- Créer une ou plusieurs discussions à un ou plusieurs contacts sur un ou plusieurs serveurs
- Rechercher des contacts sur les serveurs configurés par l'administrateur
- Sélectionner une discussion parmis celles créées
- Voir le fil de discussion
- Envoyer et recevoir des messages
- Mettre à jour la pastille « Messages nons lus ».

## Architecture

### Utilisation et réseau

```mermaid
flowchart TD
    A([Administrateur]) -->|Configuration| S[Serveur]
    S --> DB[(SQL)]
    U([Utilisateur]) -->|Connection| APP[Application]
    APP <-->|Connection & Synchronisation| S
    APP -->|Envoie message| S
    S -->|Transmet message| C([Contact])  
```

### Architecture de l'aplication

```text
                          Messages
  Fenêtre     /--Contact ╱    EnTête
 ╱  Discussions   BtnAjouter ╱    MessagesContact
┏━━╱━━━━━━━╱━━━━━╱━━━━╱━━━━━╱━━━━╱━━━━━━━━━━━━━━┓
┃┌╱───────╱─────╱─┐ ╒╱═════╱════╱═════════════╤╕┃
┃| Discus╱ions [+]| || O Nom id@serveur.tld   ||┃
┃|      ╱         | |└────────╱───────────────┘|┃
┃| [o Nom     o ] | |[Message]                /---- MessagesEnvoyés
┃| [o Nom       ] | |[Message]               ╱ |┃
┃| [o Nom     o ] | |                 [Message]|┃
┃| [o Nom        ]| |[Message]                 |┃
┃| [o Nom       ] | |┌────────────────────────┐|┃
┃|                | ||[Message_txt________][>]||┃
┃└────────────────┘ ╘╧╲════════════════════╲═╲╧╛┃
┗━━━━━━━━━━━━━━━━━━━━━━╲━━━━━━━━━━━━━━━━━━━━╲━╲━┛
                        ╲                    ╲ \-- BtnEnvoyer
                         \-- BasDePage        \-- BoiteTexte

                   
                  ConteneurContacts
┏━━━━━━━━━━━━━━━━╱━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃┌──────────────╱─┐ ╒╤════════════════════════╤╕┃
┃| Discussions ╱+]| || O Nom id@serveur.tld   ||┃
┃|+--------------+| |└────────────────────────┘|┃
┃||[o Nom     o ]|| |[Message]----------------+|┃
┃||[o Nom       ]|| |[Message]                |---- ConteneurMessages
┃||[o Nom     o ]|| ||                [Message]|┃
┃||[o Nom        ]| |[Message]----------------+|┃
┃||[o Nom       ]|| |┌────────────────────────┐|┃
┃|+--------------+| ||[Message_txt________][>]||┃
┃└────────────────┘ ╘╧════════════════════════╧╛┃
┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛

                             MenuContextuel
                            ╱ Titre
                           ╱ ╱  BoiteRecherche
                          ╱ ╱  ╱          BtnAjouter
   ┏━━━━━━━━━━━━━━━━━━━━━╱━╱━━╱━━━━━━━━━━╱━━━━━━━━━┓
   ┃┌────────────────┐ ╒╱═╱══╱══════════╱════════╤╕┃
   ┃| Discussions [-]| ╱|╱O ╱om id@serv╱ur.tld   ||┃
   ┃|                ┏━━╱━━╱━━━━━━━━━━╱──────────┘|┃
   ┃| [o Nom     o ] ┃ Cré╱r nvl. dis╱┃▒          |┃
   ┃| [o Nom       ] ┃ [id_txt___][+] ┃▒          |┃
   ┃| [o Nom     o ] ┃[[o Nom][o Nom]]-------------- NomsSélectionnés
   ┃| [o Nom        ]┃┌──────────────\-------------- NomSélectionné
   ┃| [o Nom       ] ┃|[o Nom a@a.a ]|┃▒─────────┐|┃
   ┃|                ┃|[o Nom a@a.a ]|┃▒_____][>]||┃
   ┃└────────────────┃└─╱────────────┘┃▒═════════╧╛┃
   ┗━━━━━━━━━━━━━━━━━┃ ╱ [ Créer> ]   ╲▒━━━━━━━━━━━┛
                     ┗╱━━━━━━━━━━━╲━━━┛\--- RechercheRésultats
RechercheRésultat ---/ ▒▒▒▒▒▒▒▒▒▒▒▒╲▒▒▒▒  
                                    \--- BtnCréer                     
```

### Hiérarchie des composantes

```mermaid
classDiagram
    Fenêtre "1"*--"1" Discussions
    Fenêtre "1"*--"1" Messages
    Discussions "1"*-- BtnAjouter
    Discussions "1"*--"1" ConteneurContact
    ConteneurContact "1"*--"*" Contact
    Messages "1"*--"1" EnTête
    Messages "1"*--"1" ConteneurMessages
    Messages "1"*--"1" BasDePage
    ConteneurMessages "1"*--"*" MessageContact
    ConteneurMessages "1"*--"*" MessageEnvoyé
    BasDePage "1"*--"1" BoiteTexte
    BasDePage "1"*--"1" BtnEnvoyer

    class Contact{
        +avatar:Img
        +nom:String
        +lus:boolean
    }
    class EnTête{
        +avatar:Img
        +nom:String
        +contact:String
    }
    class MessageContact{
        +contenu:String
    }
    class MessageEnvoyé{
        +contenu:String
    }
```

------------------------------------------------

```mermaid
classDiagram
    MenuContextuel "1"*--"1" Titre
    MenuContextuel "1"*--"1" BoiteRecherche
    MenuContextuel "1"*--"1" BtnAjouter
    MenuContextuel "1"*--"1" NomsSélectionnées
    MenuContextuel "1"*--"1" RechercheRésultats
    MenuContextuel "1"*--"1" BtnCréer
    NomsSélectionnées "1"*--"1" NomSélectionné
    RechercheRésultats "1"*--"1" RechercheRésultat

    class NomSélectionné{
        +avatar:Img
        +nom:String
        +contact:String
    }
    
    class RechercheRésultat{
        +avatar:Img
        +nom:String
        +contact:String
    }
```

### Conception Base de Donnée

```mermaid
classDiagram
    Contacts "*"--o"1" ServeursAutorisés
    note for Contacts "'serveur' est null si 'est_local' est faux et 'utilisateur_id' est null si 'est_local est vrai'"
    Contacts "1"--"1" Utilisateurs
    note for Utilisateurs "date_connection est null si l'utilisateur n'est pas connecté"
    Contacts "*"-- ConversationsContacts
    ConversationsContacts --o"*" Conversations
    Conversations "1"*--"*" Messages
    Utilisateurs "1"*-- MessagesNonLus
    MessagesNonLus --"*" Messages
    Utilisateurs "*"*-- Invitations
    Invitations --"*" Conversations
    Messages "1"o--"1" Contacts

    class Contacts{
        contact_id:int
        nom_affichage:string
        nom_id:string
        serveur_id:int
        est_local:bool
        utilisateur_id:int
    }

    class Utilisateurs{
        utilisateur_id:int
        nom_affichage:string
        nom_id:string
        mot_de_passe:string
        date_connection:datetime
        date_dernière_interaction:datetime
    }

    class ConversationsContacts{
        contact_id:int
        conversation_id:int
    }

    class Conversations{
        conversation_id:int
    }

    class Messages{
        message_id:int
        conversation_id:int
        contacts_id:int
        date:datetime
        message:string
    }

    class MessagesNonLus{
        message_id:int
        utilisateur_id:int
    }

    class Invitations{
        conversation_id:int
        utilisateur_id:int
    }

    class ServeursAutorisés{
        serveur_id:int
        url:string
        nom_id:string
        mot_de_passe:string
    }
```

### Noeuds HTTP

- **Inscription :** `POST`, `/inscription`

  ```json
  {
    "nom_id":"<nom_id>",
    "nom_affichage":"<nom_affichage>",
    "mot_de_passe":"<mot_de_passe>"
  }
  ```
  
  - **Réponse :**

    ```json
    {
        "accepté":[true,false]
    }
    ```

- **Connection :** `GET`,`/connection`,`Authorization: Basic <bases64(nom_id:mot_de_passe)>`
  - **Réponse :**
  
    ```json
    {
        "accepté":[true,false],
        // Si "accepté"=true : 
        "jeton":"base64(nom_id:mot_de_passe:date_unix)"
    }
    ```

- **Déconnection :** `POST`, `/deconnection`, `Authorization: Bearer <jeton>`

- **Synchronisation de connection :** `GET`,`/synchronisation-connection`,`Authorization: Bearer <jeton>`
  - **Réponse :**

    ```json
    {
        "contacts":[
            {
                "ID":"<identificateur>",
                "nom":"<nom>"
            }
        ],
        "conversations":[
            {
                "ID":"<int>",
                "contacts":[/*IDs*/],
                "messages":[
                    // En ordre chronologique
                    {
                        "contact":"<identificateur>",
                        "date":"<date>",
                        "message":"<contenu>"
                    }
                ]
            }
        ],
        "conversations-non-lues":[/*IDs*/]
    }
    ```

- **Synchronisation :** `POST`,`/synchronisation`,`Authorization: Bearer <jeton>`
  
  ```json
  {
    "conversations-lues":[/*IDs*/],
    "conversations-effacées":[/*IDs*/]
  }
  ```
  
  - **Réponse :**

    ```json
    {
        "nouvelles-conversations":[
            {
                "ID":"<int>",
                "contacts":[/*IDs*/],
            }
        ],
        "nouveaux-messages":[
            {
                "conversation":"<id>",
                "contact":"<id>",
                "date":"<date>",
                "message":"<contenu>"
            }
        ]
    }
    ```

- **Nouvelle Conversation :** `POST`, `/conversation`, `Authorization: Bearer <jeton>`
  - *Envoie une invitation aux contacts*

  ```json
  {
    "contacts":[/*IDs*/]
  }
  ```

  - **Réponse :**

    ```json
    {
        "accepté":[true,false], // Rejeté si la conversation existe déjà avec les contacts
        "conversation":"<id>", // Date de création UNIX
    }
    ```

- **Invitation :** `POST`, `/invitation`, `Authorization: Bearer <jeton>`

    ```json
    {
        "conversation":"<id>", // Date de création UNIX de la conversation
        "contacts":[/*IDs*/]
    }
    ```

    - **Réponse :**

        ```json
        {"accepté":[true,false]} // Refusé si la conversation existe déjà
        ```

- **Invitation (Inter-serveurs) :** `POST`, `/invitation-relais`, `Authorization: Basic base64(nom:mdp:id_requête)`

    ```json
    {
        "conversation":"<id>", // Date de création UNIX de la conversation
        "contacts":[/*IDs*/],
        "messages":[
            {
                "contact":"<id>",
                "date":"<date>",
                "message":"<contenu>"
            }
        ]
    }
    ```

    - **Réponse :**
  
        ```json
        {"accepté":[true,false]} // Refusé si la conversation existe déjà
        ```

- **Nouveau message :** `POST`, `/message`, `Authorization: Bearer <jeton>`

    ```json
    {
        "conversation":"<id>",
        "message":"<contenu>"
    }
    ```

- **Nouveau message (inter-serveur) :** `POST`, `/message-relais`, `Authorization: Basic base64(nom:mdp:id_requête)`

    ```json
    {
        "conversation":"<id>",
        "contact":"<id>",
        "date":"<date>",
        "message":"<contenu>"
    }
    ```

- **Confirmation d'authentification serveur :** `GET`, `/authentification-serveur`, `Authorization: Basic <jeton envoyé>`
  - *Envoyé après une communication inter-serveur où le serveur veut confirmer l'identité de la source.*
  - **Réponse :**

    ```json
    {
        "jeton":"<jeton envoyé>"
    }
    ```
