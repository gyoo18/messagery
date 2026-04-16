<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmInscription
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        lblIdentifiant = New Label()
        lblMotDePasse = New Label()
        lblConfirmation = New Label()
        txtIdentifiant = New TextBox()
        txtMotDePasse = New TextBox()
        txtConfirmation = New TextBox()
        btnEnregistrer = New Button()
        btnRetour = New Button()
        txtNomAffichage = New TextBox()
        Label1 = New Label()
        txtServeur = New TextBox()
        Label2 = New Label()
        SuspendLayout()
        ' 
        ' lblIdentifiant
        ' 
        lblIdentifiant.AutoSize = True
        lblIdentifiant.Font = New Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblIdentifiant.Location = New Point(33, 25)
        lblIdentifiant.Margin = New Padding(2, 0, 2, 0)
        lblIdentifiant.Name = "lblIdentifiant"
        lblIdentifiant.Size = New Size(63, 15)
        lblIdentifiant.TabIndex = 0
        lblIdentifiant.Text = "Identifiant"
        ' 
        ' lblMotDePasse
        ' 
        lblMotDePasse.AutoSize = True
        lblMotDePasse.Font = New Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblMotDePasse.Location = New Point(33, 147)
        lblMotDePasse.Margin = New Padding(2, 0, 2, 0)
        lblMotDePasse.Name = "lblMotDePasse"
        lblMotDePasse.Size = New Size(83, 15)
        lblMotDePasse.TabIndex = 1
        lblMotDePasse.Text = "Mot de passe"
        ' 
        ' lblConfirmation
        ' 
        lblConfirmation.AutoSize = True
        lblConfirmation.Font = New Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblConfirmation.Location = New Point(33, 187)
        lblConfirmation.Margin = New Padding(2, 0, 2, 0)
        lblConfirmation.Name = "lblConfirmation"
        lblConfirmation.Size = New Size(144, 15)
        lblConfirmation.TabIndex = 2
        lblConfirmation.Text = "Confirmer mot de passe"
        ' 
        ' txtIdentifiant
        ' 
        txtIdentifiant.Location = New Point(236, 23)
        txtIdentifiant.Margin = New Padding(2)
        txtIdentifiant.Name = "txtIdentifiant"
        txtIdentifiant.Size = New Size(172, 23)
        txtIdentifiant.TabIndex = 3
        ' 
        ' txtMotDePasse
        ' 
        txtMotDePasse.Location = New Point(236, 145)
        txtMotDePasse.Margin = New Padding(2)
        txtMotDePasse.Name = "txtMotDePasse"
        txtMotDePasse.Size = New Size(172, 23)
        txtMotDePasse.TabIndex = 6
        ' 
        ' txtConfirmation
        ' 
        txtConfirmation.Location = New Point(236, 187)
        txtConfirmation.Margin = New Padding(2)
        txtConfirmation.Name = "txtConfirmation"
        txtConfirmation.Size = New Size(172, 23)
        txtConfirmation.TabIndex = 7
        ' 
        ' btnEnregistrer
        ' 
        btnEnregistrer.BackColor = Color.ForestGreen
        btnEnregistrer.Font = New Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnEnregistrer.ForeColor = SystemColors.Control
        btnEnregistrer.Location = New Point(40, 259)
        btnEnregistrer.Margin = New Padding(2)
        btnEnregistrer.Name = "btnEnregistrer"
        btnEnregistrer.Size = New Size(155, 45)
        btnEnregistrer.TabIndex = 8
        btnEnregistrer.Text = "Enregistrer"
        btnEnregistrer.UseVisualStyleBackColor = False
        ' 
        ' btnRetour
        ' 
        btnRetour.Location = New Point(236, 259)
        btnRetour.Margin = New Padding(2)
        btnRetour.Name = "btnRetour"
        btnRetour.Size = New Size(158, 45)
        btnRetour.TabIndex = 9
        btnRetour.Text = "Retour"
        btnRetour.UseVisualStyleBackColor = True
        ' 
        ' txtNomAffichage
        ' 
        txtNomAffichage.Location = New Point(236, 64)
        txtNomAffichage.Margin = New Padding(2)
        txtNomAffichage.Name = "txtNomAffichage"
        txtNomAffichage.Size = New Size(172, 23)
        txtNomAffichage.TabIndex = 4
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(33, 66)
        Label1.Margin = New Padding(2, 0, 2, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(99, 15)
        Label1.TabIndex = 8
        Label1.Text = "Nom d'affichage"
        ' 
        ' txtServeur
        ' 
        txtServeur.Location = New Point(236, 101)
        txtServeur.Margin = New Padding(2)
        txtServeur.Name = "txtServeur"
        txtServeur.Size = New Size(172, 23)
        txtServeur.TabIndex = 5
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(33, 103)
        Label2.Margin = New Padding(2, 0, 2, 0)
        Label2.Name = "Label2"
        Label2.Size = New Size(52, 15)
        Label2.TabIndex = 10
        Label2.Text = "Serveur"
        ' 
        ' FrmInscription
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(484, 317)
        Controls.Add(txtServeur)
        Controls.Add(Label2)
        Controls.Add(txtNomAffichage)
        Controls.Add(Label1)
        Controls.Add(btnRetour)
        Controls.Add(btnEnregistrer)
        Controls.Add(txtConfirmation)
        Controls.Add(txtMotDePasse)
        Controls.Add(txtIdentifiant)
        Controls.Add(lblConfirmation)
        Controls.Add(lblMotDePasse)
        Controls.Add(lblIdentifiant)
        Margin = New Padding(2)
        Name = "FrmInscription"
        Text = "FrmInscription"
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents lblIdentifiant As Label
    Friend WithEvents lblMotDePasse As Label
    Friend WithEvents lblConfirmation As Label
    Friend WithEvents txtIdentifiant As TextBox
    Friend WithEvents txtMotDePasse As TextBox
    Friend WithEvents txtConfirmation As TextBox
    Friend WithEvents btnEnregistrer As Button
    Friend WithEvents btnRetour As Button
    Friend WithEvents txtNomAffichage As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtServeur As TextBox
    Friend WithEvents Label2 As Label
End Class
