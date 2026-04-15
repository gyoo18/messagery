<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMessagerie
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
        Me.lblBienvenue = New System.Windows.Forms.Label()
        Me.lblUtilisateurs = New System.Windows.Forms.Label()
        Me.lstUtilisateurs = New System.Windows.Forms.ListBox()
        Me.rtbDiscussion = New System.Windows.Forms.RichTextBox()
        Me.txtMessage = New System.Windows.Forms.TextBox()
        Me.btnEnvoyer = New System.Windows.Forms.Button()
        Me.btnDeconnexion = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblBienvenue
        '
        Me.lblBienvenue.AutoSize = True
        Me.lblBienvenue.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBienvenue.Location = New System.Drawing.Point(12, 34)
        Me.lblBienvenue.Name = "lblBienvenue"
        Me.lblBienvenue.Size = New System.Drawing.Size(183, 37)
        Me.lblBienvenue.TabIndex = 0
        Me.lblBienvenue.Text = "Bienvenue :"
        '
        'lblUtilisateurs
        '
        Me.lblUtilisateurs.AutoSize = True
        Me.lblUtilisateurs.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.125!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUtilisateurs.Location = New System.Drawing.Point(21, 104)
        Me.lblUtilisateurs.Name = "lblUtilisateurs"
        Me.lblUtilisateurs.Size = New System.Drawing.Size(282, 31)
        Me.lblUtilisateurs.TabIndex = 1
        Me.lblUtilisateurs.Text = "Utilisateurs connectés"
        '
        'lstUtilisateurs
        '
        Me.lstUtilisateurs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstUtilisateurs.FormattingEnabled = True
        Me.lstUtilisateurs.ItemHeight = 25
        Me.lstUtilisateurs.Location = New System.Drawing.Point(27, 174)
        Me.lstUtilisateurs.Name = "lstUtilisateurs"
        Me.lstUtilisateurs.Size = New System.Drawing.Size(420, 579)
        Me.lstUtilisateurs.TabIndex = 2
        '
        'rtbDiscussion
        '
        Me.rtbDiscussion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbDiscussion.Location = New System.Drawing.Point(481, 174)
        Me.rtbDiscussion.Name = "rtbDiscussion"
        Me.rtbDiscussion.Size = New System.Drawing.Size(402, 580)
        Me.rtbDiscussion.TabIndex = 3
        Me.rtbDiscussion.Text = ""
        '
        'txtMessage
        '
        Me.txtMessage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMessage.Location = New System.Drawing.Point(27, 763)
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(645, 31)
        Me.txtMessage.TabIndex = 4
        '
        'btnEnvoyer
        '
        Me.btnEnvoyer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEnvoyer.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnEnvoyer.Location = New System.Drawing.Point(696, 750)
        Me.btnEnvoyer.Name = "btnEnvoyer"
        Me.btnEnvoyer.Size = New System.Drawing.Size(187, 56)
        Me.btnEnvoyer.TabIndex = 5
        Me.btnEnvoyer.Text = "Envoyer"
        Me.btnEnvoyer.UseVisualStyleBackColor = False
        '
        'btnDeconnexion
        '
        Me.btnDeconnexion.Location = New System.Drawing.Point(707, 12)
        Me.btnDeconnexion.Name = "btnDeconnexion"
        Me.btnDeconnexion.Size = New System.Drawing.Size(176, 59)
        Me.btnDeconnexion.TabIndex = 6
        Me.btnDeconnexion.Text = "Déconnexion"
        Me.btnDeconnexion.UseVisualStyleBackColor = True
        '
        'FrmMessagerie
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(952, 818)
        Me.Controls.Add(Me.btnDeconnexion)
        Me.Controls.Add(Me.btnEnvoyer)
        Me.Controls.Add(Me.txtMessage)
        Me.Controls.Add(Me.rtbDiscussion)
        Me.Controls.Add(Me.lstUtilisateurs)
        Me.Controls.Add(Me.lblUtilisateurs)
        Me.Controls.Add(Me.lblBienvenue)
        Me.Name = "FrmMessagerie"
        Me.Text = "FrmMessagerie"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblBienvenue As Label
    Friend WithEvents lblUtilisateurs As Label
    Friend WithEvents lstUtilisateurs As ListBox
    Friend WithEvents rtbDiscussion As RichTextBox
    Friend WithEvents txtMessage As TextBox
    Friend WithEvents btnEnvoyer As Button
    Friend WithEvents btnDeconnexion As Button
End Class
