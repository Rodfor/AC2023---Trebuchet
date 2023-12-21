Imports System.Net.NetworkInformation
Imports System.Reflection
Imports Microsoft.VisualBasic.FileIO
Imports System.Runtime.Caching
Imports System.IO

Module Module17

    Public Sub Crucible()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Crucible.txt"
        Dim Lijnen() As String = System.IO.File.ReadAllLines(pad)
        Dim Pattern(Lijnen.Count - 1, Lijnen.First.Length - 1, 3, 9) As Node

        For i = 0 To Lijnen.Count() - 1
            Dim lijn = Lijnen(i)
            For j = 0 To lijn.Length - 1
                If i <> Pattern.GetLowerBound(0) Then
                    For x = 0 To 9
                        Pattern(i, j, Direction.Down, x) = New Node(i, j, Val(lijn(j)), Direction.Down, x)
                    Next
                End If

                If i <> Pattern.GetUpperBound(0) Then
                    For x = 0 To 9
                        Pattern(i, j, Direction.Up, x) = New Node(i, j, Val(lijn(j)), Direction.Up, x)
                    Next
                End If

                If j <> Pattern.GetLowerBound(1) Then
                    For x = 0 To 9
                        Pattern(i, j, Direction.Right, x) = New Node(i, j, Val(lijn(j)), Direction.Right, x)
                    Next
                End If

                If j <> Pattern.GetUpperBound(1) Then
                    For x = 0 To 9
                        Pattern(i, j, Direction.Left, x) = New Node(i, j, Val(lijn(j)), Direction.Left, x)
                    Next
                End If
            Next
        Next

        Dim targetNode = Pattern(Pattern.GetUpperBound(0), Pattern.GetUpperBound(1), Direction.Down, 0)
        Dim startnode = New Node(Pattern.GetLowerBound(0), Pattern.GetLowerBound(1), 2, Direction.Right, 0)
        startnode.hCost = GetDistance(startnode, targetNode)
        Pattern(Pattern.GetLowerBound(0), Pattern.GetLowerBound(1), Direction.Right, 0) = startnode

        Dim OpenList As New Heap()
        OpenList.Add(startnode)
        OpenList.items(0).parent = startnode

        ' Dim closedList As New HashSet(Of Node)

        Dim currentNode = OpenList.RemoveFirst

        Do While currentNode.hCost > 0
            'Console.WriteLine(currentNode.x.ToString + "," + currentNode.y.ToString + "," + currentNode.Dir.ToString + "," + currentNode.timesMoved.ToString + " : H:" + currentNode.hCost.ToString + " G:" + currentNode.gCost.ToString + " F:" + currentNode.fCost.ToString + " P:" + currentNode.parent.Name)

            For Each B In GetBuren(Pattern, currentNode)
                If B Is Nothing OrElse B.Visited Then
                    Continue For
                End If

                Dim CostToBuur As Integer = currentNode.gCost + B.value
                If CostToBuur < B.gCost OrElse Not OpenList.items.Contains(B) Then
                    B.gCost = CostToBuur
                    B.hCost = GetDistance(B, targetNode)
                    B.parent = currentNode

                    'Console.WriteLine(B.x.ToString + "," + B.y.ToString + " : H:" + B.hCost.ToString + " G:" + B.gCost.ToString + " F:" + B.fCost.ToString + " P:" + currentNode.Name)

                    If Not OpenList.items.Contains(B) Then
                        OpenList.Add(B)
                    Else
                        OpenList.Update(B)
                    End If
                End If
            Next

            currentNode.Visited = True
            currentNode = OpenList.RemoveFirst

        Loop

        Dim path As New List(Of Node)

        While Not currentNode.Name = startnode.Name
            path.Add(currentNode)
            'Console.WriteLine(currentNode.x.ToString + "," + currentNode.y.ToString + " : V:" + currentNode.value.ToString + " P:" + currentNode.parent.Name)
            currentNode = currentNode.parent
        End While

        path.Reverse()
        Dim som = 0
        For Each N In path
            som += N.value
            Console.WriteLine(N.x.ToString + "," + N.y.ToString + "," + N.Dir.ToString + "," + N.timesMoved.ToString + " : V:" + N.value.ToString + " P:" + N.parent.Name)
        Next

        Console.WriteLine(som)
    End Sub

    Public Function GetBuren(Pattern(,,,) As Node, N As Node) As List(Of Node)
        Dim Buren As New List(Of Node)



        If N.timesMoved < 3 Then
            Select Case N.Dir
                Case Direction.Up
                    If N.x + (N.timesMoved - 2) > Pattern.GetLowerBound(0) Then
                        Buren.Add(Pattern(N.x - 1, N.y, Direction.Up, N.timesMoved + 1))
                    End If

                Case Direction.Down
                    If N.x + (2 - N.timesMoved) < Pattern.GetUpperBound(0) Then
                        Buren.Add(Pattern(N.x + 1, N.y, Direction.Down, N.timesMoved + 1))
                    End If

                Case Direction.Left
                    If N.y + (N.timesMoved - 2) > Pattern.GetLowerBound(1) Then
                        Buren.Add(Pattern(N.x, N.y - 1, Direction.Left, N.timesMoved + 1))
                    End If

                Case Direction.Right
                    If N.y + (2 - N.timesMoved) < Pattern.GetUpperBound(1) Then
                        Buren.Add(Pattern(N.x, N.y + 1, Direction.Right, N.timesMoved + 1))
                    End If
            End Select

        Else
            If N.x > Pattern.GetLowerBound(0) Then
                If N.Dir = Direction.Up Then
                    If N.timesMoved < 9 Then Buren.Add(Pattern(N.x - 1, N.y, Direction.Up, N.timesMoved + 1))
                ElseIf N.Dir <> Direction.Down AndAlso N.x > Pattern.GetLowerBound(0) + 3 Then
                    Buren.Add(Pattern(N.x - 1, N.y, Direction.Up, 0))
                End If
            End If

            If N.x < Pattern.GetUpperBound(0) Then
                If N.Dir = Direction.Down Then
                    If N.timesMoved < 9 Then Buren.Add(Pattern(N.x + 1, N.y, Direction.Down, N.timesMoved + 1))
                ElseIf N.Dir <> Direction.Up AndAlso N.x < Pattern.GetUpperBound(0) - 3 Then
                    Buren.Add(Pattern(N.x + 1, N.y, Direction.Down, 0))
                End If
            End If

            If N.y > Pattern.GetLowerBound(1) Then
                If N.Dir = Direction.Left Then
                    If N.timesMoved < 9 Then Buren.Add(Pattern(N.x, N.y - 1, Direction.Left, N.timesMoved + 1))
                ElseIf N.Dir <> Direction.Right AndAlso N.y > Pattern.GetLowerBound(1) + 3 Then
                    Buren.Add(Pattern(N.x, N.y - 1, Direction.Left, 0))
                End If
            End If

            If N.y < Pattern.GetUpperBound(1) Then
                If N.Dir = Direction.Right Then
                    If N.timesMoved < 9 Then Buren.Add(Pattern(N.x, N.y + 1, Direction.Right, N.timesMoved + 1))
                ElseIf N.Dir <> Direction.Left AndAlso N.y < Pattern.GetUpperBound(1) - 3 Then
                    Buren.Add(Pattern(N.x, N.y + 1, Direction.Right, 0))
                End If
            End If

        End If


        'If N.x > Pattern.GetLowerBound(0) Then
        '    If N.Dir = Direction.Up Then
        '        If N.timesMoved < 2 Then Buren.Add(Pattern(N.x - 1, N.y, Direction.Up, N.timesMoved + 1))
        '    ElseIf N.Dir <> Direction.Down Then
        '        Buren.Add(Pattern(N.x - 1, N.y, Direction.Up, 0))
        '    End If
        'End If

        'If N.x < Pattern.GetUpperBound(0) Then
        '    If N.Dir = Direction.Down Then
        '        If N.timesMoved < 2 Then Buren.Add(Pattern(N.x + 1, N.y, Direction.Down, N.timesMoved + 1))
        '    ElseIf N.Dir <> Direction.Up Then
        '        Buren.Add(Pattern(N.x + 1, N.y, Direction.Down, 0))
        '    End If
        'End If

        'If N.y > Pattern.GetLowerBound(1) Then
        '    If N.Dir = Direction.Left Then
        '        If N.timesMoved < 2 Then Buren.Add(Pattern(N.x, N.y - 1, Direction.Left, N.timesMoved + 1))
        '    ElseIf N.Dir <> Direction.Right Then
        '        Buren.Add(Pattern(N.x, N.y - 1, Direction.Left, 0))
        '    End If
        'End If

        'If N.y < Pattern.GetUpperBound(1) Then
        '    If N.Dir = Direction.Right Then
        '        If N.timesMoved < 2 Then Buren.Add(Pattern(N.x, N.y + 1, Direction.Right, N.timesMoved + 1))
        '    ElseIf N.Dir <> Direction.Left Then
        '        Buren.Add(Pattern(N.x, N.y + 1, Direction.Right, 0))
        '    End If
        'End If

        Return Buren
    End Function

    Public Function GetDistance(N1 As Node, N2 As Node) As Integer
        Dim dstX = Math.Abs(N1.x - N2.x)
        Dim dstY = Math.Abs(N1.y - N2.y)

        Return dstX + dstY
    End Function


    Public Class Node
        Public x As Integer
        Public y As Integer

        Public gCost As Integer
        Public hCost As Integer

        Public value As Short

        Public parent As Node

        Public timesMoved As Short
        Public Dir As Direction

        Public Visited As Boolean

        Public Property HeapIndex As Integer

        Public ReadOnly Property Name As String
            Get
                Return x.ToString + "," + y.ToString
            End Get
        End Property

        Public ReadOnly Property fCost As Integer
            Get
                Return gCost + hCost
            End Get
        End Property

        Public Sub New(x As Integer, y As Integer, value As Short, dir As Direction, aantal As Short)
            Me.x = x
            Me.y = y
            Me.value = value
            Me.Dir = dir
            Me.timesMoved = aantal
        End Sub

        Public Function CompareTo(N As Node) As Integer
            If fCost = N.fCost Then
                Return hCost - N.hCost
            Else
                Return fCost - N.fCost
            End If
        End Function
    End Class


    Public Class Heap
        Public items As New List(Of Node)
        Public Count As Integer

        Public Sub Add(item As Node)
            item.HeapIndex = Count
            items.Add(item)
            SortUp(item)
            Count += 1
        End Sub

        Public Function RemoveFirst() As Node
            Dim N = items.First
            Count -= 1
            items(0) = items(Count)
            items(0).HeapIndex = 0
            items.RemoveAt(Count)
            If Count > 0 Then SortDown(items(0))
            Return N
        End Function

        Public Sub Update(N As Node)
            SortUp(N)
        End Sub


        Public Sub SortUp(item As Node)
            Dim parentIndex = (item.HeapIndex - 1) / 2
            While parentIndex >= 0
                Dim parentItem = items(parentIndex)
                If item.CompareTo(parentItem) < 0 Then
                    Swap(item, parentItem)
                    parentIndex = (item.HeapIndex - 1) / 2
                Else
                    Exit While
                End If
            End While
        End Sub

        Public Sub SortDown(item As Node)
            While True
                Dim IndexLeft = item.HeapIndex * 2 + 1
                Dim IndexRight = item.HeapIndex * 2 + 2
                Dim IndexSwap = 0

                If IndexLeft < Count Then
                    IndexSwap = IndexLeft
                    If IndexRight < Count Then
                        If items(IndexLeft).CompareTo(items(IndexRight)) > 0 Then
                            IndexSwap = IndexRight
                        End If
                    End If

                    If item.CompareTo(items(IndexSwap)) > 0 Then
                        Swap(item, items(IndexSwap))
                    Else
                        Exit While
                    End If
                Else
                    Exit While
                End If

            End While


        End Sub

        Private Sub Swap(itemA As Node, itemB As Node)
            items(itemA.HeapIndex) = itemB
            items(itemB.HeapIndex) = itemA
            Dim index = itemA.HeapIndex
            itemA.HeapIndex = itemB.HeapIndex
            itemB.HeapIndex = index
        End Sub

    End Class



End Module

