<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmCréerConversation
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
        conteneurContactsChoisis = New FlowLayoutPanel()
        conteneurContacts = New FlowLayoutPanel()
        txtChercher = New TextBox()
        BtnChercher = New Button()
        BtnCréer = New Button()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Arial Black", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(22, 19)
        Label1.Margin = New Padding(6, 0, 6, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(690, 54)
        Label1.TabIndex = 0
        Label1.Text = "Créer une nouvelle conversation"
        ' 
        ' conteneurContactsChoisis
        ' 
        conteneurContactsChoisis.AutoScroll = True
        conteneurContactsChoisis.Location = New Point(22, 173)
        conteneurContactsChoisis.Margin = New Padding(6)
        conteneurContactsChoisis.MinimumSize = New Size(652, 119)
        conteneurContactsChoisis.Name = "conteneurContactsChoisis"
        conteneurContactsChoisis.Size = New Size(652, 141)
        conteneurContactsChoisis.TabIndex = 1
        ' 
        ' conteneurContacts
        ' 
        conteneurContacts.AutoScroll = True
        conteneurContacts.BackColor = SystemColors.Control
        conteneurContacts.BorderStyle = BorderStyle.FixedSingle
        conteneurContacts.FlowDirection = FlowDirection.TopDown
        conteneurContacts.Location = New Point(22, 326)
        conteneurContacts.Margin = New Padding(6)
        conteneurContacts.Name = "conteneurContacts"
        conteneurContacts.Size = New Size(650, 638)
        conteneurContacts.TabIndex = 2
        conteneurContacts.WrapContents = False
        ' 
        ' txtChercher
        ' 
        txtChercher.Location = New Point(22, 105)
        txtChercher.Margin = New Padding(6)
        txtChercher.Name = "txtChercher"
        txtChercher.Size = New Size(569, 39)
        txtChercher.TabIndex = 3
        ' 
        ' BtnChercher
        ' 
        BtnChercher.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnChercher.Location = New Point(605, 83)
        BtnChercher.Margin = New Padding(6)
        BtnChercher.Name = "BtnChercher"
        BtnChercher.Size = New Size(69, 77)
        BtnChercher.TabIndex = 4
        BtnChercher.Text = "🔎"
        BtnChercher.UseVisualStyleBackColor = True
        ' 
        ' BtnCréer
        ' 
        BtnCréer.Font = New Font("Arial Black", 11F, FontStyle.Bold)
        BtnCréer.Location = New Point(245, 979)
        BtnCréer.Margin = New Padding(6)
        BtnCréer.Name = "BtnCréer"
        BtnCréer.Size = New Size(178, 75)
        BtnCréer.TabIndex = 5
        BtnCréer.Text = "Créer >"
        BtnCréer.UseVisualStyleBackColor = True
        ' 
        ' FrmCréerConversation
        ' 
        AutoScaleDimensions = New SizeF(13F, 32F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.ControlLight
        ClientSize = New Size(740, 1071)
        Controls.Add(BtnCréer)
        Controls.Add(BtnChercher)
        Controls.Add(txtChercher)
        Controls.Add(conteneurContacts)
        Controls.Add(conteneurContactsChoisis)
        Controls.Add(Label1)
        Margin = New Padding(6)
        Name = "FrmCréerConversation"
        Text = "Form1"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents conteneurContactsChoisis As FlowLayoutPanel
    Friend WithEvents conteneurContacts As FlowLayoutPanel
    Friend WithEvents txtChercher As TextBox
    Friend WithEvents BtnChercher As Button
    Friend WithEvents BtnCréer As Button
End Class
