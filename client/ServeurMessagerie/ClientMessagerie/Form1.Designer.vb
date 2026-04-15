<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmConnexion
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
        Me.lblNom = New System.Windows.Forms.Label()
        Me.lblMotDePasse = New System.Windows.Forms.Label()
        Me.txtNom = New System.Windows.Forms.TextBox()
        Me.txtMotDePasse = New System.Windows.Forms.TextBox()
        Me.btnConnexion = New System.Windows.Forms.Button()
        Me.btnInscription = New System.Windows.Forms.Button()
        Me.btnQuitter = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblNom
        '
        Me.lblNom.AutoSize = True
        Me.lblNom.Font = New System.Drawing.Font("Arial", 10.125!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNom.Location = New System.Drawing.Point(47, 58)
        Me.lblNom.Name = "lblNom"
        Me.lblNom.Size = New System.Drawing.Size(240, 32)
        Me.lblNom.TabIndex = 0
        Me.lblNom.Text = "Nom d'utilisateur"
        '
        'lblMotDePasse
        '
        Me.lblMotDePasse.AutoSize = True
        Me.lblMotDePasse.Font = New System.Drawing.Font("Arial", 10.125!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMotDePasse.Location = New System.Drawing.Point(59, 147)
        Me.lblMotDePasse.Name = "lblMotDePasse"
        Me.lblMotDePasse.Size = New System.Drawing.Size(186, 32)
        Me.lblMotDePasse.TabIndex = 1
        Me.lblMotDePasse.Text = "Mot de passe"
        '
        'txtNom
        '
        Me.txtNom.Location = New System.Drawing.Point(321, 58)
        Me.txtNom.Name = "txtNom"
        Me.txtNom.Size = New System.Drawing.Size(326, 31)
        Me.txtNom.TabIndex = 2
        '
        'txtMotDePasse
        '
        Me.txtMotDePasse.Location = New System.Drawing.Point(321, 147)
        Me.txtMotDePasse.Name = "txtMotDePasse"
        Me.txtMotDePasse.Size = New System.Drawing.Size(326, 31)
        Me.txtMotDePasse.TabIndex = 3
        Me.txtMotDePasse.UseSystemPasswordChar = True
        '
        'btnConnexion
        '
        Me.btnConnexion.BackColor = System.Drawing.Color.ForestGreen
        Me.btnConnexion.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnConnexion.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.btnConnexion.Location = New System.Drawing.Point(65, 260)
        Me.btnConnexion.Name = "btnConnexion"
        Me.btnConnexion.Size = New System.Drawing.Size(191, 99)
        Me.btnConnexion.TabIndex = 4
        Me.btnConnexion.Text = "Se connecter"
        Me.btnConnexion.UseVisualStyleBackColor = False
        '
        'btnInscription
        '
        Me.btnInscription.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInscription.ForeColor = System.Drawing.SystemColors.InactiveCaptionText
        Me.btnInscription.Location = New System.Drawing.Point(298, 260)
        Me.btnInscription.Name = "btnInscription"
        Me.btnInscription.Size = New System.Drawing.Size(189, 99)
        Me.btnInscription.TabIndex = 5
        Me.btnInscription.Text = "S'inscrire"
        Me.btnInscription.UseVisualStyleBackColor = True
        '
        'btnQuitter
        '
        Me.btnQuitter.Location = New System.Drawing.Point(526, 260)
        Me.btnQuitter.Name = "btnQuitter"
        Me.btnQuitter.Size = New System.Drawing.Size(177, 99)
        Me.btnQuitter.TabIndex = 6
        Me.btnQuitter.Text = "Quitter"
        Me.btnQuitter.UseVisualStyleBackColor = True
        '
        'FrmConnexion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(716, 450)
        Me.Controls.Add(Me.btnQuitter)
        Me.Controls.Add(Me.btnInscription)
        Me.Controls.Add(Me.btnConnexion)
        Me.Controls.Add(Me.txtMotDePasse)
        Me.Controls.Add(Me.txtNom)
        Me.Controls.Add(Me.lblMotDePasse)
        Me.Controls.Add(Me.lblNom)
        Me.Name = "FrmConnexion"
        Me.Text = "Connexion"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblNom As Label
    Friend WithEvents lblMotDePasse As Label
    Friend WithEvents txtNom As TextBox
    Friend WithEvents txtMotDePasse As TextBox
    Friend WithEvents btnConnexion As Button
    Friend WithEvents btnInscription As Button
    Friend WithEvents btnQuitter As Button
End Class
