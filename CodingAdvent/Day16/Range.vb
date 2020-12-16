Namespace Day16

    Public Class Range
        Public ReadOnly Property Name As String
        Public ReadOnly Property Upper As UInt32
        Public ReadOnly Property Lower As UInt32

        Public Sub New(name As String, lower As UInteger, upper As UInteger)
            Me.Name = name
            Me.Upper = upper
            Me.Lower = lower
        End Sub

        Public Function Matches(x As UInt32) As Boolean
            Return Upper >= x AndAlso Lower <= x
        End Function

    End Class

    Public Class RangeTupel
        Public Property Range1 As Range
        Public Property Range2 As Range

        Public Sub New(range1 As Range, range2 As Range)
            Me.Range1 = range1
            Me.Range2 = range2
        End Sub

        Public Function FitsList(L As List(Of UInteger)) As Boolean
            If L.First >= Range1.Lower AndAlso Range2.Upper >= L.Last Then
                Dim index = L.FindIndex(Function(i) i > Range1.Upper)
                Return L(index) >= Range2.Lower
            End If
            Return False
        End Function

    End Class

    Public Class TicketRules

        Private ReadOnly Rule As List(Of Range)

        Public Sub New(rule As List(Of Range))
            Me.Rule = rule
        End Sub

        Public Function Test(Value As UInt32) As Boolean
            Return Rule.Fold(Function(r, acc) acc Or r.Matches(Value), False)
        End Function

        Public Function TestRange(S As String) As Boolean
            Return S.Split(","c).Fold(Function(Value, acc2) acc2 AndAlso Rule.Fold(Function(r, acc) acc Or r.Matches(CUInt(Value)), False), True)
        End Function

    End Class


    Public Class TicketMatcher

        Public Property Rules As List(Of RangeTupel)

        Public Sub New(rule As List(Of RangeTupel))
            Me.Rules = rule
        End Sub

        ReadOnly Dict As New Dictionary(Of Integer, List(Of UInteger))

        Public Sub Compute(Ticket As String)
            Dim sp = Ticket.Split(","c).ToList.Map(Function(x) CUInt(x)).ZipWithIndex

            For Each item In sp
                If Not Dict.ContainsKey(item.Index) Then
                    Dict.Add(item.Index, New List(Of UInteger))
                End If
                Dict(item.Index).Add(item.Value)
            Next
        End Sub

        Public Function MatchDictAndRanges() As Dictionary(Of Integer, String)
            Dim Matched As New Dictionary(Of Integer, String)
            Dim found = 0
            For Each Item In Dict
                Item.Value.Sort()
                Dim fits As List(Of RangeTupel) = Rules.Filter(Function(t) Not t.FitsList(Item.Value))
                If fits.Count = 1 Then
                    Rules.Remove(fits(0))
                    Matched.Add(Item.Key, fits(0).Range1.Name)
                    found += 1
                End If
            Next

            While found > 0
                found = 0
                For Each item In Dict
                    Dim fits As List(Of RangeTupel) = Rules.Filter(Function(t) Not t.FitsList(item.Value))
                    If fits.Count = 1 Then
                        Rules.Remove(fits(0))
                        Matched.Add(item.Key, fits(0).Range1.Name)
                        found += 1
                    End If
                Next
            End While

            Return Matched
        End Function

    End Class

End Namespace
