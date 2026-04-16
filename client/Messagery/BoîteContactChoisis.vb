Public Class BoîteContactChoisis
    Public conteneur As FlowLayoutPanel
    Public afficher As Boolean = True

    Private contact As Contact
    Private clicCallback As Action(Of Contact)

    Public Sub New(ByRef c As Contact, action As Action(Of Contact))
        Me.contact = c

        ' Conteneur
        Me.conteneur = New FlowLayoutPanel()
        Me.conteneur.FlowDirection = FlowDirection.TopDown
        Me.conteneur.WrapContents = False
        Me.conteneur.AutoSize = True
        Me.conteneur.BackColor = SystemColors.ScrollBar
        Me.conteneur.Cursor = Cursors.Hand

        ' ConteneurDessus
        Dim conteneurDessus = New FlowLayoutPanel()
        conteneurDessus.WrapContents = False
        conteneurDessus.AutoSize = True
        conteneurDessus.Margin = New Padding(0)
        Me.conteneur.Controls.Add(conteneurDessus)

        ' Avatar
        Dim avatar = New PictureBox
        avatar.Margin = New Padding(10)
        avatar.Size = New Size(27, 27)
        conteneurDessus.Controls.Add(avatar)

        ' Nom d'affichage
        Dim nomAffichage = New Label()
        nomAffichage.Margin = New Padding(3, 10, 3, 10)
        nomAffichage.Font = New Font("Arial Black", 10, FontStyle.Bold)
        nomAffichage.Text = c.nom_affichage
        conteneurDessus.Controls.Add(nomAffichage)

        ' Identifiant
        Dim identifiant = New Label()
        identifiant.Margin = New Padding(0)
        identifiant.Font = New Font("Segoe UI", 9, FontStyle.Italic)
        identifiant.ForeColor = SystemColors.ControlDarkDark
        identifiant.Text = c.nom_id & "@" & c.serveur
        Me.conteneur.Controls.Add(identifiant)

        ' clicCallback
        Me.clicCallback = action
        AddHandler Me.conteneur.Click, AddressOf Me.distribuerClicCallback
    End Sub

    Private Sub distribuerClicCallback()
        Me.clicCallback(Me.contact)
    End Sub
End Class
