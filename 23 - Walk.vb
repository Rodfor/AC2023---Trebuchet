Module Module23
    Dim Pattern(,) As Punt
    Dim targetPunt As Punt
    Dim lengtes As New List(Of Long)

    Public Sub Walk()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Walk.txt"
        Dim Lijnen() As String = System.IO.File.ReadAllLines(pad)

        ReDim Pattern(Lijnen.Count - 1, Lijnen.First.Length - 1)

        For i = 0 To Pattern.GetUpperBound(0)
            Dim lijn = Lijnen(i)
            For j = 0 To Pattern.GetUpperBound(1)
                Pattern(i, j) = New Punt(i, j, lijn(j))
            Next
        Next

        targetPunt = Pattern(Pattern.GetUpperBound(0) - 1, Pattern.GetUpperBound(1) - 1)
        Dim startPunten As New List(Of Punt)

        For Each P In Pattern
            If P.value = "." AndAlso P.x < Pattern.GetUpperBound(0) AndAlso P.x > Pattern.GetLowerBound(0) AndAlso P.y < Pattern.GetUpperBound(1) AndAlso P.y > Pattern.GetLowerBound(1) Then
                If GetBuren(P).Count <> 2 Then
                    startPunten.Add(P)
                End If
            End If
        Next



        For Each S In startPunten
            S.Linked = New Dictionary(Of Punt, Integer)

            For Each C In GetBuren(S)

                Dim current = C
                Dim currentpath As New List(Of Punt)
                currentpath.Add(S)

                While Not startPunten.Contains(current)
                    currentpath.Add(current)
                    For Each B In GetBuren(current)
                        If Not currentpath.Contains(B) Then current = B
                    Next
                End While

                S.Linked.Add(current, currentpath.Count)

            Next
        Next

        Dim startpunt = startPunten.First

        Dim visited As New List(Of Punt)
        visited.Add(startPunten.First)
        Visit(startPunten.First, 2, visited)

        Dim hoogste As Long = 0

        For Each I In lengtes
            If I > hoogste Then
                hoogste = I
            End If
        Next

        Console.WriteLine(hoogste)

    End Sub

    Public Sub Visit(Start As Punt, lengte As Long, visited As List(Of Punt))

        For Each KVP In Start.Linked
            If KVP.Key.Name = targetPunt.Name Then
                lengtes.Add(lengte + KVP.Value)
                Exit Sub
            End If
            If Not visited.Contains(KVP.Key) Then
                Dim newvisited = New List(Of Punt)
                newvisited.AddRange(visited)
                newvisited.Add(KVP.Key)
                Visit(KVP.Key, KVP.Value + lengte, newvisited)
            End If
        Next
    End Sub

    Public Function GetBuren(N As Punt) As List(Of Punt)
        Dim Buren As New List(Of Punt)

        If Pattern(N.x + 1, N.y).value <> "#" Then
            Buren.Add(Pattern(N.x + 1, N.y))
        End If

        If Pattern(N.x - 1, N.y).value <> "#" Then
            Buren.Add(Pattern(N.x - 1, N.y))
        End If

        If Pattern(N.x, N.y + 1).value <> "#" Then
            Buren.Add(Pattern(N.x, N.y + 1))
        End If

        If Pattern(N.x, N.y - 1).value <> "#" Then
            Buren.Add(Pattern(N.x, N.y - 1))
        End If

        Return Buren

    End Function


    Public Class Punt
        Public x As Integer
        Public y As Integer
        Public Name As Integer

        Public value As Char

        Public Linked As Dictionary(Of Punt, Integer)

        Public Sub New(x As Integer, y As Integer, value As Char)
            Me.x = x
            Me.y = y
            Me.value = value
            Me.Name = x * 1000 + y
        End Sub

    End Class

End Module

