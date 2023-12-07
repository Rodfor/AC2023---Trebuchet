Imports Microsoft.VisualBasic.FileIO

Module Module7
    Public Sub Camel()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Camel.txt"
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

        Dim Hands As New List(Of Hand)

        For Each L In lijnen
            Dim Hand As New Hand
            For Each c As Char In L(0)
                Hand.Cards.Add(c)
            Next
            Hand.Bid = L(1)
            Hand.CalcStrength()
            Hands.Add(Hand)

        Next

        Dim rankedHands = Hands.OrderBy(Function(x) x.Strength)

        Dim som As Long = 0

        For i = 0 To rankedHands.Count - 1
            som += (rankedHands(i).Bid * (i + 1))
            Console.WriteLine(String.Join("", rankedHands(i).Cards.ToArray) + " " + rankedHands(i).Type.ToString + " " + rankedHands(i).Strength.ToString + " " + rankedHands(i).HighestKey)
        Next

        Console.WriteLine(som)

    End Sub

End Module

Public Class Hand
    Public Enum HandType
        HighCard = 0
        OnePair = 1
        TwoPair = 2
        ThreeOfAKind = 3
        FullHouse = 4
        FourOfAKind = 5
        FiveOfAKind = 6
    End Enum

    Public Cards As New List(Of Char)
    Public Bid As Integer
    Public Strength As Long
    Public Type As HandType
    Public HighestKey As Char

    Public Sub CalcStrength()
        Dim StrengthString = ""
        Dim CardDictionary As New Dictionary(Of Char, Integer)
        HighestKey = Cards(0)

        For i = 0 To Cards.Count - 1
            If CardDictionary.ContainsKey(Cards(i)) Then
                CardDictionary(Cards(i)) += 1
            Else
                CardDictionary.Add(Cards(i), 1)
            End If

            If Cards(i) <> "J" AndAlso (HighestKey = "J" OrElse (CardDictionary(Cards(i)) > CardDictionary(HighestKey))) Then HighestKey = Cards(i)

            Select Case Cards(i)
                Case "T"
                    StrengthString += "10"
                Case "J"
                    'StrengthString += "11"
                    StrengthString += "01"
                Case "Q"
                    StrengthString += "12"
                Case "K"
                    StrengthString += "13"
                Case "A"
                    StrengthString += "14"
                Case Else
                    StrengthString += "0"
                    StrengthString += Cards(i)
            End Select
        Next

        If CardDictionary.ContainsKey("J") AndAlso CardDictionary.Count > 1 Then
            If HighestKey = "J" Then
                CardDictionary(Cards(1)) += 1
            Else
                CardDictionary(HighestKey) += CardDictionary("J")
            End If
            CardDictionary.Remove("J")
        End If

        Select Case CardDictionary.Count
            Case 1
                Type = HandType.FiveOfAKind
            Case 2
                If CardDictionary.Values.First = 1 OrElse CardDictionary.Values.First = 4 Then
                    Type = HandType.FourOfAKind
                Else
                    Type = HandType.FullHouse
                End If
            Case 3
                For Each c In CardDictionary.Values
                    If c = 3 Then
                        Type = HandType.ThreeOfAKind
                        Exit Select
                    End If
                Next
                Type = HandType.TwoPair
            Case 4
                Type = HandType.OnePair
            Case Else
                Type = HandType.HighCard
        End Select

        Dim Typestr As String = Type

        StrengthString = Typestr + StrengthString
        Strength = StrengthString

    End Sub


End Class

