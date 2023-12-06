Imports Microsoft.VisualBasic.FileIO
Imports System.Text.RegularExpressions
Module Module4
    Public Sub Cards()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Cards.txt"
        Dim lijnen As New List(Of String())

        Using parser = New TextFieldParser(pad)
            parser.TextFieldType = FieldType.FixedWidth
            parser.SetFieldWidths(9, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3)
            'parser.SetFieldWidths(7, 3, 3, 3, 3, 3, 2, 3, 3, 3, 3, 3, 3, 3, 3)
            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                lijnen.Add(fields)
            End While

            parser.Close()
            parser.Dispose()
        End Using

        Dim OrigenalGames As New Dictionary(Of Integer, Game)

        For Each lijn In lijnen
            Dim spel = New Game
            spel.Nummer = Val(Regex.Replace(lijn(0), "[^0-9]", String.Empty))

            For i = 1 To 10
                'For i = 1 To 5
                spel.Waardes.Add(lijn(i))
            Next

            For i = 12 To 36
                'For i = 7 To 14
                spel.Winners.Add(lijn(i))
            Next

            OrigenalGames.Add(spel.Nummer, spel)
        Next

        Dim Games As List(Of Game) = OrigenalGames.Values.ToList
        Dim aantalGames As Integer = Games.Count

        While Games.Count > 0
            Dim newGames As New List(Of Game)
            For Each spel In Games
                spel.BerekenAantalWins()
                If spel.wins > 0 Then
                    ' Console.WriteLine("Spel " + spel.Nummer.ToString + ": " + spel.wins.ToString + " wins")
                    newGames.AddRange(GetDuplicates(spel, OrigenalGames))
                End If
            Next
            aantalGames += newGames.Count
            Games = newGames
            Console.WriteLine(newGames.Count.ToString + " duplicates added")
        End While

        Console.WriteLine(aantalGames.ToString)

        'Dim totaal As Integer = 0

        'For Each spel In Games
        '    Dim wins As Integer = 0
        '    For Each nummer In spel.Waardes
        '        If spel.Winners.Contains(nummer) Then
        '            Console.WriteLine(spel.Nummer.ToString + ": " + nummer.ToString)
        '            wins += 1
        '        End If
        '    Next

        '    If wins > 0 Then
        '        Dim punten As Integer = 2 ^ (wins - 1)
        '        Console.WriteLine("Spel " + spel.Nummer.ToString + ": " + punten.ToString + " punten")
        '        totaal += punten
        '    Else
        '        Console.WriteLine("Spel " + spel.Nummer.ToString + ": 0 punten")
        '    End If
        'Next

        'console.WriteLine("Totaal " + totaal.ToString)

    End Sub

    Public Function GetDuplicates(spel As Game, Games As Dictionary(Of Integer, Game)) As List(Of Game)
        Dim Duplicates = New List(Of Game)
        Dim i = 0
        While i < spel.wins
            i += 1
            Duplicates.Add(Games(spel.Nummer + i))
        End While
        Return Duplicates
    End Function


End Module

Public Class Game
    Public Nummer As Integer
    Public Waardes As New List(Of Integer)
    Public Winners As New List(Of Integer)
    Public wins As Integer?

    Public Sub BerekenAantalWins()

        If wins IsNot Nothing Then Exit Sub
        wins = 0

        For Each waarde In Waardes
            If Winners.Contains(waarde) Then
                'Console.WriteLine(Nummer.ToString + ": " + waarde.ToString)
                wins += 1
            End If
        Next

    End Sub

End Class
