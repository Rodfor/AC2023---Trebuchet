Imports System.Net.NetworkInformation
Imports System.Reflection
Imports Microsoft.VisualBasic.FileIO

Module Module13
    Public Sub Mirror()
        Dim pad = "C:\Users\Matthias\Source\Repos\Rodfor\AC2023---Trebuchet\Mirrors.txt"
Dim Patterns As New List(Of List(of String))
        Dim Pattern as New List(Of String)

        Using reader = My.Computer.FileSystem.OpenTextFileReader(pad)
            While Not reader.EndOfStream
                Dim Line As string = reader.ReadLine
                If Line <> "" Then
                    Pattern.Add(Line)
                Else
                    Patterns.Add(Pattern.ToArray.ToList)
                    Pattern = New List(Of String)
                End If
            End While
            Patterns.Add(Pattern)
            reader.Close
            reader.Dispose
        End Using

        Dim Totaal As Integer = 0
        Dim prevTotaal As Integer = 0
    
        For each P In Patterns
            Dim Dupes = MaakDupes(P)       
            Dim prevAnswer = CheckSymmetry(P)
            If prevAnswer = -1 Then
                Console.WriteLine("ERROR PREVIOUS")
            End If

            Dim Answer = CheckDupes(Dupes,prevAnswer)
             If Answer is nothing then
                Console.WriteLine("ERROR NOW")
            End If

            Totaal += Answer
            prevTotaal += prevAnswer
        Next

        Console.WriteLine(Totaal.ToString + " -- " + prevTotaal.ToString)
        
    End Sub

    Private Function CheckDupes(Dupes As List(Of List(Of String)), prevAnswer As Integer)
        Dim Sym As Integer

        For Each D In Dupes
            Sym = CheckSymmetry(D, prevAnswer)
            If Sym <> -1 andalso Sym <> prevAnswer Then
                Console.WriteLine(Sym.ToString + " -- " + prevAnswer.ToString)
                For each P In D
                    Console.WriteLine(P)
                Next
                Console.WriteLine("")
                Return Sym
            End If
        Next
    End Function

    Private Function MaakDupes(Pattern As List(Of String)) As List(Of List(Of String))
        Dim Dupes = New List(Of List(Of String))
        For i = 0 to pattern.count - 1
            For j = 0 To Pattern(0).Length - 1
                Dim P  = Pattern.ToArray.ToList
                If P(i)(j) = "." Then
                    P(i) = P(i).Substring(0, j) + "#" + P(i).Substring(j + 1)
                Else
                    P(i) = P(i).Substring(0, j) + "." + P(i).Substring(j + 1)
                End If

                Dupes.Add(P)
            Next
        Next

        Return Dupes
    End Function

    Private Function CheckSymmetry(Pattern As List(Of String), optional filter As Integer = -1) As Integer
        Dim Aantal As Integer = -1

        Aantal = CheckHorizontal(Pattern, filter)
        If Aantal = -1 Then
            Aantal = CheckVertical(Pattern, filter)
        End If

        Return Aantal
    End Function

    Private Function CheckHorizontal(Pattern As List(Of String), optional filter As Integer = -1) As Integer
        For  i = 0 To  Pattern.Count - 2
            if ControleHorizontaal(Pattern, i, 0) Then
                Dim getal As integer = (i + 1) * 100  
                If getal <> Filter Then
                    Return getal        
                End If
            End If
        Next
        Return - 1
    End Function

    Private Function ControleHorizontaal(Pattern As List(Of String), i As Integer, offset As integer) As Boolean
        If Pattern(i - offset) = Pattern(i + offset + 1)
            If i + offset + 1 = Pattern.Count - 1 Orelse i - offset = 0 Then
                Return True
            Else
                Return ControleHorizontaal(Pattern, i, offset + 1)
            End If
        Else
            Return False
        End If
    End Function

    Private Function Checkvertical(Pattern As List(Of String), optional filter As Integer = -1) As Integer
        For i = 0 To Pattern(0).Length - 2
            If ControleVerticaal(Pattern, i, 0) Then
                Dim getal =  i + 1 
                 If getal <> Filter Then
                    Return getal   
                End If
            End If
        Next

        Return -1
    End Function

    Private Function ControleVerticaal(Pattern As List(Of String), i As Integer, offset As integer) As Boolean
        For each P In Pattern
            If P(i-offset) <> P(i + offset + 1) Then
                Return False
            End If
        Next

        If i + offset + 1 = Pattern(0).Length - 1 Orelse i - offset = 0 Then
            Return True
        Else
            Return ControleVerticaal(Pattern, i, offset + 1)
        End If

    End Function

End Module