using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using System.IO;

using CSMSL;
using CSMSL.Analysis;
using CSMSL.Chemistry;
using CSMSL.IO;
using CSMSL.Proteomics;
using CSMSL.Spectral;
using CSMSL.Util;


namespace SCPCompanion
{
    public class AnalyzedRun
    {
        public int QuantOrder { get; set; }
        public string RawFileName { get; set; }
        public List<double> MS2ITs { get; set; }
        public List<double> MS3ITs { get; set; }
        public List<double> MS2IonFluxs { get; set; }
        public List<double> MS2SumSNs { get; set; }
        public List<double> MS3IonFluxs { get; set; }
        public List<double> MS3SumSNs { get; set; }

        public double MedianMS2IT { get; set; }
        public double MedianMS3IT { get; set; }
        public double MedianMS2IonFlux { get; set; }
        public double MedianMS2SN { get; set; }
        public double MedianMS3IonFlux { get; set; }
        public double MedianMS3SN { get; set; }

        public int SuggestedSNCutoff { get; set; }
        public int SuggestedMaxIT { get; set; }

        private List<MSnScan> MSnScans { get; set; }

        public AnalyzedRun(int quantOrder, string rawfileName, List<MSnScan> msnScans)
        {
            QuantOrder = quantOrder;

            //RawFile Name
            RawFileName = Path.GetFileNameWithoutExtension(rawfileName);

            //Initialize the lists
            MS2ITs = new List<double>();
            MS3ITs = new List<double>();
            MS2IonFluxs = new List<double>();
            MS2SumSNs = new List<double>();
            MS3IonFluxs = new List<double>();
            MS3SumSNs = new List<double>();

            MSnScans = new List<MSnScan>();

            //Initialize Variables
            MedianMS2IT = -1;
            MedianMS3IT = -1;
            MedianMS2IonFlux = -1;
            MedianMS2SN = -1;
            MedianMS3IonFlux = -1;
            MedianMS3SN = -1;
            SuggestedSNCutoff = 120;
            SuggestedMaxIT = 10;

            //Cycle through each of the scans in order to extract the data
            foreach (MSnScan scan in msnScans)
            {
                int msOrder = scan.Order;

                //Extract the injection time
                double injectionTime = scan.InjectionTime;
                if (msOrder == 2)
                {
                    MS2ITs.Add(injectionTime);
                    MS2SumSNs.Add(scan.SumSN);
                    double ionFlux = scan.SumSN / injectionTime;
                    MS2IonFluxs.Add(ionFlux);
                }
                else if (msOrder == 3)
                {
                    MS3ITs.Add(injectionTime);
                    MS3SumSNs.Add(scan.SumSN);
                    double ionFlux = scan.SumSN / injectionTime;
                    MS3IonFluxs.Add(ionFlux);
                }

                //Add the scan to the list of all the scans
                MSnScans.Add(scan);
            }


            //Calculate the medians
            MedianMS2IT = Math.Round(Median(MS2ITs), 2);
            MedianMS3IT = Math.Round(Median(MS3ITs), 2);
            MedianMS2IonFlux = Math.Round(Median(MS2IonFluxs), 2);
            MedianMS2SN = Math.Round(Median(MS2SumSNs), 2);
            MedianMS3IonFlux = Math.Round(Median(MS3IonFluxs), 2);
            MedianMS3SN = Math.Round(Median(MS3SumSNs), 2);
        }

        public void CalculateSuggestedValues(double carrierProteomeLevel, double slope, double yIntercept, double maxITAdjustment)
        {
            double ionFlux = -1;
            if(QuantOrder == 2)
            {
                ionFlux = MedianMS2IonFlux;
            }
            else if(QuantOrder == 3)
            {
                ionFlux = MedianMS3IonFlux;
            }

            double suggestedSNCutoff = (carrierProteomeLevel * slope) + yIntercept;
            double suggestedMaxIT = (suggestedSNCutoff / ionFlux) * maxITAdjustment;

            int divide = 10;
            while(suggestedSNCutoff/divide > 10)
            {
                SuggestedSNCutoff = (int) Math.Ceiling(suggestedSNCutoff / divide) * divide;

                divide *= 10;
            }

            if(suggestedMaxIT > 1000)
            {
                divide = 10;
                while (suggestedMaxIT / divide > 10)
                {
                    SuggestedMaxIT = (int)Math.Ceiling(suggestedMaxIT / divide) * divide;

                    divide *= 10;
                }
            }
            else
            {
                divide = 10;
                while (suggestedMaxIT / divide > 1)
                {
                    SuggestedMaxIT = (int)Math.Ceiling(suggestedMaxIT / divide) * divide;

                    divide *= 10;
                }
            }
        }

        private double Median(List<double> numberList)
        {
            if(numberList.Count == 0)
            {
                return -1;
            }

            double[] numbers = numberList.ToArray();

            int numberCount = numbers.Count();
            int halfIndex = numbers.Count() / 2;
            var sortedNumbers = numbers.OrderBy(n => n);
            double median;
            if ((numberCount % 2) == 0)
            {
                median = ((sortedNumbers.ElementAt(halfIndex) + sortedNumbers.ElementAt(halfIndex - 1)) / 2);
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }

            return median;
        }

        public string PrintSummary()
        {
            string retString = "";

            if (QuantOrder == 2)
            {
                retString = PrintMS2Data();
            }
            else if (QuantOrder == 3)
            {
                retString = PrintMS3Data();
            }

            return retString;
        }

        public string PrintMS2Data()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(RawFileName);
            sb.Append(',');
            sb.Append(MedianMS2IT);
            sb.Append(',');
            sb.Append(MedianMS2SN);
            sb.Append(',');
            sb.Append(MedianMS2IonFlux);
            sb.Append(',');
            sb.Append(SuggestedSNCutoff);
            sb.Append(',');
            sb.Append(SuggestedMaxIT);

            return sb.ToString();
        }

        public string PrintMS3Data()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(RawFileName);
            sb.Append(',');
            sb.Append(MedianMS2IT);
            sb.Append(',');
            sb.Append(MedianMS3IT);
            sb.Append(',');
            sb.Append(MedianMS3SN);
            sb.Append(',');
            sb.Append(MedianMS3IonFlux);
            sb.Append(',');
            sb.Append(SuggestedSNCutoff);
            sb.Append(',');
            sb.Append(SuggestedMaxIT);

            return sb.ToString();
        }

        public void PrintQuantScans(string Path, List<Modification> tmtIons)
        {
            using (StreamWriter writer = new StreamWriter(Path + "\\SCPCompanionAnalysis_MS" + QuantOrder + "_" + RawFileName + ".csv"))
            {
                StringBuilder headerLine = new StringBuilder();

                if(QuantOrder == 2)
                {
                    headerLine.Append("RawFileName,RetentionTime,MS2 Scan Number,MS2 Injection Time,Sum SN,Ion Flux");
                }
                else if (QuantOrder == 3)
                {
                    headerLine.Append("RawFileName,RetentionTime,MS2 Scan Number,MS3 Scan Number,MS2 Inj Time,MS3 Inj Time,Sum SN,Ion Flux");
                }

                foreach (Modification tmt in tmtIons)
                {
                    headerLine.Append(',');
                    headerLine.Append(tmt.Name);              
                }

                writer.WriteLine(headerLine.ToString());

                List<string> printList = new List<string>();
                if(QuantOrder == 2) { printList = PrintMS2Scans(); } else if (QuantOrder == 3) { printList = PrintMS3Scans(); }

                foreach(string printLine in printList)
                {
                    writer.WriteLine(printLine);
                }
            }
        }

        public List<string> PrintMS2Scans()
        {
            List<string> scanStrings = new List<string>();
            foreach (MSnScan scan in MSnScans)
            {
                StringBuilder sb2 = new StringBuilder();
                sb2.Append(RawFileName);
                sb2.Append(',');
                sb2.Append(scan.RetentionTime);
                sb2.Append(',');
                sb2.Append(scan.ScanNumber);
                sb2.Append(',');
                sb2.Append(scan.InjectionTime);
                sb2.Append(',');
                sb2.Append(scan.SumSN);
                sb2.Append(',');
                sb2.Append(scan.IonFlux);

                foreach(KeyValuePair<string, double> kvp in scan.SNDict)
                {
                    sb2.Append(',');
                    sb2.Append(kvp.Value);
                }

                scanStrings.Add(sb2.ToString());
            }

            return (scanStrings);
        }

        public List<string> PrintMS3Scans()
        {
            List<string> scanStrings = new List<string>();
            foreach (MSnScan scan in MSnScans)
            {
                StringBuilder sb2 = new StringBuilder();
                sb2.Append(RawFileName);
                sb2.Append(',');
                sb2.Append(scan.RetentionTime);
                sb2.Append(',');
                sb2.Append(scan.ParentScanNumber);
                sb2.Append(',');
                sb2.Append(scan.ScanNumber);
                sb2.Append(',');
                sb2.Append(scan.ParentInjectionTime);
                sb2.Append(',');
                sb2.Append(scan.InjectionTime);
                sb2.Append(',');
                sb2.Append(scan.SumSN);
                sb2.Append(',');
                sb2.Append(scan.IonFlux);

                foreach (KeyValuePair<string, double> kvp in scan.SNDict)
                {
                    sb2.Append(',');
                    sb2.Append(kvp.Value);
                }

                scanStrings.Add(sb2.ToString());
            }

            return (scanStrings);
        }
    }
}
