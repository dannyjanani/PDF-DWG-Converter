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
        Me.cbPaperSize = New System.Windows.Forms.ComboBox()
        Me.cbOverrideSettings = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbPath = New System.Windows.Forms.TextBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnConvert = New System.Windows.Forms.Button()
        Me.btCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'cbPaperSize
        '
        Me.cbPaperSize.Enabled = False
        Me.cbPaperSize.FormattingEnabled = True
        Me.cbPaperSize.Location = New System.Drawing.Point(12, 120)
        Me.cbPaperSize.Name = "cbPaperSize"
        Me.cbPaperSize.Size = New System.Drawing.Size(309, 21)
        Me.cbPaperSize.TabIndex = 0
        Me.cbPaperSize.Text = "Select paper size"
        '
        'cbOverrideSettings
        '
        Me.cbOverrideSettings.AutoSize = True
        Me.cbOverrideSettings.Location = New System.Drawing.Point(15, 69)
        Me.cbOverrideSettings.Name = "cbOverrideSettings"
        Me.cbOverrideSettings.Size = New System.Drawing.Size(236, 17)
        Me.cbOverrideSettings.TabIndex = 1
        Me.cbOverrideSettings.Text = "Override Default Drawing Plot Configurations"
        Me.cbOverrideSettings.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 104)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Paper Size"
        '
        'tbPath
        '
        Me.tbPath.Location = New System.Drawing.Point(13, 35)
        Me.tbPath.Name = "tbPath"
        Me.tbPath.Size = New System.Drawing.Size(308, 20)
        Me.tbPath.TabIndex = 3
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(327, 35)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 4
        Me.btnBrowse.Text = "Browse..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(189, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Directory Containing DWGs to Convert"
        '
        'btnConvert
        '
        Me.btnConvert.Location = New System.Drawing.Point(15, 163)
        Me.btnConvert.Name = "btnConvert"
        Me.btnConvert.Size = New System.Drawing.Size(98, 42)
        Me.btnConvert.TabIndex = 6
        Me.btnConvert.Text = "Convert"
        Me.btnConvert.UseVisualStyleBackColor = True
        '
        'btCancel
        '
        Me.btCancel.Location = New System.Drawing.Point(132, 163)
        Me.btCancel.Name = "btCancel"
        Me.btCancel.Size = New System.Drawing.Size(98, 42)
        Me.btCancel.TabIndex = 7
        Me.btCancel.Text = "Cancel"
        Me.btCancel.UseVisualStyleBackColor = True
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(408, 231)
        Me.Controls.Add(Me.btCancel)
        Me.Controls.Add(Me.btnConvert)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.tbPath)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cbOverrideSettings)
        Me.Controls.Add(Me.cbPaperSize)
        Me.Name = "Main"
        Me.Text = "DWG to PDF Converter Version 1.0"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbPaperSize As System.Windows.Forms.ComboBox
    Friend WithEvents cbOverrideSettings As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbPath As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnConvert As System.Windows.Forms.Button
    Friend WithEvents btCancel As System.Windows.Forms.Button

End Class
