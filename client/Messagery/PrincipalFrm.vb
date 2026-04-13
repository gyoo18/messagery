Imports System.ComponentModel
Imports System.Net.Http
Imports System.Text.Json

Public Class PrincipalFrm
    Public serveur As Serveur
    Public état As État

    Private boîtesConversations As Dictionary(Of Integer, BoîteConversation)
    Private conversationActive As Integer

    Private travailleurMiseÀJour As BackgroundWorker

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim rep = Await Me.serveur.Synchronisation_Connection()
        If rep Is Nothing Then
            MsgBox("Une erreur est survenue lors de la communication avec le serveur.")
            Exit Sub
        End If

        Me.état.contacts = rep.contacts
        Me.état.conversations = rep.conversations

        Me.boîtesConversations = New Dictionary(Of Integer, BoîteConversation)(Me.état.conversations.Values.Count)
        For i = 0 To Me.état.conversations.Values.Count - 1
            Dim conv = Me.état.conversations.Values(i)
            Me.boîtesConversations(conv.ID) = New BoîteConversation(conv)
            Me.boîtesConversations(conv.ID).enregistrer_ouvrir_conversation_callback(
                New Action(Of Conversation)(
                    Sub(c As Conversation)
                        Me.afficherConversation(c)
                    End Sub
                ))
            Me.BoîtesConversationsConteneur.Controls.Add(Me.boîtesConversations(conv.ID).Conteneur)
        Next

        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Public Async Sub deconnection() Handles MyBase.FormClosing
        Me.serveur.deconnecter()
        Application.Exit()
    End Sub

    Private Sub afficherConversation(conv As Conversation)
        If Me.état.conversations.ContainsKey(Me.conversationActive) Then
            Me.boîtesConversations(Me.conversationActive).Désactiver()
        End If

        Me.conversationActive = conv.ID

        ' Afficher les contacts dans l'en-tête
        Dim nom As String = String.Empty
        Dim id As String = String.Empty
        For i = 0 To conv.contacts.Count - 2
            nom &= conv.contacts(i).nom_affichage & ", "
            id &= conv.contacts(i).nom_id & "@" & conv.contacts(i).serveur & ", "
        Next
        nom &= conv.contacts(conv.contacts.Count - 1).nom_affichage
        id &= conv.contacts(conv.contacts.Count - 1).nom_id & "@" & conv.contacts(conv.contacts.Count - 1).serveur

        Me.EnTêteNomsAffichage.Text = nom
        Me.EnTêteIdentificateurs.Text = id

        ' Ajouter les messages
        Me.MessagesConteneur.Controls.Clear()
        ' Les messages sont déjà ordonnés des plus récents aux plus vieux.
        Dim police_ids = New Font("Segoe UI", 8, FontStyle.Italic)
        Dim police_date = New Font("Segoe UI", 8, FontStyle.Regular)
        For Each m In conv.messages
            ' Conteneur
            Dim conteneur = New FlowLayoutPanel()
            conteneur.MinimumSize = New Size(466, 39)
            conteneur.AutoSizeMode = AutoSizeMode.GrowOnly
            conteneur.AutoSize = True
            conteneur.FlowDirection = If(Me.état.session = m.contact.nom_id & "@" & m.contact.serveur, FlowDirection.RightToLeft, FlowDirection.LeftToRight)
            Me.MessagesConteneur.Controls.Add(conteneur)

            ' Conteneur du message
            Dim paneau = New FlowLayoutPanel()
            paneau.AutoSize = True
            paneau.FlowDirection = FlowDirection.TopDown
            paneau.BackColor = SystemColors.ControlLight
            conteneur.Controls.Add(paneau)

            ' En-tête Conteneur
            Dim EnTête = New FlowLayoutPanel()
            EnTête.AutoSize = True
            EnTête.FlowDirection = FlowDirection.LeftToRight
            EnTête.Margin = New Padding(5)
            paneau.Controls.Add(EnTête)

            ' En-tête contenu
            Dim nom_affichage = New Label()
            nom_affichage.Text = m.contact.nom_affichage
            EnTête.Controls.Add(nom_affichage)

            Dim identificateur = New Label()
            identificateur.Text = m.contact.nom_id & "@" & m.contact.serveur
            identificateur.Font = police_ids
            identificateur.ForeColor = SystemColors.ControlDarkDark
            EnTête.Controls.Add(identificateur)

            Dim date_publication = New Label()
            date_publication.Text = m.date_publication.ToString("dd MMM yyyy, HH:mm:ss")
            date_publication.Font = police_date
            date_publication.ForeColor = SystemColors.ControlDarkDark
            EnTête.Controls.Add(date_publication)

            ' Message
            Dim contenu = New Label()
            contenu.Text = m.contenu
            paneau.Controls.Add(contenu)
        Next
    End Sub

    Public Sub BtnEnvoyer_Click() Handles BtnEnvoyer.Click
        If Not Me.état.conversations.ContainsKey(Me.conversationActive) Then
            Exit Sub
        End If

        Me.serveur.envoyer_message(Me.MessageEntrée.Text, Me.conversationActive)

        ' Créer le nouveau message
        Dim m = New Message With {
            .contact = Me.état.contacts(Me.état.session),
            .date_publication = DateAndTime.Now,
            .contenu = Me.MessageEntrée.Text
        }
        Me.état.conversations(Me.conversationActive).messages.Add(m)

        ' Ajouter le message à l'écran

        Dim police_ids = New Font("Segoe UI", 8, FontStyle.Italic)
        Dim police_date = New Font("Segoe UI", 8, FontStyle.Regular)
        ' Conteneur
        Dim conteneur = New FlowLayoutPanel()
        conteneur.MinimumSize = New Size(466, 39)
        conteneur.AutoSizeMode = AutoSizeMode.GrowOnly
        conteneur.AutoSize = True
        conteneur.FlowDirection = If(Me.état.session = m.contact.nom_id & "@" & m.contact.serveur, FlowDirection.RightToLeft, FlowDirection.LeftToRight)
        Me.MessagesConteneur.Controls.Add(conteneur)

        ' Conteneur du message
        Dim paneau = New FlowLayoutPanel()
        paneau.AutoSize = True
        paneau.FlowDirection = FlowDirection.TopDown
        paneau.BackColor = SystemColors.ControlLight
        conteneur.Controls.Add(paneau)

        ' En-tête Conteneur
        Dim EnTête = New FlowLayoutPanel()
        EnTête.AutoSize = True
        EnTête.FlowDirection = FlowDirection.LeftToRight
        EnTête.Margin = New Padding(5)
        paneau.Controls.Add(EnTête)

        ' En-tête contenu
        Dim nom_affichage = New Label()
        nom_affichage.Text = m.contact.nom_affichage
        EnTête.Controls.Add(nom_affichage)

        Dim identificateur = New Label()
        identificateur.Text = m.contact.nom_id & "@" & m.contact.serveur
        identificateur.Font = police_ids
        identificateur.ForeColor = SystemColors.ControlDarkDark
        EnTête.Controls.Add(identificateur)

        Dim date_publication = New Label()
        date_publication.Text = m.date_publication.ToString("dd MMM yyyy, HH:mm:ss")
        date_publication.Font = police_date
        date_publication.ForeColor = SystemColors.ControlDarkDark
        EnTête.Controls.Add(date_publication)

        ' Message
        Dim contenu = New Label()
        contenu.Text = m.contenu
        paneau.Controls.Add(contenu)
    End Sub

    Public Async Sub misÀJour() Handles BackgroundWorker1.DoWork
        Do While True
            Dim sync = Await Me.serveur.synchronisation()

            For Each c In sync.nouvelles_conversations
                Me.état.conversations(c.Key) = c.Value
                Me.boîtesConversations(c.Key) = New BoîteConversation(c.Value)
                Me.boîtesConversations(c.Key).enregistrer_ouvrir_conversation_callback(
                New Action(Of Conversation)(
                    Sub(conv As Conversation)
                        Me.afficherConversation(conv)
                    End Sub
                ))
                Me.BoîtesConversationsConteneur.Controls.Add(Me.boîtesConversations(c.Key).Conteneur)
            Next

            For Each m In sync.nouveaux_messages
                Me.état.conversations(m.Item1).messages.Add(m.Item2)
            Next

            Threading.Thread.Sleep(5000)
        Loop
    End Sub
End Class
