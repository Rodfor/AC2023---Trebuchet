Imports Microsoft.VisualBasic.FileIO


Module Module6
    Public Sub Time()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Time.txt"
        Dim lijnen As New List(Of String())

        Using parser = New TextFieldParser(pad)
            'parser.TextFieldType = FieldType.FixedWidth
            'parser.SetFieldWidths(6, 6, 6, 6)
            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters("  ")

            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                lijnen.Add(fields)
            End While

            parser.Close()
            parser.Dispose()
        End Using

        Dim results = New List(Of Integer)

        Dim i = 0
        While i < lijnen(0).Count
            Dim winnaars As Integer = 0
            Dim tijdslimiet = lijnen(0)(i)
            Dim record = lijnen(1)(i)

            For j = 1 To tijdslimiet - 1
                Dim afstand = j * (tijdslimiet - j)
                If afstand > record Then
                    winnaars += 1
                End If
            Next
            results.Add(winnaars)
            Console.WriteLine(winnaars)
            i += 1
        End While

        ' Dim totaal = 1

        'For Each result In results
        '    totaal *= result
        'Next

        'Console.WriteLine(totaal)

    End Sub
End Module
