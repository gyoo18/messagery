Public Class BoîteChoixContact
    Public conteneur As FlowLayoutPanel
    Public afficher As Boolean = True

    Private contact As Contact
    Private clicCallback As Action(Of Contact)

    Public Sub New(ByRef c As Contact, action As Action(Of Contact))
        Me.contact = c

        ' Conteneur
        Me.conteneur = New FlowLayoutPanel()
        Me.conteneur.Size = New Size(328, 51)
        Me.conteneur.BackColor = SystemColors.ControlLight
        Me.conteneur.BorderStyle = BorderStyle.FixedSingle
        Me.conteneur.Cursor = Cursors.Hand

        ' Avatar
        Dim avatar = New PictureBox()
        avatar.Size = New Size(27, 27)
        avatar.Margin = New Padding(10)
        Me.conteneur.Controls.Add(avatar)

        ' Nom d'affichage
        Dim nomAffichage = New Label()
        nomAffichage.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        nomAffichage.Text = c.nom_affichage
        Me.conteneur.Controls.Add(nomAffichage)

        ' Identificateur
        Dim identificateur = New Label()
        identificateur.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        identificateur.ForeColor = SystemColors.ControlDarkDark
        identificateur.Text = c.nom_id & "@" & c.serveur
        Me.conteneur.Controls.Add(identificateur)

        ' clicCallback
        Me.clicCallback = action
        AddHandler Me.conteneur.Click, AddressOf Me.distrubuerClicCallbacks
    End Sub

    Private Sub distrubuerClicCallbacks()
        Me.clicCallback(Me.contact)
    End Sub
End Class
