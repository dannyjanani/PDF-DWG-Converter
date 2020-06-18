Imports System.IO
Imports System.Threading
Imports iTextSharp.text.pdf
Imports iTextSharp.text.xml
Public Class Main
    ' Declaring constants such as the delimeter to be used to parse the data, the files to be searched for, and some others.
    Const delimeter As String = "[%]"
    Const paramPath As String = "params.out"
    Dim abortPath As String = "abort.ex"
    Const fileDelim As String = "\"
    Dim saveDoc As String = ""
    Dim saveFolder As String = ""

    ' If a user clicks the browse button on the form, it will open a dialog based on the radio buttons checked by the user.
    ' If the user chooses to convert a single file, it will open a dialog to search for a file that is a PDF.
    ' If the user chooses to convert an entire folder, it will open a dialog to search for a folder.
    ' It will then save the selection into tbFile.Text which is a string.
    Private Sub btnFile_Click(sender As Object, e As EventArgs) Handles btnFile.Click
        If rbFile.Checked Then
            Dim filesearch As New OpenFileDialog
            filesearch.Filter = "Adobe PDF (.pdf)|*.pdf"
            filesearch.Title = "Select a file to convert from a PDF to a DWG"
            If filesearch.ShowDialog = Windows.Forms.DialogResult.OK Then
                tbFile.Text = filesearch.FileName
                Dim splitFile() As String : splitFile = Split(tbFile.Text, fileDelim)
                saveDoc = Replace(splitFile(UBound(splitFile)), ".pdf", "")
            End If
        ElseIf rbFolder.Checked Then
            Dim foldersearch As New FolderBrowserDialog
            foldersearch.Description = "The folder you select here will take all the PDFs in the folder and convert them to DWGs. NOTE: The save name will be similar to the file name + the page number."
            If foldersearch.ShowDialog = Windows.Forms.DialogResult.OK Then
                tbFile.Text = foldersearch.SelectedPath
            End If
        End If
    End Sub

    ' If a user clicks the convert button on the form, it will first delete an abort file in case it was existing from before.
    ' The abort file is generated when the cancel button is pressed and the VBA embedded drawing will stop running if it finds it in the directory.
    ' It then checks that all conditions are met (the form isn't missing information) and proceeds based on the checkbox the user selected.
    ' If the user chooses to convert a single file, it will save the parent folder as the saveFolder to add the DWG created to the same directory and calls convert.
    ' If the user chooses to convert an entire folder, it will save the selected folder as the saveFolder to add the DWG created to the same directory.
    ' It then gets all the files in that directory and removes the extension for each file to save that as a DWG and calls convert.
    Private Sub btnConvert_Click(sender As Object, e As EventArgs) Handles btnConvert.Click
        DeleteFile(abortPath)
        Dim proceed As Boolean : proceed = checkConditions()
        If proceed = True Then
            If rbFile.Checked Then
                saveFolder = Path.GetDirectoryName(tbFile.Text)
                convertFiles()
            ElseIf rbFolder.Checked Then
                saveFolder = tbFile.Text
                Dim fileListing() As String : fileListing = Directory.GetFiles(tbFile.Text)
                Dim paramOut As String : paramOut = ""
                writeToFile(paramPath, paramOut, False)
                For Each fileString In fileListing
                    Dim splitFile() As String : splitFile = Split(fileString, fileDelim)
                    saveDoc = Replace(splitFile(UBound(splitFile)), ".pdf", "")
                    If LCase(fileString).EndsWith(".pdf") Then
                        convertPath(fileString)
                    End If
                Next
            End If
        End If
    End Sub
    ' If the text changed and the box is yellow, return it to the original dialog color
    ' The box is usually turned to yellow when the box is empty and convert is pressed as well as if the file/folder is not found.
    Private Sub tbFile_TextChanged(sender As Object, e As EventArgs) Handles tbFile.TextChanged
        If tbFile.BackColor = Color.Yellow Then
            tbFile.BackColor = SystemColors.Window
        End If
    End Sub
    ' If the text changed and the box is yellow, return it to the original dialog color
    ' The box is usually turned to yellow when the box is empty.
    Private Sub tbPageStart_TextChanged(sender As Object, e As EventArgs) Handles tbPageStart.TextChanged
        If tbPageStart.BackColor = Color.Yellow Then
            tbPageStart.BackColor = SystemColors.Window
        End If
    End Sub
    ' If the text changed and the box is yellow, return it to the original dialog color
    ' The box is usually turned to yellow when the box is empty.
    Private Sub tbPageEnd_TextChanged(sender As Object, e As EventArgs) Handles tbPageEnd.TextChanged
        If tbPageEnd.BackColor = Color.Yellow Then
            tbPageEnd.BackColor = SystemColors.Window
        End If
    End Sub
    ' If the radio buttons (check buttons) are changed we want to disable/enable certain buttons/boxes
    Private Sub rbFile_CheckedChanged(sender As Object, e As EventArgs) Handles rbFile.CheckedChanged
        rbPageRange.Enabled = sender.checked
    End Sub
    ' If the radio buttons (check buttons) are changed we want to disable/enable certain buttons/boxes
    Private Sub rbFolder_CheckedChanged(sender As Object, e As EventArgs) Handles rbFolder.CheckedChanged
        tbFile.Text = ""
        tbPageStart.Text = ""
        tbPageEnd.Text = ""
        rbAll.Checked = True
    End Sub
    ' If the radio buttons (check buttons) are changed we want to disable/enable certain buttons/boxes
    Private Sub rbPageRange_CheckedChanged(sender As Object, e As EventArgs) Handles rbPageRange.CheckedChanged
        tbPageStart.Enabled = sender.checked
        tbPageEnd.Enabled = sender.checked
    End Sub
    ' Checks to see if there are errors and displays a textbox and highlights all errors.
    ' Errors checked: file/folder does not exist, text box is empty, pages not selected. It then returns a boolean called proceed
    Public Function checkConditions() As Boolean
        Dim proceed As Boolean : proceed = True
        Dim errorMessage As String = "Please correct the errors listed below, then try again." & vbNewLine

        If rbFile.Checked And tbFile.Text <> "" And Not File.Exists(tbFile.Text) Then
            errorMessage += "The specified file does not exist" & vbNewLine
            tbFile.BackColor = Color.Yellow
            proceed = False
        End If
        If rbFile.Checked And tbFile.Text = "" Then
            errorMessage += "Please enter a valid file" & vbNewLine
            tbFile.BackColor = Color.Yellow
            proceed = False
        End If
        If rbFolder.Checked And tbFile.Text <> "" And Not Directory.Exists(tbFile.Text) Then
            errorMessage += "The specified directory does not exist" & vbNewLine
            tbFile.BackColor = Color.Yellow
            proceed = False
        End If
        If rbFolder.Checked And tbFile.Text = "" Then
            errorMessage += "Please enter a valid directory" & vbNewLine
            tbFile.BackColor = Color.Yellow
            proceed = False
        End If
        If rbPageRange.Checked And tbPageStart.Text = "" Then
            errorMessage += "Please enter a page to start conversion from" & vbNewLine
            tbPageStart.BackColor = Color.Yellow
            proceed = False
        End If
        If rbPageRange.Checked And tbPageEnd.Text = "" Then
            errorMessage += "Please enter a page to end conversion" & vbNewLine
            tbPageEnd.BackColor = Color.Yellow
            proceed = False
        End If
        If proceed = False Then
            MessageBox.Show(errorMessage)
        End If
        Return proceed
    End Function
    ' Here is where the heavy work is done. This function is called if the Convert File button is checked.
    ' First it checks if you want to convert all pages or only specific pages and it will iterate through those numbers in the for loop.
    ' It writes to a file "params.out" the file to convert, delimeter to separate, page number, delimeter, name we want to save it as.
    ' Finally, it opens the drawing with the embedded VBA code.
    Public Sub convertFiles()
        Dim paramOut As String : paramOut = ""
        Dim savename As String : savename = ""
        If rbAll.Checked Then
            Dim pdfread As New PdfReader(tbFile.Text)
            Dim numberOfPages As Integer : numberOfPages = pdfread.NumberOfPages
            Console.WriteLine(numberOfPages)
            Console.ReadLine()
            For page As Int32 = 1 To numberOfPages
                If paramOut <> "" Then
                    paramOut = paramOut & vbNewLine
                End If
                If numberOfPages = 1 Then
                    savename = saveFolder & "\" & saveDoc & ".dwg"
                Else
                    savename = saveFolder & "\" & saveDoc & "Pg" & page & ".dwg"
                End If
                paramOut = paramOut & tbFile.Text & delimeter & page & delimeter & savename
            Next
        ElseIf rbPageRange.Checked Then
            Dim startpage As Int32 = Convert.ToInt32(tbPageStart.Text)
            Dim endpage As Int32 = Convert.ToInt32(tbPageEnd.Text)
            For page = startpage To endpage
                If paramOut <> "" Then
                    paramOut = paramOut & vbNewLine
                End If
                savename = saveFolder & "\" & saveDoc & "Pg" & page & ".dwg"
                paramOut = paramOut & tbFile.Text & delimeter & page & delimeter & savename
            Next
        End If
        writeToFile(paramPath, paramOut, False)
        Process.Start("PDFConverter.dwg")
    End Sub
    ' Here is where the heavy work is done. This function is called if the Convert Folder button is checked.
    ' It takes in a parameter fileString which is the name and path to the first file sent from the convert function.
    ' First makes sure that All pages button is selected then continues to read the amount of pages of the document.
    ' It then checks if the amount of pages is 1 and if it is, it won't concatenate "Pg" to the drawing name.
    ' It writes to a file "params.out" the file to convert, delimeter to separate, page number, delimeter, name we want to save it as.
    ' Finally, it opens the drawing with the embedded VBA code.
    Public Sub convertPath(fileString As String)
        Dim paramOut As String : paramOut = ""
        Dim savename As String : savename = ""
        If rbAll.Checked Then
            Dim pdfread As New PdfReader(fileString)
            Dim numberOfPages As Integer : numberOfPages = pdfread.NumberOfPages
            Console.WriteLine(numberOfPages)
            Console.ReadLine()
            For page As Int32 = 1 To numberOfPages
                If paramOut <> "" Then
                    paramOut = paramOut & vbNewLine
                End If
                If numberOfPages = 1 Then
                    savename = saveFolder & "\" & saveDoc & ".dwg"
                Else
                    savename = saveFolder & "\" & saveDoc & "Pg" & page & ".dwg"
                End If
                paramOut = paramOut & fileString & delimeter & page & delimeter & savename
            Next
        End If
        writeToFile(paramPath, paramOut, True)
        Process.Start("PDFConverter.dwg")
    End Sub
    ' Creates a file called abort.ex to tell the VBA program, I want to cancel execution.
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        writeToFile(abortPath, "", False)
    End Sub
End Class
