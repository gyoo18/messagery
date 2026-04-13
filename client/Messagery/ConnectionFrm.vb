Imports System.Net.Http
Imports System.Net.Http.Json
Imports System.Text
Imports System.Text.Json
Imports System.Text.Json.Nodes

Public Class ConnectionFrm
    Private serveur As Serveur

    Private frm_principal As PrincipalFrm

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Erreur.Hide()
        Me.serveur = New Serveur()
    End Sub

    Private Async Sub BtnConnecter_Click(sender As Object, e As EventArgs) Handles BtnConnecter.Click
        Me.Erreur.Hide()

        Dim identificateur As String = NomUtilisateur.Text

        If Not Utilitaires.est_identifiant_valide(identificateur) Then
            Me.Erreur.Text = "Erreur de syntaxe dans l'identifiant. Veuillez spécifier un identifiant du type <nom_id>@<serveur>."
            Me.Erreur.Show()
            Exit Sub
        End If

        Dim nom_id As String = identificateur.Split("@")(0)
        Dim serveur As String = identificateur.Split("@")(1)
        Dim mdp As String = MotDePasse.Text

        Dim erreur = Await Me.serveur.Connecter(nom_id, serveur, mdp)
        If erreur IsNot Nothing Then
            Me.Erreur.Text = erreur
            Me.Erreur.Show()
            Exit Sub
        End If

        PrincipalFrm.serveur = Me.serveur
        PrincipalFrm.état = New État With {.session = identificateur}
        Me.Hide()
        PrincipalFrm.Show()
    End Sub
End Class