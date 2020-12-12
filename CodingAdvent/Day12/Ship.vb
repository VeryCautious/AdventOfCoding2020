Imports System.Drawing
Imports CautiousMathLib
Namespace Day12

    Public Class Ship

        Public Property PositionNorth As Integer = 0
        Public Property PositionEast As Integer = 0

        Public Property Direction As New Point(1, 0)

        Public Overridable Sub HandleInput(S As String)
            Dim Value As Integer = CInt(S.Substring(1))

            Select Case S(0)
                Case "N"c
                    PositionNorth += Value
                Case "S"c
                    PositionNorth -= Value
                Case "E"c
                    PositionEast += Value
                Case "W"c
                    PositionEast -= Value
                Case "L"c
                    TurnLeft(Value)
                Case "R"c
                    TurnLeft(-Value)
                Case "F"c
                    PositionNorth += Direction.Y * Value
                    PositionEast += Direction.X * Value
            End Select
        End Sub

        Public ReadOnly Property ManhattanDistance As Integer
            Get
                Return Math.Abs(PositionNorth) + Math.Abs(PositionEast)
            End Get
        End Property

        Protected Sub TurnLeft(Degrees As Double)
            Degrees = (2 * Math.PI * Degrees) / 360
            Dim M As New Mat3(
                New Vec3(Math.Cos(Degrees), -Math.Sin(Degrees), 0),
                New Vec3(Math.Sin(Degrees), Math.Cos(Degrees), 0),
                New Vec3(0, 0, 0))
            Dim V As New Vec3(Direction)
            Dim erg = M * V
            Direction = New Point(CInt(erg.X), CInt(erg.Y))
        End Sub

    End Class

    Public Class WayPointShip
        Inherits Ship

        Sub New()
            Direction = New Point(10, 1)
        End Sub

        Public Overrides Sub HandleInput(S As String)
            Dim Value As Integer = CInt(S.Substring(1))

            Select Case S(0)
                Case "N"c
                    Direction = New Point(Direction.X, Direction.Y + Value)
                Case "S"c
                    Direction = New Point(Direction.X, Direction.Y - Value)
                Case "E"c
                    Direction = New Point(Direction.X + Value, Direction.Y)
                Case "W"c
                    Direction = New Point(Direction.X - Value, Direction.Y)
                Case "L"c
                    TurnLeft(Value)
                Case "R"c
                    TurnLeft(-Value)
                Case "F"c
                    PositionNorth += Direction.Y * Value
                    PositionEast += Direction.X * Value
            End Select
        End Sub

    End Class

End Namespace
