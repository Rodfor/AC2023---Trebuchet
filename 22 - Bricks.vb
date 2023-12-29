Module Module22
    Dim Pattern(10, 10, 300) As Brick

    Public Sub Bricks()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Bricks.txt"
        Dim Lijnen() As String = System.IO.File.ReadAllLines(pad)
        Dim Bricks As New Dictionary(Of Integer, Brick)

        For i = 0 To Lijnen.Count - 1
            Dim B As New Brick(Lijnen(i))
            B.Name = i
            Bricks.Add(i, B)
        Next

        Dim BricksByHeight As List(Of Brick) = Bricks.Values.OrderBy(Function(x) x.Lowest).ToList

        For Each B In BricksByHeight
            If B.Lowest <> 1 Then
                B.drop()
            End If
        Next

        Dim teller As Integer = 0

        For Each B In BricksByHeight
            Dim safe = True
            For Each S In B.Supporting
                If S.SupportedBy.Count <= 1 Then
                    safe = False
                End If
            Next
            If safe Then
                Console.WriteLine(B.Name.ToString + " safe")
                teller += 1
            End If
        Next

        Console.WriteLine(teller)
        'Console.WriteLine()

        'For x = 0 To Pattern.GetUpperBound(0)
        '    For z = 0 To 130
        '        For y = 0 To Pattern.GetUpperBound(1)
        '            Console.Write(If(Pattern(x, y, z) IsNot Nothing, Pattern(x, y, z).Name, "-"))
        '        Next
        '        Console.WriteLine()
        '    Next
        '    Console.WriteLine()
        '    Console.WriteLine()
        'Next


    End Sub


    Public Class Brick
        Public x1 As Integer
        Public y1 As Integer
        Public z1 As Integer

        Public x2 As Integer
        Public y2 As Integer
        Public z2 As Integer

        Public Name As Integer

        Public Supporting As New List(Of Brick)
        Public SupportedBy As New List(Of Brick)

        Public ReadOnly Property Lowest As Integer
            Get
                Return z1
            End Get
        End Property

        Public ReadOnly Property Highest As Integer
            Get
                Return z2
            End Get
        End Property

        Public Sub New(lijn As String)
            Dim coords = lijn.Split("~")
            Dim begincoords = coords(0).Split(",")
            Dim eindcoords = coords(1).Split(",")

            x1 = begincoords(0)
            y1 = begincoords(1)
            z1 = begincoords(2)

            x2 = eindcoords(0)
            y2 = eindcoords(1)
            z2 = eindcoords(2)

            If z1 = 1 Then Fill()

        End Sub

        Public Sub Drop()
            Dim newLowest = 1

            For x = 0 To x2 - x1
                For y = 0 To y2 - y1
                    Dim z = newLowest
                    While z < z1
                        If Pattern(x1 + x, y1 + y, z) IsNot Nothing Then
                            If Not SupportedBy.Contains(Pattern(x1 + x, y1 + y, z)) Then
                                If SupportedBy.Count <> 0 Then
                                    If SupportedBy.First.Highest < Pattern(x1 + x, y1 + y, z).Highest Then
                                        SupportedBy = New List(Of Brick)
                                    End If
                                End If
                                SupportedBy.Add(Pattern(x1 + x, y1 + y, z))
                                newLowest = Pattern(x1 + x, y1 + y, z).Highest
                                z = newLowest
                            End If
                        End If
                        z += 1
                    End While
                Next
            Next

            If SupportedBy.Count > 0 Then
                newLowest += 1
                For Each B In SupportedBy
                    B.Supporting.Add(Me)
                Next

            End If


            Console.WriteLine(Name.ToString + " dropped " + z1.ToString + " naar " + newLowest.ToString + " supported by :" + SupportedBy.Count.ToString)

            Dim hoogte = z2 - z1
            z1 = newLowest
            z2 = newLowest + hoogte

            Fill()

        End Sub

        Public Sub Fill()

            For i = 0 To x2 - x1
                Pattern(x1 + i, y1, z1) = Me
            Next
            For i = 0 To y2 - y1
                Pattern(x1, y1 + i, z1) = Me
            Next
            For i = 0 To z2 - z1
                Pattern(x1, y1, z1 + i) = Me
            Next
        End Sub
    End Class


End Module

