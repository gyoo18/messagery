INSERT INTO Utilisateurs VALUES
(1,"jed02","Jean Dufour","mdp_jed02",NULL,NULL),
(2,"mar06","Marc-André Roubiot","mdp_mar06",NULL,NULL),
(3,"pam03","Paul Martin","mdp_pam03",NULL,NULL);

INSERT INTO Contacts VALUES
(1,"jed02","Jean Dufour",NULL,1,1),
(2,"mar06","Marc-André Roubiot",NULL,1,2),
(3,"pam03","Paul Martin",NULL,1,3);

INSERT INTO Conversations VALUES (1), (2);

INSERT INTO ConversationsContacts (contact_id, conversation_id) VALUES 
(1,1), (2,1), (2,2), (3,2);

INSERT INTO Messages (message_id, conversation_id, contact_id, date, message) VALUES
(1,1,1,"2026-04-13T15:24:00","Bonjour Marc"),
(2,1,2,"2026-04-13T15:24:05","Bonjour Jean"),
(3,2,2,"2026-04-13T15:24:10","Bonjour Paul"),
(4,2,3,"2026-04-13T15:24:15","Bonjour Marc");