Public Class FrmCréerConversation
    Private communication As Communication

    ' Dictionnaire des Controls des contacts à choisir,[<nom_id@serveur>:BoîteChoixContact]
    Private choixContacts As Dictionary(Of String, BoîteChoixContact) = New Dictionary(Of String, BoîteChoixContact)
    ' Lorsqu'un contact est choisi, il doit être retiré de la liste, mais pour éviter de toujours les recréer, on les mets ici.
    Private choixContactsChoisis As Dictionary(Of String, BoîteChoixContact) = New Dictionary(Of String, BoîteChoixContact)
    ' Dictionnaire des Controls des contacts choisis, [<nom_id@serveur>:BoîteContactChoisis]
    Private contactsChoisis As Dictionary(Of String, BoîteContactChoisis) = New Dictionary(Of String, BoîteContactChoisis)
    ' Lorsqu'un contact est retiré de la liste des choisis, il faut le retirer, mais pour éviter de toujours les recréer, on les mets ici.
    Private contactsChoisisRetirés As Dictionary(Of String, BoîteContactChoisis) = New Dictionary(Of String, BoîteContactChoisis)

    Public Sub New(ByRef com As Communication)
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.communication = com
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each c As Contact In État.contacts.Values
            Dim bcc = New BoîteChoixContact(c, New Action(Of Contact)(
                Sub(contact As Contact)
                    Me.ajouterContact(contact)
                End Sub
            ))
            Me.choixContacts(c.nom_id & "@" & c.serveur) = bcc
            Me.conteneurContacts.Controls.Add(bcc.conteneur)
        Next
    End Sub

    Private Sub ajouterContact(contact As Contact)
        Dim id As String = contact.nom_id & "@" & contact.serveur
        Dim contactChoix As BoîteChoixContact = Me.choixContacts(id)
        Me.choixContacts.Remove(id)
        Me.choixContactsChoisis(id) = contactChoix
        Me.conteneurContacts.Controls.Remove(contactChoix.conteneur)

        If Not Me.contactsChoisisRetirés.Keys.Contains(id) Then
            Me.contactsChoisisRetirés(id) = New BoîteContactChoisis(contact, New Action(Of Contact)(
                Sub(c As Contact)
                    Me.retirerContact(c)
                End Sub
            ))
        End If

        Dim contactChoisi As BoîteContactChoisis = Me.contactsChoisisRetirés(id)
        Me.contactsChoisisRetirés.Remove(id)
        Me.contactsChoisis(id) = contactChoisi
        Me.conteneurContactsChoisis.Controls.Add(contactChoisi.conteneur)
    End Sub

    Private Sub retirerContact(contact As Contact)
        Dim id As String = contact.nom_id & "@" & contact.serveur
        Dim contactChoix As BoîteChoixContact = Me.choixContactsChoisis(id)
        Me.choixContactsChoisis.Remove(id)
        Me.choixContacts(id) = contactChoix
        Me.conteneurContacts.Controls.Add(contactChoix.conteneur)

        Dim contactChoisi As BoîteContactChoisis = Me.contactsChoisis(id)
        Me.contactsChoisis.Remove(id)
        Me.contactsChoisisRetirés(id) = contactChoisi
        Me.conteneurContactsChoisis.Controls.Remove(contactChoisi.conteneur)
    End Sub
End Class