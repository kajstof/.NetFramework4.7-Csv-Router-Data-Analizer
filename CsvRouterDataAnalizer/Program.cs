using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvRouterDataAnalizer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\MWM_Visib_Analytics_20170221193733.csv"))
            {
                List<CsvRouterData> csvDatabase = new List<CsvRouterData>();

                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                int i = 0;

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    csvDatabase.Add(new CsvRouterData(fields[0], Convert.ToDateTime(fields[6])));
                    i++;
                }

                Console.WriteLine($"Razem elementów przed sortowaniem: {i}");
                csvDatabase.Sort();
                Console.WriteLine($"Razem elementów: {csvDatabase.Count}");
                var uniqueMacAddresses = csvDatabase.GroupBy(k => k.Mac).ToList();
                Console.WriteLine($"Unikalnych adresów mac: {uniqueMacAddresses.Count()}");

                var macAddressesDaysConnectedInOneMonth = csvDatabase.GroupBy(k => new { k.Mac, k.DateStart.Date })
                    .Select(group => new
                    {
                        Mac = group.Key.Mac,
                        Count = group.Count()
                    })
                    .GroupBy(k => k.Mac)
                    .Select(group => new
                    {
                        Mac = group.Key,
                        Count = group.Count()
                    })
                    .GroupBy(k => k.Count)
                    .Select(group => new
                    {
                        ConnectedTimes = group.Key,
                        Count = group.Count()
                    })
                    .OrderBy(k => k.ConnectedTimes)
                    .ToList();
                foreach (var x in macAddressesDaysConnectedInOneMonth)
                {
                    Console.WriteLine($"{x.ConnectedTimes}\t{x.Count}");
                }
                Console.WriteLine($"Ile unikalnych dni połączeń: {macAddressesDaysConnectedInOneMonth.Count()}");

                var macs = csvDatabase.GroupBy(k => new { k.Mac, k.DateStart.Date })
                    .Select(group => new
                    {
                        Mac = group.Key.Mac,
                        Count = group.Count(),
                        Day = group.Key.Date.Date,
                        DifferenceTime = (group.Last().DateStart - group.First().DateStart).TotalMinutes,
                        DifferenceGrp = (group.Last().DateStart - group.First().DateStart).TotalMinutes < 60 ? "1. -in <= czas < 060" :
                            (group.Last().DateStart - group.First().DateStart).TotalMinutes < 120 ? "2. 060 <= czas < 120" :
                            (group.Last().DateStart - group.First().DateStart).TotalMinutes < 180 ? "3. 120 <= czas < 180" :
                            (group.Last().DateStart - group.First().DateStart).TotalMinutes < 240 ? "4. 180 <= czas < 240" :
                            (group.Last().DateStart - group.First().DateStart).TotalMinutes < 300 ? "5. 240 <= czas < 300" :
                            (group.Last().DateStart - group.First().DateStart).TotalMinutes < 360 ? "6. 300 <= czas < 360" : "7. 360 <= czas < inf"
                    })
                    .GroupBy(k => k.DifferenceGrp)
                    .Select(group => new
                    {
                        Group = group.Key,
                        Count = group.Count()
                    })
                    .OrderBy(k => k.Group)
                    .ToList();

                foreach (var x in macs)
                {
                    Console.WriteLine($"{x.Group}\t{x.Count}");
                }
                var tmp = csvDatabase.GroupBy(k => new { k.Mac, k.DateStart.Date })
                    .Select(group => new
                    {
                        Mac = group.Key.Mac,
                        Count = group.Count(),
                        Day = group.Key.Date.Date,
                        DifferenceTime = (group.Last().DateStart - group.First().DateStart).TotalMinutes,
                        DifferenceGrp = (group.Last().DateStart - group.First().DateStart).TotalMinutes < 60 ? "1. -in <= czas < 060" :
                            (group.Last().DateStart - group.First().DateStart).TotalMinutes < 120 ? "2. 060 <= czas < 120" :
                            (group.Last().DateStart - group.First().DateStart).TotalMinutes < 180 ? "3. 120 <= czas < 180" :
                            (group.Last().DateStart - group.First().DateStart).TotalMinutes < 240 ? "4. 180 <= czas < 240" :
                            (group.Last().DateStart - group.First().DateStart).TotalMinutes < 300 ? "5. 240 <= czas < 300" :
                            (group.Last().DateStart - group.First().DateStart).TotalMinutes < 360 ? "6. 300 <= czas < 360" : "7. 360 <= czas < inf"
                    })
                    .Where(k => k.DifferenceTime > 360);
                Console.WriteLine($"{tmp.First()}");

                Console.ReadKey();
            }
        }
    }
}
