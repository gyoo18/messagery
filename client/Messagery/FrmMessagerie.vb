Imports System.ComponentModel

Public Class FrmMessagerie
    Public communication As Communication

    Private boîtesConversations As Dictionary(Of Integer, BoîteConversation)
    Private conversationActive As Integer

    Private travailleurMiseÀJour As BackgroundWorker

    Private texteParDefaut As String = "écrire un message..."

    Public Sub New(ByRef com As Communication)
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.communication = com
    End Sub

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim rep = Await Me.communication.Synchronisation_Connection()
        If rep Is Nothing Then
            MsgBox("Une erreur est survenue lors de la communication avec le serveur.")
            Exit Sub
        End If

        État.contacts = rep.contacts
        État.conversations = rep.conversations

        Me.boîtesConversations = New Dictionary(Of Integer, BoîteConversation)(État.conversations.Values.Count)
        For i = 0 To État.conversations.Values.Count - 1
            Dim conv = État.conversations.Values(i)
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

    Public Sub détruire() Handles MyBase.FormClosing
        ' Ferme l'application
        Me.déconnection()
        Application.Exit()
    End Sub

    Private Async Sub déconnection()
        Me.communication.deconnecter()
        État.session = Nothing ' Drapeau indicant au reste de l'application que la session est suspendue
    End Sub

    Private Sub afficherConversation(conv As Conversation)
        If État.conversations.ContainsKey(Me.conversationActive) Then
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
        For Each m In conv.messages
            Me.MessagesConteneur.Controls.Add(New BoîteMessage(m).Conteneur)
        Next
    End Sub

    Private Sub MessageEntrée_Enter(sender As Object, e As EventArgs) Handles MessageEntrée.Enter
        If MessageEntrée.Text = texteParDefaut Then
            MessageEntrée.Text = ""
            MessageEntrée.ForeColor = Color.Black
        End If
    End Sub

    Private Sub MessageEntrée_Leave(sender As Object, e As EventArgs) Handles MessageEntrée.Leave
        If MessageEntrée.Text.Trim() = "" Then
            MessageEntrée.Text = texteParDefaut
            MessageEntrée.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub MessageEntrée_KeyDown(sender As Object, e As KeyEventArgs) Handles MessageEntrée.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            BtnEnvoyer.PerformClick()
        End If
    End Sub

    Public Sub BtnEnvoyer_Click() Handles BtnEnvoyer.Click
        If Not État.conversations.ContainsKey(Me.conversationActive) Then
            Exit Sub
        End If

        Me.communication.envoyer_message(Me.MessageEntrée.Text, Me.conversationActive)

        ' Créer le nouveau message
        Dim m = New Message With {
            .contact = État.contacts(État.session),
            .date_publication = DateAndTime.Now,
            .contenu = Me.MessageEntrée.Text
        }
        État.conversations(Me.conversationActive).messages.Add(m)

        ' Ajouter le message à l'écran
        Me.MessagesConteneur.Controls.Add(New BoîteMessage(m).Conteneur)
    End Sub

    Public Async Sub misÀJour() Handles BackgroundWorker1.DoWork
        Do While True
            If État.session Is Nothing Then
                Exit Do
            End If

            Dim sync = Await Me.communication.synchronisation()

            For Each c In sync.nouvelles_conversations
                État.conversations(c.Key) = c.Value
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
                État.conversations(m.Item1).messages.Add(m.Item2)
            Next

            Threading.Thread.Sleep(5000)
        Loop
    End Sub

    Private Sub BtnDéconnecter_Click(sender As Object, e As EventArgs) Handles BtnDéconnecter.Click
        Me.déconnection()
        FrmConnection.Show()
        Me.Hide()
    End Sub

    Private Async Sub BtnAjouterConversation_Click(sender As Object, e As EventArgs) Handles BtnAjouteConversation.Click
        Dim frmCréeerConversation As New FrmCréerConversation(Me.communication)

        frmCréeerConversation.ShowDialog()

        Dim sync = Await Me.communication.synchronisation()

        If sync Is Nothing Then
            MessageBox.Show("Impossible de mettre à jour les conversations.")
            Return
        End If

        For Each c In sync.nouvelles_conversations
            If Not État.conversations.ContainsKey(c.Key) Then
                État.conversations(c.Key) = c.Value

                Me.boîtesConversations(c.Key) = New BoîteConversation(c.Value)
                Me.boîtesConversations(c.Key).enregistrer_ouvrir_conversation_callback(
                New Action(Of Conversation)(
                    Sub(conv As Conversation)
                        Me.afficherConversation(conv)
                    End Sub
                ))

                Me.BoîtesConversationsConteneur.Controls.Add(Me.boîtesConversations(c.Key).Conteneur)
            End If
        Next
    End Sub
End Class
