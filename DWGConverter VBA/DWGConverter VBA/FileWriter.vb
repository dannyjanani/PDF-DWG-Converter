'@author Daniel Valle (valleda@coned.com)
Imports System.IO

Module FileWriter
    Dim ignoredExtensions As String
    Public Function writeToFile(ByVal fileName As String, ByVal output As String, ByVal _append As Boolean) As Boolean
        Dim success As Boolean : success = True
        Try
            Dim ioFile As New StreamWriter(fileName, _append)
            ioFile.WriteLine(output)
            ioFile.Flush()
            ioFile.Close()
            ioFile.Dispose()
        Catch ex As Exception
            success = False
        End Try
        Return success
    End Function
    Public Function FileExists(ByVal FileToTest As String) As Boolean
        FileExists = (Dir(FileToTest) <> "")
    End Function
    Sub DeleteFile(ByVal FileToDelete As String)
        If FileExists(FileToDelete) Then 'See above
            ' First remove readonly attribute, if set
            SetAttr(FileToDelete, vbNormal)
            ' Then delete the file
            Kill(FileToDelete)
        End If
    End Sub
End Module
