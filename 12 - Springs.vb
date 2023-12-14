Imports Microsoft.VisualBasic.FileIO
Module Module12
     Public sub Spring()
        Dim pad = "C:\Users\Matthias\Source\Repos\Rodfor\AC2023---Trebuchet\Springs.txt"
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


        Dim Rijen As new List(Of String)
        Dim Info As New List(Of List(Of Integer))

        For each lijn In lijnen
            Rijen.Add(lijn(0))
            Dim RijInfo As new List(Of Integer)

            For each R In lijn(1).split(",")
                RijInfo.Add(R)
            Next
            Info.Add(RijInfo)
        Next



        For i = 0 To Rijen.Count - 1
            Rijen(i) = Rijen(i).Trim(".")
            Dim Groups As List(Of string) = Rijen(i).Split(".").ToList
            Dim GroupsKapot As New List(Of String)
            For each G In Groups
                If G <>  "." AndAlso G <> ""
                    GroupsKapot.Add(G)
                    Console.Write(G + ", ")
                End If   
            Next



            For j = 0 To GroupsKapot.Count
                If Not GroupsKapot(j).Contains("?") Then

                End If
            Next




            Console.WriteLine(" ")
        Next

    End Sub
End Module
