<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.label1 = New System.Windows.Forms.Label()
        Me.tbFile = New System.Windows.Forms.TextBox()
        Me.btnFile = New System.Windows.Forms.Button()
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbPageRange = New System.Windows.Forms.RadioButton()
        Me.rbAll = New System.Windows.Forms.RadioButton()
        Me.tbPageEnd = New System.Windows.Forms.TextBox()
        Me.tbPageStart = New System.Windows.Forms.TextBox()
        Me.label5 = New System.Windows.Forms.Label()
        Me.btnConvert = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.rbFolder = New System.Windows.Forms.RadioButton()
        Me.rbFile = New System.Windows.Forms.RadioButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.groupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(12, 79)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(215, 13)
        Me.label1.TabIndex = 7
        Me.label1.Text = "Choose the file or folder you want to convert"
        '
        'tbFile
        '
        Me.tbFile.Location = New System.Drawing.Point(12, 95)
        Me.tbFile.Name = "tbFile"
        Me.tbFile.Size = New System.Drawing.Size(310, 20)
        Me.tbFile.TabIndex = 8
        '
        'btnFile
        '
        Me.btnFile.Location = New System.Drawing.Point(328, 95)
        Me.btnFile.Name = "btnFile"
        Me.btnFile.Size = New System.Drawing.Size(85, 23)
        Me.btnFile.TabIndex = 9
        Me.btnFile.Text = "Browse..."
        Me.btnFile.UseVisualStyleBackColor = True
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.rbPageRange)
        Me.groupBox1.Controls.Add(Me.rbAll)
        Me.groupBox1.Controls.Add(Me.tbPageEnd)
        Me.groupBox1.Controls.Add(Me.tbPageStart)
        Me.groupBox1.Controls.Add(Me.label5)
        Me.groupBox1.Location = New System.Drawing.Point(12, 121)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(183, 87)
        Me.groupBox1.TabIndex = 23
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Pages to Convert"
        '
        'rbPageRange
        '
        Me.rbPageRange.AutoSize = True
        Me.rbPageRange.Enabled = False
        Me.rbPageRange.Location = New System.Drawing.Point(7, 59)
        Me.rbPageRange.Name = "rbPageRange"
        Me.rbPageRange.Size = New System.Drawing.Size(85, 17)
        Me.rbPageRange.TabIndex = 1
        Me.rbPageRange.Text = "Page Range"
        Me.rbPageRange.UseVisualStyleBackColor = True
        '
        'rbAll
        '
        Me.rbAll.AutoSize = True
        Me.rbAll.Checked = True
        Me.rbAll.Location = New System.Drawing.Point(7, 25)
        Me.rbAll.Name = "rbAll"
        Me.rbAll.Size = New System.Drawing.Size(69, 17)
        Me.rbAll.TabIndex = 0
        Me.rbAll.TabStop = True
        Me.rbAll.Text = "All Pages"
        Me.rbAll.UseVisualStyleBackColor = True
        '
        'tbPageEnd
        '
        Me.tbPageEnd.Enabled = False
        Me.tbPageEnd.Location = New System.Drawing.Point(149, 58)
        Me.tbPageEnd.Name = "tbPageEnd"
        Me.tbPageEnd.Size = New System.Drawing.Size(28, 20)
        Me.tbPageEnd.TabIndex = 14
        '
        'tbPageStart
        '
        Me.tbPageStart.Enabled = False
        Me.tbPageStart.Location = New System.Drawing.Point(101, 58)
        Me.tbPageStart.Name = "tbPageStart"
        Me.tbPageStart.Size = New System.Drawing.Size(28, 20)
        Me.tbPageStart.TabIndex = 13
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(132, 61)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(16, 13)
        Me.label5.TabIndex = 15
        Me.label5.Text = "---"
        '
        'btnConvert
        '
        Me.btnConvert.Location = New System.Drawing.Point(231, 146)
        Me.btnConvert.Name = "btnConvert"
        Me.btnConvert.Size = New System.Drawing.Size(91, 43)
        Me.btnConvert.TabIndex = 24
        Me.btnConvert.Text = "Convert"
        Me.btnConvert.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(328, 146)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(91, 43)
        Me.btnCancel.TabIndex = 25
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbFolder)
        Me.GroupBox2.Controls.Add(Me.rbFile)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Location = New System.Drawing.Point(18, 16)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(210, 56)
        Me.GroupBox2.TabIndex = 29
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Convert a large file or an entire folder"
        '
        'rbFolder
        '
        Me.rbFolder.AutoSize = True
        Me.rbFolder.Location = New System.Drawing.Point(107, 25)
        Me.rbFolder.Name = "rbFolder"
        Me.rbFolder.Size = New System.Drawing.Size(84, 17)
        Me.rbFolder.TabIndex = 1
        Me.rbFolder.Text = "Entire Folder"
        Me.rbFolder.UseVisualStyleBackColor = True
        '
        'rbFile
        '
        Me.rbFile.AutoSize = True
        Me.rbFile.Checked = True
        Me.rbFile.Location = New System.Drawing.Point(7, 25)
        Me.rbFile.Name = "rbFile"
        Me.rbFile.Size = New System.Drawing.Size(73, 17)
        Me.rbFile.TabIndex = 0
        Me.rbFile.TabStop = True
        Me.rbFile.Text = "Single File"
        Me.rbFile.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(132, 61)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 13)
        Me.Label3.TabIndex = 15
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(455, 216)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnConvert)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.btnFile)
        Me.Controls.Add(Me.tbFile)
        Me.Controls.Add(Me.label1)
        Me.Name = "Main"
        Me.Text = "Batch PDF to DWG Converter"
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents tbFile As System.Windows.Forms.TextBox
    Private WithEvents btnFile As System.Windows.Forms.Button
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents rbPageRange As System.Windows.Forms.RadioButton
    Private WithEvents rbAll As System.Windows.Forms.RadioButton
    Private WithEvents tbPageEnd As System.Windows.Forms.TextBox
    Private WithEvents tbPageStart As System.Windows.Forms.TextBox
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents btnConvert As System.Windows.Forms.Button
    Private WithEvents btnCancel As System.Windows.Forms.Button
    Private WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Private WithEvents rbFolder As System.Windows.Forms.RadioButton
    Private WithEvents rbFile As System.Windows.Forms.RadioButton
    Private WithEvents Label3 As System.Windows.Forms.Label

End Class
