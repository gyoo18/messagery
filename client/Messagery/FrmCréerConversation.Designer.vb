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
        FlowLayoutPanel3 = New FlowLayoutPanel()
        FlowLayoutPanel4 = New FlowLayoutPanel()
        PictureBox2 = New PictureBox()
        Label5 = New Label()
        Label4 = New Label()
        conteneurContacts = New FlowLayoutPanel()
        FlowLayoutPanel2 = New FlowLayoutPanel()
        PictureBox1 = New PictureBox()
        Label2 = New Label()
        Label3 = New Label()
        txtChercher = New TextBox()
        BtnChercher = New Button()
        BtnCréer = New Button()
        conteneurContactsChoisis.SuspendLayout()
        FlowLayoutPanel3.SuspendLayout()
        FlowLayoutPanel4.SuspendLayout()
        CType(PictureBox2, ComponentModel.ISupportInitialize).BeginInit()
        conteneurContacts.SuspendLayout()
        FlowLayoutPanel2.SuspendLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Arial Black", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(12, 9)
        Label1.Name = "Label1"
        Label1.Size = New Size(351, 27)
        Label1.TabIndex = 0
        Label1.Text = "Créer une nouvelle conversation"
        ' 
        ' conteneurContactsChoisis
        ' 
        conteneurContactsChoisis.AutoScroll = True
        conteneurContactsChoisis.Controls.Add(FlowLayoutPanel3)
        conteneurContactsChoisis.Location = New Point(12, 81)
        conteneurContactsChoisis.MinimumSize = New Size(351, 56)
        conteneurContactsChoisis.Name = "conteneurContactsChoisis"
        conteneurContactsChoisis.Size = New Size(351, 66)
        conteneurContactsChoisis.TabIndex = 1
        ' 
        ' FlowLayoutPanel3
        ' 
        FlowLayoutPanel3.AutoSize = True
        FlowLayoutPanel3.BackColor = SystemColors.ScrollBar
        FlowLayoutPanel3.BorderStyle = BorderStyle.FixedSingle
        FlowLayoutPanel3.Controls.Add(FlowLayoutPanel4)
        FlowLayoutPanel3.Controls.Add(Label4)
        FlowLayoutPanel3.Cursor = Cursors.Hand
        FlowLayoutPanel3.FlowDirection = FlowDirection.TopDown
        FlowLayoutPanel3.Location = New Point(3, 3)
        FlowLayoutPanel3.MinimumSize = New Size(0, 45)
        FlowLayoutPanel3.Name = "FlowLayoutPanel3"
        FlowLayoutPanel3.Size = New Size(106, 58)
        FlowLayoutPanel3.TabIndex = 0
        FlowLayoutPanel3.WrapContents = False
        ' 
        ' FlowLayoutPanel4
        ' 
        FlowLayoutPanel4.AutoSize = True
        FlowLayoutPanel4.Controls.Add(PictureBox2)
        FlowLayoutPanel4.Controls.Add(Label5)
        FlowLayoutPanel4.Location = New Point(0, 0)
        FlowLayoutPanel4.Margin = New Padding(0)
        FlowLayoutPanel4.Name = "FlowLayoutPanel4"
        FlowLayoutPanel4.Size = New Size(104, 41)
        FlowLayoutPanel4.TabIndex = 1
        FlowLayoutPanel4.WrapContents = False
        ' 
        ' PictureBox2
        ' 
        PictureBox2.Location = New Point(7, 7)
        PictureBox2.Margin = New Padding(7)
        PictureBox2.Name = "PictureBox2"
        PictureBox2.Size = New Size(27, 27)
        PictureBox2.TabIndex = 0
        PictureBox2.TabStop = False
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Font = New Font("Arial Black", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label5.Location = New Point(44, 10)
        Label5.Margin = New Padding(3, 10, 3, 10)
        Label5.Name = "Label5"
        Label5.Size = New Size(57, 18)
        Label5.TabIndex = 1
        Label5.Text = "Label5"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, CByte(0))
        Label4.ForeColor = SystemColors.ControlDarkDark
        Label4.Location = New Point(3, 41)
        Label4.Name = "Label4"
        Label4.Size = New Size(42, 15)
        Label4.TabIndex = 1
        Label4.Text = "Label4"
        ' 
        ' conteneurContacts
        ' 
        conteneurContacts.AutoScroll = True
        conteneurContacts.BackColor = SystemColors.Control
        conteneurContacts.BorderStyle = BorderStyle.FixedSingle
        conteneurContacts.Controls.Add(FlowLayoutPanel2)
        conteneurContacts.FlowDirection = FlowDirection.TopDown
        conteneurContacts.Location = New Point(12, 153)
        conteneurContacts.Name = "conteneurContacts"
        conteneurContacts.Size = New Size(351, 300)
        conteneurContacts.TabIndex = 2
        conteneurContacts.WrapContents = False
        ' 
        ' FlowLayoutPanel2
        ' 
        FlowLayoutPanel2.BackColor = SystemColors.ControlLight
        FlowLayoutPanel2.Controls.Add(PictureBox1)
        FlowLayoutPanel2.Controls.Add(Label2)
        FlowLayoutPanel2.Controls.Add(Label3)
        FlowLayoutPanel2.Cursor = Cursors.Hand
        FlowLayoutPanel2.Location = New Point(3, 3)
        FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        FlowLayoutPanel2.Size = New Size(328, 51)
        FlowLayoutPanel2.TabIndex = 0
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Location = New Point(10, 10)
        PictureBox1.Margin = New Padding(10)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(27, 27)
        PictureBox1.TabIndex = 0
        PictureBox1.TabStop = False
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(50, 12)
        Label2.Margin = New Padding(3, 12, 3, 12)
        Label2.Name = "Label2"
        Label2.Size = New Size(60, 21)
        Label2.TabIndex = 1
        Label2.Text = "Label2"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.ForeColor = SystemColors.ControlDarkDark
        Label3.Location = New Point(116, 15)
        Label3.Margin = New Padding(3, 15, 3, 15)
        Label3.Name = "Label3"
        Label3.Size = New Size(41, 15)
        Label3.TabIndex = 2
        Label3.Text = "Label3"
        ' 
        ' txtChercher
        ' 
        txtChercher.Location = New Point(12, 49)
        txtChercher.Name = "txtChercher"
        txtChercher.Size = New Size(308, 23)
        txtChercher.TabIndex = 3
        ' 
        ' BtnChercher
        ' 
        BtnChercher.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnChercher.Location = New Point(326, 39)
        BtnChercher.Name = "BtnChercher"
        BtnChercher.Size = New Size(37, 36)
        BtnChercher.TabIndex = 4
        BtnChercher.Text = "🔎"
        BtnChercher.UseVisualStyleBackColor = True
        ' 
        ' BtnCréer
        ' 
        BtnCréer.Font = New Font("Arial Black", 11F, FontStyle.Bold)
        BtnCréer.Location = New Point(132, 459)
        BtnCréer.Name = "BtnCréer"
        BtnCréer.Size = New Size(96, 35)
        BtnCréer.TabIndex = 5
        BtnCréer.Text = "Créer >"
        BtnCréer.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.ControlLight
        ClientSize = New Size(375, 502)
        Controls.Add(BtnCréer)
        Controls.Add(BtnChercher)
        Controls.Add(txtChercher)
        Controls.Add(conteneurContacts)
        Controls.Add(conteneurContactsChoisis)
        Controls.Add(Label1)
        Name = "Form1"
        Text = "Form1"
        conteneurContactsChoisis.ResumeLayout(False)
        conteneurContactsChoisis.PerformLayout()
        FlowLayoutPanel3.ResumeLayout(False)
        FlowLayoutPanel3.PerformLayout()
        FlowLayoutPanel4.ResumeLayout(False)
        FlowLayoutPanel4.PerformLayout()
        CType(PictureBox2, ComponentModel.ISupportInitialize).EndInit()
        conteneurContacts.ResumeLayout(False)
        FlowLayoutPanel2.ResumeLayout(False)
        FlowLayoutPanel2.PerformLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents conteneurContactsChoisis As FlowLayoutPanel
    Friend WithEvents conteneurContacts As FlowLayoutPanel
    Friend WithEvents FlowLayoutPanel2 As FlowLayoutPanel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label2 As Label
    Friend WithEvents FlowLayoutPanel3 As FlowLayoutPanel
    Friend WithEvents FlowLayoutPanel4 As FlowLayoutPanel
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents txtChercher As TextBox
    Friend WithEvents BtnChercher As Button
    Friend WithEvents BtnCréer As Button
End Class
