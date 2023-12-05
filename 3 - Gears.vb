Imports Microsoft.VisualBasic.FileIO
Imports System.Text.RegularExpressions
Module Module3
    Public Sub Gears()
        Dim pad = "C:\Users\Matthias\Source\Repos\Rodfor\AC2023---Trebuchet\Gear.txt"
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

        Dim som As Integer = 0

        Dim r = 0
        While r < rows
            Dim c = 0
            While c < columns
                If array(r, c) = "*" Then
                    som += FindRatio(array, r, c, columns, rows)
                End If
                c += 1
            End While
            r += 1
        End While


        'Dim som As Integer = 0
        'Dim r = 0
        'While r < rows
        '    Dim c = 0
        '    While c < columns
        '        If IsNumeric(array(r, c)) Then
        '            Dim lastindex As Integer = FindLastNumber(array, r, c, columns)
        '            Dim nummer As String = ""
        '            Dim charFound As Boolean = False
        '            While c <= lastindex
        '                nummer += array(r, c)
        '                If charFound OrElse FindChar(array, r, c, columns, rows) Then
        '                    charFound = True
        '                End If
        '                c += 1
        '            End While

        '            If charFound Then
        '                som += nummer
        '                Console.WriteLine(nummer + " : passed")
        '            Else
        '                Console.WriteLine(nummer + " : failed")
        '            End If
        '        End If
        '        c += 1
        '    End While
        '    r += 1
        'End While

        Console.WriteLine(som)
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

    Public Function FindChar(array As Char(,), r As Integer, c As Integer, columns As Integer, rows As Integer)
        If FindCharPos(array, r, c - 1, columns, rows) Then
            Return True
        End If
        If FindCharPos(array, r, c + 1, columns, rows) Then
            Return True
        End If
        If FindCharPos(array, r - 1, c, columns, rows) Then
            Return True
        End If
        If FindCharPos(array, r - 1, c - 1, columns, rows) Then
            Return True
        End If
        If FindCharPos(array, r - 1, c + 1, columns, rows) Then
            Return True
        End If
        If FindCharPos(array, r + 1, c, columns, rows) Then
            Return True
        End If
        If FindCharPos(array, r + 1, c - 1, columns, rows) Then
            Return True
        End If
        If FindCharPos(array, r + 1, c + 1, columns, rows) Then
            Return True
        End If

        Return False

    End Function

    Public Function FindCharPos(array As Char(,), r As Integer, c As Integer, columns As Integer, rows As Integer)
        If c < 0 OrElse c >= columns OrElse r < 0 OrElse r >= rows Then
            Return False
        End If

        If Not IsNumeric(array(r, c)) AndAlso array(r, c) <> "." Then
            Console.WriteLine(array(r, c))
            Return True
        End If

        Return False
    End Function

    Private Function FindRatio(array As Char(,), r As Integer, c As Integer, columns As Integer, rows As Integer) As Integer
        Dim Nummers As New Dictionary(Of Tuple(Of Integer, Integer), Integer)
        If FindNumberPos(array, r, c - 1, columns, rows) Then
            Dim lastIndex = FindLastNumber(array, r, c - 1, columns)
            Dim Firstindex = FindFirstNumber(array, r, c - 1)
            If Not Nummers.ContainsKey(New Tuple(Of Integer, Integer)(r, Firstindex)) Then
                Nummers.Add(New Tuple(Of Integer, Integer)(r, Firstindex), Getnummer(array, r, Firstindex, lastIndex))
            End If
        End If
        If FindNumberPos(array, r, c + 1, columns, rows) Then
            Dim lastIndex = FindLastNumber(array, r, c + 1, columns)
            Dim Firstindex = FindFirstNumber(array, r, c + 1)
            If Not Nummers.ContainsKey(New Tuple(Of Integer, Integer)(r, Firstindex)) Then
                Nummers.Add(New Tuple(Of Integer, Integer)(r, Firstindex), Getnummer(array, r, Firstindex, lastIndex))
            End If
        End If
        If FindNumberPos(array, r - 1, c, columns, rows) Then
            Dim lastIndex = FindLastNumber(array, r - 1, c, columns)
            Dim Firstindex = FindFirstNumber(array, r - 1, c)
            If Not Nummers.ContainsKey(New Tuple(Of Integer, Integer)(r - 1, Firstindex)) Then
                Nummers.Add(New Tuple(Of Integer, Integer)(r - 1, Firstindex), Getnummer(array, r - 1, Firstindex, lastIndex))
            End If
        End If
        If FindNumberPos(array, r - 1, c - 1, columns, rows) Then
            Dim lastIndex = FindLastNumber(array, r - 1, c - 1, columns)
            Dim Firstindex = FindFirstNumber(array, r - 1, c - 1)
            If Not Nummers.ContainsKey(New Tuple(Of Integer, Integer)(r - 1, Firstindex)) Then
                Nummers.Add(New Tuple(Of Integer, Integer)(r - 1, Firstindex), Getnummer(array, r - 1, Firstindex, lastIndex))
            End If
        End If
        If FindNumberPos(array, r - 1, c + 1, columns, rows) Then
            Dim lastIndex = FindLastNumber(array, r - 1, c + 1, columns)
            Dim Firstindex = FindFirstNumber(array, r - 1, c + 1)
            If Not Nummers.ContainsKey(New Tuple(Of Integer, Integer)(r - 1, Firstindex)) Then
                Nummers.Add(New Tuple(Of Integer, Integer)(r - 1, Firstindex), Getnummer(array, r - 1, Firstindex, lastIndex))
            End If
        End If
        If FindNumberPos(array, r + 1, c, columns, rows) Then
            Dim lastIndex = FindLastNumber(array, r + 1, c, columns)
            Dim Firstindex = FindFirstNumber(array, r + 1, c)
            If Not Nummers.ContainsKey(New Tuple(Of Integer, Integer)(r + 1, Firstindex)) Then
                Nummers.Add(New Tuple(Of Integer, Integer)(r + 1, Firstindex), Getnummer(array, r + 1, Firstindex, lastIndex))
            End If
        End If
        If FindNumberPos(array, r + 1, c - 1, columns, rows) Then
            Dim lastIndex = FindLastNumber(array, r + 1, c - 1, columns)
            Dim Firstindex = FindFirstNumber(array, r + 1, c - 1)
            If Not Nummers.ContainsKey(New Tuple(Of Integer, Integer)(r + 1, Firstindex)) Then
                Nummers.Add(New Tuple(Of Integer, Integer)(r + 1, Firstindex), Getnummer(array, r + 1, Firstindex, lastIndex))
            End If
        End If
        If FindNumberPos(array, r + 1, c + 1, columns, rows) Then
            Dim lastIndex = FindLastNumber(array, r + 1, c + 1, columns)
            Dim Firstindex = FindFirstNumber(array, r + 1, c + 1)
            If Not Nummers.ContainsKey(New Tuple(Of Integer, Integer)(r + 1, Firstindex)) Then
                Nummers.Add(New Tuple(Of Integer, Integer)(r + 1, Firstindex), Getnummer(array, r + 1, Firstindex, lastIndex))
            End If
        End If

        If Nummers.Count = 2 Then
            Dim nummerlist = Nummers.Values.ToList()
            Dim ratio = nummerlist(0) * nummerlist(1)
            Console.WriteLine("pos : " + r.ToString + "," + c.ToString + " : " + ratio.ToString)
            Return ratio
        ElseIf Nummers.Count < 2 Then
            Console.WriteLine("pos : " + r.ToString + "," + c.ToString + " : te weinig")
            Return 0
        Else
            Console.WriteLine("pos : " + r.ToString + "," + c.ToString + " : te veel")
            Return 0
        End If
    End Function


    Public Function FindNumberPos(array As Char(,), r As Integer, c As Integer, columns As Integer, rows As Integer) As Boolean
        If c < 0 OrElse c >= columns OrElse r < 0 OrElse r >= rows Then
            Return False
        End If

        If IsNumeric(array(r, c)) Then
            ' Console.WriteLine(array(r, c))
            Return True
        End If

        Return False
    End Function

    Public Function FindFirstNumber(array As Char(,), r As Integer, c As Integer) As Integer
        Dim newC As Integer = c - 1
        If newC < 0 Then Return c
        If IsNumeric(array(r, newC)) Then
            Return FindFirstNumber(array, r, newC)
        Else
            Return c
        End If
    End Function

    Public Function Getnummer(array As Char(,), r As Integer, first As Integer, last As Integer) As Integer
        Dim nummer As String = ""
        For c = first To last
            nummer += array(r, c)
        Next

        Return nummer
    End Function


End Module
