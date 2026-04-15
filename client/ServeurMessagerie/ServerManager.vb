Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

Public Class ServerManager

    Private listener As TcpListener
    Private serveurActif As Boolean = False
    Private clients As New Dictionary(Of String, TcpClient)

    Public Event LogAjoute(message As String)
    Public Event ListeClientsMiseAJour(noms As List(Of String))

    Public Sub Demarrer(port As Integer)
        listener = New TcpListener(IPAddress.Any, port)
        listener.Start()
        serveurActif = True

        RaiseEvent LogAjoute("Serveur démarré sur le port " & port)

        Dim threadEcoute As New Thread(AddressOf AccepterClients)
        threadEcoute.IsBackground = True
        threadEcoute.Start()
    End Sub

    Public Sub Arreter()
        serveurActif = False

        Try
            listener.Stop()
        Catch ex As Exception
        End Try

        RaiseEvent LogAjoute("Serveur arrêté.")
    End Sub

    Private Sub AccepterClients()
        While serveurActif
            Try
                Dim client As TcpClient = listener.AcceptTcpClient()

                Dim t As New Thread(Sub() GererClient(client))
                t.IsBackground = True
                t.Start()
            Catch ex As Exception
            End Try
        End While
    End Sub

    Private Sub GererClient(client As TcpClient)
        Try
            Dim stream As NetworkStream = client.GetStream()
            Dim buffer(4096) As Byte

            While True
                Dim bytesRead As Integer = stream.Read(buffer, 0, buffer.Length)
                If bytesRead <= 0 Then Exit While

                Dim message As String = Encoding.UTF8.GetString(buffer, 0, bytesRead)
                TraiterMessage(client, message)
            End While
        Catch ex As Exception
        End Try
    End Sub

    Private Sub TraiterMessage(client As TcpClient, message As String)
        Dim parties() As String = message.Split("|"c)

        Select Case parties(0)

            Case "LOGIN"
                Dim nom As String = parties(1)

                If Not clients.ContainsKey(nom) Then
                    clients.Add(nom, client)
                    RaiseEvent LogAjoute(nom & " est connecté.")
                    EnvoyerListeUtilisateurs()
                    MettreAJourListeClients()
                End If

            Case "MSG"
                Dim expediteur As String = parties(1)
                Dim destinataire As String = parties(2)
                Dim contenu As String = parties(3)

                RaiseEvent LogAjoute(expediteur & " -> " & destinataire & " : " & contenu)

                If clients.ContainsKey(destinataire) Then
                    Dim clientDest As TcpClient = clients(destinataire)
                    Dim streamDest As NetworkStream = clientDest.GetStream()
                    Dim rep As String = "RECEIVE|" & expediteur & "|" & contenu
                    Dim data As Byte() = Encoding.UTF8.GetBytes(rep)
                    streamDest.Write(data, 0, data.Length)
                End If

            Case "LOGOUT"
                Dim nom As String = parties(1)

                If clients.ContainsKey(nom) Then
                    clients.Remove(nom)
                    RaiseEvent LogAjoute(nom & " s'est déconnecté.")
                    EnvoyerListeUtilisateurs()
                    MettreAJourListeClients()
                End If

        End Select
    End Sub

    Private Sub EnvoyerListeUtilisateurs()
        Dim liste As String = String.Join(",", clients.Keys)
        Dim message As String = "USERLIST|" & liste
        Dim data As Byte() = Encoding.UTF8.GetBytes(message)

        For Each c In clients.Values
            Try
                Dim stream As NetworkStream = c.GetStream()
                stream.Write(data, 0, data.Length)
            Catch ex As Exception
            End Try
        Next
    End Sub

    Private Sub MettreAJourListeClients()
        RaiseEvent ListeClientsMiseAJour(clients.Keys.ToList())
    End Sub

End Class