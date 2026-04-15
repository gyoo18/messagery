Public Class FrmInscription

    Private Sub FrmInscription_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtMotDePasse.UseSystemPasswordChar = True
        txtConfirmation.UseSystemPasswordChar = True
    End Sub

    Private Sub btnEnregistrer_Click(sender As Object, e As EventArgs) Handles btnEnregistrer.Click
        Dim nom As String = txtNom.Text.Trim()
        Dim mdp As String = txtMotDePasse.Text.Trim()
        Dim confirmation As String = txtConfirmation.Text.Trim()

        If nom = "" Or mdp = "" Or confirmation = "" Then
            MessageBox.Show("Veuillez remplir tous les champs.")
            Exit Sub
        End If

        If mdp <> confirmation Then
            MessageBox.Show("Les mots de passe ne correspondent pas.")
            Exit Sub
        End If

        If DatabaseHelper.InscrireUtilisateur(nom, mdp) Then
            MessageBox.Show("Inscription réussie.")
            Me.Close()
        Else
            MessageBox.Show("Erreur lors de l'inscription ou utilisateur déjà existant.")
        End If
    End Sub

    Private Sub btnRetour_Click(sender As Object, e As EventArgs) Handles btnRetour.Click
        Me.Close()
    End Sub

End Class
