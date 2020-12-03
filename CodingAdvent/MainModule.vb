Imports CautiousDotNetExtensionLib
Module MainModule

    Sub Main()
        My.Computer.FileSystem.CurrentDirectory = "C:\Users\iansk\source\repos\CodingAdvent\CodingAdvent\Inputs"

        Day4()
        Console.ReadKey()
    End Sub


    Private Sub Day4()
        Dim Inp = GetInpLineByLine(4)

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

        Inp.RemoveAll(Function(C As Condition) As Boolean
                          Return Not C.IsValidOld
                      End Function)

        Inp2.RemoveAll(Function(C As Condition) As Boolean
                           Return Not C.IsValidNew
                       End Function)

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
        Return My.Computer.FileSystem.ReadAllText("Inp" + Index.ToString + ".txt").Replace(vbNewLine, "§").Split("§"c).ToList
    End Function

End Module
