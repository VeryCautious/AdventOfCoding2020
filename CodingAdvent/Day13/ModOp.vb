Namespace Day13

    Module ModOp
        Public Function gcd(a As Long, b As Long) As Tuple(Of Long, Long, Long)
            If a = 0 Then
                Return New Tuple(Of Long, Long, Long)(b, 0, 1)
            End If
            Dim t = gcd(b Mod a, a)
            Return New Tuple(Of Long, Long, Long)(t.Item1, t.Item3 - CLng(Math.Floor(b / a)) * t.Item2, t.Item2)
        End Function

        Public Function ModInverse(n As Long, p As Long) As Long
            Dim t = gcd(n, p)
            Debug.Assert(t.Item1 = 1)
            Return t.Item2 Mod p
        End Function

        Public Function ChinRem(busses As List(Of Tuple(Of Integer, Integer)), modulo As Long) As Long
            Dim x As Long = 0

            For Each b In busses
                Dim n As Long = CLng(Math.Floor(modulo / b.Item2))
                Dim inverse As Long = ModInverse(n, CLng(b.Item2))
                x = (x + CLng(b.Item1) * n * inverse) Mod modulo
                If x < 0 Then
                    x += modulo
                End If
            Next
            Return x Mod modulo
        End Function

    End Module

End Namespace
