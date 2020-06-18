Imports System.IO

Public Class Request
    'Private Shared RequestID As Integer = 0
    Private Delimeter As String
    Private DrawingList As New List(Of String)
    Private ParameterPath As String
    Private RequestLocation As String
    Private RequestID As Integer
    Public Sub New(_paramPath As String, _drawingList As List(Of String), _RequestID As Integer)
        Delimeter = "[%]"
        ParameterPath = _paramPath
        DrawingList = _drawingList
        RequestID = _RequestID
    End Sub

    Public Function GetRequestID() As Integer
        Return RequestID
    End Function
    'Private Function GetNextID() As Integer
    '    RequestID = RequestID + 1
    '    Return RequestID
    'End Function

    Public Function GetRequestLocation(ByVal _rID As Integer) As String
        Return RequestLocation
    End Function
    'Public Sub ReplacePathAndMove(ByVal pathString As String)
    '    RequestLocation = pathString & "ProcessedDrawings\" & GetNextID() & "\"
    '    ParameterPath = Replace(ParameterPath, pathString, RequestLocation)

    '    Dim drawingpath As String
    '    Dim oldPath As String
    '    For Each drawingpath In DrawingList
    '        oldPath = drawingpath
    '        drawingpath = Replace(drawingpath, pathString, RequestLocation)
    '        File.Move(oldPath, drawingpath)
    '    Next
    'End Sub
    Public Overrides Function ToString() As String
        Dim output As String = ParameterPath
        Dim filepath As String
        For Each filepath In DrawingList
            output = output & Delimeter & filepath
        Next
        Return output
    End Function
    Public Function GetParameterPath() As String
        Return ParameterPath
    End Function

    Public Function GetDrawingList() As List(Of String)
        Return DrawingList
    End Function
End Class
