Imports Microsoft.VisualBasic.FileIO
Imports System.Threading

Public Module Maps
    Public sub Mapping()
      Dim pad = "C:\Users\Matthias\Source\Repos\Rodfor\AC2023---Trebuchet\Maps.txt"
        Dim lijnen As New List(Of String())

        Using parser = New TextFieldParser(pad)
            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters(" ")

            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                lijnen.Add(fields)
            End While

            parser.Close()
            parser.Dispose()
        End Using

        Dim Instructions As String = lijnen(0)(0)
        Dim Maps As new Dictionary(Of String,  Kaart)

        For i = 1 To lijnen.Count -1
            Dim K As New Kaart
            K.Naam = Lijnen(i)(0)
            Maps.add(K.Naam, K)
        Next

        For i = 1 To lijnen.Count -1
            Dim links = Lijnen(i)(2).Trim("(").Trim(",")
            Dim rechts = Lijnen(i)(3).Trim(")")
            Maps(Lijnen(i)(0)).L = Maps(links)
            Maps(Lijnen(i)(0)).R = Maps(rechts)
        Next

       ' Dim HuidigePos = Maps("AAA")
       Dim HuidigePos As New List(Of Kaart)
        HuidigePos.AddRange(Maps.values.ToList.Where(Function(x) x.Naam.Last = "A"))
        Dim endpaths As new List(Of Long)


        Parallel.ForEach(HuidigePos, Sub(K)
                Dim nieuwePos As Kaart = K 
                Dim J As Integer = 0
                Dim som As Long = 0
                While nieuwePos.Naam.Last <> "Z"
                    If j = Instructions.Length Then
                        j = 0
                    End If
                    select Case Instructions(j)
                        Case "R"
                            NieuwePos = Maps(NieuwePos.R.Naam)
                        Case "L"
                            NieuwePos = Maps(NieuwePos.L.Naam)
                    End Select
                    J += 1
                    som += 1
                End While
                endpaths.Add(som)
               
            End Sub)


        for i = 0 To endpaths.Count - 1
             Console.WriteLine(endpaths(i).ToString)
        next

        Dim Paths As Long = 1

        For i = 0 To endpaths.Count - 1
            Paths = LCM(Paths, endpaths(i))
            Console.WriteLine(paths.ToString)
        Next

        Console.WriteLine(Paths)

        'While HuidigePos.Where(Function(x) x.Naam.Last = "Z").Count <> paths
        '    Dim NieuwePos As New Concurrent.ConcurrentBag(Of Kaart)
        '    If j = Instructions.Length Then
        '        j = 0
        '    End If

        '    Parallel.ForEach(HuidigePos, Sub(K)
        '        Select Case Instructions(j)
        '            Case "R"
        '                NieuwePos.Add(Maps(K.r.Naam))
        '            Case "L"
        '                 NieuwePos.Add(Maps(K.l.Naam))
        '        End Select
        '    End Sub)
          
        '    HuidigePos = NieuwePos
        '    j += 1
        '    som += 1
        'End While

        
    End Sub

    Private Function GCD(a As Long, b As Long) As Long
	    If a = 0 Then
		    Return b
	    End If

	    While b <> 0
		    If a > b Then
			    a -= b
		    Else
			    b -= a
		    End If
	    End While

	    Return a
    End Function

    Public Function LCM(a As Long, b As Long) As Long
	    Return (a * b) \ GCD(a, b)
    End Function

End Module

Public Class Kaart
    Public Naam As String
    Public L As Kaart
    Public R As Kaart
End Class
