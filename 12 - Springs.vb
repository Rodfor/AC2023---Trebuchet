Imports System.Net.NetworkInformation
Imports Microsoft.VisualBasic.FileIO

Module Module12
    Public Sub Spring()
        Dim pad = "C:\Users\Matthias\Source\Repos\Rodfor\AC2023---Trebuchet\Springs.txt"
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
            
            'Springs.Add(SpringGroups)
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

            For Each l In Info
                Console.Write(l.ToString + ", ")
            Next

            SpringInfo.Add(Info)
            ' Console.WriteLine("")
        Next

' Dim SpringLists As New List(Of List(Of String))

'For i = 0 To springs.Count - 1
'    Dim SpringList As New List(Of String)
'    Dim S = springs(i)
'    S.Trim(".")
'    For Each g In S.Split(".")
'        If g <> "." AndAlso g <> "" Then
'            SpringList.Add(g)
'        End If
'    Next
'    SplitsFirst(SpringList, SpringInfo(i))
'    SplitsLast(SpringList, SpringInfo(i))
'    Console.WriteLine(String.Join(".", SpringList.ToArray) + " - " + String.Join(",", SpringInfo(i)))
'    'SpringLists.Add(SpringList)
'    springs(i) = String.Join(".", SpringList.ToArray)
'Next


        Dim totaal As Long = 0

        For i = 0 To springs.Count - 1
            totaal += Getmogelijkheden(springs(i), SpringInfo(i))       
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


    Private Function GetMogelijkHeden(Spring As String, Info As List(Of Integer)) As Long
        Dim Statussen As New Dictionary(Of Tuple(Of Integer, Integer), Long)
        Statussen.Add(Tuple.Create(0, 0), 1)

        For Each c In Spring
            Console.WriteLine(Statussen.Count)
            Dim NewStatussen As New Dictionary(Of Tuple(Of Integer, Integer), Long)
            Select Case c
                Case "."
                    'Dim ToDelete = Statussen.Where(Function(x) x.Item2 <> 0 AndAlso x.Item2 < Info(x.Item1))
                    'For Each S In ToDelete
                    '    Statussen.Remove(S)
                    'Next
                    Dim noNewGroup = Statussen.Where(Function(x) x.Key.Item1 <= Info.Count AndAlso x.Key.Item2 = 0)
                    For Each S In noNewGroup
                        NewStatussen.Add(S.Key, S.Value)
                    Next

                    Dim ToIncrementGroup = Statussen.Where(Function(x) x.Key.Item1 < Info.Count AndAlso x.Key.Item2 = Info(x.Key.Item1))
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
                     
                    Dim ToIncrementGroup = Statussen.Where(Function(x) x.Key.Item1 < Info.Count AndAlso x.Key.Item2 < Info(x.Key.Item1))
                    For Each S In ToIncrementGroup
                        Dim NewS = Tuple.Create(S.Key.Item1, S.Key.Item2 + 1)
                            NewStatussen.Add(NewS, S.Value)
                    Next

                Case "?"
                    For Each S In Statussen
                         If S.Key.Item1 < Info.Count Then                    
                            If S.Key.Item2 = Info(S.Key.Item1)                                      ' Moet punt zijn (brick bij #)
                                Dim NewS = Tuple.Create(S.Key.Item1 + 1, 0)                         '.
                                If NewStatussen.ContainsKey(NewS) Then
                                    NewStatussen(NewS) += S.Value
                                Else
                                    NewStatussen.Add(NewS, S.Value)
                                End If
                            ElseIf S.Key.Item2 = 0                                                  'Kan beide zijn
                                Dim NewS = Tuple.Create(S.Key.Item1, S.Key.Item2 + 1)               '#
                                NewStatussen.Add(NewS, S.Value)

                                NewS = Tuple.Create(S.Key.Item1, 0)                                 '.
                                If NewStatussen.ContainsKey(NewS) Then
                                    NewStatussen(NewS) += S.Value
                                Else
                                    NewStatussen.Add(NewS, S.Value)
                                End If
                            Elseif S.Key.Item2 < Info(S.Key.Item1)                                  'moet # zijn (brick bij .)
                                Dim NewS = Tuple.Create(S.Key.Item1, S.Key.Item2 + 1)               '#
                                NewStatussen.Add(NewS, S.Value)

                            End If
                        Else
                            if NewStatussen.ContainsKey(S.Key) Then
                                NewStatussen(S.Key) += S.Value
                            Else
                                NewStatussen.Add(S.key, S.Value)
                            End If
                        End if
                    Next
            End Select

           
            Statussen = NewStatussen

        Next
        Dim totaal As Long = 0
        For each status In Statussen
            If status.Key.Item1 = Info.Count
                totaal += status.Value
            ElseIf status.Key.Item1 = Info.Count -1 AndAlso status.Key.Item2 = Info(status.Key.Item1)
               totaal += status.Value
            End If
        Next

        Console.WriteLine(Spring + " - " + totaal.ToString)
        Return totaal
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