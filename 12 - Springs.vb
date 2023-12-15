Imports Microsoft.VisualBasic.FileIO

Module Module12
    Public Sub Spring()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Springs.txt"
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

        Dim springs As New List(Of String)
        Dim SpringInfo As New List(Of List(Of Integer))

        For i = 0 To lijnen.Count - 1
            Dim SpringGroups As String = lijnen(i)(0)
            springs.Add(SpringGroups + "?" + SpringGroups + "?" + SpringGroups + "?" + SpringGroups + "?" + SpringGroups)

            ' Console.Write(springs(i) + " - ")

            Dim Info As New List(Of Integer)
            For Each c In lijnen(i)(1).Split(",")
                Info.Add(c)
            Next

            Info.AddRange(Info)
            Info.AddRange(Info)

            For Each c In lijnen(i)(1).Split(",")
                Info.Add(c)
            Next

            'For Each l In Info
            '    Console.Write(l.ToString + ", ")
            'Next

            SpringInfo.Add(Info)
            ' Console.WriteLine("")

        Next

        Dim SpringLists As New List(Of List(Of String))

        For i = 0 To springs.Count - 1
            Dim SpringList As New List(Of String)
            Dim S = springs(i)
            S.Trim(".")
            For Each g In S.Split(".")
                If g <> "." AndAlso g <> "" Then
                    SpringList.Add(g)
                End If
            Next
            SplitsFirst(SpringList, SpringInfo(i))
            SplitsLast(SpringList, SpringInfo(i))
            Console.WriteLine(String.Join(".", SpringList.ToArray) + " - " + String.Join(",", SpringInfo(i)))
            'SpringLists.Add(SpringList)
            springs(i) = String.Join(".", SpringList.ToArray)
        Next


        Dim totaal As Long = 0

        For i = 0 To springs.Count - 1
            Dim Mogelijkheden = Getmogelijkheiden(springs(i), SpringInfo(i))
            Console.WriteLine(springs(i) + " - " + Mogelijkheden.ToString)
            totaal += Mogelijkheden
        Next

        Console.WriteLine(totaal.ToString)

    End Sub


    Private Sub SplitsFirst(springs As List(Of String), info As List(Of Integer))
        If springs.First.Length = info.First Then
            springs.RemoveAt(0)
            info.RemoveAt(0)
            SplitsFirst(springs, info)
        End If
    End Sub

    Private Sub SplitsLast(springs As List(Of String), info As List(Of Integer))
        If springs.Last.Length = info.Last Then
            springs.RemoveAt(springs.Count - 1)
            info.RemoveAt(info.Count - 1)
            SplitsLast(springs, info)
        End If
    End Sub


    Private Function GetMogelijkHeden(Spring As String, Info As List(Of Integer))
        Dim Statussen As New Dictionary(Of Tuple(Of Integer, Integer), Integer)
        Statussen.Add(Tuple.Create(0, 0), 1)

        For Each c In Spring
            Dim NewStatussen As New Dictionary(Of Tuple(Of Integer, Integer), Integer)
            Select Case c
                Case "."
                    'Dim ToDelete = Statussen.Where(Function(x) x.Item2 <> 0 AndAlso x.Item2 < Info(x.Item1))
                    'For Each S In ToDelete
                    '    Statussen.Remove(S)
                    'Next
                    Dim noNewGroup = Statussen.Where(Function(x) x.Key.Item2 = 0)
                    For Each S In noNewGroup
                        NewStatussen.Add(S.Key, S.Value)
                    Next

                    Dim ToIncrementGroup = Statussen.Where(Function(x) x.Key.Item2 = Info(x.Key.Item1))
                    For Each S In ToIncrementGroup
                        Dim NewS = Tuple.Create(S.Key.Item1 + 1, 0)
                        If NewStatussen.ContainsKey(NewS) Then
                            NewStatussen(NewS) += S.Value
                        Else
                            NewStatussen.Add(NewS, S.Value)
                        End If
                    Next

                Case "#"
                    ' Dim Teveel = Statussen.Where(Function(x) x.Key.Item2 = Info(x.Key.Item1))

                    Dim ToIncrementGroup = Statussen.Where(Function(x) x.Key.Item2 < Info(x.Key.Item1))
                    For Each S In ToIncrementGroup
                        Dim NewS = Tuple.Create(S.Key.Item1 + 1, 0)
                        NewStatussen.Add(NewS, S.Value)
                    Next

                Case "?"
                    For Each S In Statussen

                    Next

            End Select

        Next


        If Index = Spring.Length Then
            Return 1
        End If
        Dim Som As Integer
        Select Case Spring(Index)
            Case "."
                If Info(status.Item1) = status.Item2 Then
                    Dim newStatus As Tuple(Of Integer, Integer, Integer) = (status.Item1 + 1, 0, status.Item3)
                    Som = GetMogelijkHeden(Spring, Info, Index + 1, newStatus)
                End If
            Case "#"

            Case "?"

        End Select
    End Function


    'Private Function Getmogelijkheiden(str As String, info As List(Of Integer)) As Integer
    '    Dim Som As Integer = 0

    '    Dim index = str.IndexOf("?")
    '    If index <> -1 Then
    '        Som += Getmogelijkheiden(str.Substring(0, index) + "." + str.Substring(index + 1), info)
    '        Som += Getmogelijkheiden(str.Substring(0, index) + "#" + str.Substring(index + 1), info)
    '        Return Som
    '    Else
    '        Return CheckStringMogelijk(str, info)
    '    End If
    'End Function

    'Private Function CheckStringMogelijk(str As String, info As List(Of Integer)) As Integer
    '    Dim groups As New List(Of String)

    '    For Each g In str.Trim(".").Split(".")
    '        If g.Contains("#") Then
    '            groups.Add(g)
    '        End If
    '    Next

    '    Dim succes As Boolean = True

    '    If groups.Count = info.Count Then
    '        For i = 0 To groups.Count - 1
    '            If groups(i).Length <> info(i) Then
    '                succes = False
    '            End If
    '        Next
    '    Else
    '        succes = False
    '    End If

    '    If succes Then
    '        Console.WriteLine(str + " - OK")
    '    End If

    '    If succes Then Return 1 Else Return 0

    'End Function

End Module