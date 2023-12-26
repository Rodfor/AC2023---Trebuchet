Module Module21
    Public Sub Steps()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Steps.txt"
        Dim Lijnen() As String = System.IO.File.ReadAllLines(pad)

        Dim Pattern(Lijnen.Count - 1, Lijnen.First.Length - 1) As Node
        Dim Possibilities As New List(Of Node)

        For i = 0 To Lijnen.Count() - 1
            Dim lijn = Lijnen(i)
            For j = 0 To lijn.Length - 1
                Pattern(i, j) = New Node(i, j, lijn(j))
                If lijn(j) = "S" Then
                    Possibilities.Add(Pattern(i, j))
                    Possibilities.First.value = "."
                End If
            Next
        Next

        Dim steps As Integer = 70
        Dim count As Integer = 0

        Dim FastUpFirst As Path = Nothing
        Dim FastDownFirst As Path = Nothing
        Dim FastLeftFirst As Path = Nothing
        Dim FastRightFirst As Path = Nothing

        While count < steps
            count += 1
            Dim newPosibilities As New List(Of Node)
            For Each N In Possibilities
                If N.x > Pattern.GetLowerBound(0) AndAlso Pattern(N.x - 1, N.y).value = "." Then
                    If Not newPosibilities.Contains(Pattern(N.x - 1, N.y)) Then
                        newPosibilities.Add(Pattern(N.x - 1, N.y))
                        If N.x = Pattern.GetLowerBound(0) + 1 AndAlso FastUpFirst Is Nothing Then FastUpFirst = New Path(Pattern(N.x - 1, N.y), count)
                    End If
                End If
                If N.x < Pattern.GetUpperBound(0) AndAlso Pattern(N.x + 1, N.y).value = "." Then
                    If Not newPosibilities.Contains(Pattern(N.x + 1, N.y)) Then
                        newPosibilities.Add(Pattern(N.x + 1, N.y))
                        If N.x = Pattern.GetUpperBound(0) - 1 AndAlso FastDownFirst Is Nothing Then FastDownFirst = New Path(Pattern(N.x + 1, N.y), count)
                    End If
                End If
                If N.y > Pattern.GetLowerBound(1) AndAlso Pattern(N.x, N.y - 1).value = "." Then
                    If Not newPosibilities.Contains(Pattern(N.x, N.y - 1)) Then
                        newPosibilities.Add(Pattern(N.x, N.y - 1))
                        If N.y = Pattern.GetLowerBound(1) + 1 AndAlso FastLeftFirst Is Nothing Then FastLeftFirst = New Path(Pattern(N.x, N.y - 1), count)
                    End If
                End If
                If N.y < Pattern.GetUpperBound(1) AndAlso Pattern(N.x, N.y + 1).value = "." Then
                    If Not newPosibilities.Contains(Pattern(N.x, N.y + 1)) Then
                        newPosibilities.Add(Pattern(N.x, N.y + 1))
                        If N.y = Pattern.GetUpperBound(1) - 1 AndAlso FastRightFirst Is Nothing Then FastRightFirst = New Path(Pattern(N.x, N.y + 1), count)
                    End If
                End If
            Next

            Possibilities = newPosibilities
            Console.WriteLine(Possibilities.Count)
        End While

        Console.WriteLine(Possibilities.Count)

        For i = 0 To Pattern.GetUpperBound(0)
            For j = 0 To Pattern.GetUpperBound(1)
                If Pattern(i, j).value = "." AndAlso Not Possibilities.Contains(Pattern(i, j)) And (i + j) Mod 2 = 1 Then
                    Console.WriteLine(i.ToString + "," + j.ToString + " unreachable")
                End If
            Next
        Next

    End Sub


    Public Class Node
        Public x As Integer
        Public y As Integer
        Public value As Char

        Public Sub New(x As Integer, y As Integer, value As Char)
            Me.x = x
            Me.y = y
            Me.value = value
        End Sub
    End Class

    Public Class Path
        Public Node As Node
        Public Length As Integer

        Public Sub New(node As Node, length As Integer)
            Me.Node = node
            Me.Length = length
        End Sub
    End Class

End Module

