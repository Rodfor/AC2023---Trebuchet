
Imports System.Numerics

Module Module19
    Public Flows As New Dictionary(Of String, Flow)

    Public Sub Sort()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Sorting.txt"
        Dim Lijnen() As String = System.IO.File.ReadAllLines(pad)

        Dim i As Integer = 0
        Dim Lijn As String = Lijnen.First

        While Lijn <> ""
            Dim F As New Flow(Lijn)
            i += 1
            Lijn = Lijnen(i)
        End While

        i += 1

        Dim A = New Flow("A")
        Dim R = New Flow("R")

        Dim rules As List(Of List(Of Rule)) = Flows("in").Process(New List(Of Rule))
        Dim nummers() As Integer = Enumerable.Range(1, 4000).ToArray

        Dim possibilities As New Dictionary(Of Char, Integer()) From {
            {"x", nummers},
            {"m", nummers},
            {"a", nummers},
            {"s", nummers}
        }

        Dim som As New BigInteger(0)

        For Each path In rules
            Dim PathPossibilities = New Dictionary(Of Char, Integer())(possibilities)
            For Each Rule In path
                Rule.Check(PathPossibilities)
            Next

            Dim pathSom As New BigInteger(1)

            For Each V In PathPossibilities.Values
                pathSom *= V.Count
            Next

            Console.WriteLine(pathSom)
            som += pathSom
        Next

        Console.WriteLine("")
        Console.WriteLine(som)

        'Dim Parts As New List(Of Part)

        'While i < Lijnen.Count
        '    Dim P As New Part(Lijnen(i))
        '    Parts.Add(P)
        '    i += 1
        'End While

        'Dim Accepted As New List(Of Part)
        'Dim Rejected As New List(Of Part)

        'For Each P In Parts
        '    If P.Process() Then Accepted.Add(P) Else Rejected.Add(P)
        'Next

        'Dim som As Long = 0

        'For Each P In Accepted
        '    som += P.value
        'Next

        'Console.WriteLine(som)

    End Sub

    Public Class Flow
        Public Name As String
        Public Rules As List(Of Rule)

        'Public Sub New(Init As String, Accept As Boolean)
        '    Name = Init
        '    Rules = New List(Of Rule)
        '    Rules.Add(New Rule(Name))
        '    Flows.Add(Name, Me)
        'End Sub

        Public Sub New(init As String)
            Dim temp = init.Split("{")
            Name = temp.First

            Rules = New List(Of Rule)
            Dim rulestring = temp.Last.Trim("}").Split(",")
            For Each R In rulestring
                Dim Rule = New Rule(R)
                Rules.Add(Rule)
            Next

            Flows.Add(Name, Me)
        End Sub

        Public Function Process(prevRules As List(Of Rule)) As List(Of List(Of Rule))
            Dim currentRules As New List(Of List(Of Rule))

            If Name = "A" Then
                currentRules.Add(prevRules)
                Return currentRules
            ElseIf Name = "R" Then
                Return currentRules
            End If

            For Each R In Rules
                Dim RuleList As New List(Of Rule)
                RuleList.AddRange(prevRules)
                RuleList.Add(R)
                currentRules.AddRange(Flows(R.Flow).Process(RuleList))
                prevRules.Add(R.reverse)
            Next

            Return currentRules
        End Function

        'Public Function Process(P As Part) As Boolean
        '    If Name = "A" Then
        '        Return True
        '    ElseIf Name = "R" Then
        '        Return False
        '    End If

        '    For Each R In Rules
        '        If R.Check(P) Then
        '            Return Flows(R.Flow).Process(P)
        '        End If
        '    Next
        'End Function
    End Class

    Public Class Rule
        Public Comparer As Char
        Public Prop As Char
        Public Value As Integer

        Public Flow As String

        Public Sub New()

        End Sub

        Public Sub New(init As String)
            If init.Contains("<") Then
                Comparer = "<"
                Dim split = init.Split("<")
                Prop = split.First
                Dim splitLast = split.Last.Split(":")
                Value = splitLast.First
                Flow = splitLast.Last
            ElseIf init.Contains(">") Then
                Comparer = ">"
                Dim split = init.Split(">")
                Prop = split.First
                Dim splitLast = split.Last.Split(":")
                Value = splitLast.First
                Flow = splitLast.Last
            Else
                Flow = init
                Comparer = Flow
            End If

        End Sub

        Public Function Reverse() As Rule
            Dim R As New Rule
            R.Prop = Prop

            Select Case Comparer
                Case "<"
                    R.Comparer = ">"
                    R.Value = Value - 1
                Case ">"
                    R.Comparer = "<"
                    R.Value = Value + 1
            End Select

            Return R

        End Function

        Public Sub Check(parts As Dictionary(Of Char, Integer()))

            Select Case Comparer
                Case "<"
                    parts(Prop) = parts(Prop).Where(Function(X) X < Value).ToArray
                Case ">"
                    parts(Prop) = parts(Prop).Where(Function(X) X > Value).ToArray
                Case "R"
                    Console.WriteLine("ERROR")
            End Select
        End Sub

        'Public Function Check(P As Part)
        '    Select Case Comparer
        '        Case "<"
        '            Return P.Props(Prop) < Value
        '        Case ">"
        '            Return P.Props(Prop) > Value
        '        Case "A"
        '            Return True
        '        Case "R"
        '            Return False
        '        Case Else
        '            Return True
        '    End Select
        'End Function
    End Class

    'Public Class Part
    '    Public Props As New Dictionary(Of Char, Integer)
    '    Public value As Long = 0

    '    Public Sub New(init As String)
    '        Dim values = init.Trim("{").Trim("}").Split(",")
    '        For Each V In values
    '            Props.Add(V.First, V.Substring(2))
    '            value += V.Substring(2)
    '        Next
    '    End Sub

    '    Public Function Process() As Boolean
    '        Return Flows("in").Process(Me)
    '    End Function

    'End Class

End Module

