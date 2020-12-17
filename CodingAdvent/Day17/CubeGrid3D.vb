Namespace Day17

    Public Class CubeGrid3D
        Dim CurrentSize As (X As Integer, Y As Integer, Z As Integer) = (1, 1, 1)

        Dim Grid(CurrentSize.X, CurrentSize.Y, CurrentSize.Z) As Boolean

        Public Sub New()
            Grid.Initialize()
        End Sub

        Public Sub New(List As List(Of String))
            Grid.Initialize()
            For y As Integer = 0 To List.Count - 1
                For x As Integer = 0 To List(0).Length - 1
                    If List(y)(x) = "#"c Then
                        Dim c = (x, y, 0)
                        SetValue(c, True)
                    End If
                Next
            Next
        End Sub

        Public ReadOnly Property ActiveCubes As Integer
            Get
                Dim c = 0
                For Each B In Grid
                    If B Then
                        c += 1
                    End If
                Next
                Return c
            End Get
        End Property

        Private Function ToArrayIndex(T As (X As Integer, Y As Integer, Z As Integer)) As (X As Integer, Y As Integer, Z As Integer)
            Return (T.X + CInt(CurrentSize.X / 2), T.Y + CInt(CurrentSize.Y / 2), T.Z + CInt(CurrentSize.Z / 2))
        End Function

        Private Function ToCoord(T As (X As Integer, Y As Integer, Z As Integer)) As (X As Integer, Y As Integer, Z As Integer)
            Return (T.X - CInt(CurrentSize.X / 2), T.Y - CInt(CurrentSize.Y / 2), T.Z - CInt(CurrentSize.Z / 2))
        End Function

        Public Function GetValue(T As (X As Integer, Y As Integer, Z As Integer)) As Boolean
            T = ToArrayIndex(T)
            If T.X < 0 OrElse T.Y < 0 OrElse T.Z < 0 OrElse
                T.X >= CurrentSize.X OrElse T.Y >= CurrentSize.Y OrElse T.Z >= CurrentSize.Z Then
                Return False
            End If
            Return Grid(T.X, T.Y, T.Z)
        End Function

        Public Sub SetValue(T As (X As Integer, Y As Integer, Z As Integer), V As Boolean)
            Dim TOrg = T
            T = ToArrayIndex(T)
            If T.X >= CurrentSize.X - 2 OrElse T.X < 2 OrElse
                T.Y > CurrentSize.Y - 2 OrElse T.Y < 2 OrElse
                T.Z > CurrentSize.Z - 2 OrElse T.Z < 2 Then
                DoubleGrid()
                SetValue(TOrg, V)
            Else
                Grid(T.X, T.Y, T.Z) = V
            End If
        End Sub

        Private Sub DoubleGrid()
            Dim NewGrid(CurrentSize.X * 2, CurrentSize.Y * 2, CurrentSize.Z * 2) As Boolean
            NewGrid.Initialize()

            For x As Integer = 0 To CurrentSize.X - 1
                For y As Integer = 0 To CurrentSize.Y - 1
                    For z As Integer = 0 To CurrentSize.Z - 1
                        NewGrid(x + CInt(CurrentSize.X / 2), y + CInt(CurrentSize.Y / 2), z + CInt(CurrentSize.Z / 2)) = Grid(x, y, z)
                    Next
                Next
            Next
            Grid = NewGrid
            CurrentSize = (CurrentSize.X * 2, CurrentSize.Y * 2, CurrentSize.Z * 2)
        End Sub

        Public Function CubeAdje(T As (x As Integer, y As Integer, z As Integer)) As Integer
            Dim count As Integer = 0
            For x As Integer = -1 To 1
                For y As Integer = -1 To 1
                    For z As Integer = -1 To 1
                        If x <> 0 Or y <> 0 Or z <> 0 Then
                            If GetValue((T.x + x, T.y + y, T.z + z)) Then
                                count += 1
                            End If
                        End If
                    Next
                Next
            Next
            Return count
        End Function

        Public Function GetNextIteration() As CubeGrid3D
            Dim newC As New CubeGrid3D

            For x As Integer = 0 To CurrentSize.X - 1
                For y As Integer = 0 To CurrentSize.Y - 1
                    For z As Integer = 0 To CurrentSize.Z - 1
                        Dim c = ToCoord((x, y, z))
                        Dim adj = CubeAdje(c)
                        If GetValue(c) And (adj = 2 Or adj = 3) Then
                            newC.SetValue(c, True)
                        End If
                        If Not GetValue(c) And adj = 3 Then
                            newC.SetValue(c, True)
                        End If
                    Next
                Next
            Next

            Return newC
        End Function


        Public Function PlaneToString(Z As Integer) As String
            Z += CInt(CurrentSize.Z / 2)
            Dim s As String = ""
            For y As Integer = 0 To CurrentSize.Y - 1
                For x As Integer = 0 To CurrentSize.X - 1
                    s += If(Grid(x, y, Z), "#", "_")
                Next
                s += vbNewLine
            Next
            Return s
        End Function
    End Class

    Public Class CubeGrid4D
        Dim CurrentSize As (X As Integer, Y As Integer, Z As Integer, W As Integer) = (1, 1, 1, 1)

        Dim Grid(CurrentSize.X, CurrentSize.Y, CurrentSize.Z, CurrentSize.W) As Boolean

        Public Sub New()
            Grid.Initialize()
        End Sub

        Public Sub New(List As List(Of String))
            Grid.Initialize()
            For y As Integer = 0 To List.Count - 1
                For x As Integer = 0 To List(0).Length - 1
                    If List(y)(x) = "#"c Then
                        Dim c = (x, y, 0, 0)
                        SetValue(c, True)
                    End If
                Next
            Next
        End Sub

        Public ReadOnly Property ActiveCubes As Integer
            Get
                Dim c = 0
                For Each B In Grid
                    If B Then
                        c += 1
                    End If
                Next
                Return c
            End Get
        End Property

        Private Function ToArrayIndex(T As (X As Integer, Y As Integer, Z As Integer, W As Integer)) As (X As Integer, Y As Integer, Z As Integer, W As Integer)
            Return (T.X + CInt(CurrentSize.X / 2), T.Y + CInt(CurrentSize.Y / 2), T.Z + CInt(CurrentSize.Z / 2), T.W + CInt(CurrentSize.W / 2))
        End Function

        Private Function ToCoord(T As (X As Integer, Y As Integer, Z As Integer, W As Integer)) As (X As Integer, Y As Integer, Z As Integer, W As Integer)
            Return (T.X - CInt(CurrentSize.X / 2), T.Y - CInt(CurrentSize.Y / 2), T.Z - CInt(CurrentSize.Z / 2), T.W - CInt(CurrentSize.W / 2))
        End Function

        Public Function GetValue(T As (X As Integer, Y As Integer, Z As Integer, W As Integer)) As Boolean
            T = ToArrayIndex(T)
            If T.X < 0 OrElse T.Y < 0 OrElse T.Z < 0 OrElse T.W < 0 OrElse
                T.X >= CurrentSize.X OrElse T.Y >= CurrentSize.Y OrElse T.Z >= CurrentSize.Z OrElse T.W >= CurrentSize.W Then
                Return False
            End If
            Return Grid(T.X, T.Y, T.Z, T.W)
        End Function

        Public Sub SetValue(T As (X As Integer, Y As Integer, Z As Integer, W As Integer), V As Boolean)
            Dim TOrg = T
            T = ToArrayIndex(T)
            If T.X >= CurrentSize.X - 2 OrElse T.X < 2 OrElse
                T.Y > CurrentSize.Y - 2 OrElse T.Y < 2 OrElse
                T.W > CurrentSize.W - 2 OrElse T.W < 2 OrElse
                T.Z > CurrentSize.Z - 2 OrElse T.Z < 2 Then
                DoubleGrid()
                SetValue(TOrg, V)
            Else
                Grid(T.X, T.Y, T.Z, T.W) = V
            End If
        End Sub

        Private Sub DoubleGrid()
            Dim NewGrid(CurrentSize.X * 2, CurrentSize.Y * 2, CurrentSize.Z * 2, CurrentSize.W * 2) As Boolean
            NewGrid.Initialize()

            For W As Integer = 0 To CurrentSize.W - 1
                For x As Integer = 0 To CurrentSize.X - 1
                    For y As Integer = 0 To CurrentSize.Y - 1
                        For z As Integer = 0 To CurrentSize.Z - 1
                            NewGrid(x + CInt(CurrentSize.X / 2), y + CInt(CurrentSize.Y / 2), z + CInt(CurrentSize.Z / 2), W + CInt(CurrentSize.W / 2)) = Grid(x, y, z, W)
                        Next
                    Next
                Next
            Next
            Grid = NewGrid
            CurrentSize = (CurrentSize.X * 2, CurrentSize.Y * 2, CurrentSize.Z * 2, CurrentSize.W * 2)
        End Sub

        Public Function CubeAdje(T As (x As Integer, y As Integer, z As Integer, w As Integer)) As Integer
            Dim count As Integer = 0
            For W As Integer = -1 To 1
                For x As Integer = -1 To 1
                    For y As Integer = -1 To 1
                        For z As Integer = -1 To 1
                            If x <> 0 Or y <> 0 Or z <> 0 Or W <> 0 Then
                                If GetValue((T.x + x, T.y + y, T.z + z, T.w + W)) Then
                                    count += 1
                                End If
                            End If
                        Next
                    Next
                Next
            Next
            Return count
        End Function

        Public Function GetNextIteration() As CubeGrid4D
            Dim newC As New CubeGrid4D
            For W As Integer = 0 To CurrentSize.W - 1
                For x As Integer = 0 To CurrentSize.X - 1
                    For y As Integer = 0 To CurrentSize.Y - 1
                        For z As Integer = 0 To CurrentSize.Z - 1
                            Dim c = ToCoord((x, y, z, W))
                            Dim adj = CubeAdje(c)
                            If GetValue(c) And (adj = 2 Or adj = 3) Then
                                newC.SetValue(c, True)
                            End If
                            If Not GetValue(c) And adj = 3 Then
                                newC.SetValue(c, True)
                            End If
                        Next
                    Next
                Next
            Next
            Return newC
        End Function

    End Class

End Namespace
