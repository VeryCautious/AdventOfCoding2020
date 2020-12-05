Public Class Passport

    Private ReadOnly Dict As New Dictionary(Of String, String)

    Public Sub New(Data As String)
        Dim kv = Data.Split(" "c).Map(
            Function(s As String) New KeyValuePair(Of String, String)(s.Split(":"c)(0), s.Split(":"c)(1))
            )
        For Each KvItem In kv
            Dict.Add(KvItem.Key, KvItem.Value)
        Next
    End Sub

    Public Function HasKeys(RequiredKeys As String()) As Boolean
        Dim FoldFunc = Function(Val As String, Acc As Boolean) As Boolean
                           Return Acc AndAlso Dict.ContainsKey(Val)
                       End Function

        Return RequiredKeys.Fold(FoldFunc, True)
    End Function

    Public ReadOnly Property BirthYear As Integer
        Get
            Return CInt(Dict("byr"))
        End Get
    End Property

    Public ReadOnly Property IssueYear As Integer
        Get
            Return CInt(Dict("iyr"))
        End Get
    End Property

    Public ReadOnly Property ExpireYear As Integer
        Get
            Return CInt(Dict("eyr"))
        End Get
    End Property

    Public ReadOnly Property Hight As String
        Get
            Return Dict("hgt")
        End Get
    End Property

    Public ReadOnly Property HightValue As Integer
        Get
            Return CInt(Hight.Replace("cm", "").Replace("in", ""))
        End Get
    End Property

    Public ReadOnly Property HairColor As String
        Get
            Return Dict("hcl")
        End Get
    End Property

    Public ReadOnly Property EyeColor As String
        Get
            Return Dict("ecl")
        End Get
    End Property

    Public ReadOnly Property PassportID As String
        Get
            Return Dict("pid")
        End Get
    End Property

    Public Function HasValidData() As Boolean
        If Not (BirthYear >= 1920 And BirthYear <= 2002) Then
            Return False
        End If

        If Not IssueYear.IsBetween(2010, 2020) Then
            Return False
        End If

        If Not ExpireYear.IsBetween(2020, 2030) Then
            Return False
        End If

        If Hight.EndsWith("cm") AndAlso Not HightValue.IsBetween(150, 193) Then
            Return False
        ElseIf Hight.EndsWith("in") AndAlso Not HightValue.IsBetween(59, 76) Then
            Return False
        ElseIf Not (Hight.EndsWith("in") Or Hight.EndsWith("cm")) Then
            Return False
        End If

        Dim ValidHairChars = "0123456789abcdef".ToCharArray
        If Not HairColor.StartsWith("#") Or Not HairColor.Count = 7 Then
            Return False
        End If

        If Not HairColor.Substring(1).ToCharArray.Fold(
            Function(c As Char, accu As Boolean) accu AndAlso ValidHairChars.Contains(c),
            True
            ) Then
            Return False
        End If

        Dim EyeColors = {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}
        If Not EyeColors.Contains(EyeColor) Then
            Return False
        End If

        Dim pid = 0
        If Not (Integer.TryParse(PassportID, pid) AndAlso PassportID.Count = 9) Then
            Return False
        End If

        Return True
    End Function

    Public Overrides Function ToString() As String
        Return BirthYear.ToString + " " + IssueYear.ToString + " " + ExpireYear.ToString + vbNewLine +
            Hight.ToString + " " + HairColor + " " + EyeColor.ToString + vbNewLine +
            PassportID
    End Function

End Class
