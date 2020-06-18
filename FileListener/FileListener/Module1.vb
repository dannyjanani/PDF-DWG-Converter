Imports System.Threading
Imports System.IO
Imports System.Collections.Generic
Imports System.Collections.Concurrent

Module Module1
    ' Declaring threads and constants used in this program
    Private Requests As ConcurrentBag(Of Request)
    Const filePath As String = "F:\ProgramTest\"
    Private DispatcherRunning As Boolean = False
    Private ListenerRunning As Boolean = False
    Private DeleterRunning As Boolean = False
    Private OldFileDeleterRunning As Boolean = False
    Private DispatcherLock As New Object
    Private ListenerLock As New Object
    Private DeleterLock As New Object
    Private OldFileDeleterLock As New Object
    Private RequestID As Integer
    Dim newDirectory As String = ""
    ' Start File Listener
    Sub Main()
        Requests = New ConcurrentBag(Of Request)
        LaunchMonitor()
        RequestID = 0
    End Sub
    ' Get current request ID
    Private Function GetCurrentID() As Integer
        Return RequestID
    End Function
    ' Get next request ID
    Private Function GetNextID() As Integer
        RequestID = RequestID + 1
        Return RequestID
    End Function
    ' start the thread monitor which makes sure all threads are running
    Private Sub LaunchMonitor()
        Dim monitor As New Thread(AddressOf MonitorThreads)
        monitor.Start()
    End Sub
    Private Sub DeleteCompletedRequestProcessors()

    End Sub
    ' Launch dispatcher thread
    Private Sub LaunchDispatcher()
        Dim dispatcher As New Thread(AddressOf ProcessDispatcherThread)
        dispatcher.Start()
    End Sub
    ' Launch listener thread
    Private Sub LaunchListener()
        Dim listener As New Thread(AddressOf ProcessListenerThread)
        listener.Start()
    End Sub
    ' Launch deleter thread
    Private Sub LaunchDeleter()
        Dim deleter As New Thread(AddressOf ProcessDeleterThread)
        deleter.Start()
    End Sub
    ' Launch old file deleter thread
    Private Sub LaunchOldFileDeleter()
        Dim OldFileDeleter As New Thread(AddressOf ProcessOldFileDeleterThread)
        OldFileDeleter.Start()
    End Sub
    ' Make sure all threads are running. If a thread isn't running, launch it.
    Private Sub MonitorThreads()
        While (True)
            Thread.Sleep(5000)
            If Not GetLock(DispatcherLock, DispatcherRunning) Then
                LaunchDispatcher()
            End If
            If Not GetLock(ListenerLock, ListenerRunning) Then
                LaunchListener()
            End If
            If Not GetLock(DeleterLock, DeleterRunning) Then
                LaunchDeleter()
            End If
            If Not GetLock(OldFileDeleterLock, OldFileDeleterRunning) Then
                LaunchOldFileDeleter()
            End If
        End While
    End Sub
    ' set/unset a lock on an object a thread is currently using
    Private Sub SetLock(ByRef obj As Object, ByRef prop As Boolean, ByVal value As Boolean)
        SyncLock obj
            prop = value
        End SyncLock
    End Sub
    ' if a thread is running this will return true, otherwise returns false.
    Private Function GetLock(ByRef obj As Object, ByRef prop As Boolean) As Boolean
        SyncLock obj
            Return prop
        End SyncLock
    End Function
    ' If it finds a request, it would generate an RQT file containing all the paths to the files that need to be converted
    ' and launches a new instance of AutoCAD for each request.
    Private Sub ProcessDispatcherThread()
        Try
            While (True)
                SetLock(DispatcherLock, DispatcherRunning, True)
                Thread.Sleep(5000)
                Dim request As Request : request = Nothing
                While (Not Requests.IsEmpty)
                    If (Requests.TryTake(request)) Then
                        File.WriteAllText(filePath & request.GetRequestID & ".rqt", request.ToString())
                        'Process.Start(filePath & GetCurrentID() & ".dwg")
                        Process.Start("C:\Program Files\Autodesk\AutoCAD 2017\acad.exe", filePath & request.GetRequestID() & ".dwg")
                    End If
                End While
                SetLock(DispatcherLock, DispatcherRunning, False)
            End While
        Catch ex As Exception
            SetLock(DispatcherLock, DispatcherRunning, False)
            Console.WriteLine("ERROR in Dispatcher: " & ex.Message & ", " & ex.InnerException.ToString)
        End Try
    End Sub
    ' Recursively get all files that end with dwg in all folders of the selected folder path.
    ' So if you select a parent directory, it will find all files in every folder and add them to be processed.
    Private Sub ProcessDirectory(ByVal dirpath As String, ByRef pathList As List(Of String))
        Dim directoryList() As String : directoryList = Directory.GetDirectories(dirpath)
        For Each subdir As String In directoryList
            ProcessDirectory(subdir, pathList)
        Next
        Dim fileList() As String : fileList = Directory.GetFiles(dirpath)
        For Each filepath As String In fileList
            If (filepath.ToLower.EndsWith(".dwg")) Then
                pathList.Add(filepath)
            End If
        Next
    End Sub
    ' Listens for files every 5 seconds.
    ' If it finds files, it will create a request and move the directory into Processing and copies the Embedded Drawing onto the server to be launched.
    ' It then saves the path to the params.out file.
    Private Sub ProcessListenerThread()
        Try
            While (True)
                SetLock(ListenerLock, ListenerRunning, True)
                Thread.Sleep(5000)
                Console.WriteLine(filePath)

                Dim dirList() As String : dirList = Directory.GetDirectories(filePath)
                Dim dirString As String

                For Each dirString In dirList
                    Console.WriteLine(dirString)
                    If Not LCase(dirString).Contains("process") Then
                        Dim RequestID As Integer : RequestID = GetNextID()
                        newDirectory = filePath & "Processing\" & Replace(dirString, filePath, "")
                        Directory.Move(dirString, newDirectory)
                        File.Copy("DWGConverter.dwg", filePath & "\" & RequestID & ".dwg")
                        Dim DrawingList As New List(Of String)
                        ProcessDirectory(newDirectory, DrawingList)
                        Dim ParamPath As String
                        If newDirectory.EndsWith("\") Then
                            ParamPath = newDirectory & "params.out"
                        Else
                            ParamPath = newDirectory & "\params.out"
                        End If

                        Dim request As New Request(ParamPath, DrawingList, RequestID)
                        Requests.Add(request)
                    End If
                Next
                SetLock(ListenerLock, ListenerRunning, False)
            End While
        Catch ex As Exception
            SetLock(ListenerLock, ListenerRunning, False)
            Console.WriteLine("ERROR in Listener: " & ex.Message & ", " & ex.InnerException.ToString)
        End Try
    End Sub
    ' The VBA drawing creates a .done file when it is complete.
    ' If this sees a .done file it will delete the DWG, BAK, DONE files that are generated to complete execution.
    Private Sub ProcessDeleterThread()
        Try
            While (True)
                SetLock(DeleterLock, DeleterRunning, True)
                Thread.Sleep(5000)

                Dim fileList() As String : fileList = Directory.GetFiles(filePath)
                Dim deleteDwg As String : deleteDwg = ""
                Dim deleteBak As String : deleteBak = ""

                For Each files In fileList
                    If LCase(files.EndsWith(".done")) Then
                        Thread.Sleep(30000)
                        File.Delete(files)
                        files = Replace(files, " ", "")
                        deleteDwg = Replace(files, ".done", ".dwg")
                        deleteBak = Replace(files, ".done", ".bak")
                        If File.Exists(deleteBak) Then
                            File.Delete(deleteBak)
                        End If
                        File.Delete(deleteDwg)
                    End If
                Next
                SetLock(DeleterLock, DeleterRunning, False)
            End While
        Catch ex As Exception
            SetLock(DeleterLock, DeleterRunning, False)
            Console.WriteLine("ERROR in Deleter: " & ex.Message & ", " & ex.InnerException.ToString)
        End Try
    End Sub
    ' If directories are found that are more than 7 days old, that directory would be removed.
    Private Sub ProcessOldFileDeleterThread()
        Try
            While (True)
                SetLock(OldFileDeleterLock, OldFileDeleterRunning, True)
                Thread.Sleep(5000)
                Dim di As New IO.DirectoryInfo(filePath & "ProcessedDrawings\")
                Dim dirs() As IO.DirectoryInfo = di.GetDirectories
                For x As Integer = dirs.Count - 1 To 0 Step -1
                    If dirs(x).CreationTime.AddDays(7) < Now Then
                        My.Computer.FileSystem.DeleteDirectory(dirs(x).FullName, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    End If
                Next
                SetLock(OldFileDeleterLock, OldFileDeleterRunning, False)
            End While
        Catch ex As Exception
            SetLock(OldFileDeleterLock, OldFileDeleterRunning, False)
            Console.WriteLine("ERROR in Old File Deleter: " & ex.Message & ", " & ex.InnerException.ToString)
        End Try
    End Sub
End Module
