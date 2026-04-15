Imports System.Data
Imports System.Windows.Forms
Imports System.Media

Public Class FrmMessagerie

    Public Property NomUtilisateur As String

    Private gestionClient As New ClientManager()
    Private destinataireActuel As String = ""
    Private destinataireNomPur As String = ""
    Private WithEvents timerRefresh As New Timer()
    Private texteParDefaut As String = "écrire un message..."

    Private Sub FrmMessagerie_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblBienvenue.Text = "Bienvenue : " & NomUtilisateur
        rtbDiscussion.ReadOnly = True

        txtMessage.Text = texteParDefaut
        txtMessage.ForeColor = Color.Gray

        AddHandler gestionClient.MessageRecu, AddressOf AfficherMessageRecu
        AddHandler gestionClient.ListeUtilisateursRecue, AddressOf MettreAJourListeUtilisateurs
        AddHandler gestionClient.InformationRecue, AddressOf AfficherInformation
        AddHandler gestionClient.ConnexionPerdue, AddressOf GererConnexionPerdue

        If gestionClient.Connecter("127.0.0.1", 5000, NomUtilisateur) Then
            DatabaseHelper.MettreStatutUtilisateur(NomUtilisateur, "En ligne")
            rtbDiscussion.AppendText("=== Connecté au serveur ===" & Environment.NewLine & Environment.NewLine)
        Else
            MessageBox.Show("Impossible de se connecter au serveur. Lance d'abord ServeurMessagerie.")
        End If

        timerRefresh.Interval = 2000
        timerRefresh.Start()
    End Sub

    Private Sub txtMessage_Enter(sender As Object, e As EventArgs) Handles txtMessage.Enter
        If txtMessage.Text = texteParDefaut Then
            txtMessage.Text = ""
            txtMessage.ForeColor = Color.Black
        End If
    End Sub

    Private Sub txtMessage_Leave(sender As Object, e As EventArgs) Handles txtMessage.Leave
        If txtMessage.Text.Trim() = "" Then
            txtMessage.Text = texteParDefaut
            txtMessage.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub txtMessage_KeyDown(sender As Object, e As KeyEventArgs) Handles txtMessage.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            btnEnvoyer.PerformClick()
        End If
    End Sub

    Private Sub lstUtilisateurs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstUtilisateurs.SelectedIndexChanged
        If lstUtilisateurs.SelectedItem Is Nothing Then Exit Sub

        Dim texteSelectionne As String = lstUtilisateurs.SelectedItem.ToString()
        destinataireActuel = texteSelectionne
        destinataireNomPur = ExtraireNomUtilisateur(texteSelectionne)

        ChargerDiscussion()
    End Sub

    Private Function ExtraireNomUtilisateur(texte As String) As String
        If texte.Contains(" (") Then
            Return texte.Substring(0, texte.IndexOf(" ("))
        End If

        Return texte
    End Function

    Private Sub btnEnvoyer_Click(sender As Object, e As EventArgs) Handles btnEnvoyer.Click
        Dim contenu As String = txtMessage.Text.Trim()

        If destinataireNomPur = "" Then
            MessageBox.Show("Veuillez sélectionner un utilisateur.")
            Exit Sub
        End If

        If contenu = "" Or contenu = texteParDefaut Then
            MessageBox.Show("Veuillez écrire un message.")
            Exit Sub
        End If

        If DatabaseHelper.EnregistrerMessage(NomUtilisateur, destinataireNomPur, contenu) Then
            gestionClient.EnvoyerMessage(NomUtilisateur, destinataireNomPur, contenu)

            txtMessage.Clear()
            txtMessage.Text = texteParDefaut
            txtMessage.ForeColor = Color.Gray

            ChargerDiscussion()
        Else
            MessageBox.Show("Le message n'a pas été enregistré.")
        End If
    End Sub

    Private Sub ChargerDiscussion()
        rtbDiscussion.Clear()

        If destinataireNomPur = "" Then Exit Sub

        Dim table As DataTable = DatabaseHelper.ObtenirMessages(NomUtilisateur, destinataireNomPur)

        For Each row As DataRow In table.Rows
            Dim expediteur As String = row("Expediteur").ToString()
            Dim contenu As String = row("Contenu").ToString()
            Dim dateEnvoi As DateTime = Convert.ToDateTime(row("DateEnvoi"))

            If expediteur = NomUtilisateur Then
                rtbDiscussion.AppendText("Moi (" & dateEnvoi.ToString("HH:mm") & ") :" & Environment.NewLine)
                rtbDiscussion.AppendText("   " & contenu & Environment.NewLine & Environment.NewLine)
            Else
                rtbDiscussion.AppendText(expediteur & " (" & dateEnvoi.ToString("HH:mm") & ") :" & Environment.NewLine)
                rtbDiscussion.AppendText("   " & contenu & Environment.NewLine & Environment.NewLine)
            End If
        Next

        rtbDiscussion.SelectionStart = rtbDiscussion.TextLength
        rtbDiscussion.ScrollToCaret()
    End Sub

    Private Sub AfficherMessageRecu(expediteur As String, contenu As String)
        If Me.InvokeRequired Then
            Me.Invoke(Sub() AfficherMessageRecu(expediteur, contenu))
        Else
            SystemSounds.Asterisk.Play()

            If expediteur = destinataireNomPur Then
                ChargerDiscussion()
            Else
                MessageBox.Show("Nouveau message de " & expediteur)
            End If
        End If
    End Sub

    Private Sub MettreAJourListeUtilisateurs(utilisateurs As List(Of String))
        If Me.InvokeRequired Then
            Me.Invoke(Sub() MettreAJourListeUtilisateurs(utilisateurs))
        Else
            Dim ancienneSelection As String = destinataireNomPur
            Dim table As DataTable = DatabaseHelper.ObtenirUtilisateursAvecStatut()

            lstUtilisateurs.Items.Clear()

            For Each row As DataRow In table.Rows
                Dim nom As String = row("NomUtilisateur").ToString()
                Dim statut As String = row("Statut").ToString()

                If nom <> NomUtilisateur Then
                    lstUtilisateurs.Items.Add(nom & " (" & statut & ")")
                End If
            Next

            If ancienneSelection <> "" Then
                For Each item As Object In lstUtilisateurs.Items
                    Dim nomPur As String = ExtraireNomUtilisateur(item.ToString())
                    If nomPur = ancienneSelection Then
                        lstUtilisateurs.SelectedItem = item
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub AfficherInformation(message As String)
        If Me.InvokeRequired Then
            Me.Invoke(Sub() AfficherInformation(message))
        Else
            rtbDiscussion.AppendText(message & Environment.NewLine)
        End If
    End Sub

    Private Sub GererConnexionPerdue()
        If Me.InvokeRequired Then
            Me.Invoke(Sub() GererConnexionPerdue())
        Else
            MessageBox.Show("Connexion au serveur perdue.")
        End If
    End Sub

    Private Sub timerRefresh_Tick(sender As Object, e As EventArgs) Handles timerRefresh.Tick
        If destinataireNomPur <> "" Then
            ChargerDiscussion()
        End If
    End Sub

    Private Sub btnDeconnexion_Click(sender As Object, e As EventArgs) Handles btnDeconnexion.Click
        Try
            timerRefresh.Stop()
            DatabaseHelper.MettreStatutUtilisateur(NomUtilisateur, "Hors ligne")
            gestionClient.Deconnecter(NomUtilisateur)
        Catch ex As Exception
        End Try

        Dim f As New FrmConnexion()
        f.Show()
        Me.Close()
    End Sub

    Private Sub FrmMessagerie_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            timerRefresh.Stop()
            DatabaseHelper.MettreStatutUtilisateur(NomUtilisateur, "Hors ligne")
            gestionClient.Deconnecter(NomUtilisateur)
        Catch ex As Exception
        End Try
    End Sub

End Class