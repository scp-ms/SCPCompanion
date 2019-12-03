using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPCompanion
{
    public class MSnScan
    {
        public int Order { get; set; }

        public MSnScan ParentScan { get; set; }
        public MSnScan ChildScan { get; set; }

        public int ScanNumber { get; set; }
        public double RetentionTime { get; set; }
        public double InjectionTime { get; set; }
        public double SumSN { get; set; }
        public double IonFlux { get; set; }

        public Dictionary<string, double> SNDict { get; set; }

        public int ParentScanNumber { get; set; }
        public int ChildScanNumber { get; set; }

        public double ParentInjectionTime { get; set; }

        public MSnScan()
        {
            SNDict = new Dictionary<string, double>();
        }
    }
}
