Public Class FrmConnexion

    ' 🔹 Bouton Se connecter
    Private Sub btnConnexion_Click(sender As Object, e As EventArgs) Handles btnConnexion.Click

        Dim nom As String = txtNom.Text.Trim()
        Dim mdp As String = txtMotDePasse.Text.Trim()

        ' Vérification des champs
        If nom = "" Or mdp = "" Then
            MessageBox.Show("Veuillez remplir tous les champs.")
            Exit Sub
        End If

        ' Vérification dans la base
        If DatabaseHelper.VerifierConnexion(nom, mdp) Then
            MessageBox.Show("Connexion réussie")

            ' Ouvrir la messagerie
            Dim f As New FrmMessagerie()
            f.NomUtilisateur = nom
            f.Show()
            Me.Hide()

        Else
            MessageBox.Show("Nom ou mot de passe incorrect")
        End If

    End Sub

    ' 🔹 Bouton S'inscrire
    Private Sub btnInscription_Click(sender As Object, e As EventArgs) Handles btnInscription.Click

        Dim f As New FrmInscription()
        f.ShowDialog()

    End Sub

    ' 🔹 Bouton Quitter
    Private Sub btnQuitter_Click(sender As Object, e As EventArgs) Handles btnQuitter.Click

        Application.Exit()

    End Sub

    ' 🔹 Au chargement du formulaire
    Private Sub FrmConnexion_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Cacher le mot de passe
        txtMotDePasse.UseSystemPasswordChar = True

    End Sub

End Class