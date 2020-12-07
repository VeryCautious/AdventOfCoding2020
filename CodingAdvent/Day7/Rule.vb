Namespace Day7
    Public Class Rule

        Public ReadOnly Property OuterBag As String
        Public ReadOnly Property InnerbagsAndAmmount As List(Of Tuple(Of String, Integer))


        Public Sub New(Line As String)
            Dim Inp = Line.Replace("bags", "§").Match("{0}§ contain{1}.")
            OuterBag = Inp(0).TrimEnd()

            If Inp(1).Contains("no other") Then
                InnerbagsAndAmmount = New List(Of Tuple(Of String, Integer))
            Else
                InnerbagsAndAmmount = Inp(1).Split(","c).
                    Map(Function(S As String) S.Trim.Replace(" §", "").Replace(" bag", "").Match("{0} {1}")).
                    Map(Function(S As String()) New Tuple(Of String, Integer)(S(1), CInt(S(0)))).ToList
            End If

            'Console.Write(OuterBag + "| ")
            'InnerbagsAndAmmount.ForEach(Sub(x) Console.Write(x.Item1 + ", "))
            'Console.Write(vbNewLine)
        End Sub

        Public Function OuterBagCanContain(Innerbag As String, Ammount As Integer) As Boolean
            For Each Item In InnerbagsAndAmmount
                If Item.Item1 = Innerbag AndAlso Item.Item2 >= Ammount Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Function BagsNeeded(RuleDict As Dictionary(Of String, Day7.Rule)) As Integer
            Return 1 + InnerbagsAndAmmount.Map(Function(t) t.Item2 * RuleDict(t.Item1).BagsNeeded(RuleDict)).Sum()
        End Function

    End Class
End Namespace

