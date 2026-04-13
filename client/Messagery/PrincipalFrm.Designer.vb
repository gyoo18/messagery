<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PrincipalFrm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        GroupBox1 = New GroupBox()
        BoîtesConversationsConteneur = New FlowLayoutPanel()
        Button1 = New Button()
        Panel1 = New Panel()
        Panel3 = New Panel()
        BtnEnvoyer = New Button()
        MessageEntrée = New TextBox()
        EnTêteIdentificateurs = New Label()
        EnTêteNomsAffichage = New Label()
        PictureBox2 = New PictureBox()
        FlowLayoutPanel2 = New FlowLayoutPanel()
        MessagesConteneur = New FlowLayoutPanel()
        BackgroundWorker1 = New ComponentModel.BackgroundWorker()
        GroupBox1.SuspendLayout()
        Panel1.SuspendLayout()
        Panel3.SuspendLayout()
        CType(PictureBox2, ComponentModel.ISupportInitialize).BeginInit()
        FlowLayoutPanel2.SuspendLayout()
        SuspendLayout()
        ' 
        ' GroupBox1
        ' 
        GroupBox1.BackColor = SystemColors.Control
        GroupBox1.Controls.Add(BoîtesConversationsConteneur)
        GroupBox1.Controls.Add(Button1)
        GroupBox1.Location = New Point(6, 2)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(248, 624)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        GroupBox1.Text = "Discussions"
        ' 
        ' BoîtesConversationsConteneur
        ' 
        BoîtesConversationsConteneur.AutoScroll = True
        BoîtesConversationsConteneur.AutoScrollMargin = New Size(1, 1)
        BoîtesConversationsConteneur.AutoScrollMinSize = New Size(1, 1)
        BoîtesConversationsConteneur.Location = New Point(6, 64)
        BoîtesConversationsConteneur.Name = "BoîtesConversationsConteneur"
        BoîtesConversationsConteneur.Size = New Size(236, 539)
        BoîtesConversationsConteneur.TabIndex = 1
        ' 
        ' Button1
        ' 
        Button1.BackColor = SystemColors.ControlLight
        Button1.Font = New Font("Arial Black", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button1.Location = New Point(200, 22)
        Button1.Name = "Button1"
        Button1.Size = New Size(36, 36)
        Button1.TabIndex = 0
        Button1.Text = "+"
        Button1.UseVisualStyleBackColor = False
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Panel3)
        Panel1.Location = New Point(264, 12)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(465, 614)
        Panel1.TabIndex = 1
        ' 
        ' Panel3
        ' 
        Panel3.BackColor = SystemColors.ControlLight
        Panel3.Controls.Add(BtnEnvoyer)
        Panel3.Controls.Add(MessageEntrée)
        Panel3.Location = New Point(0, 557)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(465, 57)
        Panel3.TabIndex = 0
        ' 
        ' BtnEnvoyer
        ' 
        BtnEnvoyer.BackColor = SystemColors.ControlLight
        BtnEnvoyer.Font = New Font("Arial Black", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        BtnEnvoyer.Location = New Point(423, 15)
        BtnEnvoyer.Name = "BtnEnvoyer"
        BtnEnvoyer.Size = New Size(36, 36)
        BtnEnvoyer.TabIndex = 2
        BtnEnvoyer.Text = ">"
        BtnEnvoyer.UseVisualStyleBackColor = False
        ' 
        ' MessageEntrée
        ' 
        MessageEntrée.Location = New Point(15, 19)
        MessageEntrée.Margin = New Padding(15)
        MessageEntrée.Name = "MessageEntrée"
        MessageEntrée.Size = New Size(402, 23)
        MessageEntrée.TabIndex = 0
        ' 
        ' EnTêteIdentificateurs
        ' 
        EnTêteIdentificateurs.AutoSize = True
        EnTêteIdentificateurs.Font = New Font("Arial", 12F, FontStyle.Italic)
        EnTêteIdentificateurs.ForeColor = SystemColors.ControlDarkDark
        EnTêteIdentificateurs.Location = New Point(205, 15)
        EnTêteIdentificateurs.Margin = New Padding(3, 15, 3, 15)
        EnTêteIdentificateurs.Name = "EnTêteIdentificateurs"
        EnTêteIdentificateurs.Size = New Size(111, 19)
        EnTêteIdentificateurs.TabIndex = 6
        EnTêteIdentificateurs.Text = "id@serveur.tld"
        ' 
        ' EnTêteNomsAffichage
        ' 
        EnTêteNomsAffichage.AutoSize = True
        EnTêteNomsAffichage.Font = New Font("Arial Black", 12F, FontStyle.Bold)
        EnTêteNomsAffichage.Location = New Point(60, 15)
        EnTêteNomsAffichage.Margin = New Padding(3, 15, 3, 15)
        EnTêteNomsAffichage.Name = "EnTêteNomsAffichage"
        EnTêteNomsAffichage.Size = New Size(139, 23)
        EnTêteNomsAffichage.TabIndex = 5
        EnTêteNomsAffichage.Text = "nom_affichage"
        ' 
        ' PictureBox2
        ' 
        PictureBox2.Location = New Point(15, 15)
        PictureBox2.Margin = New Padding(15)
        PictureBox2.Name = "PictureBox2"
        PictureBox2.Size = New Size(27, 27)
        PictureBox2.TabIndex = 4
        PictureBox2.TabStop = False
        ' 
        ' FlowLayoutPanel2
        ' 
        FlowLayoutPanel2.BackColor = SystemColors.ControlLight
        FlowLayoutPanel2.Controls.Add(PictureBox2)
        FlowLayoutPanel2.Controls.Add(EnTêteNomsAffichage)
        FlowLayoutPanel2.Controls.Add(EnTêteIdentificateurs)
        FlowLayoutPanel2.Location = New Point(264, 12)
        FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        FlowLayoutPanel2.Size = New Size(465, 57)
        FlowLayoutPanel2.TabIndex = 1
        ' 
        ' MessagesConteneur
        ' 
        MessagesConteneur.AutoScroll = True
        MessagesConteneur.FlowDirection = FlowDirection.TopDown
        MessagesConteneur.Location = New Point(263, 68)
        MessagesConteneur.Name = "MessagesConteneur"
        MessagesConteneur.Size = New Size(466, 502)
        MessagesConteneur.TabIndex = 1
        ' 
        ' PrincipalFrm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(735, 632)
        Controls.Add(MessagesConteneur)
        Controls.Add(FlowLayoutPanel2)
        Controls.Add(Panel1)
        Controls.Add(GroupBox1)
        Name = "PrincipalFrm"
        Text = "Form1"
        GroupBox1.ResumeLayout(False)
        Panel1.ResumeLayout(False)
        Panel3.ResumeLayout(False)
        Panel3.PerformLayout()
        CType(PictureBox2, ComponentModel.ISupportInitialize).EndInit()
        FlowLayoutPanel2.ResumeLayout(False)
        FlowLayoutPanel2.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents BoîtesConversationsConteneur As FlowLayoutPanel
    Friend WithEvents Button1 As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents EnTêteIdentificateurs As Label
    Friend WithEvents EnTêteNomsAffichage As Label
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents FlowLayoutPanel2 As FlowLayoutPanel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents BtnEnvoyer As Button
    Friend WithEvents MessageEntrée As TextBox
    Friend WithEvents MessagesConteneur As FlowLayoutPanel
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker

End Class
