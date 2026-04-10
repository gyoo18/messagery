Public Class Contact
    Public ID As String

    Public Conteneur As FlowLayoutPanel
    Public Avatar As PictureBox
    Public Nom As Label
    Public Lus As RadioButton
    Public BtnOuvrir As Button

    Public Sub New(ID As String)
        Me.Conteneur = New FlowLayoutPanel()
        Me.Conteneur.AutoSize = True
        Me.Avatar = New PictureBox()
        Me.Avatar.Image = Avatar
        Me.Conteneur.Controls.Add(Me.Avatar)
        Me.Nom = New Label()
        Me.Nom.Text = Nom
        Me.Conteneur.Controls.Add(Me.Nom)
        Me.Lus = New RadioButton
        Me.Lus.AutoCheck = False
        Me.Lus.Checked = False
        Me
    End Sub

End Class
