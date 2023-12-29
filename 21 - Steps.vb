Module Module21
    Dim Pattern(,) As Node

    Public Sub Steps()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Steps.txt"
        Dim Lijnen() As String = System.IO.File.ReadAllLines(pad)

        ReDim Pattern(Lijnen.Count - 1, Lijnen.First.Length - 1)
        Dim startnode As Node
        Dim index As Integer

        For i = 0 To Lijnen.Count() - 1
            Dim lijn = Lijnen(i)
            For j = 0 To lijn.Length - 1
                Pattern(i, j) = New Node(i, j, lijn(j))
                If lijn(j) = "S" Then
                    Pattern(i, j).value = "."
                    startnode = Pattern(i, j)
                    index = j
                End If
            Next
        Next



        Dim D = 26501365
        Dim w = Pattern.GetUpperBound(0) + 1
        Dim N = (D - index) / w

        Dim E = Flood(startnode, 3 * w)
        Dim O = Flood(startnode, (3 * w) + 1)

        Dim sA = ((3 * w) - 3) / 2 + 1
        Dim A1 = Flood(Pattern(0, 0), sA)
        Dim A2 = Flood(Pattern(0, Pattern.GetUpperBound(1)), sA)
        Dim A3 = Flood(Pattern(Pattern.GetUpperBound(0), 0), sA)
        Dim A4 = Flood(Pattern(Pattern.GetUpperBound(0), Pattern.GetUpperBound(1)), sA)
        Dim A = A1 + A2 + A3 + A4

        Dim sB = (w - 3) / 2 + 1
        Dim B1 = Flood(Pattern(0, 0), sB)
        Dim B2 = Flood(Pattern(0, Pattern.GetUpperBound(1)), sB)
        Dim B3 = Flood(Pattern(Pattern.GetUpperBound(0), 0), sB)
        Dim B4 = Flood(Pattern(Pattern.GetUpperBound(0), Pattern.GetUpperBound(1)), sB)
        Dim B = B1 + B2 + B3 + B4

        Dim T1 = Flood(Pattern(0, index), w)
        Dim T2 = Flood(Pattern(index, 0), w)
        Dim T3 = Flood(Pattern(w - 1, index), w)
        Dim T4 = Flood(Pattern(index, w - 1), w)

        Dim T = T1 + T2 + T3 + T4


        Dim uitkomtst = ((N - 1) ^ 2) * O + (N ^ 2) * E + (N - 1) * A + N * B + T

        Console.WriteLine(uitkomtst)


    End Sub

    Public Function Flood(start As Node, steps As Integer) As Integer
        Dim count = 1
        Dim possibilities As New List(Of Node)
        possibilities.Add(start)

        While count < steps
            count += 1
            Dim newPosibilities As New List(Of Node)
            For Each N In Possibilities
                If N.x > Pattern.GetLowerBound(0) AndAlso Pattern(N.x - 1, N.y).value = "." Then
                    If Not newPosibilities.Contains(Pattern(N.x - 1, N.y)) Then
                        newPosibilities.Add(Pattern(N.x - 1, N.y))

                    End If
                End If
                If N.x < Pattern.GetUpperBound(0) AndAlso Pattern(N.x + 1, N.y).value = "." Then
                    If Not newPosibilities.Contains(Pattern(N.x + 1, N.y)) Then
                        newPosibilities.Add(Pattern(N.x + 1, N.y))

                    End If
                End If
                If N.y > Pattern.GetLowerBound(1) AndAlso Pattern(N.x, N.y - 1).value = "." Then
                    If Not newPosibilities.Contains(Pattern(N.x, N.y - 1)) Then
                        newPosibilities.Add(Pattern(N.x, N.y - 1))

                    End If
                End If
                If N.y < Pattern.GetUpperBound(1) AndAlso Pattern(N.x, N.y + 1).value = "." Then
                    If Not newPosibilities.Contains(Pattern(N.x, N.y + 1)) Then
                        newPosibilities.Add(Pattern(N.x, N.y + 1))

                    End If
                End If
            Next

            Possibilities = newPosibilities
        End While

        Return possibilities.Count

    End Function


    Public Class Node
        Public x As Integer
        Public y As Integer
        Public value As Char
        Public flooded As Boolean = False

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

