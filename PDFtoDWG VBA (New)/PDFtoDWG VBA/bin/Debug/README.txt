This program takes a directory of PDF files, creates a file called params.out with the names of all the files and opens a drawing in AutoCAD which has VBA code embedded into it. The VBA code then splits the params.out file using a delimeter and uses the data appropriately.


What's left to be done:
_______________________

	1. To set up new threads on the file listener and add request functions and everything just like the DWGtoPDF Converter (VBA Drawing and Visual Studio code).
	2. Place it on the server (Workstation) for anyone to be able to use it.


Improvements to be made in future versions:

___________________________________________

	1. Currently it saves a DWG configuration as 8.5 X 11 paper size. We want to give the user the option to choose the size which requires modification of the form in visual studio (having a 	    	   dropdown with various sizes (like the DWGtoPDF converter) as well as changing the VBA file to add a plot config (the DWGtoPDF converter can be used as a reference).
	
	2. Find a way to bind XREFs or make it a relative path so when I send the converted drawings to the designer, the drawing won't have objects pointing to the server.


CODE FROM VBA EMBEDDED DRAWING "PDFConverter.dwg":
_________________________________________________

' Declares a few constants such as the parameter file, abort execution files, and the delimeter
Const paramPath As String = "params.out"
Const disableExecutionPath As String = "disable.ex"
Const abortPath As String = "abort.ex"
Const delimeter As String = "[%]"
______________________________________________________________________________________________________________________________________________________

' This sets up all the files in corresponding variables and calls the ImportPDF function to do the conversion.
' We start off by reading the params.out file and while the file still has data and the abort.ex file is not in the directory,
' we make sure the dataline is not an empty string and then split the parameters into an array of strings called splitParam.
' We now parse the file we are converting, the page, and what we one to save the file as in 3 variables and pass those to ImportPDF function.
' After it completes all drawings it closes the file and goes to next.
' NOTE: You will get an error if you try to run this 2 times in a row. In order to avoid you must completely quit AutoCAD or open a new instance of it like the DWGtoPDFConverter.
' What you can do is change ThisDrawing.Close to ThisDrawing.SendCommand "_QUIT " below where it says "QUIT AUTOCAD HERE"
Public Sub doSomething(ByVal currentDirPath As String)
    Dim FileNum As Integer
    Dim DataLine As String
    Dim errorMsg As String: errorMsg = ""
    FileNum = FreeFile()
    Open (currentDirPath & "\" & paramPath) For Input As #FileNum
    Do While Not EOF(FileNum)
        On Error GoTo ReportError
        If Dir(curDir & "\" & abortPath) = "" Then
            Line Input #FileNum, DataLine
            If DataLine = "" Then
                Line Input #FileNum, DataLine
            End If
            Dim splitParam() As String: splitParam = Split(DataLine, delimeter)
            Dim userfile As String: userfile = splitParam(0)
            Dim page As Integer: page = splitParam(1)
            Dim usersave As String: usersave = splitParam(2)
            Importpdf userfile, usersave, page
       Else
            MsgBox ("Project successfully aborted")
            DeleteFile abortPath
            ThisDrawing.Close
            Exit Sub
        End If
NextDrawing:
    Loop
    Close #FileNum
    MsgBox ("Execution Succesfully Completed!")
    ThisDrawing.Close
    ' QUIT AUTOCAD HERE
    Exit Sub
ReportError:
    MsgBox Err.Description
    Err.Clear
    GoTo NextDrawing
End Sub
______________________________________________________________________________________________________________________________________________________
' This function takes in parameters sent from the doSomething function.
' There is a file dialog that pops up and disables this from executing well so we first disable the dialog, then we run the PDFIMPORT command
' using the page and parameters given, then we set the dialog back to what it was originally and save and close the drawing.
Public Function Importpdf(userfile As String, usersave As String, page As Integer)
    Dim dwg As AcadDocument
    Set dwg = NewDrawing
    Dim fileDialog As Integer: fileDialog = CInt(ThisDrawing.GetVariable("FILEDIA"))
    ThisDrawing.SetVariable "FILEDIA", 0
    Dim cmd As String: cmd = "-PDFIMPORT f" & Chr(13) & userfile & Chr(13) & page & Chr(13) & "10.0,10.0" & Chr(13) & "1.0" & Chr(13) & "0.0" & Chr(13)
    dwg.SendCommand cmd

    dwg.SaveAs usersave
    ThisDrawing.SetVariable "FILEDIA", fileDialog
    dwg.Close
End Function
______________________________________________________________________________________________________________________________________________________
' This function makes sure the disable.ex is not in the directory (if you want to edit code create this empty file in that directory)
' If it isn't it will call doSomething function and if it is it wont do anything.
Private Sub UserForm_Activate()
    Me.Hide
    Dim curDir As String: curDir = Replace(LCase(ThisDrawing.Path), "converter.dwg", "")
    If Dir(curDir & "\" & disableExecutionPath) = "" Then
        doSomething (curDir)
    End If
End Sub
______________________________________________________________________________________________________________________________________________________
' This function creates a new drawing to execute the PDFIMPORT command.
Public Function NewDrawing() As AcadDocument
    Dim strTemplatePath As String: strTemplatePath = "acad.dwt"
    Dim docObj As AcadDocument
    
    Set docObj = ThisDrawing.Application.Documents.Add(strTemplatePath)
    Set NewDrawing = docObj
End Function
______________________________________________________________________________________________________________________________________________________
' Checks if a specific file passed into it exists
Function FileExists(ByVal FileToTest As String) As Boolean
   FileExists = (Dir(FileToTest) <> "")
End Function
______________________________________________________________________________________________________________________________________________________
' This function deletes a file passed into it
Sub DeleteFile(ByVal FileToDelete As String)
   If FileExists(FileToDelete) Then
      SetAttr FileToDelete, vbNormal
      Kill FileToDelete
   End If
End Sub
______________________________________________________________________________________________________________________________________________________