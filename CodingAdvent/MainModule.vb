﻿Module MainModule

    Sub Main()
        My.Computer.FileSystem.CurrentDirectory = "C:\Users\iansk\source\repos\CodingAdvent\CodingAdvent\Inputs"

        Day9()
        Console.ReadKey()
    End Sub

    Private Sub Day9()
        Dim RawInput = GetInpLineByLine(9).Map(Function(s) CType(s, ULong))
        Dim XCypher As New Day9.XChiperReader(25)

        Try
            For Each Item In RawInput
                XCypher.ReadInteger(Item)
            Next
        Catch ex As ArgumentException
            Console.WriteLine(XCypher.PeekLast.ToString + " did not match the pattern")
        End Try

        Dim InvalidNumber = XCypher.PeekLast
        Dim SetSize As Integer = 2

        While SetSize <= RawInput.Count

            For I As Integer = 0 To RawInput.Count - SetSize
                Dim SubSet = RawInput.GetRange(I, SetSize)
                If SubSet.Fold(Function(v, acc) acc + v, CType(0, ULong)) = InvalidNumber Then
                    SubSet.Sort()
                    Console.WriteLine(String.Format("The weakness is {0}", SubSet.First + SubSet.Last))
                    Exit Sub
                End If
            Next

            SetSize += 1
        End While

        Console.WriteLine("Did not find a weakness")
    End Sub

    Private Sub Day8()
        Dim RawInput = GetInpLineByLine(8)
        Dim HHConsole As New Day8.HandHeldConsoleSimulator(RawInput.ToArray)
        HHConsole.StartSimulation()
        Console.WriteLine("The final accumulatorvalue is " + HHConsole.AccumulatorValue.ToString)


        For I As Integer = 0 To RawInput.Count - 1

            Dim repFunc As Func(Of String, String)
            If RawInput(I).StartsWith("nop") Then
                repFunc = Function(S) S.Replace("nop", "jmp")
            ElseIf RawInput(I).StartsWith("jmp") Then
                repFunc = Function(S) S.Replace("jmp", "nop")
            Else
                Continue For
            End If

            Dim CopyOfCode = RawInput.ToArray
            CopyOfCode(I) = repFunc(CopyOfCode(I))
            Dim TestConsole As New Day8.HandHeldConsoleSimulator(CopyOfCode)
            TestConsole.StartSimulation()

            If TestConsole.Terminated Then
                Console.WriteLine("Terminated!")
                Console.WriteLine("Replaced cmd in line " + I.ToString)
                Console.WriteLine("The final accumulatorvalue of the fixed console is " + TestConsole.AccumulatorValue.ToString)
                Exit Sub
            End If
        Next

        Console.WriteLine("Found nothing...")
    End Sub

    Private Sub Day7()
        Dim RawInput = GetInpLineByLine(7)
        Dim InpRules = RawInput.Map(Function(Line As String) New Day7.Rule(Line))

        Dim CanDeriveGoldDict As New Dictionary(Of String, Day7.Rule)
        Dim NotCheckedParants As New List(Of Day7.Rule)


        NotCheckedParants = InpRules.Map(Function(x) x)
        NotCheckedParants.RemoveAll(Function(r) Not r.OuterBagCanContain("shiny gold", 1))
        For Each Item In NotCheckedParants
            CanDeriveGoldDict.Add(Item.OuterBag, Item)
        Next

        If NotCheckedParants.Count > 0 Then
            Do
                Dim NewFoundRules As New List(Of Day7.Rule)

                Dim MasterRule = NotCheckedParants(0)
                NotCheckedParants.RemoveAt(0)

                Dim CopyInp = InpRules.Map(Function(x) x)
                CopyInp.RemoveAll(Function(r) Not r.OuterBagCanContain(MasterRule.OuterBag, 1))
                CopyInp.RemoveAll(Function(r) CanDeriveGoldDict.ContainsKey(r.OuterBag))
                NewFoundRules.AddRange(CopyInp)

                For Each Item In NewFoundRules
                    CanDeriveGoldDict.Add(Item.OuterBag, Item)
                    NotCheckedParants.Add(Item)
                Next
            Loop While NotCheckedParants.Count > 0
        End If


        Console.WriteLine(String.Format("{0} Rules can lead to you packing a gold bag", CanDeriveGoldDict.Count))


        Dim RuleDict As New Dictionary(Of String, Day7.Rule)
        For Each Item In InpRules
            RuleDict.Add(Item.OuterBag, Item)
        Next

        Console.WriteLine(String.Format("{0} Bags are needed for a shiny gold bag", RuleDict("shiny gold").BagsNeeded(RuleDict) - 1))
    End Sub


    Private Sub Day6()
        ''I know that this is not clean code, but I just want to try some stuff out :P
        Dim SummOfDistincAnswersByGroups = GetInpLineByLine(6).
            Fold(Function(Line As String, Acc As List(Of List(Of Char)))
                     If Line = "" Then
                         Acc.Add(New List(Of Char))
                     Else
                         Acc(Acc.Count - 1).AddRange(Line.ToCharArray)
                     End If
                     Return Acc
                 End Function, {New List(Of Char)}.ToList).
            Map(Function(L As List(Of Char)) L.Distinct.Count).
            Fold(Function(i As Integer, acc As Integer) i + acc, 0)

        Console.WriteLine(String.Format("The Summ of Answers anyone answered yes to is {0}", SummOfDistincAnswersByGroups))

        Dim SummOfAnswersWhereEveryOneSaidYes = GetInpLineByLine(6).
            Fold(Function(Line As String, Acc As List(Of Dictionary(Of Char, Integer)))
                     If Line = "" Then
                         Dim Dict = New Dictionary(Of Char, Integer) From {
                             {"#"c, 0}
                         }
                         Acc.Add(Dict)
                     Else
                         Dim Dict = Acc(Acc.Count - 1)
                         For Each C In Line.ToArray
                             If Not Dict.ContainsKey(C) Then
                                 Dict.Add(C, 1)
                             Else
                                 Dict(C) += 1
                             End If
                         Next
                         Dict("#"c) += 1
                     End If
                     Return Acc
                 End Function, {New Dictionary(Of Char, Integer) From {{"#"c, 0}}}.ToList).
            Map(Function(Dict As Dictionary(Of Char, Integer)) As Integer
                    Dim I As Integer = 0
                    For Each KV In Dict
                        If Not KV.Key = "#"c AndAlso KV.Value = Dict("#"c) Then
                            I += 1
                        End If
                    Next
                    Return I
                End Function).Sum()
        Console.WriteLine(String.Format("Summ where EVERYONE said yes is {0}", SummOfAnswersWhereEveryOneSaidYes))
    End Sub

    Private Sub Day5()

        Dim BinStrToNbr = Function(S As String) As Integer
                              Return S.ToCharArray.Fold(Function(c, acc) Integer.Parse(c) + 2 * acc, 0)
                          End Function

        Dim InpSplitRowCol = GetInpLineByLine(5).
            Map(Function(s As String) s.Replace("F", "0").Replace("B", "1").Replace("L", "0").Replace("R", "1")).
            Map(Function(s As String) {s.Substring(0, 7), s.Substring(7)})
        Dim Bin = InpSplitRowCol.Map(Function(s As String()) {BinStrToNbr(s(0)), BinStrToNbr(s(1))})
        Dim SeatIDs = Bin.Map(Function(RowCol As Integer()) RowCol(0) * 8 + RowCol(1))
        SeatIDs.Sort()

        Dim Nbrs = GetIndexCollection(0, 128 * 8 + 7).ToList
        Nbrs.RemoveAll(Function(x As Integer) SeatIDs.Contains(x))

        Dim MySeat As Integer = -1
        For Each PossibleSeatID In Nbrs
            If SeatIDs.Contains(PossibleSeatID - 1) AndAlso SeatIDs.Contains(PossibleSeatID + 1) Then
                MySeat = PossibleSeatID
                Exit For
            End If
        Next
        Console.WriteLine(String.Format("The highest seatID is {0}", SeatIDs.Last))
        Console.WriteLine("My Seat is " + MySeat.ToString)
    End Sub

    Private Sub Day4()

        Dim NeededTags = {"pid", "ecl", "hcl", "byr", "iyr", "eyr", "hgt"}

        Dim MappingFunction = Function(line As String, PassPortStringList As List(Of String)) As List(Of String)
                                  If line = "" Then
                                      PassPortStringList.Add("")
                                  Else
                                      Dim i = PassPortStringList.Count - 1
                                      If PassPortStringList(i) = "" Then
                                          PassPortStringList(i) = line
                                      Else
                                          PassPortStringList(i) += " " + line
                                      End If
                                  End If
                                  Return PassPortStringList
                              End Function

        Dim Inp = GetInpLineByLine(4).Fold(MappingFunction, {""}.ToList)
        Dim Passports = Inp.Map(Function(Data As String) New Day4.Passport(Data))
        Passports.RemoveAll(Function(P As Day4.Passport) Not P.HasKeys(NeededTags))
        Console.WriteLine(String.Format("There are {0} passports with valid keys", Passports.Count))

        Passports.RemoveAll(Function(P As Day4.Passport) Not P.HasValidData)
        Console.WriteLine(String.Format("There are {0} valid passports", Passports.Count))
    End Sub

    Private Sub Day3()
        Dim Inp = GetInpLineByLine(3)
        Dim Terra As New Day3.Terra(Inp)
        Dim Slopes() As Tuple(Of Integer, Integer) = {
            New Tuple(Of Integer, Integer)(1, 1),
            New Tuple(Of Integer, Integer)(3, 1),
            New Tuple(Of Integer, Integer)(5, 1),
            New Tuple(Of Integer, Integer)(7, 1),
            New Tuple(Of Integer, Integer)(1, 2)
        }

        Dim TreesBySlope = Slopes.Map(Function(s As Tuple(Of Integer, Integer)) Terra.CalcTreeEncounters(s))

        Dim MultErg = TreesBySlope.Fold(Function(v As Integer, acc As Integer) v * acc, 1)

        Dim Zip = TreesBySlope.Zip(Slopes, Function(x As Integer, s As Tuple(Of Integer, Integer)) String.Format("({1},{2}):{0}", x, s.Item1, s.Item2))
        Dim ConsoleOutp = Zip.Fold(Function(s As String, acc As String) acc & ", " & s, "").Substring(2)

        Console.WriteLine(String.Format("Multiplied {0} trees.", MultErg))
        Console.WriteLine(ConsoleOutp)
    End Sub

    Private Sub Day2()
        Dim Func As New Func(Of String, Condition)(
            Function(InpStr As String) As Condition
                Dim split = InpStr.Split(":"c)
                Return New Condition(split(0), split(1))
            End Function
            )

        Dim Inp = GetInpLineByLine(2).Map(Func)
        Dim Inp2 = Inp.Clone

        Inp.RemoveAll(Function(C As Condition) Not C.IsValidOld)
        Inp2.RemoveAll(Function(C As Condition) Not C.IsValidNew)

        Console.WriteLine(Inp.Count.ToString + " Passwords are correct in old!")
        Console.WriteLine(Inp2.Count.ToString + " Passwords are correct in new!")
    End Sub

    Private Class Condition

        Public Sub New(CondString As String, Password As String)
            Dim CondStringSplit = CondString.Split(" "c)
            Dim CondNbrSplit = CondStringSplit(0).Split("-"c)

            Password = Password.Trim

            Dim c As Char = CChar(CondStringSplit(1))
            Dim lower As Integer = CInt(CondNbrSplit(0))
            Dim upper As Integer = CInt(CondNbrSplit(1))

            Dim removed = Password.Replace(c, "")
            Dim CharCount = Password.Length - removed.Length

            IsValidOld = CharCount >= lower And CharCount <= upper
            IsValidNew = Password.ElementAt(lower - 1) = c Xor Password.ElementAt(upper - 1) = c
        End Sub

        Public ReadOnly Property IsValidOld As Boolean
        Public ReadOnly Property IsValidNew As Boolean
    End Class

    Private Sub Day1()
        Dim StrL = My.Computer.FileSystem.ReadAllText("Inp1.txt").Replace(vbNewLine, "|").Split("|"c)
        Dim IntL As New List(Of Integer)
        For i As Integer = 0 To StrL.Length - 1
            IntL.Add(CInt(StrL(i)))
        Next
        IntL.Sort()

        Dim ToFind As Integer = 2020

        For i As Integer = 0 To IntL.Count - 1
            If IntL.Contains(ToFind - IntL(i)) Then
                Console.WriteLine(((ToFind - IntL(i)) * IntL(i)).ToString)
                Exit For
            End If
        Next
        For u As Integer = 0 To IntL.Count - 1
            For i As Integer = 0 To IntL.Count - 1
                Dim num = IntL(i) + IntL(u)
                If IntL.Contains(ToFind - num) Then
                    Console.WriteLine(((ToFind - num) * IntL(i) * IntL(u)).ToString)
                    Exit Sub
                End If
            Next
        Next
        Console.WriteLine("No Number found")
    End Sub

    Private Function GetInpLineByLine(Index As Integer) As List(Of String)
        Return GetInpLineByLine(Index.ToString)
    End Function

    Private Function GetInpLineByLine(Index As String) As List(Of String)
        Return My.Computer.FileSystem.ReadAllText("Inp" + Index + ".txt").Replace(vbNewLine, "§").Split("§"c).ToList
    End Function

End Module
