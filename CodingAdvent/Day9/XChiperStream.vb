Namespace Day9

    Public Class XChiperReader

        Private ReadOnly Q As New Queue(Of ULong)
        Private ReadOnly Property PreambleSize As Integer

        Public Sub New(PreambleSize As Integer)
            Me.PreambleSize = PreambleSize
        End Sub

        Public Sub ReadInteger(I As ULong)
            If Q.Count > PreambleSize Then Q.Dequeue()

            Q.Enqueue(I)
            If Q.Count > PreambleSize Then CheckConsistence()
        End Sub

        Private Sub CheckConsistence()
            Dim l = Q.Last()
            Dim t = FindTwoToSumm(l, Q.ToArray)
        End Sub

        Private Function FindTwoToSumm(ToFind As ULong, C As ICollection(Of ULong)) As Tuple(Of ULong, ULong)
            For i As Integer = 0 To C.Count - 1
                If ToFind >= C(i) AndAlso C.Contains(ToFind - C(i)) Then
                    Return New Tuple(Of ULong, ULong)(ToFind - C(i), C(i))
                End If
            Next
            Throw New ArgumentException(ToFind.ToString + " does not match the pattern")
        End Function

        Public ReadOnly Property PeekFirst As ULong
            Get
                Return Q.Peek()
            End Get
        End Property

        Public ReadOnly Property PeekLast As ULong
            Get
                Return Q.Last
            End Get
        End Property

    End Class

End Namespace
