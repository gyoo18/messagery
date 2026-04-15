Public Class FrmServeur

    Private gestionServeur As New ServerManager()

    Private Sub FrmServeur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AddHandler gestionServeur.LogAjoute, AddressOf AjouterLog
        AddHandler gestionServeur.ListeClientsMiseAJour, AddressOf ActualiserClients
    End Sub

    Private Sub btnDemarrer_Click(sender As Object, e As EventArgs) Handles btnDemarrer.Click
        Try
            gestionServeur.Demarrer(5000)
            btnDemarrer.Enabled = False
            btnArreter.Enabled = True
        Catch ex As Exception
            MessageBox.Show("Erreur au démarrage : " & ex.Message)
        End Try
    End Sub

    Private Sub btnArreter_Click(sender As Object, e As EventArgs) Handles btnArreter.Click
        gestionServeur.Arreter()
        btnDemarrer.Enabled = True
        btnArreter.Enabled = False
    End Sub

    Private Sub AjouterLog(message As String)
        If Me.InvokeRequired Then
            Me.Invoke(Sub() AjouterLog(message))
        Else
            lstLogs.Items.Add(DateTime.Now.ToString("HH:mm:ss") & " - " & message)
        End If
    End Sub

    Private Sub ActualiserClients(noms As List(Of String))
        If Me.InvokeRequired Then
            Me.Invoke(Sub() ActualiserClients(noms))
        Else
            lstClients.Items.Clear()

            For Each nom In noms
                lstClients.Items.Add(nom)
            Next
        End If
    End Sub

End Class
