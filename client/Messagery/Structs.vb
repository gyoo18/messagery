Public Class Contact
    Public nom_id As String
    Public serveur As String
    Public nom_affichage As String
End Class

Public Class Message
    Public contenu As String
    Public date_publication As DateTime
    Public contact As Contact
End Class

Public Class Conversation
    Public ID As Integer
    Public contacts As List(Of Contact)
    Public messages As List(Of Message)
    Public est_lue As Boolean
End Class

Public Class État
    Public contacts As Dictionary(Of String, Contact)
    Public conversations As Dictionary(Of Integer, Conversation)
    Public session As String
End Class