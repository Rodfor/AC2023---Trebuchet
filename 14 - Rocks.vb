Imports System.Net.NetworkInformation
Imports System.Reflection
Imports Microsoft.VisualBasic.FileIO

Module Module14
    Public Sub Rock()
        Dim pad = "C:\Users\Matthias\Source\Repos\Rodfor\AC2023---Trebuchet\Rocks.txt"
        Dim Lijnen() as String = System.IO.File.ReadAllLines(pad)
        Dim Pattern(Lijnen.Count - 1, Lijnen.First.Length - 1) as String

        For i = 0 To Lijnen.Count() - 1
            Dim lijn = Lijnen(i)
            For j = 0 To lijn.length - 1
                Pattern(i,j) = lijn(j)
                 Console.Write(Pattern(i,j))
            Next
            Console.WriteLine("")
        Next

        Console.WriteLine("  |    |   ")
       
        'TiltNorth(Pattern)

        Dim PatternDic As New Dictionary(Of integer, List(Of Integer))

        For x = 1 To 1000
       

            Spin(Pattern)
            Dim Load = CalculateNorth(Pattern)

           If PatternDic.ContainsKey(Load) Then
                PatternDic(Load).Add(x)
            Else
                PatternDic.Add(Load, New List(Of Integer))
                PatternDic(Load).Add(x)
           End If

            If x Mod 100000 = 0 Then
                Console.WriteLine("     |SPIN|      " + x.ToString)
                Console.WriteLine(PatternDic.Count)
            End If
           ' For i = 0 To Pattern.GetUpperBound(0)
           '     For j = 0 To Pattern.GetUpperBound(1)
           '         Console.Write(Pattern(i, j))
           '     Next
           '     Console.WriteLine("")
           ' Next
           'Console.WriteLine("  |    |   ")
       
        Next

        For each kvp In PatternDic
            Console.WriteLine(kvp.Key.ToString +" - " + String.Join(",", kvp.Value.ToArray))
        Next


        
    End Sub

    Public Sub Spin(Pattern As String(,))        
        TiltNorth(Pattern)
        
        'Console.WriteLine("  | Move N   |   ")
        'For i = 0 To Pattern.GetUpperBound(0)
        '    For j = 0 To Pattern.GetUpperBound(1)
        '        Console.Write(Pattern(i,j))
        '    Next
        '    Console.WriteLine("")
        'Next

        TiltWest(Pattern)
        
        '        Console.WriteLine("  |  Move W  |   ")
        'For i = 0 To Pattern.GetUpperBound(0)
        '    For j = 0 To Pattern.GetUpperBound(1)
        '        Console.Write(Pattern(i,j))
        '    Next
        '    Console.WriteLine("")
        'Next
        TiltSouth(Pattern)
        
        '        Console.WriteLine("  | Move S   |   ")
        'For i = 0 To Pattern.GetUpperBound(0)
        '    For j = 0 To Pattern.GetUpperBound(1)
        '        Console.Write(Pattern(i,j))
        '    Next
        '    Console.WriteLine("")
        'Next
        '        Console.WriteLine("  | Move E   |   ")
        TiltEast(Pattern)

     
        
        'For i = 0 To Pattern.GetUpperBound(0)
        '    For j = 0 To Pattern.GetUpperBound(1)
        '        Console.Write(Pattern(i,j))
        '    Next
        '    Console.WriteLine("")
        'Next
    End Sub

    Public Sub TiltNorth(Pattern As String(,))

        For i = 1 To Pattern.GetUpperBound(0)
            For j = 0 To Pattern.GetUpperBound(1)
                If Pattern(i,j) = "O" Then
                    MoveUp(Pattern, i, j)
                End If
            Next
        Next
    End Sub

    Public Sub MoveUp(Pattern As String(,), i As Integer, j As Integer)
        If i <> 0
            If Pattern(i-1, j) = "." Then
                Pattern(i,j) = "."
                Pattern(i-1,j) = "O"
                MoveUp(Pattern, i-1, j)
            End If
        End If
    End Sub

     Public Sub TiltSouth(Pattern As String(,))
        Dim i = Pattern.GetUpperBound(0) - 1 

        While i >= 0
            For j = 0 To Pattern.GetUpperBound(1)
                If Pattern(i,j) = "O" Then
                    MoveDown(Pattern, i, j)
                End If
            Next
            i -=1
        End While
    End Sub

    Public Sub MoveDown(Pattern As String(,), i As Integer, j As Integer)
        If i <> Pattern.GetUpperBound(0)
            If Pattern(i+1, j) = "." Then
                Pattern(i,j) = "."
                Pattern(i+1,j) = "O"
                MoveDOwn(Pattern, i+1, j)
            End If
        End If
    End Sub


     Public Sub TiltWest(Pattern As String(,))   

        for j = 1 to Pattern.GetUpperBound(1)
             For i = 0 To Pattern.GetUpperBound(0)
                If Pattern(i,j) = "O" Then
                    MoveLeft(Pattern, i, j)
                End If
            Next
        Next
    End Sub

    Public Sub MoveLeft(Pattern As String(,), i As Integer, j As Integer)
        If j <> 0
            If Pattern(i, j - 1) = "." Then
                Pattern(i,j) = "."
                Pattern(i,j - 1) = "O"
                MoveLeft(Pattern, i, j - 1)
            End If
        End If
    End Sub

    Public Sub TiltEast(Pattern As String(,))   
        Dim j = Pattern.GetUpperBound(1) - 1

        While j >= 0
             For i = 0 To Pattern.GetUpperBound(0)
                If Pattern(i,j) = "O" Then
                    MoveRight(Pattern, i, j)
                End If
            Next
            j -= 1
        End While
    End Sub

    Public Sub MoveRight(Pattern As String(,), i As Integer, j As Integer)
        If j <> Pattern.GetUpperBound(1)
            If Pattern(i, j + 1) = "." Then
                Pattern(i,j) = "."
                Pattern(i,j + 1) = "O"
                MoveRight(Pattern, i, j + 1)
            End If
        End If
    End Sub



      Public Function CalculateNorth(Pattern As String(,)) As Integer
        Dim som = 0

        For i = 0 To Pattern.GetUpperBound(0)
            For j = 0 To Pattern.GetUpperBound(1)
                If Pattern(i,j) = "O" Then
                    som +=  Pattern.GetUpperBound(0) - i + 1
                End If
            Next
        Next

        Return som
    End Function
End Module

Public Class Holder
    Public Pattern As String(,)
    Public Spins As new List(Of Integer)
End Class