Namespace Day19

    Public Class TerminalRule

        Public ReadOnly From As NonTerminal
        Public ReadOnly Terminal As Terminal

        Public Sub New(From As NonTerminal, Terminal As Terminal)
            Me.From = From
            Me.Terminal = Terminal
        End Sub

        Public Function CanProduce(T As Terminal) As Boolean
            Return Terminal.Equals(T)
        End Function
        Public Overrides Function ToString() As String
            Return String.Format("{0}->{1}", From, Terminal)
        End Function
    End Class

    Public Class NonTerminalRule

        Public ReadOnly From As NonTerminal
        Public ReadOnly ToValue1 As NonTerminal
        Public ReadOnly ToValue2 As NonTerminal

        Public Sub New(from As NonTerminal, toValue1 As NonTerminal, toValue2 As NonTerminal)
            Me.From = from
            Me.ToValue1 = toValue1
            Me.ToValue2 = toValue2
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("{0}->{1},{2}", From, ToValue1, ToValue2)
        End Function

        Public Function CanProduce(S1 As NonTerminal, S2 As NonTerminal) As Boolean
            Return ToValue1.Equals(S1) And ToValue1.Equals(S2)
        End Function
    End Class

    Public Class Terminal
        Public Property Symbol As String
        Public Sub New(symbol As String)
            Me.Symbol = symbol
        End Sub
        Public Overrides Function ToString() As String
            Return Symbol
        End Function
    End Class

    Public Class NonTerminal
        Public Property Symbol As String
        Public Sub New(symbol As String)
            Me.Symbol = symbol
        End Sub
        Public Overrides Function ToString() As String
            Return Symbol
        End Function
    End Class

    Public Class CYK

        Public ReadOnly Property TerminalRules As List(Of TerminalRule)
        Public ReadOnly Property NonTerminalRules As List(Of NonTerminalRule)
        Public ReadOnly Property Startsymbol As NonTerminal

        Public Sub New(terminalRules As List(Of TerminalRule), nonTerminalRules As List(Of NonTerminalRule), Startsymbol As NonTerminal)
            Me.TerminalRules = terminalRules
            Me.NonTerminalRules = nonTerminalRules
            Me.Startsymbol = Startsymbol
        End Sub

        Public Function CanBeConstructed(S As String) As Boolean
            Dim a(S.Length - 1) As Terminal
            For i As Integer = 0 To S.Length - 1
                a(i) = AddTerminal(S(i))
            Next
            Return CanBeConstructed(a)
        End Function

        Public Function CanBeConstructed(S As Terminal()) As Boolean
            Dim n As Integer = S.Length
            Dim A(n - 1, n - 1) As List(Of NonTerminal)

            For x As Integer = 0 To n - 1
                For y As Integer = 0 To n - 1
                    A(x, y) = New List(Of NonTerminal)
                Next
            Next

            For I As Integer = 0 To n - 1
                For Each Rule As TerminalRule In TerminalRules
                    If Rule.CanProduce(S(I)) AndAlso Not A(I, 0).Contains(Rule.From) Then
                        A(I, 0).Add(Rule.From)
                    End If
                Next
            Next



            For j As Integer = 2 To n
                For i As Integer = 1 To n - j + 1
                    For k As Integer = 1 To j - 1
                        For Each Rule In NonTerminalRules
                            If A(i - 1, k - 1).Contains(Rule.ToValue1) AndAlso
                                A(i + k - 1, j - k - 1).Contains(Rule.ToValue2) AndAlso
                                Not A(i - 1, j - 1).Contains(Rule.From) Then
                                A(i - 1, j - 1).Add(Rule.From)
                            End If
                        Next
                    Next
                Next
            Next
            'PrintPyramide(A, n)
            Return A(0, n - 1).Contains(Startsymbol)
        End Function

        Private Sub PrintPyramide(A(,) As List(Of NonTerminal), n As Integer)
            For j As Integer = 0 To n - 1
                Dim Line As String = ""
                For i As Integer = 0 To n - 1 - j
                    Dim s = ""
                    For Each Item In A(i, j)
                        s += Item.ToString
                    Next
                    Line += PadString(s) + "|"
                Next
                Console.WriteLine(Line)
            Next
        End Sub

        Private Function PadString(S As String) As String
            While S.Length < 5
                S += " "
            End While
            Return S
        End Function

        ReadOnly Terminals As New Dictionary(Of Char, Day19.Terminal)
        ReadOnly NonTerminals As New Dictionary(Of Integer, Day19.NonTerminal)
        Public Sub New(Rules As List(Of String), StartSymbol As String)
            NonTerminalRules = New List(Of NonTerminalRule)
            TerminalRules = New List(Of TerminalRule)
            For Each Rule In Rules
                Dim m = Rule.Match("{0}: {1}")
                If m(1).Contains(" ") Then
                    Dim m2 = m(1).Trim.Split(" "c)
                    NonTerminalRules.Add(New NonTerminalRule(
                        AddNonTerminal(m(0)),
                        AddNonTerminal(m2(0)),
                        AddNonTerminal(m2(1))
                    ))
                Else ''Is TerminalRule
                    TerminalRules.Add(New TerminalRule(AddNonTerminal(m(0)), AddTerminal(m(1))))
                End If
            Next
            Me.Startsymbol = AddNonTerminal(StartSymbol)
        End Sub

        Private Function AddNonTerminal(T As String) As NonTerminal
            Dim first = CInt(T)
            If Not NonTerminals.ContainsKey(first) Then
                NonTerminals.Add(first, New NonTerminal(T))
            End If
            Return NonTerminals(first)
        End Function

        Private Function AddTerminal(T As String) As Terminal
            T = T.Replace("""", "")
            If Not Terminals.ContainsKey(T(0)) Then
                Terminals.Add(T(0), New Terminal(T(0)))
            End If
            Return Terminals(T(0))
        End Function
    End Class


    Public Module ChompskyNormalizer

        Public Function Normalize(L As List(Of String)) As List(Of String)
            Dim L1 = RemoveSingles(RemoveMoreThanTwo(RemoveSlasches(L)))
            Debug.Assert(AssertRulesAreCompsky(L1))
            Return L1.Map(Function(x) x.Item1 + ": " + x.Item2.Trim)
        End Function


        Private Function RemoveSingles(L As List(Of (String, String))) As List(Of (String, String))
            Dim f As Predicate(Of (String, String)) = Function(x) IsTerminal(x.Item2) OrElse x.Item2.Trim.Split(" "c).Count >= 2
            Dim Singles = L.Filter(f)
            L.RemoveAll(Function(x) Not f(x))

            Dim D As New Dictionary(Of String, List(Of String))
            Dim ret As New List(Of (String, String))
            For Each Item In L
                ret.Add(Item)
                If Not D.ContainsKey(Item.Item1) Then
                    D.Add(Item.Item1, New List(Of String))
                End If
                D(Item.Item1.Trim).Add(Item.Item2)
            Next

            For Each S In Singles
                If Not D.ContainsKey(S.Item2.Trim) Then
                    D.Add(S.Item2, New List(Of String))
                End If
                For Each Item In D(S.Item2.Trim)
                    ret.Add((S.Item1, Item))
                Next
            Next
            Return ret
        End Function

        Private Function IsTerminal(S As String) As Boolean
            Return S.Contains("""")
        End Function


        Private Function RemoveMoreThanTwo(L As List(Of (String, String))) As List(Of (String, String))
            Dim ret As New List(Of (String, String))
            For Each Item In L
                Dim sp = Item.Item2.Trim.Split(" "c)
                If sp.Count > 2 Then
                    Dim Term = Item.Item2
                    Dim NewNonterm = (L.Count * 2 + ret.Count).ToString
                    While sp.Count > 2
                        Term = Term.Replace(sp(0) + " " + sp(1), NewNonterm)
                        ret.Add((NewNonterm, sp(0) + " " + sp(1)))
                        sp = Term.Split(" "c)
                        NewNonterm = (L.Count * 2 + ret.Count).ToString
                    End While
                    ret.Add((Item.Item1, Term))
                Else
                    ret.Add(Item)
                End If
            Next
            Return ret
        End Function

        Private Function RemoveSlasches(L As List(Of String)) As List(Of (String, String))
            Dim ret As New List(Of (String, String))
            For Each item In L
                If item.Contains("|"c) Then
                    Dim m = item.Split(":"c)
                    For Each SubItem In m(1).Split("|"c)
                        ret.Add((m(0), SubItem.TrimEnd))
                    Next
                Else
                    Dim sp = item.Split(":"c)
                    ret.Add((sp(0), sp(1)))
                End If
            Next
            Return ret
        End Function

        Private Function AssertRulesAreCompsky(Rules As List(Of (String, String))) As Boolean
            Dim i As Integer
            For Each Item In Rules
                Dim m = {Item.Item1, Item.Item2}
                For Each SubItem In m(1).Split("|"c)
                    Dim m2 = SubItem.Trim.MatchToDict("{0} {1}")
                    If m2.FoundMatch AndAlso Not (Integer.TryParse(m2.Matches(0), i) AndAlso Integer.TryParse(m2.Matches(1), i)) Then
                        Return False
                    ElseIf Not m2.FoundMatch Then
                        If Not SubItem.Trim.Replace("""", "").Length = 1 Then
                            Return False
                        End If
                    End If
                Next
            Next
            Return True
        End Function


    End Module

End Namespace
