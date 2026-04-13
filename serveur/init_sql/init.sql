CREATE DATABASE IF NOT EXISTS messagery
CHARACTER SET "utf8mb4";

USE messagery;

CREATE TABLE IF NOT EXISTS ServeursAutorisés(
    serveur_id INT PRIMARY KEY,
    url VARCHAR(255),
    nom_id VARCHAR(255),
    mot_de_passe VARCHAR(255)
);

CREATE TABLE IF NOT EXISTS Utilisateurs(
    utilisateur_id INT PRIMARY KEY,
    nom_affichage VARCHAR(255),
    nom_id VARCHAR(255),
    mot_de_passe VARCHAR(255),
    date_connection DATETIME,
    date_dernière_interaction DATETIME
);

CREATE TABLE IF NOT EXISTS Contacts(
    contact_id INT PRIMARY KEY,
    nom_affichage VARCHAR(255),
    nom_id VARCHAR(255),
    serveur_id INT,
    est_local BOOLEAN,
    utilisateur_id INT,

    CONSTRAINT FK_ContactsServeurs FOREIGN KEY (serveur_id) REFERENCES ServeursAutorisés(serveur_id),
    CONSTRAINT FK_ContactsUtilisateurs FOREIGN KEY (utilisateur_id) REFERENCES Utilisateurs(utilisateur_id)
);

CREATE TABLE IF NOT EXISTS Conversations(
    conversation_id INT PRIMARY KEY
);

CREATE TABLE IF NOT EXISTS Messages(
    message_id INT PRIMARY KEY,
    conversation_id INT,
    contact_id INT,
    date DATETIME,
    message TEXT,

    CONSTRAINT FK_MessageConversation FOREIGN KEY (conversation_id) REFERENCES Conversations(conversation_id),
    CONSTRAINT FK_MessageContact FOREIGN KEY (contact_id) REFERENCES Contacts(contact_id)
);

CREATE TABLE IF NOT EXISTS ConversationsContacts(
    contact_id INT,
    conversation_id INT,

    CONSTRAINT FK_ConversationsContactsContact FOREIGN KEY (contact_id) REFERENCES Contacts(contact_id),
    CONSTRAINT FK_ConversationsContactsConversation FOREIGN KEY (conversation_id) REFERENCES Conversations(conversation_id)
);

CREATE TABLE IF NOT EXISTS Invitations(
    conversation_id INT,
    utilisateur_id INT,

    CONSTRAINT FK_InvitationConversation FOREIGN KEY (conversation_id) REFERENCES Conversations(conversation_id),
    CONSTRAINT FK_InvitationUtilisateur FOREIGN KEY (utilisateur_id) REFERENCES Utilisateurs(utilisateur_id)
);

CREATE TABLE IF NOT EXISTS MessagesNonLus(
    message_id INT,
    utilisateur_id INT,

    CONSTRAINT FK_MessageNonLusMessages FOREIGN KEY (message_id) REFERENCES Messages(message_id),
    CONSTRAINT FK_MessageNonLusUtilisateur FOREIGN KEY (utilisateur_id) REFERENCES Utilisateurs(utilisateur_id)
);