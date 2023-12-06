Imports Microsoft.VisualBasic.FileIO

Module Module5
    Public Enum Mode
        Seeds
        SeedToSoil
        SoilToFertilizer
        FertilizerToWater
        WaterToLight
        LightToTemperature
        TemperatureToHumidity
        HumidityToLocation
    End Enum
    Public Sub Seeds()
        Dim pad = "C:\Users\mle.SERVER\source\repos\AC2023 - Trebuchet\Seeds.txt"
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


        Dim mappingmode As Mode = Mode.Seeds
        Dim soilMap As New List(Of Map)
        Dim fertilizerMap As New List(Of Map)
        Dim waterMap As New List(Of Map)
        Dim lightMap As New List(Of Map)
        Dim temperatureMap As New List(Of Map)
        Dim humidityMap As New List(Of Map)
        Dim locationMap As New List(Of Map)
        Dim zaadRanges As New List(Of Range)

        For Each Lijn In lijnen

            Select Case Lijn(0)
                Case "seeds:"
                    'For Each value In Lijn
                    '    If Not IsNumeric(value) Then Continue For
                    '    Dim seed As New Seed With {
                    '                .Nr = value
                    '            }
                    '    seeds.Add(seed)
                    'Next
                    Dim startvalue As Long
                    Dim i = 0
                    While i < Lijn.Count
                        If IsNumeric(Lijn(i)) Then
                            If i Mod 2 = 1 Then
                                startvalue = Lijn(i)
                            Else
                                Dim zaad As New Range
                                zaad.First = startvalue
                                zaad.Last = startvalue + Lijn(i) - 1
                                zaadRanges.Add(zaad)
                            End If
                        End If
                        i += 1
                    End While
                Case "seed-to-soil"
                    mappingmode = Mode.SeedToSoil
                Case "soil-to-fertilizer"
                    mappingmode = Mode.SoilToFertilizer
                Case "fertilizer-to-water"
                    mappingmode = Mode.FertilizerToWater
                Case "water-to-light"
                    mappingmode = Mode.WaterToLight
                Case "light-to-temperature"
                    mappingmode = Mode.LightToTemperature
                Case "temperature-to-humidity"
                    mappingmode = Mode.TemperatureToHumidity
                Case "humidity-to-location"
                    mappingmode = Mode.HumidityToLocation
                Case "", "map:"
                    Continue For
                Case Else
                    Dim map As New Map With {
                                    .Destination = Lijn(0),
                                    .Source = Lijn(1),
                                    .Range = Lijn(2)
                                }
                    Select Case mappingmode
                        Case Mode.SeedToSoil
                            soilMap.Add(map)
                        Case Mode.SoilToFertilizer
                            fertilizerMap.Add(map)
                        Case Mode.FertilizerToWater
                            waterMap.Add(map)
                        Case Mode.WaterToLight
                            lightMap.Add(map)
                        Case Mode.LightToTemperature
                            temperatureMap.Add(map)
                        Case Mode.TemperatureToHumidity
                            humidityMap.Add(map)
                        Case Mode.HumidityToLocation
                            locationMap.Add(map)
                    End Select
            End Select
        Next

        Dim lowestLocation As Long = Long.MaxValue

        Dim Soil As New List(Of Range)
        Dim Fertilizer As New List(Of Range)
        Dim Water As New List(Of Range)
        Dim Light As New List(Of Range)
        Dim Temperature As New List(Of Range)
        Dim Humidity As New List(Of Range)
        Dim Location As New List(Of Range)

        Soil.AddRange(FindMapRange(zaadRanges, soilMap))
        Console.WriteLine("Soil done")

        Fertilizer.AddRange(FindMapRange(Soil, fertilizerMap))
        Console.WriteLine("Fertilizer done")

        Water.AddRange(FindMapRange(Fertilizer, waterMap))
        Console.WriteLine("Water done")

        Light.AddRange(FindMapRange(Water, lightMap))
        Console.WriteLine("Light done")

        Temperature.AddRange(FindMapRange(Light, temperatureMap))
        Console.WriteLine("Temperature done")

        Humidity.AddRange(FindMapRange(Temperature, humidityMap))
        Console.WriteLine("Humidity done")

        Location.AddRange(FindMapRange(Humidity, locationMap))
        Console.WriteLine("Location done")

        lowestLocation = Location.Aggregate(Function(x, y) If(x.First < y.First, x, y)).First

        'For Each nr In Seeds()
        '    Soil = FindMap(nr, soilMap)
        '    Fertilizer = FindMap(Soil, fertilizerMap)
        '    Water = FindMap(Fertilizer, waterMap)
        '    Light = FindMap(Water, lightMap)
        '    Temperature = FindMap(Light, temperatureMap)
        '    Humidity = FindMap(Temperature, humidityMap)
        '    Location = FindMap(Humidity, locationMap)

        '    If lowestLocation Is Nothing OrElse lowestLocation > Location Then
        '        lowestLocation = Location
        '    End If
        'Next

        Console.WriteLine("Loc :" + lowestLocation.ToString)

    End Sub

    Public Function FindMap(value As Long, Mapping As List(Of Map)) As Long
        Dim map = Mapping.Where(Function(x) x.Source <= value AndAlso x.SourceEnd >= value)
        If map.Count = 1 Then
            Return map.First.GetMappedValue(value)
        ElseIf map.Count = 0 Then
            Return value
        Else
            Console.WriteLine("teveel mapping gevonden " + map.Count.ToString)
        End If
    End Function

    Public Function FindMapRange(Ranges As List(Of Range), Mapping As List(Of Map)) As List(Of Range)
        Dim NewRanges As New List(Of Range)
        For Each range In Ranges
            NewRanges.AddRange(FindMapRangeSingle(range, Mapping))
        Next
        Return NewRanges
    End Function

    Public Function FindMapRangeSingle(BronRange As Range, Mapping As List(Of Map)) As List(Of Range)
        Dim NewRanges As New List(Of Range)
        Dim maps = Mapping.Where(Function(x) x.Source <= BronRange.First AndAlso x.SourceEnd >= BronRange.First)
        If maps.Count = 1 Then
            Dim map = maps.First
            Dim newrange = map.GetMappedRange(BronRange)
            NewRanges.Add(newrange)
            If BronRange.Range > newrange.Range Then
                Dim restRange = New Range
                restRange.First = BronRange.First + newrange.Range
                restRange.Last = restRange.First + (BronRange.Range - newrange.Range - 1)
                NewRanges.AddRange(FindMapRangeSingle(restRange, Mapping))
            End If
        ElseIf maps.Count = 0 Then
            Dim map = FindNextRange(BronRange, Mapping)
            If map Is Nothing Then
                NewRanges.Add(BronRange)
            Else
                Dim newrange As New Range
                newrange.First = BronRange.First
                newrange.Last = map.Source - 1
                NewRanges.Add(newrange)

                Dim restrange As New Range
                restrange.First = map.Source
                restrange.Last = BronRange.Last

                NewRanges.AddRange(FindMapRangeSingle(restrange, Mapping))
            End If
        Else
            Console.WriteLine("teveel mapping gevonden " + maps.Count.ToString)
        End If

        Return NewRanges
    End Function

    Public Function FindNextRange(BronRange As Range, Mapping As List(Of Map)) As Map
        Dim map = Mapping.Aggregate(Function(x, y) If(x.Source - BronRange.First > 0 AndAlso (y.Source - BronRange.First < 0 OrElse x.Source - BronRange.First < Math.Abs(y.Source - BronRange.First)), x, y))
        If BronRange.First > map.Source OrElse BronRange.Last < map.Source Then
            Return Nothing
        Else
            Return map
        End If
    End Function


End Module





Public Class Range
    Public First As Long
    Public Last As Long
    Public ReadOnly Property Range As Long
        Get
            Return Last - First + 1
        End Get
    End Property

End Class

Public Class Map
    Public Source As Long
    Public Destination As Long
    Public Range As Long

    Public ReadOnly Property SourceEnd As Long
        Get
            Return Source + Range - 1
        End Get
    End Property

    Public Function GetMappedValue(Sourcevalue As Long) As Long
        If Sourcevalue < Source OrElse Sourcevalue >= (Source + Range) Then
            Throw New Exception("Sourcevalue not in bounds" + Sourcevalue.ToString + "in " + Source.ToString + " + " + Range.ToString)
        Else
            Dim returnvalue = Destination + Sourcevalue - Source
            'Console.WriteLine(Sourcevalue.ToString + " mapped " + returnvalue.ToString)
            Return returnvalue
        End If
    End Function

    Public Function GetMappedRange(SourceRange As Range) As Range
        Dim newRange As New Range
        newRange.First = GetMappedValue(SourceRange.First)
        newRange.Last = GetMappedValue(Math.Min(SourceRange.Last, SourceEnd))
        Return newRange
    End Function

End Class