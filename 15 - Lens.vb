Imports System.Net.NetworkInformation
Imports System.Reflection
Imports Microsoft.VisualBasic.FileIO

Module Module15
    Public Sub Lens()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Lens.txt"
        Dim lijn As List(Of String)

        Using parser = New TextFieldParser(pad)
            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters(",")

            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                lijn = fields.ToList()
            End While

            parser.Close()
            parser.Dispose()
        End Using


        Dim boxes As New Dictionary(Of Integer, System.Collections.Specialized.OrderedDictionary)

        For Each H In lijn
            Dim Insert As Boolean
            Dim splitString As String()
            Dim lens As Integer

            If H.Contains("-") Then
                splitString = H.Split("-")
                Insert = False
            Else
                splitString = H.Split("=")
                lens = splitString.Last
                Insert = True
            End If

            Dim label As String = splitString.First
            Dim box = FindHashValue(label)

            If Insert Then
                If Not boxes.ContainsKey(box) Then boxes.Add(box, New System.Collections.Specialized.OrderedDictionary())
                If Not boxes(box).Contains(label) Then
                    boxes(box).Add(label, lens)
                Else
                    boxes(box)(label) = lens
                End If
            Else
                If boxes.ContainsKey(box) AndAlso boxes(box).Contains(label) Then
                    boxes(box).Remove(label)
                End If
            End If

            'Console.WriteLine(H + ":")
            'For Each B In boxes
            '    Console.Write($"Box {B.Key}: ")
            '    For Each L In B.Value
            '        Console.Write($" [{L.Key} {L.Value}]")
            '    Next
            '    Console.WriteLine()
            'Next

            'Console.WriteLine()
        Next

        Dim totalPower As Long = 0

        For Each B In boxes
            Dim slot As Integer = 0
            For Each L In B.Value
                slot += 1
                Dim power As Integer = B.Key + 1
                power *= slot
                power *= L.Value

                Console.WriteLine(L.Key + " : " + power.ToString)
                totalPower += power
            Next
        Next

        Console.WriteLine(totalPower)


    End Sub

    Public Function FindHashValue(input As String) As Integer
        Dim Currentvalue As Long = 0

        For Each c As Char In input
            Currentvalue += Asc(c)
            Currentvalue *= 17
            Currentvalue = Currentvalue Mod 256
        Next

        'Console.WriteLine(Currentvalue.ToString + " - " + input)

        Return Currentvalue
    End Function

End Module