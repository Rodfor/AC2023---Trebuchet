Imports System.IO
Imports Microsoft.VisualBasic.FileIO
Imports System.Text.RegularExpressions
Module Module1

    Public Sub Trebuchet()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\trebuchet.txt"
        Dim lijnen As New List(Of String())

        Using parser = New TextFieldParser(pad)
            parser.SetDelimiters(" ")
            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                lijnen.Add(fields)
            End While

            parser.Close()
            parser.Dispose()
        End Using

        Dim Nummer As Integer = 0
        Dim nummers As New List(Of String)

        For Each lijn In lijnen
            Dim tekst As String = lijn(0)

            tekst = tekst.Replace("zero", "z0ro")
            tekst = tekst.Replace("one", "o1e")
            tekst = tekst.Replace("two", "t2o")
            tekst = tekst.Replace("three", "t3ree")
            tekst = tekst.Replace("four", "f4ur")
            tekst = tekst.Replace("five", "f5ve")
            tekst = tekst.Replace("six", "s6x")
            tekst = tekst.Replace("seven", "s7ven")
            tekst = tekst.Replace("eight", "e8ght")
            tekst = tekst.Replace("nine", "n9ne")

            Console.WriteLine(tekst)
            Dim num As String = Regex.Replace(tekst, "[^0-9]", String.Empty)

            Dim firstdigit As String = num.First
            Dim lastdigit As String = ""
            lastdigit = num.Last
            Dim newnum As String = firstdigit + lastdigit
            Nummer += newnum
        Next

        Console.WriteLine(Nummer)

    End Sub

End Module
