Namespace Day11
    Public Class Ferry

        Protected Grid(,) As SeatStates

        Protected ReadOnly RowSize As Integer
        Protected ReadOnly ColSize As Integer

        Protected MaxOcc As Integer = 4

        Public Sub New(Rows As List(Of String))
            Dim Grid(0 To Rows(0).Length - 1, 0 To Rows.Count - 1) As SeatStates
            Me.Grid = Grid
            RowSize = Rows(0).Length
            ColSize = Rows.Count
            For Each Row In Rows.ZipWithIndex
                For Each C In Row.Value.ToCharArray.ZipWithIndex
                    Me.Grid(C.Index, Row.Index) = CharToSeatState(C.Value)
                Next
            Next
        End Sub

        Public Property State As SeatStates(,)
            Get
                Return Grid
            End Get
            Set(value As SeatStates(,))
                Grid = value
            End Set
        End Property

        Public ReadOnly Property OccupiedSeats As Integer
            Get
                Dim i As Integer = 0
                For x As Integer = 0 To RowSize - 1
                    For y As Integer = 0 To ColSize - 1
                        If Grid(x, y) = SeatStates.Occupied Then i += 1
                    Next
                Next
                Return i
            End Get
        End Property

        Public Function AreSame(State As SeatStates(,)) As Boolean
            For x As Integer = 0 To RowSize - 1
                For y As Integer = 0 To ColSize - 1
                    If Not Grid(x, y) = State(x, y) Then Return False
                Next
            Next
            Return True
        End Function

        Public Function CalculateNextState() As SeatStates(,)
            Dim NewGrid(0 To RowSize - 1, 0 To ColSize - 1) As SeatStates

            For x As Integer = 0 To RowSize - 1
                For y As Integer = 0 To ColSize - 1
                    Select Case Grid(x, y)
                        Case SeatStates.Occupied
                            If NumberAroundSeat(x, y) >= MaxOcc Then
                                NewGrid(x, y) = SeatStates.Empty
                            Else
                                NewGrid(x, y) = SeatStates.Occupied
                            End If
                        Case SeatStates.Empty
                            If NumberAdj(x, y) = 0 Then
                                NewGrid(x, y) = SeatStates.Occupied
                            Else
                                NewGrid(x, y) = SeatStates.Empty
                            End If
                        Case Else
                            NewGrid(x, y) = Grid(x, y)
                    End Select
                Next
            Next

            Return NewGrid
        End Function

        Protected Overridable Function NumberAdj(x As Integer, y As Integer) As Integer
            Dim found As Integer = 0

            For gX As Integer = Math.Max(0, x - 1) To Math.Min(RowSize - 1, x + 1)
                For gY As Integer = Math.Max(0, y - 1) To Math.Min(ColSize - 1, y + 1)
                    If gY <> y Or gX <> x Then
                        If Grid(gX, gY) = SeatStates.Occupied Then
                            found += 1
                        End If
                    End If
                Next
            Next

            Return found
        End Function

        Protected Overridable Function NumberAroundSeat(x As Integer, y As Integer) As Integer
            Return NumberAdj(x, y)
        End Function


        Private Function CharToSeatState(C As Char) As SeatStates
            Select Case C
                Case "."c
                    Return SeatStates.Floor
                Case "L"c
                    Return SeatStates.Empty
                Case "#"c
                    Return SeatStates.Occupied
                Case Else
                    Throw New ArgumentException
            End Select
        End Function
        Private Function SeatStateToChar(C As SeatStates) As Char
            Select Case C
                Case SeatStates.Floor
                    Return "."c
                Case SeatStates.Empty
                    Return "L"c
                Case SeatStates.Occupied
                    Return "#"c
                Case Else
                    Throw New ArgumentException
            End Select
        End Function

        Public Overrides Function ToString() As String
            Dim s As String = ""
            For y As Integer = 0 To ColSize - 1
                For x As Integer = 0 To RowSize - 1
                    s += SeatStateToChar(Grid(x, y)).ToString
                Next
                s += vbNewLine
            Next

            Return s
        End Function

    End Class

    Public Class OtherFerry
        Inherits Ferry

        Public Sub New(Rows As List(Of String))
            MyBase.New(Rows)
            MaxOcc = 5
        End Sub

        Protected Overrides Function NumberAroundSeat(x As Integer, y As Integer) As Integer
            Dim found As Integer = 0

            found += Iter(x, y, 0, 1)
            found += Iter(x, y, 1, 0)
            found += Iter(x, y, 0, -1)
            found += Iter(x, y, -1, 0)
            found += Iter(x, y, 1, 1)
            found += Iter(x, y, -1, 1)
            found += Iter(x, y, 1, -1)
            found += Iter(x, y, -1, -1)

            Return found
        End Function

        Protected Overrides Function NumberAdj(x As Integer, y As Integer) As Integer
            Dim found As Integer = 0

            found += Iter(x, y, 0, 1)
            found += Iter(x, y, 1, 0)
            found += Iter(x, y, 0, -1)
            found += Iter(x, y, -1, 0)
            found += Iter(x, y, 1, 1)
            found += Iter(x, y, -1, 1)
            found += Iter(x, y, 1, -1)
            found += Iter(x, y, -1, -1)

            Return found
        End Function

        Private Function Iter(x As Integer, y As Integer, dx As Integer, dy As Integer) As Integer
            Dim cx As Integer = x + dx
            Dim cy As Integer = y + dy

            While InBound(cx, cy)
                If Grid(cx, cy) = SeatStates.Occupied Then
                    Return 1
                End If
                If Grid(cx, cy) = SeatStates.Empty Then
                    Return 0
                End If
                cx += dx
                cy += dy
            End While
            Return 0
        End Function

        Private Function InBound(x As Integer, y As Integer) As Boolean
            Return x >= 0 AndAlso y >= 0 AndAlso x < RowSize AndAlso y < ColSize
        End Function

    End Class

    Public Enum SeatStates
        Floor
        Empty
        Occupied
    End Enum
End Namespace