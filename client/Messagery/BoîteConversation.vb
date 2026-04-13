Imports System.DirectoryServices

Public Class BoîteConversation
    Public Conversation As Conversation
    Public Conteneur As FlowLayoutPanel
    Public Avatar As PictureBox
    Public NomAffichage As Label
    Public Lus As RadioButton
    Public BtnOuvrir As Button

    Private ouvrir_conversation_callbacks As List(Of Action(Of Conversation)) = New List(Of Action(Of Conversation))()

    Public Sub New(conv As Conversation)
        Me.Conversation = conv

        ' FlowLayoutPanel
        Me.Conteneur = New FlowLayoutPanel()
        Me.Conteneur.AutoSize = True
        Me.Conteneur.Margin = New Padding(3)
        Me.Conteneur.BackColor = SystemColors.ControlLight
        Me.Conteneur.BorderStyle = BorderStyle.FixedSingle

        ' PictureBox
        Me.Avatar = New PictureBox()
        ' Me.Avatar.Image = Avatar
        Me.Avatar.Size = New Size(27, 27)
        Me.Avatar.Margin = New Padding(10)
        Me.Conteneur.Controls.Add(Me.Avatar)

        ' FlowLayoutPanel2
        Dim panel = New FlowLayoutPanel()
        panel.AutoSize = False
        panel.Size = New Size(117, 39)
        Me.Conteneur.Controls.Add(panel)

        ' Panel - Label
        Me.NomAffichage = New Label()
        Me.NomAffichage.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        Me.NomAffichage.AutoSize = True
        Me.NomAffichage.Margin = New Padding(3, 10, 3, 10)
        For i = 0 To conv.contacts.Count - 2
            Me.NomAffichage.Text &= conv.contacts(i).nom_affichage & ", "
        Next
        Me.NomAffichage.Text &= conv.contacts(conv.contacts.Count - 1).nom_affichage
        panel.Controls.Add(Me.NomAffichage)

        ' RadioButton
        Me.Lus = New RadioButton()
        Me.Lus.Text = String.Empty
        Me.Lus.AutoCheck = False
        Me.Lus.Checked = Not conv.est_lue
        Me.Lus.Margin = New Padding(3, 15, 3, 15)
        Me.Lus.Size = New Size(14, 13)
        Me.Conteneur.Controls.Add(Me.Lus)

        ' Button
        Me.BtnOuvrir = New Button()
        Me.BtnOuvrir.Text = ">"
        Me.BtnOuvrir.Size = New Size(23, 23)
        Me.BtnOuvrir.Margin = New Padding(3, 10, 3, 10)
        AddHandler Me.BtnOuvrir.Click, AddressOf Me.distribuer_ouvrir_conversation_callback
        Me.Conteneur.Controls.Add(Me.BtnOuvrir)
    End Sub

    Public Sub enregistrer_ouvrir_conversation_callback(callback As Action(Of Conversation))
        Me.ouvrir_conversation_callbacks.Add(callback)
    End Sub

    Public Sub distribuer_ouvrir_conversation_callback()
        Me.Activer()
        For Each c In Me.ouvrir_conversation_callbacks
            c(Me.Conversation)
        Next
    End Sub

    Public Sub Activer()
        Me.Conteneur.BackColor = SystemColors.Control
        Me.BtnOuvrir.Margin = New Padding(3, 10, 10, 3)
    End Sub

    Public Sub Désactiver()
        Me.Conteneur.BackColor = SystemColors.ControlLight
        Me.BtnOuvrir.Margin = New Padding(3, 10, 3, 10)
    End Sub

End Class
