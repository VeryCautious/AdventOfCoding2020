Namespace Day18
    Public Class Therm

        Public Overridable ReadOnly Property Value As Long

        Public Shared Function PaseString(S As String, Lvls As List(Of Char)()) As Therm
            Dim Depth As Integer = 0

            If S.Length = 1 Then
                Return New Therm(CLng(S))
            End If

            For Lvl As Integer = 0 To Lvls.Count - 1
                For I As Integer = S.Length - 1 To 0 Step -1
                    If S(I) = ")"c Then Depth += 1
                    If S(I) = "("c Then Depth -= 1

                    If Depth = 0 AndAlso Lvls(Lvl).Contains(S(I)) Then
                        Dim args = GetThermsForOperator(I, S)
                        Return CreateThermOperatorBySymbol(
                                (PaseString(args.Item1, Lvls), PaseString(args.Item2, Lvls)),
                                S(I)
                            )
                    End If
                Next
            Next
            Throw New ArgumentException
        End Function

        Private Shared Function CreateThermOperatorBySymbol(Therms As (Therm, Therm), symbol As Char) As ThermOperator
            Select Case symbol
                Case "+"c
                    Return New PlusThermOperator(Therms.Item1, Therms.Item2)
                Case "*"c
                    Return New MalThermOperator(Therms.Item1, Therms.Item2)
                Case Else
                    Throw New ArgumentException
            End Select
        End Function

        Private Shared Function GetThermsForOperator(OpIndex As Integer, S As String) As (String, String)
            Dim first As String
            Dim second As String

            If S(0) = "("c AndAlso S(OpIndex - 1) = ")"c AndAlso IndSamePer(0, OpIndex - 1, S) Then
                first = S.GetValueBetweenTwoIndex(0, OpIndex - 1)
            Else
                first = S.Substring(0, OpIndex)
            End If

            If S(OpIndex + 1) = "("c AndAlso S(S.Length - 1) = ")"c AndAlso IndSamePer(OpIndex + 1, S.Length - 1, S) Then
                second = S.GetValueBetweenTwoIndex(OpIndex + 1, S.Length - 1)
            Else
                second = S.GetValueBetweenTwoIndex(OpIndex, S.Length)
            End If

            Return (first, second)
        End Function

        Private Shared Function IndSamePer(i1 As Integer, i2 As Integer, s As String) As Boolean
            Dim depth As Integer = 0
            For I As Integer = i1 To i2
                If s(I) = "("c Then depth += 1
                If s(I) = ")"c Then depth -= 1
                If s(I) = ")"c AndAlso depth = 0 AndAlso i2 <> I Then
                    Return False
                End If
            Next
            Return True
        End Function

        Public Sub New(Value As Long)
            Me.Value = Value
        End Sub

        Protected Sub New()

        End Sub

    End Class

    Public MustInherit Class ThermOperator
        Inherits Therm

        Public Overrides ReadOnly Property Value As Long
            Get
                Return UserOperator(Therm1.Value, Therm2.Value)
            End Get
        End Property

        ReadOnly Therm1 As Therm, Therm2 As Therm

        Public MustOverride Function UserOperator(v1 As Long, v2 As Long) As Long

        Public Sub New(Therm1 As Therm, Therm2 As Therm)
            Me.Therm1 = Therm1
            Me.Therm2 = Therm2
        End Sub

    End Class


    Public Class PlusThermOperator
        Inherits ThermOperator

        Public Sub New(Therm1 As Therm, Therm2 As Therm)
            MyBase.New(Therm1, Therm2)
        End Sub

        Public Overrides Function UserOperator(v1 As Long, v2 As Long) As Long
            Return v1 + v2
        End Function
    End Class


    Public Class MalThermOperator
        Inherits ThermOperator

        Public Sub New(Therm1 As Therm, Therm2 As Therm)
            MyBase.New(Therm1, Therm2)
        End Sub

        Public Overrides Function UserOperator(v1 As Long, v2 As Long) As Long
            Return v1 * v2
        End Function
    End Class

End Namespace