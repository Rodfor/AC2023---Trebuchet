Imports System.Net.NetworkInformation
Imports System.Reflection
Imports Microsoft.VisualBasic.FileIO
Imports System.Runtime.Caching
Imports System.IO

Module Module18

    Public Sub Dig()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Dig.txt"
        Dim lijnen As New List(Of String())

        Using parser = New TextFieldParser(pad)
            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters(" ")

            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                lijnen.Add(fields)
            End While

            parser.Close()
            parser.Dispose()
        End Using

        Dim Plan As New List(Of Rule)
        Dim lengte As Long = 2

        For Each L In lijnen
            Dim R As New Rule()
            Select Case L(0)
                Case "U"
                    R.dir = Direction.Up
                Case "D"
                    R.dir = Direction.Down
                Case "L"
                    R.dir = Direction.Left
                Case "R"
                    R.dir = Direction.Right
            End Select

            R.Length = Val(L(1))
            R.RGBHEX = L(2)

            Plan.Add(R)
            lengte += R.Length

        Next


        lengte = 2
        For Each R In Plan
            R.HEXtoRule()
            lengte += R.Length
            Console.WriteLine(R.dir.ToString + " - " + R.Length.ToString)
        Next

        Dim Vertices As New List(Of Tuple(Of Integer, Integer))

        Dim prevX = 0
        Dim prevY = 0

        For Each R In Plan
            Select Case R.dir
                Case Direction.Up
                    Vertices.Add(Tuple.Create(prevX - R.Length, prevY))
                Case Direction.Down
                    Vertices.Add(Tuple.Create(prevX + R.Length, prevY))
                Case Direction.Left
                    Vertices.Add(Tuple.Create(prevX, prevY - R.Length))
                Case Direction.Right
                    Vertices.Add(Tuple.Create(prevX, prevY + R.Length))
            End Select

            prevX = Vertices.Last.Item1
            prevY = Vertices.Last.Item2
            Console.WriteLine(prevX.ToString + "," + prevY.ToString)
        Next

        Dim P1 As New System.Numerics.BigInteger(0)
        Dim P2 As New System.Numerics.BigInteger(0)

        Dim Plus1 As New System.Numerics.BigInteger(Vertices(Vertices.Count - 1).Item1)
        Dim Plus2 As New System.Numerics.BigInteger(Vertices(Vertices.Count - 1).Item2)
        Plus1 *= Vertices(0).Item2
        Plus2 *= Vertices(0).Item1
        P1 += Plus1
        P2 += Plus2

        For i = 0 To Vertices.Count - 2
            Plus1 = New System.Numerics.BigInteger(Vertices(i).Item1)
            Plus2 = New System.Numerics.BigInteger(Vertices(i).Item2)
            Plus1 *= Vertices(i + 1).Item2
            Plus2 *= Vertices(i + 1).Item1
            P1 += Plus1
            P2 += Plus2
        Next

        Dim Area = P1
        Area -= P2
        Area /= 2
        If Area.Sign = -1 Then Area *= -1

        Console.WriteLine(Area + lengte / 2)

        'Dim Pattern(400, 400) As Node

        'Dim posX = 150
        'Dim posY = 0


        ''Dim Pattern(10, 10) As Node

        ''Dim posX = 0
        ''Dim posY = 0

        'Dim startNode = New Node With {
        '    .x = posX,
        '    .y = posY,
        '    .value = "#"
        '}

        'Pattern(posX, posY) = startNode

        'For Each R In Plan
        '    Select Case R.dir
        '        Case Direction.Up
        '            For i = 1 To R.Length
        '                Dim N As New Node With {
        '                    .value = "#",
        '                    .x = posX - i,
        '                    .y = posY
        '                }
        '                Pattern(posX - i, posY) = N
        '            Next
        '            posX -= R.Length
        '        Case Direction.Down
        '            For i = 1 To R.Length
        '                Dim N As New Node With {
        '                    .value = "#",
        '                    .x = posX + i,
        '                    .y = posY
        '                }
        '                Pattern(posX + i, posY) = N
        '            Next
        '            posX += R.Length
        '        Case Direction.Left
        '            For i = 1 To R.Length
        '                Dim N As New Node With {
        '                    .value = "#",
        '                    .x = posX,
        '                    .y = posY - i
        '                }
        '                Pattern(posX, posY - i) = N
        '            Next
        '            posY -= R.Length
        '        Case Direction.Right
        '            For i = 1 To R.Length
        '                Dim N As New Node With {
        '                    .value = "#",
        '                    .x = posX,
        '                    .y = posY + i
        '                }
        '                Pattern(posX, posY + i) = N
        '            Next
        '            posY += R.Length
        '    End Select
        'Next

        'Dim som As Integer = 0

        'For i = Pattern.GetLowerBound(0) To Pattern.GetUpperBound(0)
        '    For j = Pattern.GetLowerBound(1) To Pattern.GetUpperBound(1)
        '        If Pattern(i, j) Is Nothing Then
        '            Dim N As New Node With {
        '                    .value = ".",
        '                    .x = i,
        '                    .y = j
        '                }
        '            Pattern(i, j) = N
        '        Else
        '            som += 1
        '        End If
        '        'Console.Write(Pattern(i, j).value)
        '    Next

        '    Console.WriteLine("")
        'Next

        'Dim hashtags As New List(Of Integer)
        'Dim x As Integer = Pattern.GetLowerBound(0)

        'While hashtags.Count <> 2
        '    hashtags = New List(Of Integer)

        '    For y = Pattern.GetLowerBound(1) To Pattern.GetUpperBound(1)
        '        If Pattern(x, y).value = "#" Then hashtags.Add(y)
        '    Next
        '    x += 1
        'End While

        'Dim Flooded As New List(Of Node)
        'Dim startflood = Pattern(x - 1, hashtags(0) + 1)
        'startflood.value = "#"
        'Flooded.Add(startflood)

        'While Flooded.Count > 0
        '    Dim newFlooded As New List(Of Node)
        '    For Each N In Flooded
        '        som += 1
        '        If Pattern(N.x - 1, N.y).value = "." Then
        '            Pattern(N.x - 1, N.y).value = "#"
        '            newFlooded.Add(Pattern(N.x - 1, N.y))
        '        End If
        '        If Pattern(N.x + 1, N.y).value = "." Then
        '            Pattern(N.x + 1, N.y).value = "#"
        '            newFlooded.Add(Pattern(N.x + 1, N.y))
        '        End If
        '        If Pattern(N.x, N.y - 1).value = "." Then
        '            Pattern(N.x, N.y - 1).value = "#"
        '            newFlooded.Add(Pattern(N.x, N.y - 1))
        '        End If
        '        If Pattern(N.x, N.y + 1).value = "." Then
        '            Pattern(N.x, N.y + 1).value = "#"
        '            newFlooded.Add(Pattern(N.x, N.y + 1))
        '        End If
        '    Next
        '    Flooded = newFlooded
        'End While

        'For i = Pattern.GetLowerBound(0) To Pattern.GetUpperBound(0)
        '    For j = Pattern.GetLowerBound(1) To Pattern.GetUpperBound(1)
        '        Console.Write(Pattern(i, j).value)
        '    Next
        '    Console.WriteLine("")
        'Next

        'Console.WriteLine(som)



    End Sub


    Public Class Rule
        Public dir As Direction
        Public Length As Integer
        Public RGBHEX As String

        Public Sub HEXtoRule()

            RGBHEX = RGBHEX.Trim("(").Trim(")").Trim("#")
            Select Case Right(RGBHEX, 1)
                Case "0"
                    dir = Direction.Right
                Case "1"
                    dir = Direction.Down
                Case "2"
                    dir = Direction.Left
                Case "3"
                    dir = Direction.Up
            End Select

            Dim Hex = Left(RGBHEX, 5)
            Length = Convert.ToInt32(Hex, 16)
        End Sub
    End Class

    Public Class Node
        Public x As Integer
        Public y As Integer
        Public value As Char
    End Class
End Module

