Imports Microsoft.VisualBasic.FileIO

Module Module12
    Public Sub Spring()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Springs.txt"
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

        Dim springs As New List(Of String)
        Dim SpringInfo As New List(Of List(Of Integer))

        For i = 0 To lijnen.Count - 1
            Dim SpringGroups As String = lijnen(i)(0)
            springs.Add(SpringGroups + "?" + SpringGroups + "?" + SpringGroups + "?" + SpringGroups + "?" + SpringGroups)

            Console.Write(springs(i) + " - ")

            Dim Info As New List(Of Integer)
            For Each c In lijnen(i)(1).Split(",")
                Info.Add(c)
            Next

            Info.AddRange(Info)
            Info.AddRange(Info)

            For Each c In lijnen(i)(1).Split(",")
                Info.Add(c)
            Next

            For Each l In Info
                Console.Write(l.ToString + ", ")
            Next

            SpringInfo.Add(Info)
            Console.WriteLine("")

        Next

        Dim totaal As Long = 0

        For i = 0 To springs.Count - 1
            Dim Mogelijkheden = Getmogelijkheiden(springs(i), SpringInfo(i))
            Console.WriteLine(springs(i) + " - " + Mogelijkheden.ToString)
            totaal += Mogelijkheden
        Next
        Console.WriteLine(totaal.ToString)

    End Sub

    Public Function Getmogelijkheiden(str As String, info As List(Of Integer)) As Integer
        Dim Som As Integer = 0

        If str.Contains("?") Then
            Dim index = str.IndexOf("?")
            Som += Getmogelijkheiden(str.Substring(0, index) + "." + str.Substring(index + 1), info)
            Som += Getmogelijkheiden(str.Substring(0, index) + "#" + str.Substring(index + 1), info)
            Return Som
        Else
            Return CheckStringMogelijk(str, info)
        End If
    End Function

    Public Function CheckStringMogelijk(str As String, info As List(Of Integer)) As Integer
        Dim groups As New List(Of String)

        For Each g In str.Trim(".").Split(".")
            If g.Contains("#") Then
                groups.Add(g)
            End If
        Next

        Dim succes As Boolean = True

        If groups.Count = info.Count Then
            For i = 0 To groups.Count - 1
                If groups(i).Length <> info(i) Then
                    succes = False
                End If
            Next
        Else
            succes = False
        End If

        If succes Then
            Console.WriteLine(str + " - OK")
        End If

        If succes Then Return 1 Else Return 0

    End Function

End Module