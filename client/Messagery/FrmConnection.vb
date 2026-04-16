Public Class FrmConnection

    Private communication As Communication

    Public Sub New()
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.communication = New Communication()
    End Sub

    ' 🔹 Bouton Se connecter
    Private Async Sub btnConnexion_Click(sender As Object, e As EventArgs) Handles btnConnexion.Click

        Dim identificateur As String = txtNom.Text.Trim()
        Dim mdp As String = txtMotDePasse.Text.Trim()

        ' Vérification des champs
        If identificateur = "" Or mdp = "" Then
            MessageBox.Show("Veuillez remplir tous les champs.")
            Exit Sub
        End If

        If Not Utilitaires.est_identifiant_valide(identificateur) Then
            MessageBox.Show("Erreur de syntaxe dans l'identifiant. Identifiant invalide. Doit être de la forme <>")
            Exit Sub
        End If

        Dim nom_id = identificateur.Split("@")(0)
        Dim serveur = identificateur.Split("@")(1)

        Dim erreur = Await Me.communication.Connecter(nom_id, serveur, mdp)
        ' Vérification dans la base
        If erreur Is Nothing Then
            MessageBox.Show("Connexion réussie")

            ' Ouvrir la messagerie
            Dim f As New FrmMessagerie(Me.communication)
            État.session = identificateur
            f.Show()
            Me.Hide()

        Else
            MessageBox.Show("Une erreur s'est produite lors de la connection : " & vbCrLf & erreur)
        End If

    End Sub

    ' 🔹 Bouton S'inscrire
    Private Sub btnInscription_Click(sender As Object, e As EventArgs) Handles btnInscription.Click

        Dim f As New FrmInscription(Me.communication)
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