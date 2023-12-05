Imports Microsoft.VisualBasic.FileIO
Imports System.Text.RegularExpressions

Module Module2
    Public Sub Cube()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Cube.txt"
        Dim lijnen As New Dictionary(Of Integer, String())

        Using parser = New TextFieldParser(pad)
            parser.SetDelimiters(";")
            Dim gamenummer As Integer = 0
            While Not parser.EndOfData
                gamenummer += 1
                Dim fields As String() = parser.ReadFields()
                Dim startpos = fields(0).IndexOf(":") + 2
                fields(0) = fields(0).Substring(startpos)
                lijnen.Add(gamenummer, fields)
            End While

            parser.Close()
            parser.Dispose()
        End Using

        Dim games As New Dictionary(Of Integer, List(Of Dictionary(Of String, Integer)))

        For Each lijn In lijnen
            Dim gameNummer As Integer = lijn.Key
            Dim gameDate As String() = lijn.Value
            Dim reveals = New List(Of Dictionary(Of String, Integer))
            'Console.WriteLine("Game " + gameNummer.ToString())

            For Each reveal In gameDate.ToList()
                Dim cubes = New Dictionary(Of String, Integer)
                cubes.Add("blue", 0)
                cubes.Add("red", 0)
                cubes.Add("green", 0)

                'Console.WriteLine("Reveal")

                Dim cubeList = reveal.Split(",").ToList
                For Each color In cubeList
                    cubes(Regex.Replace(color, "[^a-z]", String.Empty)) = Val(Regex.Replace(color, "[^0-9]", String.Empty))
                    ' Console.WriteLine(Regex.Replace(color, "[^a-z]", String.Empty) + " : " + Regex.Replace(color, "[^0-9]", String.Empty).ToString())
                Next

                reveals.Add(cubes)
            Next

            games.Add(gameNummer, reveals)
        Next

        Dim som As Integer = 0
        Dim power As Integer = 0

        For Each game In games
            Dim failed As Boolean = False
            Dim minCubes = New Dictionary(Of String, Integer)
            minCubes.Add("blue", 0)
            minCubes.Add("red", 0)
            minCubes.Add("green", 0)

            For Each reveal In game.Value
                If reveal("red") > minCubes("red") Then minCubes("red") = reveal("red")
                If reveal("green") > minCubes("green") Then minCubes("green") = reveal("green")
                If reveal("blue") > minCubes("blue") Then minCubes("blue") = reveal("blue")
            Next

            Dim gamePower = minCubes("red") * minCubes("green") * minCubes("blue")
            Console.WriteLine(game.Key.ToString + " : " + gamePower.ToString)
            power += gamePower


            'For Each reveal In game.Value
            '    If reveal("red") > 12 Then failed = True
            '    If reveal("green") > 13 Then failed = True
            '    If reveal("blue") > 14 Then failed = True
            'Next

            'If Not failed Then
            '    Console.WriteLine(game.Key.ToString + " passed")
            '    som += game.Key
            'Else
            '    Console.WriteLine(game.Key.ToString + " failed")
            'End If
        Next

        ' Console.WriteLine(som)
        Console.WriteLine(power)
    End Sub
End Module
