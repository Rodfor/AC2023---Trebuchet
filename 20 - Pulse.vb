Module Module20
     Public Props As new Dictionary(Of String, Prop)

    Public Sub Pulse()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Pulse.txt"
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

        Dim Modules As New List(Of Prop)
        Dim count As Integer = 0

        For Each I In Props("rx").Inputs.First.Inputs
            Modules.Add(I)

        Next


        Dim high As Integer = 0
        Dim low As Integer = 0

        Dim j As Integer = 0
        Dim cycling = 0

        While cycling < Modules.Count
            j += 1

            Dim Queue As New List(Of QueueItem) From {
                New QueueItem(Nothing, Props("broadcaster"), False)
            }

            While Queue.Count > 0
                Dim newQueue As New List(Of QueueItem)

                For Each I In Queue
                    newQueue.AddRange(I.Target.Pulse(I.High))
                    If I.High AndAlso Modules.Contains(I.Source) Then
                        If I.Source.FirstHigh = 0 Then
                            I.Source.FirstHigh = j
                            Console.WriteLine(I.Source.Naam + " - " + j.ToString)
                            cycling += 1
                        End If
                    End If
                Next

                'If Queue.Where(Function(x) x.High = False AndAlso x.Target.Naam = Props("rx").Naam).Count > 0 Then
                '    Console.WriteLine(j + 1)
                '    Exit Sub
                'End If



                Queue = newQueue
            End While

            'If j = 2047 Then
            '    Console.WriteLine("X")
            'End If

            'For Each L In OutputModules
            '    For Each P In L
            '        If P.State AndAlso P.FirstHigh = 0 Then
            '            P.FirstHigh = j
            '            Console.WriteLine(P.Naam + " - " + j.ToString)
            '            cycling += 1
            '        End If
            '    Next
            'Next
        End While

        Dim multipliers As New List(Of Long)

        For Each L In Modules
            multipliers.Add(L.FirstHigh)
        Next

        Console.WriteLine(LCMList(multipliers))

    End Sub

    Public Function LCMList(nummers As List(Of Long)) As Long
        Return nummers.Aggregate(AddressOf LCM)
    End Function

    Public Function LCM(a As Long, b As Long) As Long
        Return Math.Abs(a * b) / GCD(a, b)
    End Function

    Public Function GCD(a As Long, b As Long) As Long
        Return If(b = 0, a, GCD(b, a Mod b))
    End Function

    Public CLass Prop

        Public Naam As String
        Public type As Char
        Public destString As String
        Public Destinations As new List(Of Prop)
        Public State As Boolean = False
        Public Inputs As New List(Of Prop)
        Public FirstHigh As Integer = 0

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

            ' Console.WriteLine("")
            ' Console.Write(Naam + " " + type + " - ")
            Select Case type
                Case "b"
                    For each P In Destinations
                        queue.Add(New QueueItem(Me, P, High))
                        'Console.Write(P.Naam + " " + High.ToString + ", ")
                    Next
                Case "%"
                    If Not High
                        if Not High Then State = Not State
                        For each P In Destinations
                            queue.Add(New QueueItem(Me, P, State))
                            'Console.Write(P.Naam + " " + State.ToString + ", ")
                        Next
                    End If
                Case "&"
                    State = False

                    For each I In Inputs
                        If Not I.State Then
                            State = True
                            'Console.WriteLine(Naam + " - high")
                            Exit For
                        End If
                    Next

                    For each P In Destinations
                        queue.Add(New QueueItem(Me, P, State))
                        'Console.Write(P.Naam + " " + State.ToString + ", ")
                        'If P.Naam = "hb" And State = High Then

                        'End If
                    Next
            End Select
            
            Return queue

        End Function

        ' Public Function PulsesNeeded(high As Boolean) As Long

        '    Select Case type
        '        Case "b"
        '            Return 1
        '        Case "%"
        '           If high Then
        '                Dim nummers = New List(Of Long)
        '                For each P In Inputs
        '                    nummers.Add(P.PulsesNeeded())
        '                Next
        '           End If
        '        Case "&"
        '            State = False

        '            For each I In Inputs
        '                If not I.State Then State = True
        '            Next

        '            For each P In Destinations
        '                queue.Add(New QueueItem(P, State))
        '                'Console.Write(P.Naam + " " + State.ToString + ", ")
        '            Next
        '    End Select





        '    Return count
        'End Function



    End Class

    Public Class QueueItem
        Public Target As Prop
        Public High As Boolean
        Public Source As Prop

        Public Sub New(source As Prop, target As Prop, high As Boolean)
            Me.Source = source
            Me.Target = target
            Me.High = high
        End Sub
    End Class


End Module

