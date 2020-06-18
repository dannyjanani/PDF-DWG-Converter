This program has multiple threads listening for files and if it finds a file it will dispatch a drawing depending on where the file is found. In this case it opens the DWGConverter.dwg which has embedded code in it to convert a DWG to PDFs. This will convert and send the user an email when conversion is completed. If the folder is there for over 7 days, the file listener will delete the folder.


What's left to be done:
_______________________

	1. Place it on the server (Workstation) for anyone to be able to use it.


Improvements to be made in future versions:

___________________________________________

	1. Currently it saves a DWG configuration as originally configured size. We want to give the user the option to choose the size which requires modification of the VBA code as well as  
	   to add a plot config taking parameters from the form in each individual params.out files.


CODE FROM VBA EMBEDDED DRAWING "PDFConverter.dwg":
_________________________________________________

' Declares a few constants such as the parameter file, abort execution files, and the delimeter
'Const rqtPath As String = "1.rqt"
'Const paramPath As String = "params.out"
Const serverPath As String = "F:\ProgramTest\"
Const disableExecutionPath = "disable.ex"
Const abortPath = "abort.ex"
Const delimeter As String = "[%]"
Dim body As String
Dim request As Integer
______________________________________________________________________________________________________________________________________________________
' doSomething is basically the main and takes all the params.out files and splits them into various variables to do what we need.
' It begins by taking the requestID and getting the rqt file which holds all the files that need to be converted.
' It then reads that file and takes all the parameters from the visual studio program and calls DoPDF to actually convert the documents.
' After it comepletes a drawing, it closes the file and goes to the next.
' Finally, after all drawings are complete, it deletes the RQT and Params.out files, moves the folder to Processed and generates a .done
' file to let visual studio know it is complete and we can delete the drawing in visual studio. Lastly, it emails the user a link to folder.
Public Sub doSomething()
    setRequestID
    Dim FileNum As Integer
    Dim DataLine As String
    Dim pathToParams As String
    Dim link As String
    'Dim requests As String: requests = ""
    Dim path As String: path = ThisDrawing.FullName
    Dim fileName As String: fileName = Replace(ThisDrawing.FullName, ".dwg", ".rqt") 'Dir(serverPath & "\*.rqt")
    Dim donePath As String: donePath = serverPath & Str(request) & ".done"
    'Do While fileName <> ""
        'requests = requests & fileName & delimeter
        'fileName = Dir
    'Loop
    'Dim splitFiles() As String: splitFiles = Split(requests, delimeter)
    Dim errorMsg As String: errorMsg = ""

    'Close #FileNum
    'Do While Not EOF(FileNum)
    
    'For i = 0 To UBound(splitFiles) - 1
        'rqtpath = splitFiles(i)
    FileNum = FreeFile()
    Open (fileName) For Input As #FileNum
    Dim textData As String: textData = Input$(LOF(FileNum), FileNum)
    Close #FileNum
    Dim splitParam() As String: splitParam = Split(textData, delimeter)
    Dim fullPath As String
    Dim paramPath As String: paramPath = splitParam(0)
    Dim splitStr() As String: splitStr = Split(paramPath, "\")
    Dim requestEnvironment As String: requestEnvironment = splitStr(UBound(splitStr) - 1)
    For j = 1 To UBound(splitParam)
        On Error GoTo ReportError
        'If Dir(curDir & "\" & abortPath) = "" Then
            'Line Input #FileNum, DataLine
            'Dim splitOptions() As String: splitOptions = Split(DataLine, delimeter)
            Dim pdfFile As String
            Dim dwg As AcadDocument
            'Dim fullpath As String: fullpath = splitOptions(0)
            Dim paperSize As String: paperSize = "Default"
            fullPath = splitParam(j)
            'MsgBox fullPath
            If fullPath <> "" Then
                Dim backPlot As Integer: backPlot = CInt(ThisDrawing.GetVariable("BACKGROUNDPLOT"))
                ThisDrawing.SetVariable "BACKGROUNDPLOT", 0
                Set dwg = Application.Documents.Open(fullPath)
                Application.ActiveDocument = dwg
                AutoCAD.Application.ZoomExtents
                pdfFile = Replace(fullPath, ".dwg", ".pdf")
                DoPdf3 dwg, pdfFile, paperSize
                dwg.Close False
                ThisDrawing.SetVariable "BACKGROUNDPLOT", backPlot
            Else
                MsgBox ("The DWG is not found!")
            End If
        'Else
        '    MsgBox ("Project successfully aborted")
        '    DeleteFile abortPath
        '    ThisDrawing.Close
        '    Exit Sub
        'End If
NextDrawing:
    Next
        DeleteFile (fileName)
        DeleteFile (paramPath)
        Dim ProcessingPath As String: ProcessingPath = Replace(paramPath, "params.out", "")
        Dim DestinationPath As String: DestinationPath = Replace(ProcessingPath, "\Processing", "\ProcessedDrawings")
        Name ProcessingPath As DestinationPath
    link = serverPath & "ProcessedDrawings\" & requestEnvironment
    body = "Your drawings are successfully converted to PDFs. You can find them <a href=" & Chr(34) & link & Chr(34) & ">here.</a>" _
        & vbNewLine & vbNewLine & "You have seven days to retreive the files before they are removed from the server."
    SendEmail body
    createFile donePath
    ThisDrawing.Save
    'ThisDrawing.Close
    ThisDrawing.SendCommand "_QUIT "
    Exit Sub
ReportError:
    MsgBox Err.Description
    Err.Clear
    GoTo NextDrawing
End Sub
______________________________________________________________________________________________________________________________________________________
' Sets up a new configuration to Plot using DWG to PDF.pc3 plotter
' For some reason this isn't plotting the activelayout for the layout space instead just opening it and closing it
Private Sub DoPdf(dwg As AcadDocument, pdfFile As String, paperSize As String)
    'Updates the plot
    'If paperSize <> "Default" Then
        'plotConfig.CanonicalMediaName = paperSize
    'End If
    
    'ThisDrawing.Layouts(0).configName = "DWG To PDF.pc3" 'Plot device
    'ThisDrawing.Layouts(0).CanonicalMediaName = "ANSI_full_bleed_D_(22.00_x_34.00_Inches)" 'PDF format
    
    'ThisDrawing.Layouts(0).CenterPlot = True
                
    'ThisDrawing.Layouts(0).UseStandardScale = False
    'ThisDrawing.Layouts(0).PlotRotation = ac0degrees
    'ThisDrawing.Layouts(0).PlotType = acExtents
    'ThisDrawing.Layouts(0).StandardScale = acScaleToFit
    'ThisDrawing.Application.ZoomExtents

    'ThisDrawing.ModelSpace.Layout.configName = "DWG To PDF.pc3" 'Plot device
    'ThisDrawing.ModelSpace.Layout.CanonicalMediaName = "ANSI_full_bleed_D_(22.00_x_34.00_Inches)" 'PDF format
    
    'ThisDrawing.ModelSpace.Layout.CenterPlot = True
                
    'ThisDrawing.ModelSpace.Layout.UseStandardScale = False
    'ThisDrawing.ModelSpace.Layout.PlotRotation = ac0degrees
    'ThisDrawing.ModelSpace.Layout.PlotType = acExtents
    'ThisDrawing.ModelSpace.Layout.StandardScale = acScaleToFit
    'ThisDrawing.Application.ZoomExtents
    
    ThisDrawing.ActiveLayout.configName = "DWG To PDF.pc3" 'Plot device
    ThisDrawing.ActiveLayout.CanonicalMediaName = "ANSI_full_bleed_D_(22.00_x_34.00_Inches)" 'PDF format
    
    ThisDrawing.ActiveLayout.CenterPlot = True
                
    ThisDrawing.ActiveLayout.UseStandardScale = False
    ThisDrawing.ActiveLayout.PlotRotation = ac0degrees
    ThisDrawing.ActiveLayout.PlotType = acExtents
    ThisDrawing.ActiveLayout.StandardScale = acScaleToFit
    'ThisDrawing.Application.ZoomExtents
    
    Set currentplot = dwg.Plot
    'Conversion step
    currentplot.PlotToFile pdfFile
End Sub
______________________________________________________________________________________________________________________________________________________
' Sets up a new configuration to Plot using DWG to PDF.pc3 plotter
' For some reason this is mixing up the landscape and portrait making the drawings bad
Private Sub DoPdf2(dwg As AcadDocument, pdfFile As String, paperSize As String)
    Dim ptObj As AcadPlot
    Dim ptConfigs As AcadPlotConfigurations
    Dim plotConfig As AcadPlotConfiguration
    Dim configName As String: configName = "DWG to PDF.pc3"
    Dim canonicalName As String
    
    'Create a new plot configuration with all needed parameters
    Set ptObj = dwg.Plot
    Set ptConfigs = dwg.PlotConfigurations
    'MsgBox "ct: " & ptConfigs.Count
    
    Set plotConfig = ptConfigs.Add("PDF", True)
    'Set plotConfig = ptConfigs.Item(configName)
        
    'Add a new plot configuration
    plotConfig.configName = "DWG To PDF.pc3"
    plotConfig.UseStandardScale = False
    plotConfig.StandardScale = acScaleToFit
    plotConfig.PlotRotation = ac0degrees
    plotConfig.PlotType = acExtents
    
    MsgBox "active layout " & ThisDrawing.ActiveLayout
    MsgBox "active space " & ThisDrawing.ActiveSpace
    
    plotConfig.RefreshPlotDeviceInfo

    plotConfig.CenterPlot = True

    plotConfig.CanonicalMediaName = "ANSI_full_bleed_D_(22.00_x_34.00_Inches)"

    'plotConfig.StyleSheet = "C:\.....\Plot Styles\acad.ctb"
    plotConfig.StyleSheet = "C:\Program Files\Autodesk\AutoCAD 2017\ConEd17\Plotters\Plot Styles\conedison.ctb"
    plotConfig.PlotWithPlotStyles = True

    'Updates the plot
    plotConfig.RefreshPlotDeviceInfo
    
    Dim success As Boolean
    'dwg.ActiveLayout.CopyFrom (plotConfig)
    ptObj.PlotToFile pdfFile

    ptConfigs.Item("PDF").Delete
    Set plotConfig = Nothing

End Sub
______________________________________________________________________________________________________________________________________________________
' Sets up a new configuration to Plot using DWG to PDF.pc3 plotter
' For some reason this doesn't scale the drawing to what you want it to be, rather just plots as it is already configured.
Private Function DoPdf3(dwg As AcadDocument, pdfFile As String, paperSize As String) As Boolean

    Dim ptObj As AcadPlot
    Dim ptConfigs As AcadPlotConfigurations
    Dim plotConfig As AcadPlotConfiguration
    Dim configName As String: configName = "DWG to PDF.pc3"
    Dim canonicalName As String
    
    'Create a new plot configuration with all needed parameters
    Set ptObj = dwg.Plot
    Set ptConfigs = dwg.PlotConfigurations
    'MsgBox "ct: " & ptConfigs.Count
    
    'Add a new plot configuration
    Set plotConfig = ptConfigs.Add(configName, True)
     
    plotConfig.configName = configName
    'plotConfig.ConfigName = "Adobe PDF"
    plotConfig.UseStandardScale = False
    plotConfig.StandardScale = acScaleToFit
    plotConfig.PlotRotation = ac0degrees
    plotConfig.PlotType = acExtents
    plotConfig.CenterPlot = True
    
    'canonicalName = GetTargetCanonicalMediaName(plotConfig, "ANSI D (34.00 x 22.00 Inches)")
    'If Len(canonicalName) = 0 Then
    '    MsgBox "Wrong paper name!", vbCritical
    '    Exit Function
    'End If
    
    'Updates the plot
    If paperSize <> "Default" Then
        plotConfig.CanonicalMediaName = paperSize
    End If
    'plotConfig.CanonicalMediaName = canonicalName

    'plotConfig.StyleSheet = "C:\.....\Plot Styles\acad.ctb"
    plotConfig.StyleSheet = "C:\.....\Plot Styles\conedison.ctb"
    plotConfig.PlotWithPlotStyles = True

    'Updates the plot
    plotConfig.RefreshPlotDeviceInfo
    
    Dim success As Boolean
    success = ptObj.PlotToFile(pdfFile, plotConfig.configName)

    ptConfigs.Item(configName).Delete
    Set plotConfig = Nothing

    DoPdf3 = success
    
End Function
______________________________________________________________________________________________________________________________________________________
' This function makes sure the disable.ex is not in the directory (if you want to edit code create this empty file in that directory)
' If it isn't it will call doSomething function and if it is it wont do anything.
Private Sub UserForm_Activate()
    Me.Hide
    Dim curDir As String: curDir = Replace(LCase(ThisDrawing.path), "DWGConverter.dwg", "")
    If Dir(curDir & "\" & disableExecutionPath) = "" Then
        doSomething
    Else
        ThisDrawing.SendCommand "vbaide "
    End If
End Sub
______________________________________________________________________________________________________________________________________________________
' Gets the current requestID generated from visual studio by parsing the drawing name
Private Sub setRequestID()
    request = CInt(Replace(Replace(ThisDrawing.FullName, serverPath, ""), ".dwg", ""))
End Sub
______________________________________________________________________________________________________________________________________________________
' Creates a new file; used to tell fileListener we are done by putting a .DONE file.
Private Sub createFile(donePath As String)
    Dim fso As Object
    Set fso = CreateObject("Scripting.FileSystemObject")

    Dim Fileout As Object
    Set Fileout = fso.CreateTextFile(donePath, True, True)
    Fileout.Write ""
    Fileout.Close
End Sub
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
' Sends an email to the user to let them know the conversion is complete or if it isn't.
Public Sub SendEmail(body)
    Dim outobj As Object
    Dim mailobj As Object
    Dim requester As String: requester = (Environ$("Username")) & "@coned.com"
    Dim subject As String: subject = "DWGtoPDF Converter"
    
    On Error Resume Next
    Set outobj = GetObject(, "Outlook.Application")
    If outobj Is Nothing Then
        Set outobj = CreateObject("Outlook.Application")
    End If
    On Error GoTo 0
    If outobj Is Nothing Then
        MsgBox "Cannot obtain Outlook application object!"
        Exit Sub
    End If
    Set mailobj = outobj.CreateItem(0)
    With mailobj
     .To = requester
     .subject = subject
     .HTMLbody = body
     .Send
    End With
    'Clear the memory
    Set outobj = Nothing
    Set mailobj = Nothing
End Sub
______________________________________________________________________________________________________________________________________________________