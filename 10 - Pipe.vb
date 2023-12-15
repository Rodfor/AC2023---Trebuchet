Imports Microsoft.VisualBasic.FileIO
Module Module10
     Public sub Pipes()
        Dim pad = "C:\Users\Matthias\Source\Repos\Rodfor\AC2023---Trebuchet\Pipes.txt"
        Dim lijnen As New List(Of String)

        Using reader = My.Computer.FileSystem.OpenTextFileReader(pad)
            While Not reader.EndOfStream
                Dim Line As string = reader.ReadLine
                lijnen.Add(Line)
            End While
            reader.Close
            reader.Dispose
        End Using

        Dim lijn As string
        Dim Nodes As new List(Of Node)
        Dim Start As Node

        For i = 0 To lijnen.Count - 1
            lijn = lijnen(i)
            For j = 0 to lijn.Length - 1
                Dim N As New Node
                N.row = i
                N.col = j
                N.PipeType = lijn(j)
                Nodes.Add(N)
                If N.PipeType = "S" Then
                    N.inLoop = True
                    Start = N
                End If
            Next
        Next

        Dim Route As new List(Of Node)
        Route.Add(Start)

        Dim boven = Nodes.Where(Function(x) x.row = Start.row - 1 AndAlso x.col = Start.col).FirstOrDefault
        If boven IsNot Nothing Then
            Select Case boven.PipeType
                Case "|" 
                    boven.Kant = Node.Richting.Boven
                    Start.Kant = Node.Richting.Boven
                    Route.Add(boven)
                Case "F"
                    boven.Kant = Node.Richting.rechts
                    Start.Kant = Node.Richting.Boven
                    Route.Add(boven)
                Case "7"
                    boven.Kant = Node.Richting.Links
                    Start.Kant = Node.Richting.Boven
                    Route.Add(boven)
            End Select
        End If
           
        Dim onder = Nodes.Where(Function(x) x.row = Start.row + 1 AndAlso x.col = Start.col).FirstOrDefault
        If onder IsNot Nothing Then
            Select Case onder.PipeType
                Case "|"
                    onder.Kant = Node.Richting.Onder
                    Start.Kant = Node.Richting.Onder
                    Route.Add(onder)
                Case "L"
                    onder.Kant = Node.Richting.Rechts
                    Start.Kant = Node.Richting.Onder
                    Route.Add(onder)
                Case "J"
                    onder.Kant = Node.Richting.Links
                    Start.Kant = Node.Richting.Onder
                    Route.Add(onder)
            End Select
        End If
            
        Dim links = Nodes.Where(Function(x) x.row = Start.row AndAlso x.col = Start.col - 1).FirstOrDefault
        If links IsNot Nothing Then
            Select Case links.PipeType
                Case "L", "F", "-"
                    links.Kant = Node.Richting.Boven
                    Start.Kant = Node.Richting.Links
                    Route.Add(links)
                Case "F"
                     links.Kant = Node.Richting.Onder
                    Start.Kant = Node.Richting.Links
                    Route.Add(links)
                Case "-"
                    links.Kant = Node.Richting.Links
                    Start.Kant = Node.Richting.Links
                    Route.Add(links)
            End Select
        End If

        
        Dim rechts = Nodes.Where(Function(x) x.row = Start.row  AndAlso x.col = Start.col + 1).FirstOrDefault
        If rechts IsNot Nothing Then
             Select Case rechts.PipeType
                Case "-", "J", "7"
                    rechts.Kant = Node.Richting.Rechts
                    Start.Kant = Node.Richting.Rechts
                    Route.Add(rechts)
                Case "J"
                    rechts.Kant = Node.Richting.Boven
                    Start.Kant = Node.Richting.Rechts
                    Route.Add(rechts)
                Case "7"
                    rechts.Kant = Node.Richting.Onder
                    Start.Kant = Node.Richting.Rechts
                    Route.Add(rechts)
            End Select
        End If
      
        Route.RemoveAt(2)
       

        Dim CurrentNode = Route(1)
        CurrentNode.inLoop = True
        Console.WriteLine(Start.PipeType)
        Console.WriteLine(CurrentNode.PipeType)

        Dim Buiten As new List(Of Node)
        Dim Binnen As new List(Of Node)


        While CurrentNode.PipeType <> "S"
            Dim A As Node
            Dim b As Node
            Dim NextNode As Node
            Select Case CurrentNode.PipeType
                Case "|"
                    A = Nodes.Where(Function(x) x.row = CurrentNode.row + 1 AndAlso x.col = CurrentNode.col).FirstOrDefault
                    B = Nodes.Where(Function(x) x.row = CurrentNode.row - 1  AndAlso x.col = CurrentNode.col).FirstOrDefault
                Case "-"
                    A = Nodes.Where(Function(x) x.row = CurrentNode.row  AndAlso x.col = CurrentNode.col + 1).FirstOrDefault
                    B = Nodes.Where(Function(x) x.row = CurrentNode.row  AndAlso x.col = CurrentNode.col - 1).FirstOrDefault
                Case "F"
                    A = Nodes.Where(Function(x) x.row = CurrentNode.row AndAlso x.col = CurrentNode.col + 1).FirstOrDefault
                    B = Nodes.Where(Function(x) x.row = CurrentNode.row + 1 AndAlso x.col = CurrentNode.col).FirstOrDefault
                Case "L"
                    A = Nodes.Where(Function(x) x.row = CurrentNode.row AndAlso x.col = CurrentNode.col + 1).FirstOrDefault
                    B = Nodes.Where(Function(x) x.row = CurrentNode.row - 1 AndAlso x.col = CurrentNode.col).FirstOrDefault
                Case "J"
                    A = Nodes.Where(Function(x) x.row = CurrentNode.row AndAlso x.col = CurrentNode.col - 1).FirstOrDefault
                    B = Nodes.Where(Function(x) x.row = CurrentNode.row - 1 AndAlso x.col = CurrentNode.col).FirstOrDefault
                 Case "7"
                    A = Nodes.Where(Function(x) x.row = CurrentNode.row AndAlso x.col = CurrentNode.col - 1).FirstOrDefault
                    B = Nodes.Where(Function(x) x.row = CurrentNode.row + 1 AndAlso x.col = CurrentNode.col).FirstOrDefault
                End Select

            If A IsNot Nothing AndAlso (not Route.Contains(A) OrElse A.PipeType = "S") Then
                NextNode = A
            Else 
                NextNode = B
            End If

            Dim N1 As Node
            Dim N2 As node
            Dim outofboundsLoop As New Node
            outofboundsLoop.inLoop = True

            Select Case NextNode.PipeType
                Case "|" 
                    N1 = Nodes.Where(Function(x) x.row = NextNode.row AndAlso x.col = NextNode.col - 1).FirstOrDefault
                    N2 = Nodes.Where(Function(x) x.row = NextNode.row AndAlso x.col = NextNode.col + 1).FirstOrDefault

                    If CurrentNode.Kant = Node.Richting.Boven Then
                        NextNode.Kant = Node.Richting.Boven                     
                        If N1 IsNot Nothing Then Buiten.Add(N1) Else Buiten.Add(outofboundsLoop)                        
                        If N2 IsNot Nothing Then Binnen.Add(N2) Else Binnen.Add(outofboundsLoop)
                    Else
                        NextNode.Kant = Node.Richting.Onder
                        If N1 IsNot Nothing Then Binnen.Add(N1) Else Binnen.Add(outofboundsLoop)
                        If N2 IsNot Nothing Then Buiten.Add(N2) Else Buiten.Add(outofboundsLoop)                     
                    End If
                Case "F"
                     N1 = Nodes.Where(Function(x) x.row = NextNode.row AndAlso x.col = NextNode.col - 1).FirstOrDefault
                     N2 = Nodes.Where(Function(x) x.row = NextNode.row - 1 AndAlso x.col = NextNode.col).FirstOrDefault
                    If CurrentNode.Kant = Node.Richting.Boven Then
                        NextNode.Kant = Node.Richting.Rechts                      
                        If N1 IsNot Nothing Then Buiten.Add(N1) Else Buiten.Add(outofboundsLoop)                 
                        If N2 IsNot Nothing Then Buiten.Add(N2) Else Buiten.Add(outofboundsLoop)
                    Else
                        NextNode.Kant = Node.Richting.Onder
                        If N1 IsNot Nothing Then Binnen.Add(N1) Else Binnen.Add(outofboundsLoop)     
                        If N2 IsNot Nothing Then Binnen.Add(N2) Else Binnen.Add(outofboundsLoop)     
                    End If
                Case "-"
                    N1 = Nodes.Where(Function(x) x.row = NextNode.row + 1 AndAlso x.col = NextNode.col).FirstOrDefault
                    N2 = Nodes.Where(Function(x) x.row = NextNode.row - 1 AndAlso x.col = NextNode.col).FirstOrDefault
                    If CurrentNode.Kant = Node.Richting.Rechts Then
                        NextNode.Kant = Node.Richting.Rechts                       
                        If N1 IsNot Nothing Then Binnen.Add(N1) Else Binnen.Add(outofboundsLoop)
                        If N2 IsNot Nothing Then Buiten.Add(N2) Else Buiten.Add(outofboundsLoop)                 
                    Else
                        NextNode.Kant = Node.Richting.Links
                        If N1 IsNot Nothing Then Buiten.Add(N1) Else Buiten.Add(outofboundsLoop) 
                        If N2 IsNot Nothing Then Binnen.Add(N2) Else Binnen.Add(outofboundsLoop)     
                    End If
                 Case "7"
                    N1 = Nodes.Where(Function(x) x.row = NextNode.row - 1 AndAlso x.col = NextNode.col).FirstOrDefault
                    N2 = Nodes.Where(Function(x) x.row = NextNode.row AndAlso x.col = NextNode.col + 1).FirstOrDefault
                    If CurrentNode.Kant = Node.Richting.Rechts Then
                        NextNode.Kant = Node.Richting.Onder
                        If N1 IsNot Nothing Then Buiten.Add(N1) Else Buiten.Add(outofboundsLoop)                     
                        If N2 IsNot Nothing Then Buiten.Add(N2) Else Buiten.Add(outofboundsLoop)
                    Else
                        NextNode.Kant = Node.Richting.Links
                        If N1 IsNot Nothing Then Binnen.Add(N1) Else Binnen.Add(outofboundsLoop)     
                        If N2 IsNot Nothing Then Binnen.Add(N2) Else Binnen.Add(outofboundsLoop)     
                    End If
                Case "J"
                    N1 = Nodes.Where(Function(x) x.row = NextNode.row AndAlso x.col = NextNode.col + 1).FirstOrDefault
                    N2 = Nodes.Where(Function(x) x.row = NextNode.row + 1 AndAlso x.col = NextNode.col).FirstOrDefault
                    If CurrentNode.Kant = Node.Richting.Onder Then
                        NextNode.Kant = Node.Richting.Links                   
                        If N1 IsNot Nothing Then Buiten.Add(N1) Else Buiten.Add(outofboundsLoop)                     
                        If N2 IsNot Nothing Then Buiten.Add(N2) Else Buiten.Add(outofboundsLoop)
                    Else
                        NextNode.Kant = Node.Richting.Boven
                        If N1 IsNot Nothing Then Binnen.Add(N1) Else Binnen.Add(outofboundsLoop)    
                        If N2 IsNot Nothing Then Binnen.Add(N2) Else Binnen.Add(outofboundsLoop)     
                    End If
                Case "L"
                    N1 = Nodes.Where(Function(x) x.row = NextNode.row AndAlso x.col = NextNode.col - 1).FirstOrDefault
                    N2 = Nodes.Where(Function(x) x.row = NextNode.row + 1 AndAlso x.col = NextNode.col).FirstOrDefault
                     If CurrentNode.Kant = Node.Richting.Links Then
                        NextNode.Kant = Node.Richting.Boven                       
                        If N1 IsNot Nothing Then Buiten.Add(N1) Else Buiten.Add(outofboundsLoop)                     
                        If N2 IsNot Nothing Then Buiten.Add(N2) Else Buiten.Add(outofboundsLoop)
                    Else
                        NextNode.Kant = Node.Richting.Rechts
                        If N1 IsNot Nothing Then Binnen.Add(N1) Else Binnen.Add(outofboundsLoop)     
                        If N2 IsNot Nothing Then Binnen.Add(N2) Else Binnen.Add(outofboundsLoop)
                    End If
            End Select

            CurrentNode = NextNode
            CurrentNode.inLoop = True

            Route.Add(CurrentNode)
          '  Console.WriteLine(CurrentNode.PipeType)
        End While

        Dim temp As List(Of Node)
     
        Console.WriteLine("Binnen = " + Binnen.Count.ToString)
        Console.WriteLine("Buiten = " + Buiten.Count.ToString)

        If Binnen.Count > Buiten.Count Then
            temp = Binnen
            Binnen = Buiten
            Buiten = temp
        End If

        Console.WriteLine("Binnen = " + Binnen.Count.ToString)
        Console.WriteLine("Buiten = " + Buiten.Count.ToString)

        Dim BinnenStart As New List(Of Node)

        For each N In Binnen
            If Not N.inLoop AndAlso not BinnenStart.Contains(N) Then
                N.Flooded = True
                BinnenStart.add(N)
                Console.WriteLine("S " + N.row.ToString + ", " + N.col.ToString)
            End If
        Next

        Dim EindStart As New List(Of Node)
        EindStart.AddRange(BinnenStart)

        For each N In BinnenStart
            Dim adjacent As List(Of Node) = FindAdjacent(N, Nodes)
            Console.WriteLine(N.row.ToString + ", " + N.col.ToString + " : " + adjacent.Count.ToString)
            For each J In adjacent
                If Not EindStart.Contains(J) Then
                    EindStart.Add(J)
                    Console.WriteLine(J.row.ToString + ", " + J.col.ToString)
                End If
            Next
        Next

        Console.WriteLine(Route.Count / 2)
           
        Console.WriteLine(EindStart.Count.ToString)
    End Sub

    Public Function FindAdjacent(node As Node, Nodes As List(Of Node)) As List(Of Node)
        Dim adjacent As New List(Of Node)

        Dim N1 = Nodes.Where(Function(x) x.row = node.row AndAlso x.col = node.col - 1).FirstOrDefault
        Dim N2 = Nodes.Where(Function(x) x.row = node.row AndAlso x.col = node.col + 1).FirstOrDefault
        Dim N3 = Nodes.Where(Function(x) x.row = node.row - 1 AndAlso x.col = node.col).FirstOrDefault
        Dim N4 = Nodes.Where(Function(x) x.row = node.row + 1 AndAlso x.col = node.col).FirstOrDefault

        If N1 IsNot Nothing Then adjacent.Add(N1)
         If N2 IsNot Nothing Then adjacent.Add(N2)
         If N3 IsNot Nothing Then adjacent.Add(N3)
         If N4 IsNot Nothing Then adjacent.Add(N4)

        Dim ReturnNodes As New List(Of Node)
        For each N In Adjacent
            If N.inLoop = False AndAlso N.Flooded = False Then
                N.Flooded = True
                Dim AdjacentNodes As List(Of Node) = FindAdjacent(N, nodes)
                ReturnNodes.Add(N)
                For each J In AdjacentNodes
                    If not ReturnNodes.Contains(J) Then
                        ReturnNodes.Add(J)
                    End If
                Next
            End If
        Next     

        Return ReturnNodes
    End Function

    Public Class Node
        Public Enum Richting
            Boven
            Onder
            Links
            Rechts
        End Enum

        Public row As Int32
        Public col As Int32
        Public PipeType As Char
        Public Kant As Richting
        Public inLoop As Boolean = False
        Public Flooded As Boolean = False

    End Class

End Module
