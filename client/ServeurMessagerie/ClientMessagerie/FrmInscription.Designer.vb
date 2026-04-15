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
        Me.lblNom = New System.Windows.Forms.Label()
        Me.lblMotDePasse = New System.Windows.Forms.Label()
        Me.lblConfirmation = New System.Windows.Forms.Label()
        Me.txtNom = New System.Windows.Forms.TextBox()
        Me.txtMotDePasse = New System.Windows.Forms.TextBox()
        Me.txtConfirmation = New System.Windows.Forms.TextBox()
        Me.btnEnregistrer = New System.Windows.Forms.Button()
        Me.btnRetour = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblNom
        '
        Me.lblNom.AutoSize = True
        Me.lblNom.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNom.Location = New System.Drawing.Point(57, 42)
        Me.lblNom.Name = "lblNom"
        Me.lblNom.Size = New System.Drawing.Size(209, 29)
        Me.lblNom.TabIndex = 0
        Me.lblNom.Text = "Nom d'utilisateur"
        '
        'lblMotDePasse
        '
        Me.lblMotDePasse.AutoSize = True
        Me.lblMotDePasse.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMotDePasse.Location = New System.Drawing.Point(57, 102)
        Me.lblMotDePasse.Name = "lblMotDePasse"
        Me.lblMotDePasse.Size = New System.Drawing.Size(166, 29)
        Me.lblMotDePasse.TabIndex = 1
        Me.lblMotDePasse.Text = "Mot de passe"
        '
        'lblConfirmation
        '
        Me.lblConfirmation.AutoSize = True
        Me.lblConfirmation.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConfirmation.Location = New System.Drawing.Point(57, 169)
        Me.lblConfirmation.Name = "lblConfirmation"
        Me.lblConfirmation.Size = New System.Drawing.Size(287, 29)
        Me.lblConfirmation.TabIndex = 2
        Me.lblConfirmation.Text = "Confirmer mot de passe"
        '
        'txtNom
        '
        Me.txtNom.Location = New System.Drawing.Point(405, 39)
        Me.txtNom.Name = "txtNom"
        Me.txtNom.Size = New System.Drawing.Size(292, 31)
        Me.txtNom.TabIndex = 3
        '
        'txtMotDePasse
        '
        Me.txtMotDePasse.Location = New System.Drawing.Point(405, 99)
        Me.txtMotDePasse.Name = "txtMotDePasse"
        Me.txtMotDePasse.Size = New System.Drawing.Size(292, 31)
        Me.txtMotDePasse.TabIndex = 4
        '
        'txtConfirmation
        '
        Me.txtConfirmation.Location = New System.Drawing.Point(405, 169)
        Me.txtConfirmation.Name = "txtConfirmation"
        Me.txtConfirmation.Size = New System.Drawing.Size(292, 31)
        Me.txtConfirmation.TabIndex = 5
        '
        'btnEnregistrer
        '
        Me.btnEnregistrer.BackColor = System.Drawing.Color.ForestGreen
        Me.btnEnregistrer.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEnregistrer.ForeColor = System.Drawing.SystemColors.Control
        Me.btnEnregistrer.Location = New System.Drawing.Point(68, 288)
        Me.btnEnregistrer.Name = "btnEnregistrer"
        Me.btnEnregistrer.Size = New System.Drawing.Size(265, 75)
        Me.btnEnregistrer.TabIndex = 6
        Me.btnEnregistrer.Text = "Enregistrer"
        Me.btnEnregistrer.UseVisualStyleBackColor = False
        '
        'btnRetour
        '
        Me.btnRetour.Location = New System.Drawing.Point(405, 288)
        Me.btnRetour.Name = "btnRetour"
        Me.btnRetour.Size = New System.Drawing.Size(271, 75)
        Me.btnRetour.TabIndex = 7
        Me.btnRetour.Text = "Retour"
        Me.btnRetour.UseVisualStyleBackColor = True
        '
        'FrmInscription
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(830, 430)
        Me.Controls.Add(Me.btnRetour)
        Me.Controls.Add(Me.btnEnregistrer)
        Me.Controls.Add(Me.txtConfirmation)
        Me.Controls.Add(Me.txtMotDePasse)
        Me.Controls.Add(Me.txtNom)
        Me.Controls.Add(Me.lblConfirmation)
        Me.Controls.Add(Me.lblMotDePasse)
        Me.Controls.Add(Me.lblNom)
        Me.Name = "FrmInscription"
        Me.Text = "FrmInscription"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblNom As Label
    Friend WithEvents lblMotDePasse As Label
    Friend WithEvents lblConfirmation As Label
    Friend WithEvents txtNom As TextBox
    Friend WithEvents txtMotDePasse As TextBox
    Friend WithEvents txtConfirmation As TextBox
    Friend WithEvents btnEnregistrer As Button
    Friend WithEvents btnRetour As Button
End Class
