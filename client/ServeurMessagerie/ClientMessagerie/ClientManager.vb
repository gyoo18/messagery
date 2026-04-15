Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

Public Class ClientManager

    Private client As TcpClient
    Private stream As NetworkStream
    Private threadReception As Thread

    Public Event MessageRecu(expediteur As String, contenu As String)
    Public Event ListeUtilisateursRecue(utilisateurs As List(Of String))
    Public Event InformationRecue(message As String)
    Public Event ConnexionPerdue()

    Public Function Connecter(ip As String, port As Integer, nomUtilisateur As String) As Boolean
        Try
            client = New TcpClient()
            client.Connect(ip, port)
            stream = client.GetStream()

            Envoyer("LOGIN|" & nomUtilisateur)

            threadReception = New Thread(AddressOf Ecouter)
            threadReception.IsBackground = True
            threadReception.Start()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub EnvoyerMessage(expediteur As String, destinataire As String, contenu As String)
        Envoyer("MSG|" & expediteur & "|" & destinataire & "|" & contenu)
    End Sub

    Public Sub Deconnecter(nomUtilisateur As String)
        Try
            Envoyer("LOGOUT|" & nomUtilisateur)
        Catch ex As Exception
        End Try

        Try
            If stream IsNot Nothing Then stream.Close()
        Catch ex As Exception
        End Try

        Try
            If client IsNot Nothing Then client.Close()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Envoyer(message As String)
        If stream Is Nothing Then Exit Sub

        Dim data As Byte() = Encoding.UTF8.GetBytes(message)
        stream.Write(data, 0, data.Length)
    End Sub

    Private Sub Ecouter()
        Try
            Dim buffer(4096) As Byte

            While True
                Dim bytesRead As Integer = stream.Read(buffer, 0, buffer.Length)

                If bytesRead <= 0 Then Exit While

                Dim message As String = Encoding.UTF8.GetString(buffer, 0, bytesRead)
                TraiterMessage(message)
            End While
        Catch ex As Exception
        End Try

        RaiseEvent ConnexionPerdue()
    End Sub

    Private Sub TraiterMessage(message As String)
        Dim parties() As String = message.Split("|"c)

        Select Case parties(0)

            Case "RECEIVE"
                If parties.Length >= 3 Then
                    Dim expediteur As String = parties(1)
                    Dim contenu As String = parties(2)
                    RaiseEvent MessageRecu(expediteur, contenu)
                End If

            Case "USERLIST"
                Dim liste As New List(Of String)

                If parties.Length >= 2 AndAlso parties(1).Trim() <> "" Then
                    Dim utilisateurs() As String = parties(1).Split(","c)

                    For Each u As String In utilisateurs
                        If u.Trim() <> "" Then
                            liste.Add(u.Trim())
                        End If
                    Next
                End If

                RaiseEvent ListeUtilisateursRecue(liste)

            Case Else
                RaiseEvent InformationRecue(message)

        End Select
    End Sub

End Class