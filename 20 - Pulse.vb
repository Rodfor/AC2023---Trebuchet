Module Module20
     Public Props As new Dictionary(Of String, Prop)

    Public Sub Pulse()
        Dim pad = "C:\Users\Matthias\source\repos\Rodfor\AC2023---Trebuchet\Pulse.txt"
        Dim Lijnen() As String = System.IO.File.ReadAllLines(pad)     

        For each L In Lijnen
            Dim P As New Prop
            Dim split = L.Split("-")
            If L.First = "b" Then
                P.Naam = "broadcaster"
                P.type = "b"
            Else
                P.type = split.First.Trim(" ")
                P.Naam = split.First.Trim(" ").Substring(1)
            End If
            P.destString = split.Last.Trim(">").Trim(" ")
            Props.Add(P.Naam, P)
        Next

        For each P In Props.Values.ToArray
            P.Init
        Next

        Dim count = Props("rx").PulsesNeeded(False, count)



        'Dim high As Integer = 0
        ' Dim low As Integer = 0

        Dim j As Integer = 0

        'While true
        '    j +=1
        '    Dim Queue As New List(Of QueueItem) From {
        '        New QueueItem(Props("broadcaster"), false)
        '    }
                
        '    If j Mod 10000 = 0 Then
        '        Console.WriteLine(J)
        '    End If
                
        '    While Queue.Count > 0
        '        Dim newQueue As New List(Of QueueItem)
        '       ' Dim newHigh = Queue.Where(Function(x) x.High).Count
        '        'high += newHigh
        '        'low += Queue.Count - newHigh                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   

        '        For each I In Queue
        '            newQueue.AddRange(I.Target.Pulse(I.High))
        '        Next
            
        '        If Queue.Where(Function(x) x.High = False AndAlso x.Target.naam  = Props("rx").naam).Count > 0 Then
        '            Console.WriteLine(j + 1)
        '            Exit Sub
        '        End If

        '        Queue = newQueue
        '   End While
        
            ' Console.WriteLine(high.ToString + ", " + low.ToString)
        'End while

        'Console.WriteLine(high * low)
       
    End Sub

    Public Function LCM(nummers As List(Of Long)) As Long

    End Function

    Public CLass Prop

        Public Naam As String
        Public type As Char
        Public destString As String
        Public Destinations As new List(Of Prop)
        Public State As Boolean = False
        Public Inputs As new List(Of Prop)

        Public Sub Init()
            If destString Is Nothing Then Exit Sub
            Dim split = destString.Split(",")
            For each c In split
                Dim key = c.trim(" ")
                If Not Props.Containskey(key) Then
                    Dim P As New Prop
                    P.Naam = key
                    P.type = "o"
                    Props.Add(P.Naam,P)
                End If
                Destinations.add(Props(key))
                Destinations.Last.Inputs.Add(me)
            Next
            If type = "&" Then State = True
        End Sub

        Public Function Pulse(High As Boolean) As List(Of QueueItem)
            Dim queue As New List(Of QueueItem)

            'Console.WriteLine("")
            'Console.Write(Naam + " " + type + " - ")
            Select Case type
                Case "b"
                    For each P In Destinations
                        queue.Add(New QueueItem(P, High))         
                        'Console.Write(P.Naam + " " + High.ToString + ", ")
                    Next
                Case "%"
                    If Not High
                        if Not High Then State = Not State
                        For each P In Destinations
                            queue.Add(New QueueItem(P, State))
                            'Console.Write(P.Naam + " " + State.ToString + ", ")
                        Next
                    End If
                Case "&"
                    State = False

                    For each I In Inputs
                        If not I.State Then State = True
                    Next

                    For each P In Destinations
                        queue.Add(New QueueItem(P, State))
                        'Console.Write(P.Naam + " " + State.ToString + ", ")
                    Next
            End Select
            
            Return queue

        End Function

         Public Function PulsesNeeded(high As Boolean) As Long

            Select Case type
                Case "b"
                    Return 1
                Case "%"
                   If high Then
                        Dim nummers = New List(Of Long)
                        For each P In Inputs
                            nummers.Add(P.PulsesNeeded())
                        Next
                   End If
                Case "&"
                    State = False

                    For each I In Inputs
                        If not I.State Then State = True
                    Next

                    For each P In Destinations
                        queue.Add(New QueueItem(P, State))
                        'Console.Write(P.Naam + " " + State.ToString + ", ")
                    Next
            End Select





            Return count
        End Function



    End Class

    Public Class QueueItem
        Public Target As Prop
        Public High As Boolean

        Public Sub New(target As Prop, high As Boolean)
            Me.Target = target
            Me.High = high
        End Sub
    End Class


End Module

