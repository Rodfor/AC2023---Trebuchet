Imports Microsoft.VisualBasic.FileIO
Module Module9
     Public sub Oasis()
        Dim pad = "C:\Users\Matthias\Source\Repos\Rodfor\AC2023---Trebuchet\Oasis.txt"
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

       Dim Extrapolation As new List(Of Integer)
        Dim som As Long = 0

        For each Lijn In lijnen
            Dim i = 0
            Dim Sequence As new List(Of Integer)
            Dim Sequences As New List(Of List(Of Integer))
            While i < Lijn.Count
               Sequence.Add(Lijn(i))
                i += 1
            End While

            Sequences.Add(Sequence)

            Do While Sequences.Last.Where(Function(x) x = 0).Count <> Sequences.Last.Count
                Dim Differences As New List(Of Integer)
                For j = 0 To Sequences.Last.Count - 2
                    Dim diff = Sequences.Last.Item(j + 1) - Sequences.Last.Item(j)
                    Differences.Add(diff)
                Next
                Sequences.Add(Differences)
            Loop              
            

            Sequences.Last.Add(0)

            Dim LaatsteSequence = New List(Of Integer)
            LaatsteSequence.Add(0)
            LaatsteSequence.AddRange(Sequences.Last)
            Sequences.Remove(Sequences.Last)
            Sequences.Add(LaatsteSequence)

           Dim k = Sequences.Count - 2 
                 
           While k >= 0
                Dim newNumber = Sequences(k).First - Sequences(k + 1).First
                Dim newSequence = New List(Of Integer)
                newSequence.Add(newNumber)
                newSequence.AddRange(Sequences(k))
                Sequences(k) = newSequence
                Console.WriteLine(newNumber.ToString + " Added")
                k -= 1
           End While

            Extrapolation.Add(Sequences(0).First)
            som += Sequences(0).First
        Next

        Console.WriteLine(som.ToString)
    End Sub
End Module
