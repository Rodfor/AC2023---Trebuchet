Imports System.Threading
Imports System.Threading.Tasks

Module Module25

    Public Sub Snow()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Snow.txt"
        Dim Lijnen() As String = System.IO.File.ReadAllLines(pad)

        'Dim components As New Dictionary(Of String, Component)

        'For Each L In Lijnen
        '    Dim C As Component
        '    Dim delen = L.Split(":")
        '    If components.ContainsKey(delen(0)) Then
        '        C = components(delen(0))
        '    Else
        '        C = New Component
        '        C.Name = delen(0)
        '        components.Add(C.Name, C)
        '    End If

        '    Dim linked = delen(1).Split(" ")

        '    For Each comp In linked
        '        If comp.Length <> 3 Then Continue For
        '        Dim Link As Component
        '        If components.ContainsKey(comp) Then
        '            Link = components(comp)
        '        Else
        '            Link = New Component
        '            Link.Name = comp
        '            components.Add(Link.Name, Link)
        '        End If

        '        C.Linked.Add(Link)
        '        Link.Linked.Add(C)

        '        'Dim EdgeName As String
        '        'Dim E As Edge

        '        'If C.Name > Link.Name Then
        '        '    EdgeName = C.Name + Link.Name
        '        'Else
        '        '    EdgeName = Link.Name + C.Name
        '        'End If

        '        'If Edges.ContainsKey(EdgeName) Then
        '        '    E = Edges(EdgeName)
        '        'Else
        '        '    E = New Edge
        '        '    E.Name = EdgeName
        '        '    E.A = C
        '        '    E.B = Link
        '        '    Edges.Add(EdgeName, E)
        '        'End If

        '        'C.Edges.Add(Link, E)
        '        'Link.Edges.Add(C, E)

        '    Next
        'Next


        Dim lowestCuts As Integer = 100
        Dim lowestNode As Component

        Dim components As Dictionary(Of String, Component)
        For i = 0 To 10000
            components = New Dictionary(Of String, Component)
            For Each L In Lijnen
                Dim C As Component
                Dim delen = L.Split(":")
                If components.ContainsKey(delen(0)) Then
                    C = components(delen(0))
                Else
                    C = New Component
                    C.Name = delen(0)
                    components.Add(C.Name, C)
                End If

                Dim linked = delen(1).Split(" ")

                For Each comp In linked
                    If comp.Length <> 3 Then Continue For
                    Dim Link As Component
                    If components.ContainsKey(comp) Then
                        Link = components(comp)
                    Else
                        Link = New Component
                        Link.Name = comp
                        components.Add(Link.Name, Link)
                    End If

                    C.Linked.Add(Link)
                    Link.Linked.Add(C)

                Next
            Next

            Dim nodes = components.Values.ToList

            While nodes.Count > 2
                Dim C = nodes(GetRandom(0, nodes.Count - 1))
                Dim L = C.Linked(GetRandom(0, C.Linked.Count - 1))

                Dim N As New Component

                C.Linked.RemoveAll(Function(x) L.Equals(x))
                L.Linked.RemoveAll(Function(x) C.Equals(x))

                N.AantalNodes = C.AantalNodes + L.AantalNodes

                '  Dim newlink As New List(Of Component)

                For Each S In C.Linked
                    If S.Equals(C) Then
                        Console.WriteLine("Error")
                        Continue For
                    End If
                    S.Linked.Remove(C)
                    S.Linked.Add(N)
                    N.Linked.Add(S)
                Next

                For Each T In L.Linked
                    If T.Equals(L) Then
                        Console.WriteLine("Error")
                        Continue For
                    End If
                    T.Linked.Remove(L)
                    T.Linked.Add(N)
                    N.Linked.Add(T)
                Next

                nodes.Remove(C)
                nodes.Remove(L)
                nodes.Add(N)
            End While

            Dim cuts = nodes.First.Linked.Count

            ' Console.WriteLine(cuts)
            If cuts < lowestCuts Then
                lowestNode = nodes.First
                lowestCuts = cuts
                If lowestCuts = 3 Then
                    Exit For
                End If
            End If
        Next

        Console.WriteLine(lowestCuts)
        Console.WriteLine(lowestNode.AantalNodes.ToString + " * " + (components.Count - lowestNode.AantalNodes).ToString + " = " + (lowestNode.AantalNodes * (components.Count - lowestNode.AantalNodes)).ToString)

    End Sub

    Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        ' by making Generator static, we preserve the same instance '
        ' (i.e., do not create new instances with the same seed over and over) '
        ' between calls '
        Static Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function


    Public Class Component
        Public Name As String
        Public Linked As New List(Of Component)
        Public AantalNodes As Integer = 1
    End Class

End Module

