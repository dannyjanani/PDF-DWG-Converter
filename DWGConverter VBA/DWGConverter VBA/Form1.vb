Imports System.IO
Imports System.Threading
' I need to have all the requestIDs consistent throughout all programs and not restart counting if a program quits
' Maybe a file with all previous request numbers
Public Class Main
    ' Declaring constants such as the delimeter to be used to parse the data, the files to be searched for, and some others.
    Private requestID As Integer = 0
    Const serverPath As String = "F:\ProgramTest\"
    Dim newDir As String
    Const delimeter As String = "[%]"
    Const paramPath As String = "params.out"
    Const folderpath As String = "folder.out"
    Const abortPath As String = "abort.ex"
    ' Options for the Dropdown Menu
    Dim paperSizes() As String = {"ANSI_FULL_BLEED_A_(8.50_x_11.00_Inches)", _
                                  "ANSI_FULL_BLEED_A_(11.00_x_8.50_Inches)", _
                                  "ANSI_FULL_BLEED_B_(11.00_x_17.00_Inches)", _
                                  "ANSI_FULL_BLEED_B_(17.00_x_11.00_Inches)", _
                                  "ANSI_FULL_BLEED_C_(17.00_x_22.00_Inches)", _
                                  "ANSI_FULL_BLEED_C_(22.00_x_17.00_Inches)", _
                                  "ANSI_FULL_BLEED_D_(22.00_x_34.00_Inches)", _
                                  "ANSI_FULL_BLEED_D_(34.00_x_22.00_Inches)", _
                                  "ANSI_FULL_BLEED_E_(34.00_x_44.00_Inches)", _
                                  "ARCH_FULL_BLEED_A_(9.00_x_12.00_Inches)", _
                                  "ARCH_FULL_BLEED_A_(12.00_x_9.00_Inches)", _
                                  "ARCH_FULL_BLEED_B_(12.00_x_18.00_Inches)", _
                                  "ARCH_FULL_BLEED_B_(18.00_x_12.00_Inches)", _
                                  "ARCH_FULL_BLEED_C_(18.00_x_24.00_Inches)", _
                                  "ARCH_FULL_BLEED_C_(24.00_x_18.00_Inches)", _
                                  "ARCH_FULL_BLEED_D_(24.00_x_36.00_Inches)", _
                                  "ARCH_FULL_BLEED_D_(36.00_x_24.00_Inches)", _
                                  "ARCH_FULL_BLEED_E_(36.00_x_48.00_Inches)", _
                                  "ARCH_FULL_BLEED_E1_(30.00_x_42.00_Inches)"}

    ' If a user clicks the Browse button on the form, it will open a dialog to search for a folder you want to convert DWGs to PDFs.
    ' It will then save the selection into tbpath.Text which is a string.
    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim fbd As New FolderBrowserDialog
        If fbd.ShowDialog = Windows.Forms.DialogResult.OK Then
            tbPath.Text = fbd.SelectedPath
        End If
    End Sub
    ' Recursively get all files that end with dwg in all folders of the selected folder path.
    ' So if you select a parent directory, it will find all files in every folder and add them to be processed.
    Private Sub ProcessDirectory(ByVal dirpath As String, ByRef pathTable As Dictionary(Of String, String), ByVal origBase As String, ByVal newBase As String)
        Dim directoryList() As String : directoryList = Directory.GetDirectories(dirpath)
        For Each subdir As String In directoryList
            ProcessDirectory(subdir, pathTable, origBase, newBase)
            Directory.CreateDirectory(Replace(subdir, origBase, newBase))
        Next
        Dim fileList() As String : fileList = Directory.GetFiles(dirpath)
        For Each filepath As String In fileList
            If (filepath.ToLower.EndsWith(".dwg")) Then
                pathTable.Add(filepath, Replace(filepath, origBase, newBase))
            End If
        Next
    End Sub
    ' If a user clicks the convert button on the form, it will first delete an abort file in case it was existing from before.
    ' The abort file is generated when the cancel button is pressed and the VBA embedded drawing will stop running if it finds it in the directory.
    ' It then checks that all conditions are met (the form isn't missing information) and proceeds based on the checkbox the user selected.
    ' It then creates a dictionary, gets a request ID and calls ProcessDirectory to recursively receive files in all folders.
    ' It copies all the files to the server so they can be converted by the server.
    ' Finally, it checks if the override box is checked or not and fills the params.out file according to the user's selection.
    Private Sub btnConvert_Click(sender As Object, e As EventArgs) Handles btnConvert.Click
        DeleteFile(newDir & "\" & abortPath)
        If tbPath.Text <> "" Then
            If Directory.Exists(tbPath.Text) Then
                Dim DWGPathListing As New Dictionary(Of String, String)
                newDir = serverPath & Environment.UserName & "_" & GetNextRequestID()
                Directory.CreateDirectory(newDir)
                ProcessDirectory(tbPath.Text, DWGPathListing, tbPath.Text, newDir)

                Dim paramOut As String : paramOut = ""
                Dim directoriesAvailable As String : directoriesAvailable = ""

                For Each originalPath As String In DWGPathListing.Keys
                    FileCopy(originalPath, DWGPathListing(originalPath))
                Next
                If cbOverrideSettings.Checked Then
                    paramOut = cbPaperSize.Text
                Else
                    paramOut = "Default"
                End If
                writeToFile(newDir & "\" & paramPath, paramOut, False)
                MsgBox("Converting your files now, you will receive an email upon completion!")
            Else
                MsgBox("The specified directory does not exist. Please check your entries and try again.")
                tbPath.BackColor = Color.Yellow
            End If
        Else
            MsgBox("Please enter in a directory.")
            tbPath.BackColor = Color.Yellow
        End If
    End Sub
    ' Obtain a request ID to start process.
    Private Function GetNextRequestID() As Integer
        requestID = requestID + 1
        Return requestID
    End Function
    Private Function GetCurrentRequestID() As Integer
        Return requestID
    End Function
    ' If the text changed and the box is yellow, return it to the original dialog color
    ' The box is usually turned to yellow when the box is empty and convert is pressed as well as if the folder is not found.
    Private Sub tbPath_TextChanged(sender As Object, e As EventArgs) Handles tbPath.TextChanged
        If tbPath.BackColor = Color.Yellow Then
            tbPath.BackColor = SystemColors.Window
        End If
    End Sub
    ' If the checkbox (override) is changed we want to disable/enable certain buttons/boxes
    Private Sub cbOverrideSettings_CheckedChanged(sender As Object, e As EventArgs) Handles cbOverrideSettings.CheckedChanged
        If cbOverrideSettings.Checked Then
            'cbOverrideSettings.Checked = False 'comment
            cbPaperSize.Enabled = True
            'MsgBox("ERROR: This functionality is not yet available. Please check back soon!") 'comment
        Else
            cbPaperSize.Enabled = False
        End If
    End Sub
    ' Creates a file called abort.ex to tell the VBA program, I want to cancel execution.
    Private Sub btCancel_Click(sender As Object, e As EventArgs) Handles btCancel.Click
        If FileExists(serverPath & GetCurrentRequestID() & ".dwg") Then
            writeToFile(serverPath & "Processing\" & Environment.UserName & "_" & GetCurrentRequestID() & "\" & abortPath, "", False)
        Else
            writeToFile(newDir & "\" & abortPath, "", False)
        End If
    End Sub
    ' If the checkbox (override) is changed we want to disable/enable certain buttons/boxes
    Private Sub cbPaperSize_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbPaperSize.SelectedIndexChanged
        cbPaperSize.AutoCompleteCustomSource.Add("test")
    End Sub
    ' Populate the dropdown menu
    Public Sub New()
        InitializeComponent()
        cbPaperSize.DataSource = paperSizes
    End Sub
End Class
