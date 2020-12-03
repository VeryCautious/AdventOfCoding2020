Imports CautiousDotNetExtensionLib
Namespace Day3

    Public Enum TerraItems
        Empty
        Tree
    End Enum

    Public Class Terra

        Private Property TreeMap As TerraItems(,)
        Public ReadOnly Property Width As Integer
        Public ReadOnly Property Height As Integer

        Public Sub New(List As List(Of String))
            Dim M(List(0).Length - 1, List.Count - 1) As TerraItems
            TreeMap = M

            Height = List.Count
            Width = List(0).Length

            For Each IV As IndexValuePair(Of String) In List.ZipWithIndex
                Dim row = IV.Index

                Debug.Assert(Width = IV.Value.Length)

                For column As Integer = 0 To IV.Value.Length - 1
                    TreeMap(column, row) = GetTerraItemByChar(IV.Value(column))
                Next
            Next
        End Sub

        Public Function CalcTreeEncounters(Slope As Tuple(Of Integer, Integer)) As Integer
            Dim PassedItems As New List(Of TerraItems)

            Dim x As Integer = 0
            For y As Integer = 0 To Me.Height - 1 Step Slope.Item2
                PassedItems.Add(Me.GetItemAtPosition(x, y))
                x += Slope.Item1
            Next

            PassedItems.RemoveAll(Function(TI As Day3.TerraItems) TI <> CodingAdvent.Day3.TerraItems.Tree)

            Return PassedItems.Count
        End Function

        Public Function GetItemAtPosition(x As Integer, y As Integer) As TerraItems
            Return TreeMap(x Mod Width, y)
        End Function

        Private Function GetTerraItemByChar(C As Char) As TerraItems
            Select Case C
                Case "#"c
                    Return TerraItems.Tree
                Case "."c
                    Return TerraItems.Empty
                Case Else
                    Throw New Exception
            End Select
        End Function

    End Class

End Namespace