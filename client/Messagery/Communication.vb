Imports System.Diagnostics.CodeAnalysis
Imports System.Diagnostics.Tracing
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json

Public Class Communication
    Private clientHttp As HttpClient
    Private authorizationHeader As Headers.AuthenticationHeaderValue
    Private jeton As String

    Public Async Function Inscrire(nom_id As String, serveur As String, nom_affichage As String, mot_de_passe As String) As Task(Of Boolean)
        Me.clientHttp = New HttpClient()
        Me.clientHttp.BaseAddress = New Uri("http://" & serveur)

        Try
            Dim requête = New HttpRequestMessage(HttpMethod.Post, "inscription")
            requête.Content = New StringContent(
                "{""nom_id"":""" & nom_id & """,""nom_affichage"":""" & nom_affichage & """,""mot_de_passe"":""" & mot_de_passe & """}")

            Dim réponse = Await Me.clientHttp.SendAsync(requête)
            If Not réponse.IsSuccessStatusCode Then
                MsgBox("Une erreur est survenue dans la communication avec le serveur" & vbCrLf & CStr(réponse.StatusCode) & ":" & réponse.ReasonPhrase)
                Return False
            End If
        Catch ex As Exception
            MsgBox("Une erreur est survenue dans la communication avec le serveur.")
            Console.WriteLine(ex.Message)
            Return False
        End Try

        Return True
    End Function

    Public Async Function Connecter(nom_id As String, serveur As String, mot_de_passe As String) As Task(Of String)
        Me.clientHttp = New HttpClient()
        Me.clientHttp.BaseAddress = New Uri("http://" & serveur)
        Dim requête = New HttpRequestMessage(HttpMethod.Get, "connection")
        requête.Headers.Authorization = New Headers.AuthenticationHeaderValue(
            "Basic", System.Convert.ToBase64String(
                Encoding.UTF8.GetBytes(nom_id & ":" & mot_de_passe)))

        Try
            Dim réponse As HttpResponseMessage = Await Me.clientHttp.SendAsync(requête)

            If Not réponse.IsSuccessStatusCode Then
                Console.WriteLine("HTTP GET /connecter : " & CStr(réponse.StatusCode) & ", " & réponse.ReasonPhrase)
                Return réponse.ReasonPhrase
            End If

            Dim json = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(Await réponse.Content.ReadAsStringAsync())
            If Not json.ContainsKey("jeton") Or
                Not json.ContainsKey("accepté") Or
                Not JsonSerializer.Deserialize(Of Boolean)(json("accepté")) Then

                Return "La réponse du serveur est mal formée."
            End If
            Me.jeton = JsonSerializer.Deserialize(Of String)(json("jeton"))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return "Une erreur est survenue lors de la communication avec le serveur."
        End Try

        Me.authorizationHeader = New Headers.AuthenticationHeaderValue("Bearer", Me.jeton)
        Me.clientHttp.DefaultRequestHeaders.Authorization = Me.authorizationHeader
        Return Nothing
    End Function

    Public Class SynchronisationConnectionRéponse
        Public contacts As Dictionary(Of String, Contact)
        Public conversations As Dictionary(Of Integer, Conversation)
    End Class

    Public Async Function Synchronisation_Connection() As Task(Of SynchronisationConnectionRéponse)
        Try
            Dim requête = New HttpRequestMessage(HttpMethod.Get, "synchronisation-connection")
            requête.Headers.Authorization = Me.authorizationHeader
            Dim réponse = Await Me.clientHttp.SendAsync(requête)

            If Not réponse.IsSuccessStatusCode Then
                MsgBox("Une erreur est survenue dans la communication avec le serveur" & vbCrLf & CStr(réponse.StatusCode) & ":" & réponse.ReasonPhrase)
                Return Nothing
            End If

            Dim json = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(Await réponse.Content.ReadAsStringAsync())

            ' Extraction des contacts
            If Not json.ContainsKey("contacts") Then
                MsgBox("La réponse du serveur est mal formée")
                Return Nothing
            End If
            Dim contacts_str As Dictionary(Of String, String)() = JsonSerializer.Deserialize(Of Dictionary(Of String, String)())(json("contacts"))

            Dim contacts = New Dictionary(Of String, Contact)(contacts_str.Length)
            Dim tmp As String()
            For i As Integer = 0 To contacts_str.Length - 1

                If Not contacts_str(i).ContainsKey("ID") Or
                    Not contacts_str(i).ContainsKey("nom") Or
                    Not Utilitaires.est_identifiant_valide(contacts_str(i)("ID")) Then

                    Console.WriteLine("L'un des contacts fournis par le serveur est invalide.")
                    Continue For
                End If

                tmp = contacts_str(i)("ID").Split("@")

                contacts(contacts_str(i)("ID")) = New Contact With {
                    .nom_id = tmp(0),
                    .serveur = tmp(1),
                    .nom_affichage = contacts_str(i)("nom")
                }
            Next i

            ' Extraction des conversations
            If Not json.ContainsKey("conversations") Then
                MsgBox("La réponse du serveur est mal formée")
                Return Nothing
            End If
            Dim conversations_str As Dictionary(Of String, Object)() = JsonSerializer.Deserialize(Of Dictionary(Of String, Object)())(json("conversations"))

            Dim conversations = New Dictionary(Of Integer, Conversation)(conversations_str.Length)
            Dim contacts_conversations_str As String()
            Dim contacts_conversations As List(Of Contact)
            Dim messages_str As Dictionary(Of String, Object)()
            Dim messages As List(Of Message)
            For i As Integer = 0 To conversations_str.Length - 1
                If Not conversations_str(i).ContainsKey("ID") Or
                        Not conversations_str(i).ContainsKey("contacts") Or
                        Not conversations_str(i).ContainsKey("messages") Then
                    Console.WriteLine("L'une des conversations est mal formée")
                    Continue For
                End If

                ' Extraction des contacts
                contacts_conversations_str = JsonSerializer.Deserialize(Of String())(conversations_str(i)("contacts"))
                contacts_conversations = New List(Of Contact)(contacts_conversations_str.Length)
                For j = 0 To contacts_conversations_str.Length - 1
                    If Not Utilitaires.est_identifiant_valide(contacts_conversations_str(j)) Or
                        Not contacts.ContainsKey(contacts_conversations_str(j)) Then

                        Console.WriteLine("L'un des contacts d'une des conversations est invalide")
                        Continue For
                    End If

                    contacts_conversations.Add(contacts(contacts_conversations_str(j)))
                Next j

                ' Extraction des messages
                messages_str = JsonSerializer.Deserialize(Of Dictionary(Of String, Object)())(conversations_str(i)("messages"))
                messages = New List(Of Message)(messages_str.Length)
                For j = 0 To messages_str.Length - 1
                    If Not messages_str(j).ContainsKey("contact") Or
                        Not messages_str(j).ContainsKey("date") Or
                        Not messages_str(j).ContainsKey("message") Or
                        Not Utilitaires.est_identifiant_valide(JsonSerializer.Deserialize(Of String)(messages_str(j)("contact"))) Or
                        Not contacts.ContainsKey(JsonSerializer.Deserialize(Of String)(messages_str(j)("contact"))) Then

                        Console.WriteLine("L'un des messages d'une des conversations est invalide")
                        Continue For
                    End If

                    messages.Add(New Message With {
                        .contact = contacts(JsonSerializer.Deserialize(Of String)(messages_str(j)("contact"))),
                        .date_publication = DateTime.Parse(JsonSerializer.Deserialize(Of String)(messages_str(j)("date"))),
                        .contenu = JsonSerializer.Deserialize(Of String)(messages_str(j)("message"))
                    })
                Next j

                conversations(JsonSerializer.Deserialize(Of Integer)(conversations_str(i)("ID"))) = New Conversation With {
                    .ID = JsonSerializer.Deserialize(Of Integer)(conversations_str(i)("ID")),
                    .contacts = contacts_conversations,
                    .messages = messages,
                    .est_lue = True
                }

            Next i

            ' Extraction des conversations non lues
            If Not json.ContainsKey("conversations-non-lues") Then
                MsgBox("La réponse du serveur est mal formée")
                Return Nothing
            End If
            Dim conversations_non_lues_str As String() = JsonSerializer.Deserialize(Of String())(json("conversations-non-lues"))

            For Each c In conversations_non_lues_str
                If Not conversations.ContainsKey(JsonSerializer.Deserialize(Of Integer)(c)) Then
                    Console.WriteLine("Le serveur a fournit une conversation non lue qui n'existe pas.")
                    Continue For
                End If

                conversations(JsonSerializer.Deserialize(Of Integer)(c)).est_lue = False
            Next

            Return New SynchronisationConnectionRéponse With {.contacts = contacts, .conversations = conversations}

        Catch ex As Exception
            MsgBox("Une erreur est survenue dans la communication avec le serveur.")
            Console.WriteLine(ex.Message)
            Return Nothing
        End Try

        Return Nothing
    End Function

    Public Class SynchronisationRéponse
        Public nouvelles_conversations As Dictionary(Of Integer, Conversation)
        Public nouveaux_messages As List(Of Tuple(Of Integer, Message))
    End Class

    Public Async Function synchronisation() As Task(Of SynchronisationRéponse)
        Try
            Dim requête = New HttpRequestMessage(HttpMethod.Post, "synchronisation")
            requête.Headers.Authorization = Me.authorizationHeader
            ' TODO Envoyer les bonnes informations
            requête.Content = New StringContent("{""conversations-lues"":[],""conversations-effacées"":[]}")
            Dim réponse = Await Me.clientHttp.SendAsync(requête)

            If Not réponse.IsSuccessStatusCode Then
                MsgBox("Une erreur est survenue dans la communication avec le serveur" & vbCrLf & CStr(réponse.StatusCode) & ":" & réponse.ReasonPhrase)
                Return Nothing
            End If

            Dim json = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(Await réponse.Content.ReadAsStringAsync())

            ' Extraction des conversations
            If Not json.ContainsKey("nouvelles-conversations") Then
                MsgBox("La réponse du serveur est mal formée")
                Return Nothing
            End If
            Dim conversations_str As Dictionary(Of String, Object)() = JsonSerializer.Deserialize(Of Dictionary(Of String, Object)())(json("nouvelles-conversations"))

            Dim conversations = New Dictionary(Of Integer, Conversation)(conversations_str.Length)
            Dim contacts_conversations_str As String()
            Dim contacts_conversations As List(Of Contact)
            Dim messages_str As Dictionary(Of String, Object)()
            Dim messages As List(Of Message)
            For i As Integer = 0 To conversations_str.Length - 1
                If Not conversations_str(i).ContainsKey("ID") Or
                        Not conversations_str(i).ContainsKey("contacts") Or
                        Not conversations_str(i).ContainsKey("messages") Then
                    Console.WriteLine("L'une des conversations est mal formée")
                    Continue For
                End If

                ' Extraction des contacts
                contacts_conversations_str = JsonSerializer.Deserialize(Of String())(conversations_str(i)("contacts"))
                contacts_conversations = New List(Of Contact)(contacts_conversations_str.Length)
                For j = 0 To contacts_conversations_str.Length - 1
                    If Not Utilitaires.est_identifiant_valide(contacts_conversations_str(j)) Or
                        Not État.contacts.ContainsKey(contacts_conversations_str(j)) Then

                        Console.WriteLine("L'un des contacts d'une des conversations est invalide")
                        Continue For
                    End If

                    contacts_conversations.Add(État.contacts(contacts_conversations_str(j)))
                Next j

                ' Extraction des messages
                messages_str = JsonSerializer.Deserialize(Of Dictionary(Of String, Object)())(conversations_str(i)("messages"))
                messages = New List(Of Message)(messages_str.Length)
                For j = 0 To messages_str.Length - 1
                    If Not messages_str(j).ContainsKey("contact") Or
                        Not messages_str(j).ContainsKey("date") Or
                        Not messages_str(j).ContainsKey("message") Or
                        Not Utilitaires.est_identifiant_valide(JsonSerializer.Deserialize(Of String)(messages_str(j)("contact"))) Or
                        Not État.contacts.ContainsKey(JsonSerializer.Deserialize(Of String)(messages_str(j)("contact"))) Then

                        Console.WriteLine("L'un des messages d'une des conversations est invalide")
                        Continue For
                    End If

                    messages.Add(New Message With {
                        .contact = État.contacts(JsonSerializer.Deserialize(Of String)(messages_str(j)("contact"))),
                        .date_publication = JsonSerializer.Deserialize(Of DateTime)(messages_str(j)("date")),
                        .contenu = JsonSerializer.Deserialize(Of String)(messages_str(j)("message"))
                    })
                Next j

                conversations(JsonSerializer.Deserialize(Of Integer)(conversations_str(i)("ID"))) = New Conversation With {
                    .ID = JsonSerializer.Deserialize(Of Integer)(conversations_str(i)("ID")),
                    .contacts = contacts_conversations,
                    .messages = messages,
                    .est_lue = True
                }

            Next i

            ' Extraction des nouveaux messages
            If Not json.ContainsKey("nouveaux-messages") Then
                MsgBox("La réponse du serveur est mal formée")
                Return Nothing
            End If

            Dim nouveaux_messages_str As Dictionary(Of String, Object)() = JsonSerializer.Deserialize(Of Dictionary(Of String, Object)())(json("nouveaux-messages"))
            Dim nouveaux_messages = New List(Of Tuple(Of Integer, Message))(nouveaux_messages_str.Length)
            For j = 0 To nouveaux_messages_str.Length - 1
                If Not nouveaux_messages_str(j).ContainsKey("contact") Or
                        Not nouveaux_messages_str(j).ContainsKey("date") Or
                        Not nouveaux_messages_str(j).ContainsKey("message") Or
                        Not nouveaux_messages_str(j).ContainsKey("conversation") Or
                        Not Utilitaires.est_identifiant_valide(JsonSerializer.Deserialize(Of String)(nouveaux_messages_str(j)("contact"))) Or
                        Not État.contacts.ContainsKey(JsonSerializer.Deserialize(Of String)(nouveaux_messages_str(j)("contact"))) Then

                    Console.WriteLine("L'un des messages d'une des conversations est invalide")
                    Continue For
                End If

                nouveaux_messages.Add(
                New Tuple(Of Integer, Message)(
                    JsonSerializer.Deserialize(Of Integer)(nouveaux_messages_str(j)("conversation")),
                    New Message With {
                        .contact = État.contacts(JsonSerializer.Deserialize(Of String)(nouveaux_messages_str(j)("contact"))),
                        .date_publication = DateTime.Parse(JsonSerializer.Deserialize(Of String)(nouveaux_messages_str(j)("date"))),
                        .contenu = JsonSerializer.Deserialize(Of String)(nouveaux_messages_str(j)("message"))
                    }))
            Next j


            Return New SynchronisationRéponse With {
                .nouvelles_conversations = conversations,
                .nouveaux_messages = nouveaux_messages
            }

        Catch ex As Exception
            MsgBox("Une erreur est survenue dans la communication avec le serveur.")
            Console.WriteLine(ex.Message)
            Return Nothing
        End Try

        Return Nothing
    End Function

    Public Async Sub envoyer_message(message As String, conversation_id As Integer)
        Try
            Dim requête = New HttpRequestMessage(HttpMethod.Post, "/message")
            requête.Headers.Authorization = Me.authorizationHeader

            requête.Content = New StringContent("{""conversation"":" & CStr(conversation_id) & ",""message"":""" & message & """}")
            Dim resp = Await Me.clientHttp.SendAsync(requête)
            resp.EnsureSuccessStatusCode()
        Catch ex As Exception
            MsgBox("Une erreur est survenue lors de l'envoie au serveur.")
        End Try
    End Sub

    Public Async Sub deconnecter()
        If État.session Is Nothing Then
            Exit Sub
        End If

        Try
            Dim requête = New HttpRequestMessage(HttpMethod.Post, "/deconnection")
            requête.Headers.Authorization = Me.authorizationHeader

            Dim resp = Await Me.clientHttp.SendAsync(requête)
            resp.EnsureSuccessStatusCode()
        Catch ex As Exception
            MsgBox("Une erreur est survenue lors de l'envoie au serveur.")
        End Try
    End Sub
End Class
