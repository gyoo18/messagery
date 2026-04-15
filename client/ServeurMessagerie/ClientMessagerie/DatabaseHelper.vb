Imports System.Data
Imports System.Data.SqlClient

Public Class DatabaseHelper

    Private Shared connectionString As String = "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MessagerieDB;Integrated Security=True"

    Public Shared Function InscrireUtilisateur(nom As String, motdepasse As String) As Boolean
        Try
            Using con As New SqlConnection(connectionString)
                con.Open()

                Dim verifierReq As String = "SELECT COUNT(*) FROM Utilisateurs WHERE NomUtilisateur = @nom"

                Using cmdVerif As New SqlCommand(verifierReq, con)
                    cmdVerif.Parameters.AddWithValue("@nom", nom)

                    Dim existe As Integer = Convert.ToInt32(cmdVerif.ExecuteScalar())

                    If existe > 0 Then
                        MessageBox.Show("Nom d'utilisateur déjà existant.")
                        Return False
                    End If
                End Using

                Dim req As String = "INSERT INTO Utilisateurs (NomUtilisateur, MotDePasse, Statut) VALUES (@nom, @mdp, 'Hors ligne')"

                Using cmd As New SqlCommand(req, con)
                    cmd.Parameters.AddWithValue("@nom", nom)
                    cmd.Parameters.AddWithValue("@mdp", motdepasse)
                    Return cmd.ExecuteNonQuery() > 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur inscription : " & ex.Message)
            Return False
        End Try
    End Function

    Public Shared Function VerifierConnexion(nom As String, motdepasse As String) As Boolean
        Try
            Using con As New SqlConnection(connectionString)
                con.Open()

                Dim req As String = "SELECT COUNT(*) FROM Utilisateurs WHERE NomUtilisateur = @nom AND MotDePasse = @mdp"

                Using cmd As New SqlCommand(req, con)
                    cmd.Parameters.AddWithValue("@nom", nom)
                    cmd.Parameters.AddWithValue("@mdp", motdepasse)

                    Dim resultat As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return resultat > 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur connexion : " & ex.Message)
            Return False
        End Try
    End Function

    Public Shared Sub MettreStatutUtilisateur(nom As String, statut As String)
        Try
            Using con As New SqlConnection(connectionString)
                con.Open()

                Dim req As String = "UPDATE Utilisateurs SET Statut = @statut WHERE NomUtilisateur = @nom"

                Using cmd As New SqlCommand(req, con)
                    cmd.Parameters.AddWithValue("@statut", statut)
                    cmd.Parameters.AddWithValue("@nom", nom)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur statut : " & ex.Message)
        End Try
    End Sub

    Public Shared Function EnregistrerMessage(expediteur As String, destinataire As String, contenu As String) As Boolean
        Try
            Using con As New SqlConnection(connectionString)
                con.Open()

                Dim req As String = "INSERT INTO Messages (Expediteur, Destinataire, Contenu, DateEnvoi) VALUES (@exp, @dest, @cont, GETDATE())"

                Using cmd As New SqlCommand(req, con)
                    cmd.Parameters.AddWithValue("@exp", expediteur)
                    cmd.Parameters.AddWithValue("@dest", destinataire)
                    cmd.Parameters.AddWithValue("@cont", contenu)
                    Return cmd.ExecuteNonQuery() > 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur message : " & ex.Message)
            Return False
        End Try
    End Function

    Public Shared Function ObtenirUtilisateurs(saufUtilisateur As String) As List(Of String)
        Dim liste As New List(Of String)

        Try
            Using con As New SqlConnection(connectionString)
                con.Open()

                Dim req As String = "SELECT NomUtilisateur FROM Utilisateurs WHERE NomUtilisateur <> @nom ORDER BY NomUtilisateur"

                Using cmd As New SqlCommand(req, con)
                    cmd.Parameters.AddWithValue("@nom", saufUtilisateur)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            liste.Add(reader("NomUtilisateur").ToString())
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur chargement utilisateurs : " & ex.Message)
        End Try

        Return liste
    End Function

    Public Shared Function ObtenirUtilisateursAvecStatut() As DataTable
        Dim table As New DataTable()

        Try
            Using con As New SqlConnection(connectionString)
                con.Open()

                Dim req As String = "SELECT NomUtilisateur, Statut FROM Utilisateurs ORDER BY NomUtilisateur"

                Using cmd As New SqlCommand(req, con)
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(table)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur utilisateurs : " & ex.Message)
        End Try

        Return table
    End Function

    Public Shared Function ObtenirMessages(utilisateur1 As String, utilisateur2 As String) As DataTable
        Dim table As New DataTable()

        Try
            Using con As New SqlConnection(connectionString)
                con.Open()

                Dim req As String =
                    "SELECT Expediteur, Destinataire, Contenu, DateEnvoi " &
                    "FROM Messages " &
                    "WHERE (Expediteur = @u1 AND Destinataire = @u2) " &
                    "   OR (Expediteur = @u2 AND Destinataire = @u1) " &
                    "ORDER BY DateEnvoi ASC"

                Using cmd As New SqlCommand(req, con)
                    cmd.Parameters.AddWithValue("@u1", utilisateur1)
                    cmd.Parameters.AddWithValue("@u2", utilisateur2)

                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(table)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur chargement messages : " & ex.Message)
        End Try

        Return table
    End Function

End Class