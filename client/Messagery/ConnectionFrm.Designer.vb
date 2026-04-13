<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConnectionFrm
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Label1 = New Label()
        NomUtilisateur = New TextBox()
        Label2 = New Label()
        Erreur = New Label()
        MotDePasse = New TextBox()
        BtnConnecter = New Button()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Arial Black", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(112, 79)
        Label1.Name = "Label1"
        Label1.Size = New Size(190, 27)
        Label1.TabIndex = 0
        Label1.Text = "Nom d'utilisateur"
        ' 
        ' NomUtilisateur
        ' 
        NomUtilisateur.Font = New Font("Arial Black", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        NomUtilisateur.Location = New Point(112, 118)
        NomUtilisateur.Name = "NomUtilisateur"
        NomUtilisateur.Size = New Size(190, 34)
        NomUtilisateur.TabIndex = 1
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Arial Black", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(112, 167)
        Label2.Name = "Label2"
        Label2.Size = New Size(152, 27)
        Label2.TabIndex = 2
        Label2.Text = "Mot de passe"
        ' 
        ' Erreur
        ' 
        Erreur.AutoSize = True
        Erreur.ForeColor = Color.Red
        Erreur.Location = New Point(112, 48)
        Erreur.Name = "Erreur"
        Erreur.Size = New Size(41, 15)
        Erreur.TabIndex = 5
        Erreur.Text = "Label3"
        ' 
        ' MotDePasse
        ' 
        MotDePasse.Font = New Font("Arial Black", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        MotDePasse.Location = New Point(112, 206)
        MotDePasse.Name = "MotDePasse"
        MotDePasse.PasswordChar = "*"c
        MotDePasse.Size = New Size(190, 34)
        MotDePasse.TabIndex = 3
        ' 
        ' BtnConnecter
        ' 
        BtnConnecter.Font = New Font("Arial Black", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        BtnConnecter.Location = New Point(112, 257)
        BtnConnecter.Name = "BtnConnecter"
        BtnConnecter.Size = New Size(190, 42)
        BtnConnecter.TabIndex = 6
        BtnConnecter.Text = "Se Connecter"
        BtnConnecter.UseVisualStyleBackColor = True
        ' 
        ' ConnectionFrm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(425, 341)
        Controls.Add(BtnConnecter)
        Controls.Add(Erreur)
        Controls.Add(MotDePasse)
        Controls.Add(Label2)
        Controls.Add(NomUtilisateur)
        Controls.Add(Label1)
        Name = "ConnectionFrm"
        Text = "Form2"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents NomUtilisateur As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Erreur As Label
    Friend WithEvents MotDePasse As TextBox
    Friend WithEvents BtnConnecter As Button
End Class
