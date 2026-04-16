Public Class BoîteMessage

    Private Shared police_ids = New Font("Segoe UI", 8, FontStyle.Italic)
    Private Shared police_date = New Font("Segoe UI", 8, FontStyle.Regular)

    Public Conteneur As FlowLayoutPanel
    Public Sub New(m As Message)
        ' Conteneur
        Me.Conteneur = New FlowLayoutPanel()
        Conteneur.MinimumSize = New Size(440, 39)
        conteneur.AutoSizeMode = AutoSizeMode.GrowOnly
        conteneur.AutoSize = True
        conteneur.FlowDirection = If(État.session = m.contact.nom_id & "@" & m.contact.serveur, FlowDirection.RightToLeft, FlowDirection.LeftToRight)

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
        identificateur.Font = Me.police_ids
        identificateur.ForeColor = SystemColors.ControlDarkDark
        EnTête.Controls.Add(identificateur)

        Dim date_publication = New Label()
        date_publication.Text = m.date_publication.ToString("dd MMM yyyy, HH:mm:ss")
        date_publication.Font = Me.police_date
        date_publication.ForeColor = SystemColors.ControlDarkDark
        EnTête.Controls.Add(date_publication)

        ' Message
        Dim contenu = New Label()
        contenu.Text = m.contenu
        paneau.Controls.Add(contenu)
    End Sub

End Class
