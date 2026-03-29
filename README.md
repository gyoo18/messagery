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

----------------------------------------------

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