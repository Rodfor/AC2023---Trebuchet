Module Module23
    Dim Pattern(,) As Node
    Dim startnode As Node

    Public Sub Walk()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Walk.txt"
        Dim Lijnen() As String = System.IO.File.ReadAllLines(pad)

        ReDim Pattern(Lijnen.Count - 1, Lijnen.First.Length - 1)
        Dim targetnode As Node


        For i = 0 To Pattern.GetUpperBound(0)
            Dim lijn = Lijnen(i)
            For j = 0 To Pattern.GetUpperBound(1)
                Pattern(i, j) = New Node(i, j, lijn(j))
                If i = Pattern.GetLowerBound(0) AndAlso startnode Is Nothing AndAlso lijn(j) = "." Then
                    startnode = Pattern(i, j)
                End If

                If i = Pattern.GetUpperBound(0) AndAlso targetnode Is Nothing AndAlso lijn(j) = "." Then
                    targetnode = Pattern(i, j)
                End If
            Next
        Next

        startnode.Visited = True
        startnode.hCost = GetDistance(startnode, targetnode)

        Dim OpenList As New Heap()
        OpenList.Add(startnode)
        OpenList.items(0).parent = startnode

        Dim currentNode = OpenList.RemoveFirst

        While True
            'Console.WriteLine(currentNode.x.ToString + "," + currentNode.y.ToString + "," + currentNode.value.ToString + " : H:" + currentNode.hCost.ToString + " G:" + currentNode.gcost.ToString + " F:" + currentNode.fCost.ToString + " P:" + currentNode.parent.Name.ToString + " - " + OpenList.Count.ToString)

            For Each B In GetBuren(Pattern, currentNode)
                If B Is Nothing OrElse B.value = "#" Then
                    Continue For
                End If

                Dim CostToBuur As Integer = 1
                If (CostToBuur + currentNode.gcost > B.gcost OrElse Not OpenList.items.Contains(B)) AndAlso Not currentNode.HasParent(B) Then
                    B.gcost = CostToBuur + currentNode.gcost
                    B.hCost = GetDistance(B, targetnode)
                    B.parent = currentNode

                    'Console.WriteLine(B.x.ToString + "," + B.y.ToString + " : H:" + B.hCost.ToString + " G:" + B.gcost.ToString + " F:" + B.fCost.ToString + " P:" + currentNode.Name.ToString)

                    If Not OpenList.items.Contains(B) Then
                        OpenList.Add(B)
                    Else
                        OpenList.Update(B)
                    End If
                End If
            Next

            If OpenList.Count = 0 Then
                Exit While
            End If

            currentNode = OpenList.RemoveFirst

            If currentNode.hCost = 0 Then
                If OpenList.Count = 0 Then Exit While
                currentNode = OpenList.RemoveFirst
            End If

        End While

        Dim path As New List(Of Node)
        currentNode = targetnode

        While Not currentNode.Name = startnode.Name
            path.Add(targetnode)
             Console.WriteLine(currentNode.x.ToString + "," + currentNode.y.ToString + " : V:" + currentNode.value.ToString + " P:" + currentNode.parent.Name.ToString)
            currentNode.value = "0"
            currentNode = currentNode.parent

        End While

        For i = Pattern.GetLowerBound(0) To Pattern.GetUpperBound(0)
            For j = Pattern.GetLowerBound(1) To Pattern.GetUpperBound(1)
                Console.Write(Pattern(i, j).value)
            Next
            Console.WriteLine("")
        Next


        Console.WriteLine(path.Count)


    End Sub

    Public Function GetDistance(N1 As Node, N2 As Node) As Integer
        Dim dstX = Math.Abs(N1.x - N2.x)
        Dim dstY = Math.Abs(N1.y - N2.y)

        Return dstX + dstY
    End Function

    Public Function GetBuren(Pattern(,) As Node, N As Node) As List(Of Node)
        Dim Buren As New List(Of Node)

        'Select Case N.value
        '    Case "."
        If N.x < Pattern.GetUpperBound(0) Then
            Buren.Add(Pattern(N.x + 1, N.y))
        End If

        If N.x > Pattern.GetLowerBound(0) Then
            Buren.Add(Pattern(N.x - 1, N.y))
        End If

        If N.y < Pattern.GetUpperBound(1) Then
            Buren.Add(Pattern(N.x, N.y + 1))
        End If

        If N.y > Pattern.GetLowerBound(1) Then
            Buren.Add(Pattern(N.x, N.y - 1))
        End If

        '    Case "<"
        '        Buren.Add(Pattern(N.x, N.y - 1))
        '    Case ">"
        '        Buren.Add(Pattern(N.x, N.y + 1))
        '    Case "v"
        '        Buren.Add(Pattern(N.x + 1, N.y))
        '    Case "#"

        '    Case Else
        '        Buren.Add(Pattern(N.x - 1, N.y))

        'End Select

        Return Buren

    End Function

    Public Class Node
        Public x As Integer
        Public y As Integer

        Public G As Integer?
        Public hCost As Integer

        Public value As Char

        Public parent As Node

        Public Visited As Boolean

        Public Property gcost As Integer
            Get
                If G Is Nothing Then
                    G = GetDistance(Me, startnode)
                End If
                Return G
            End Get
            Set(value As Integer)
                G = value
            End Set
        End Property


        Public Property HeapIndex As Integer

        Public Property Name As Integer

        Public ReadOnly Property fCost As Integer
            Get
                Return gCost + hCost
            End Get
        End Property

        Public Sub New(x As Integer, y As Integer, value As Char)
            Me.x = x
            Me.y = y
            Me.value = value
            Me.Name = x * 1000 + y
        End Sub

        Public Function CompareTo(N As Node) As Integer
            If fCost = N.fCost Then
                Return hCost - N.hCost
            Else
                Return fCost - N.fCost
            End If
        End Function

        Public Function HasParent(p As Node)
            If parent.Name = startnode.Name Then Return False
            If parent.Name = p.Name Then
                Return True
            Else
                Return parent.HasParent(p)
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
                If item.CompareTo(parentItem) > 0 Then
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
                        If items(IndexLeft).CompareTo(items(IndexRight)) < 0 Then
                            IndexSwap = IndexRight
                        End If
                    End If

                    If item.CompareTo(items(IndexSwap)) < 0 Then
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

