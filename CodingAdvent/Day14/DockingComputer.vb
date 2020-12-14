Namespace Day14


    Public Class DockingComputer

        Private BitMask As String
        Private ReadOnly Memory As New Dictionary(Of Integer, String)

        Public Sub ReciveCommand(Cmd As String)
            Cmd = Cmd.Replace(" ", "")

            If Cmd.StartsWith("mask") Then
                Dim s = Cmd.Match("mask={0}")
                HandleMaskSet(s(0))
            ElseIf Cmd.StartsWith("mem") Then
                Dim s = Cmd.Match("mem[{0}]={1}")
                HandleMemSet(CInt(s(0)), s(1))
            End If
        End Sub

        Private Sub HandleMemSet(Address As Integer, Value As String)
            Dim NewVal = ApplyMask(DecToBinary(CULng(Value)))
            If Memory.ContainsKey(Address) Then
                Memory(Address) = NewVal
            Else
                Memory.Add(Address, NewVal)
            End If
        End Sub

        Private Function DecToBinary(S As ULong) As String
            Dim ret As New List(Of Char)
            For I As Integer = 35 To 0 Step -1
                Dim exp = CULng(Math.Pow(2, I))
                Dim v = CULng(Math.Floor(S / exp))
                S -= exp * v
                ret.Add(v.ToString()(0))
            Next
            Return New String(ret.ToArray)
        End Function

        Public Function GetSumm() As ULong
            Dim Summ As ULong = 0
            For Each Item In Memory.Values
                Summ += Item.Fold(Function(c, acc) CULng(2) * acc + CULng(CInt(c.ToString)), CULng(0))
            Next
            Return Summ
        End Function

        Private Sub HandleMaskSet(NewMask As String)
            BitMask = NewMask
        End Sub

        Private Function ApplyMask(ByVal Value As String) As String
            Debug.Assert(BitMask.Length = Value.Length)
            Dim ret = New String(Value.Zip(BitMask, Function(c1, c2) If(c2 = "X"c, c1, c2)).ToArray)

            Return ret
        End Function


    End Class

    Public Class DockingComputer2

        Private BitMask As String
        Private ReadOnly Memory As New Dictionary(Of ULong, Integer)

        Public Sub ReciveCommand(Cmd As String)
            Cmd = Cmd.Replace(" ", "")

            If Cmd.StartsWith("mask") Then
                Dim s = Cmd.Match("mask={0}")
                HandleMaskSet(s(0))
            ElseIf Cmd.StartsWith("mem") Then
                Dim s = Cmd.Match("mem[{0}]={1}")
                HandleMemSet(ApplyMask(DecToBinary(CULng(s(0)))), CInt(s(1)))
            End If
        End Sub

        Private Sub HandleMemSet(Address As String, Value As Integer)
            Dim i = Address.IndexOf("X"c)
            If i >= 0 Then
                Dim address1 = Address.ToCharArray()
                address1(i) = 0.ToString()(0)
                Dim address2 = Address.ToCharArray()
                address2(i) = 1.ToString()(0)
                HandleMemSet(New String(address1), Value)
                HandleMemSet(New String(address2), Value)
            Else
                Memory.AddOrSet(Address.Fold(Function(c, acc) CULng(2) * acc + CULng(CInt(c.ToString)), CULng(0)), Value)
            End If
        End Sub

        Private Function DecToBinary(S As ULong) As String
            Dim ret As New List(Of Char)
            For I As Integer = 35 To 0 Step -1
                Dim exp = CULng(Math.Pow(2, I))
                Dim v = CULng(Math.Floor(S / exp))
                S -= exp * v
                ret.Add(v.ToString()(0))
            Next
            Return New String(ret.ToArray)
        End Function

        Public Function GetSumm() As ULong
            Return Memory.Values.Fold(Function(c, acc) acc + CULng(CInt(c.ToString)), CULng(0))
        End Function

        Private Sub HandleMaskSet(NewMask As String)
            BitMask = NewMask
        End Sub

        Private Function ApplyMask(ByVal Address As String) As String
            While Address.Length < 36
                Address = "0" + Address
            End While
            Debug.Assert(BitMask.Length = Address.Length)
            Dim ret = New String(Address.Zip(BitMask, Function(c1, c2) If(c2 = "0"c, c1, c2)).ToArray)

            Return ret
        End Function

    End Class

End Namespace