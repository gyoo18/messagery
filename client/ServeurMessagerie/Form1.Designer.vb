<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmServeur
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
        Me.lblTitre = New System.Windows.Forms.Label()
        Me.lblLogs = New System.Windows.Forms.Label()
        Me.lblClients = New System.Windows.Forms.Label()
        Me.btnDemarrer = New System.Windows.Forms.Button()
        Me.btnArreter = New System.Windows.Forms.Button()
        Me.lstLogs = New System.Windows.Forms.ListBox()
        Me.lstClients = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'lblTitre
        '
        Me.lblTitre.AutoSize = True
        Me.lblTitre.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.125!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitre.Location = New System.Drawing.Point(12, 9)
        Me.lblTitre.Name = "lblTitre"
        Me.lblTitre.Size = New System.Drawing.Size(610, 51)
        Me.lblTitre.TabIndex = 0
        Me.lblTitre.Text = "Serveur de messagerie interne"
        '
        'lblLogs
        '
        Me.lblLogs.AutoSize = True
        Me.lblLogs.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.875!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLogs.Location = New System.Drawing.Point(15, 147)
        Me.lblLogs.Name = "lblLogs"
        Me.lblLogs.Size = New System.Drawing.Size(257, 33)
        Me.lblLogs.TabIndex = 1
        Me.lblLogs.Text = "Journal du serveur"
        '
        'lblClients
        '
        Me.lblClients.AutoSize = True
        Me.lblClients.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.875!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClients.Location = New System.Drawing.Point(546, 147)
        Me.lblClients.Name = "lblClients"
        Me.lblClients.Size = New System.Drawing.Size(246, 33)
        Me.lblClients.TabIndex = 2
        Me.lblClients.Text = "Clients connectés"
        '
        'btnDemarrer
        '
        Me.btnDemarrer.Location = New System.Drawing.Point(90, 72)
        Me.btnDemarrer.Name = "btnDemarrer"
        Me.btnDemarrer.Size = New System.Drawing.Size(137, 53)
        Me.btnDemarrer.TabIndex = 3
        Me.btnDemarrer.Text = "Démarrer"
        Me.btnDemarrer.UseVisualStyleBackColor = True
        '
        'btnArreter
        '
        Me.btnArreter.Location = New System.Drawing.Point(405, 63)
        Me.btnArreter.Name = "btnArreter"
        Me.btnArreter.Size = New System.Drawing.Size(125, 45)
        Me.btnArreter.TabIndex = 4
        Me.btnArreter.Text = "Arrêter"
        Me.btnArreter.UseVisualStyleBackColor = True
        '
        'lstLogs
        '
        Me.lstLogs.FormattingEnabled = True
        Me.lstLogs.ItemHeight = 25
        Me.lstLogs.Location = New System.Drawing.Point(21, 205)
        Me.lstLogs.Name = "lstLogs"
        Me.lstLogs.Size = New System.Drawing.Size(448, 454)
        Me.lstLogs.TabIndex = 5
        '
        'lstClients
        '
        Me.lstClients.FormattingEnabled = True
        Me.lstClients.ItemHeight = 25
        Me.lstClients.Location = New System.Drawing.Point(552, 205)
        Me.lstClients.Name = "lstClients"
        Me.lstClients.Size = New System.Drawing.Size(468, 454)
        Me.lstClients.TabIndex = 6
        '
        'FrmServeur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1032, 898)
        Me.Controls.Add(Me.lstClients)
        Me.Controls.Add(Me.lstLogs)
        Me.Controls.Add(Me.btnArreter)
        Me.Controls.Add(Me.btnDemarrer)
        Me.Controls.Add(Me.lblClients)
        Me.Controls.Add(Me.lblLogs)
        Me.Controls.Add(Me.lblTitre)
        Me.Name = "FrmServeur"
        Me.Text = "Serveur"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitre As Label
    Friend WithEvents lblLogs As Label
    Friend WithEvents lblClients As Label
    Friend WithEvents btnDemarrer As Button
    Friend WithEvents btnArreter As Button
    Friend WithEvents lstLogs As ListBox
    Friend WithEvents lstClients As ListBox
End Class
