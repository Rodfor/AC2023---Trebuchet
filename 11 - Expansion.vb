Imports Microsoft.VisualBasic.FileIO
Module Module11
     Public sub Space()
        Dim pad = "C:\Users\Matthias\Source\Repos\Rodfor\AC2023---Trebuchet\Space.txt"
        Dim lijnen As New List(Of String)
       
        Using reader = My.Computer.FileSystem.OpenTextFileReader(pad)
            While Not reader.EndOfStream
                Dim Line As string = reader.ReadLine
                lijnen.Add(Line)
            End While
            reader.Close
            reader.Dispose
        End Using

        Dim EmptyRows As new List(Of Integer)
        Dim Galaxies As New List(Of Galaxy)

        For r = 0 To lijnen.Count - 1
            Dim GalaxyFound As Boolean = False
            For c = 0 To lijnen.Count - 1
                If lijnen(r)(c) = "#" Then
                    Dim G As New Galaxy
                    G.Row = r
                    G.Col = c
                     G.ActualRow = r
                    G.ActualCol = c
                    Galaxies.Add(G)
                    GalaxyFound  = True
                End If
            Next
            If not GalaxyFound Then EmptyRows.Add(r)
        Next

        Dim EmptyCols As New List(Of Integer)

         For c = 0 To lijnen.Count - 1
            Dim GalaxyFound As Boolean = False
            For r = 0 To lijnen.Count - 1
                 If lijnen(r)(c) = "#" Then
                    GalaxyFound = True
                 End If
            Next
            If not GalaxyFound Then EmptyCols.Add(c)
         Next

         For each G In Galaxies
            Console.WriteLine(G.Row.ToString + ", " + G.Col.ToString)
        Next

        EmptyRows.Reverse
        EmptyCols.Reverse

        For each r In EmptyRows
            Galaxies.Where(Function(x) x.Row > r).ToList.ForEach(Sub(x) x.ActualRow += 999999)
        Next

        For each c In EmptyCols
            Galaxies.Where(Function(x) x.Col > c).ToList.ForEach(Sub(x) x.ActualCol += 999999)
        Next

        For each G In Galaxies
            Console.WriteLine(G.ActualRow.ToString + ", " + G.ActualCol.ToString)
        Next

        Dim som As Long = 0

        For i = 0 To Galaxies.Count - 2
            For j = i + 1 To Galaxies.Count - 1
                Dim dist = Math.abs(Galaxies(j).ActualRow - Galaxies(i).ActualRow) + Math.abs(Galaxies(j).ActualCol - Galaxies(i).ActualCol)
                Console.WriteLine(i.ToString + " - " + j.ToString + " : " + dist.ToString)
                som += dist
            Next
        Next
        
        Console.WriteLine(som)

    End Sub
End Module

Public Class Galaxy
    Public Row As Integer 
    Public Col As Integer
    Public ActualRow As Long
    Public ActualCol As Long
End Class
