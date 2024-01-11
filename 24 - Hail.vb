Module Module24
    Public A1 As Long = 7
    Public A2 As Long = 27


    'Public A1 As Long = 200000000000000
    'Public A2 As Long = 400000000000000

    Public Sub HailA()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Hail.txt"
        Dim Lijnen() As String = System.IO.File.ReadAllLines(pad)

        Dim stenen As New List(Of Stone)
        Dim ASCII As Integer = 0

        For Each L In Lijnen
            Dim P As New Stone
            Dim split = L.Split("@")
            Dim pos = split.First.Split(",")
            Dim vel = split.Last.Split(",")

            Dim Positie As New Position(pos(0), pos(1), pos(2))
            P.Pos = Positie

            P.name = ASCII

            P.vX = vel(0)
            P.vY = vel(1)
            P.vZ = vel(2)

            stenen.Add(P)
            ASCII += 1
        Next

        Dim Lines As New List(Of Line)

        For Each S In stenen
            Dim L As New Line(S)
            Lines.Add(L)
        Next

        Dim totaal As Integer = 0

        For i = 0 To Lines.Count - 1

            'For i = 0 To 0
            For j = i + 1 To Lines.Count - 1
                If Intersect(Lines(i), Lines(j)) Then
                    totaal += 1
                    Console.WriteLine(Lines(i).Stone.name + " - " + Lines(j).Stone.name + " collided")
                End If
            Next
        Next

        Console.WriteLine(totaal)

    End Sub

    Public Sub HailB()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Hail.txt"
        Dim Lijnen() As String = System.IO.File.ReadAllLines(pad)

        Dim stenen As New List(Of Stone3D)
        Dim stenenXY As New List(Of Stone2D)
        Dim stenenXZ As New List(Of Stone2D)
        Dim ASCII As Integer = 0

        For Each L In Lijnen
            Dim P As New Stone3D
            Dim split = L.Split("@")
            Dim pos = split.First.Split(",")
            Dim vel = split.Last.Split(",")

            Dim Positie As New Vector3(pos(0), pos(1), pos(2))
            P.pos = Positie

            P.name = ASCII

            Dim velocity As New Vector3(vel(0), vel(1), vel(2))
            P.vel = velocity

            stenen.Add(P)


            Dim PXY = New Stone2D
            Dim posXY = New Vector2(pos(0), pos(1))
            Dim velXY = New Vector2(vel(0), vel(1))
            PXY.pos = posXY
            PXY.vel = velXY

            stenenXY.Add(PXY)



            Dim PXZ = New Stone2D
            Dim posXZ = New Vector2(pos(0), pos(2))
            Dim velXZ = New Vector2(vel(0), vel(2))
            PXZ.pos = posXZ
            PXZ.vel = velXZ

            stenenXZ.Add(PXZ)


            ASCII += 1
        Next


        Dim StoneXY = Solve2D(stenenXY)
        Dim StoneXZ = Solve2D(stenenXZ)

        Console.WriteLine(StoneXY.x.ToString + ", " + StoneXY.y.ToString + ", " + StoneXZ.y.ToString)
        Console.WriteLine(Math.Round(StoneXY.x + StoneXY.y + StoneXZ.y))

    End Sub

    Public Function Solve2D(stenen As List(Of Stone2D)) As Vector2
        Dim s = 500

        For v1 = -s To s
            For v2 = -s To s
                Dim vel = New Vector2(v1, v2)
                Dim Steen = Intersection(translateV(stenen(0), vel), translateV(stenen(1), vel))


                If Steen IsNot Nothing Then
                    ' Console.WriteLine(Steen.x.ToString + ", " + Steen.y.ToString)
                    If stenen.All(Function(x) Hits(translateV(x, vel), Steen)) Then
                        Return Steen
                    End If
                End If
            Next
        Next

    End Function

    Public Function Intersection(s1 As Stone2D, s2 As Stone2D) As Vector2
        Dim det = s1.vel.x * s2.vel.y - s1.vel.y * s2.vel.x
        If det = 0 Then
            Return Nothing
        End If

        Dim b0 = s1.vel.x * s1.pos.y - s1.vel.y * s1.pos.x
        Dim b1 = s2.vel.x * s2.pos.y - s2.vel.y * s2.pos.x

        Return New Vector2((s2.vel.x * b0 - s1.vel.x * b1) / det, (s2.vel.y * b0 - s1.vel.y * b1) / det)
    End Function

    Public Function translateV(S As Stone2D, velocity As Vector2) As Stone2D
        Dim newStone = New Stone2D
        Dim pos = New Vector2(S.pos.x, S.pos.y)
        Dim vel = New Vector2(S.vel.x - velocity.x, S.vel.y - velocity.y)
        newStone.pos = pos
        newStone.vel = vel

        Return newStone
    End Function


    Public Function Hits(s As Stone2D, pos As Vector2)
        Dim d = (pos.x - s.pos.x) * s.vel.y - (pos.y - s.pos.y) * s.vel.x
        'Console.WriteLine("Hits - " + d.ToString)
        Return Math.Abs(d) < 0.000001
    End Function

    Public Function Intersect(LineA As Line, LineB As Line) As Boolean
        Try
            Dim x = (LineA.b * LineB.c - LineB.b * LineA.c) / (LineA.a * LineB.b - LineB.a * LineA.b)
            Dim y = (LineA.c * LineB.a - LineB.c * LineA.a) / (LineA.a * LineB.b - LineB.a * LineA.b)

            Console.WriteLine("Collision " + x.ToString + ", " + y.ToString)

            If A1 <= x AndAlso x <= A2 AndAlso A1 <= y AndAlso y <= A2 Then
                Dim t1 = (x - LineA.Stone.Pos.X) / LineA.Stone.vX
                Dim t2 = (x - LineB.Stone.Pos.X) / LineB.Stone.vX

                If t1 >= 0 AndAlso t2 >= 0 Then
                    Return True
                Else
                    Console.WriteLine(" - Collision in past")
                End If
            End If

            Return False
        Catch ex As Exception
            Console.WriteLine("No collision")
            Return False
        End Try


    End Function

    Public Class Stone2D
        Public vel As Vector2
        Public pos As Vector2
    End Class

    Public Class Stone3D
        Public name As String

        Public vel As Vector3
        Public pos As Vector3

    End Class


    Public Class Vector2
        Public x As Decimal
        Public y As Decimal

        Public Sub New(pX As Decimal, pY As Decimal)
            Me.x = pX
            Me.y = pY
        End Sub
    End Class

    Public Class Vector3
        Public x As Decimal
        Public y As Decimal
        Public z As Decimal

        Public Sub New(pX As Decimal, pY As Decimal, pZ As Decimal)
            Me.x = pX
            Me.y = pY
            Me.z = pZ
        End Sub
    End Class
    Public Class Stone
        Public name As String

        Public Pos As Position

        Public Property X As Decimal
            Get
                Return Pos.X
            End Get
            Set(value As Decimal)
                Pos.pX = value
            End Set
        End Property

        Public Property Y As Decimal
            Get
                Return Pos.Y
            End Get
            Set(value As Decimal)
                Pos.pY = value
            End Set
        End Property
        Public Property Z As Decimal
            Get
                Return Pos.Z
            End Get
            Set(value As Decimal)
                Pos.pZ = value
            End Set
        End Property



        Public vX As Decimal
        Public vY As Decimal
        Public vZ As Decimal


    End Class

    Public Class Line
        Public Stone As Stone
        Public a As Decimal
        Public b As Decimal
        Public c As Decimal

        Public Sub New(S As Stone)
            Me.Stone = S

            a = S.vY
            b = -S.vX
            c = S.vX * S.Pos.Y - S.vY * S.Pos.X


        End Sub


    End Class

    Public Class Position
        Public pX As Decimal
        Public pY As Decimal
        Public pZ As Decimal

        Public ReadOnly Property X As Decimal
            Get
                Return pX
            End Get
        End Property

        Public ReadOnly Property Y As Decimal
            Get
                Return pY
            End Get
        End Property
        Public ReadOnly Property Z As Decimal
            Get
                Return pZ
            End Get
        End Property


        Public Sub New(pX As Decimal, pY As Decimal, pZ As Decimal)
            Me.pX = pX
            Me.pY = pY
            Me.pZ = pZ
        End Sub
    End Class
End Module

