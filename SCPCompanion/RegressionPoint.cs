using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPCompanion
{
    public class RegressionPoint
    {
        public string CarrierLevel { get; set; }
        public string SuggestedSNCutoff { get; set; }
        public string SNCutoff { get; set; }

        public RegressionPoint(double carrierLevel, double snCutoff, double snAdjustment)
        {
            CarrierLevel = carrierLevel.ToString();
            SNCutoff = snCutoff.ToString();
            SuggestedSNCutoff = (snCutoff * snAdjustment).ToString();
        }
    }
}
