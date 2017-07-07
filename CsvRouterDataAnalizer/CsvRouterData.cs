using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvRouterDataAnalizer
{
    struct CsvRouterData : IEquatable<CsvRouterData>, IComparable<CsvRouterData>
    {
        public string Mac { get; set; }
        public DateTime DateStart { get; set; }

        public CsvRouterData(string mac, DateTime dt)
        {
            Mac = mac;
            DateStart = dt;
        }

        public bool Equals(CsvRouterData other)
        {
            return this.DateStart.Equals(other.DateStart);
        }

        public int CompareTo(CsvRouterData other)
        {
            return this.DateStart.CompareTo(other.DateStart);
        }

        public override string ToString()
        {
            return $"\"{Mac}\" | {DateStart}";
        }
    }
}
