Imports System.Text.RegularExpressions

Module Utilitaires
    Public Function est_identifiant_valide(identifiant As String) As Boolean
        ' Format des identifiants : <nom>@<domaine1>.<domaine2>[.<domaine3>...](:<port>)?
        ' <nom> peut contenir [a-z0-9_-] (la casse n'est pas discriminée), mais ne peut 
        '   ni commencer, ni finir par [_|-]
        ' <domaine> est une liste de noms [a-z0-9_-] séparés par des points : 
        '   ex. : domaine.qc.ca
        ' <port> est un entier optionnel.
        ' Chaque nom doit commencer et se terminer par [a-zA-Z0-9]
        ' Le domaine ne peut ni commencer ni se terminer par un point
        ' Il doit y avoir au moins deux noms dans le domaine
        '
        ' Le regex est essentiellement composé de trois fois le groupe
        '   [a-z0-9]([\w-]*[a-z0-9])?
        ' Répartis comme ceci : <grp>@<grp>(\.<grp>)+
        ' Auquel On ajoute le port : (:[0-9]{2,5})?
        Return New Regex("^[a-z0-9]([\w-]*[a-z0-9])?@([a-z0-9]([\w-]*[a-z0-9])?(\.[a-z0-9]([\w-]*[a-z0-9])?)+|localhost)(:[0-9]{2,5})?$").Match(identifiant).Success
    End Function
End Module
