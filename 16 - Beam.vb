Imports System.Net.NetworkInformation
Imports System.Reflection
Imports Microsoft.VisualBasic.FileIO


Module Module16
    Public Enum Direction
        Up
        Right
        Down
        Left
    End Enum
    Public Sub Beam()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Beam.txt"
        Dim Lijnen() As String = System.IO.File.ReadAllLines(pad)
        Dim Pattern(Lijnen.Count - 1, Lijnen.First.Length - 1) As Node
        Dim startNodes = New List(Of PathNode)

        For i = 0 To Lijnen.Count() - 1
            Dim lijn = Lijnen(i)
            For j = 0 To lijn.Length - 1
                Pattern(i, j) = New Node(i, j, lijn(j))
                Console.Write(Pattern(i, j).value)
                If i = 0 Then
                    startNodes.Add(New PathNode(Direction.Down, Pattern(i, j)))
                ElseIf i = Lijnen.Count() - 1 Then
                    startNodes.Add(New PathNode(Direction.Up, Pattern(i, j)))
                End If
                If j = 0 Then
                    startNodes.Add(New PathNode(Direction.Right, Pattern(i, j)))
                ElseIf j = lijn.Length - 1 Then
                    startNodes.Add(New PathNode(Direction.Left, Pattern(i, j)))
                End If
            Next
            Console.WriteLine("")
        Next


        Dim counts As New List(Of Long)

        For Each S In startNodes

            Dim nodeList As New List(Of PathNode) From {
            S
        }

            Dim endState As New List(Of PathNode)
            Dim endstateNotChanged As Integer = 0

            While nodeList.Count > 0 AndAlso endstateNotChanged < 100
                Dim nodeListZonderRepeats As New List(Of PathNode)
                For Each N In nodeList
                    If endState.Where(Function(x) x.Node.x = N.Node.x AndAlso x.Node.y = N.Node.y AndAlso x.dir = N.dir).Count = 0 Then
                        nodeListZonderRepeats.Add(N)
                        endstateNotChanged = 0
                    End If
                    endState.AddRange(nodeListZonderRepeats)
                Next
                Dim newNodes = GetNextNodes(Pattern, nodeListZonderRepeats)
                newNodes.RemoveAll(Function(x) x Is Nothing OrElse x.Node Is Nothing)
                endstateNotChanged += 1
                nodeList = newNodes
            End While

            Dim state = endState.GroupBy(Function(x) x.Node)

            Dim statecount As Long = state.Count
            counts.Add(statecount)
            Console.WriteLine(S.Node.x.ToString + "," + S.Node.y.ToString + " - " + statecount.ToString)

        Next

        Dim grootste As Long = 0

        For Each C In counts
            If C > grootste Then
                grootste = C
            End If
        Next

        Console.WriteLine(grootste)
    End Sub

    Public Function GetNextNodes(Pattern(,) As Node, path As List(Of PathNode)) As List(Of PathNode)
        Dim newNodes As New Concurrent.ConcurrentBag(Of PathNode)

        Parallel.ForEach(path, Sub(N As PathNode)
                                   Dim origin = N.Node
                                   Dim nodeDir = N.dir

                                   If origin IsNot Nothing Then
                                       Select Case origin.value
                                           Case "."
                                               newNodes.Add(New PathNode(nodeDir, GetNextNode(Pattern, origin, nodeDir)))
                                           Case "/"
                                               Select Case nodeDir
                                                   Case Direction.Up
                                                       newNodes.Add(New PathNode(Direction.Right, GetNextNode(Pattern, origin, Direction.Right)))
                                                   Case Direction.Right
                                                       newNodes.Add(New PathNode(Direction.Up, GetNextNode(Pattern, origin, Direction.Up)))
                                                   Case Direction.Down
                                                       newNodes.Add(New PathNode(Direction.Left, GetNextNode(Pattern, origin, Direction.Left)))
                                                   Case Direction.Left
                                                       newNodes.Add(New PathNode(Direction.Down, GetNextNode(Pattern, origin, Direction.Down)))
                                               End Select
                                           Case "\"
                                               Select Case nodeDir
                                                   Case Direction.Up
                                                       newNodes.Add(New PathNode(Direction.Left, GetNextNode(Pattern, origin, Direction.Left)))
                                                   Case Direction.Right
                                                       newNodes.Add(New PathNode(Direction.Down, GetNextNode(Pattern, origin, Direction.Down)))
                                                   Case Direction.Down
                                                       newNodes.Add(New PathNode(Direction.Right, GetNextNode(Pattern, origin, Direction.Right)))
                                                   Case Direction.Left
                                                       newNodes.Add(New PathNode(Direction.Up, GetNextNode(Pattern, origin, Direction.Up)))
                                               End Select
                                           Case "|"
                                               Select Case nodeDir
                                                   Case Direction.Up, Direction.Down
                                                       newNodes.Add(New PathNode(nodeDir, GetNextNode(Pattern, origin, nodeDir)))
                                                   Case Direction.Left, Direction.Right
                                                       newNodes.Add(New PathNode(Direction.Up, GetNextNode(Pattern, origin, Direction.Up)))
                                                       newNodes.Add(New PathNode(Direction.Down, GetNextNode(Pattern, origin, Direction.Down)))
                                               End Select
                                           Case "-"
                                               Select Case nodeDir
                                                   Case Direction.Up, Direction.Down
                                                       newNodes.Add(New PathNode(Direction.Left, GetNextNode(Pattern, origin, Direction.Left)))
                                                       newNodes.Add(New PathNode(Direction.Right, GetNextNode(Pattern, origin, Direction.Right)))
                                                   Case Direction.Left, Direction.Right
                                                       newNodes.Add(New PathNode(nodeDir, GetNextNode(Pattern, origin, nodeDir)))
                                               End Select
                                       End Select
                                   End If
                               End Sub)
        Return newNodes.ToList()
    End Function

    Public Function GetNextNode(Pattern(,) As Node, origin As Node, dir As Direction) As Node
        Dim newNode As Node
        Select Case dir
            Case Direction.Up
                If origin.x = 0 Then Return Nothing
                newNode = Pattern(origin.x - 1, origin.y)
            Case Direction.Right
                If origin.y = Pattern.GetUpperBound(1) Then Return Nothing
                newNode = Pattern(origin.x, origin.y + 1)
            Case Direction.Down
                If origin.x = Pattern.GetUpperBound(0) Then Return Nothing
                newNode = Pattern(origin.x + 1, origin.y)
            Case Direction.Left
                If origin.y = 0 Then Return Nothing
                newNode = Pattern(origin.x, origin.y - 1)
        End Select

        newNode.Energized = True

        Return newNode
    End Function

    Public Class Node
        Public x As Integer
        Public y As Integer
        Public value As Char
        Public Energized As Boolean = False

        Public Sub New(x As Integer, y As Integer, value As Char)
            Me.x = x
            Me.y = y
            Me.value = value
        End Sub
    End Class



    Public Class PathNode
        Public Node As Node
        Public dir As Direction

        Public Sub New(dir As Direction, node As Node)
            Me.Node = node
            Me.dir = dir
        End Sub
    End Class

End Module

