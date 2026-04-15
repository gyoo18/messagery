Imports Microsoft.Win32

Public Class FrmInscription
    Private communication As Communication

    Public Sub New(ByRef com As Communication)
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.communication = com
    End Sub

    Private Sub FrmInscription_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtMotDePasse.UseSystemPasswordChar = True
        txtConfirmation.UseSystemPasswordChar = True
    End Sub

    Private Async Sub btnEnregistrer_Click(sender As Object, e As EventArgs) Handles btnEnregistrer.Click
        Dim nom_id As String = txtIdentifiant.Text.Trim()
        Dim nom_affichage As String = txtNomAffichage.Text.Trim()
        Dim serveur As String = txtServeur.Text.Trim()
        Dim mdp As String = txtMotDePasse.Text.Trim()
        Dim confirmation As String = txtConfirmation.Text.Trim()

        If nom_id = "" Or nom_affichage = "" Or serveur = "" Or mdp = "" Or confirmation = "" Then
            MessageBox.Show("Veuillez remplir tous les champs.")
            Exit Sub
        End If

        If Not Utilitaires.est_identifiant_valide(nom_id & "@" & serveur) Then
            MessageBox.Show("Erreur de syntaxe dans l'identifiant ou le serveur. L'un des deux est invalide.")
            Exit Sub
        End If

        If mdp <> confirmation Then
            MessageBox.Show("Les mots de passe ne correspondent pas.")
            Exit Sub
        End If

        If Await Me.communication.Inscrire(nom_id, serveur, nom_affichage, mdp) Then
            MessageBox.Show("Inscription réussie.")
            Me.Close()
        End If
    End Sub

    Private Sub btnRetour_Click(sender As Object, e As EventArgs) Handles btnRetour.Click
        Me.Close()
    End Sub
End Class
