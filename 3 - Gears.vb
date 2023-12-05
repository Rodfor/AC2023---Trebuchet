Imports Microsoft.VisualBasic.FileIO
Imports System.Text.RegularExpressions
Module Module3
    Public Sub Gears()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Gear.txt"
        Dim lijnen As New List(Of String)

        Using parser = New TextFieldParser(pad)
            parser.SetDelimiters(" ")
            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                lijnen.Add(fields(0))
            End While

            parser.Close()
            parser.Dispose()
        End Using

        Dim columns As Integer = lijnen(0).Length
        Dim rows As Integer = lijnen.Count

        Dim array(rows - 1, columns - 1) As Char
        Dim i As Integer = 0

        For Each lijn In lijnen
            Dim j As Integer = 0
            For Each c As Char In lijn
                array(i, j) = lijn(j)
                j += 1
            Next
            i += 1
        Next

        Dim r = 0
        While r < rows
            Dim c = 0
            While c < columns
                If IsNumeric(array(r, c)) Then
                    Dim lastindex As Integer = FindLastNumber(array, r, c, columns)
                    Dim nummer As String = ""
                    Dim charFound As Boolean = False
                    While c <= lastindex
                        nummer += array(r, c)
                        If charFound OrElse FindChar() Then
                            charFound = True
                        End If
                        c += 1
                    End While

                End If
            End While
        End While


    End Sub

    Public Function FindLastNumber(array As Char(,), r As Integer, c As Integer, columns As Integer) As Integer
        Dim newC As Integer = c + 1
        If newC = columns Then Return c
        If IsNumeric(array(r, newC)) Then
            Return FindLastNumber(array, r, newC, columns)
        Else
            Return c
        End If
    End Function

    Public Function FindChar()

    End Function


End Module
