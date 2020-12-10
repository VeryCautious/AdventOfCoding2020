Namespace Day10
    Public Structure DeltaStruct
        Public Property LastItem As Integer
        Public Property OneStepDeltas As Integer
        Public Property ThreeStepDeltas As Integer
    End Structure

    Public Class StepStone
        Public Property StepsToHere As Integer
        Public Property Index As Integer

        Sub New()
            StepsToHere = 0
            Index = 0
        End Sub

        Public Sub New(stepsToHere As Integer, index As Integer)
            Me.StepsToHere = stepsToHere
            Me.Index = index
        End Sub

        Public Function StepToIndex(Index As Integer) As StepStone
            Return New StepStone(StepsToHere + 1, Index)
        End Function

        Public Function GetAllPossibleNextSteps(Adapters As Integer()) As ICollection(Of StepStone)
            Dim ret As New List(Of StepStone)
            For I As Integer = 1 To Math.Min(3, Adapters.Count - (Index + 1))
                If Adapters(Index + I) - Adapters(Index) <= 3 Then
                    ret.Add(StepToIndex(Index + I))
                End If
            Next
            Return ret
        End Function

        Public Function IsDone(Adapters As Integer()) As Boolean
            Return Index = Adapters.Count - 1
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("({0},{1})", Index, StepsToHere)
        End Function

    End Class

End Namespace
