Imports System.Runtime.CompilerServices

Module Ext

    <Extension>
    Public Function Map(Of T, H)(L1 As List(Of T), Func As Func(Of T, H)) As List(Of H)
        Dim res As New List(Of H)

        For Each Item In L1
            res.Add(Func.Invoke(Item))
        Next

        Return res
    End Function

    <Extension>
    Public Function Clone(Of T)(L1 As List(Of T)) As List(Of T)
        Return L1.Map(Function(Item As T) As T
                          Return Item
                      End Function)
    End Function

End Module
