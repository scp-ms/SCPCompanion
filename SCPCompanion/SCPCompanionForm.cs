using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ZedGraph;
using MathNet.Numerics;

using ThermoFisher.CommonCore.Data.Business;
using ThermoFisher.CommonCore.Data.FilterEnums;
using ThermoFisher.CommonCore.Data.Interfaces;
using ThermoFisher.CommonCore.RawFileReader;

using CSMSL;
using CSMSL.Analysis;
using CSMSL.Chemistry;
using CSMSL.IO;
using CSMSL.Proteomics;
using CSMSL.Spectral;
using CSMSL.Util;

using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace SCPCompanion
{
    public partial class SCPCompanionForm : Form
    {
        BindingList<string> InputFiles;
        BindingList<AnalyzedRun> AnalyzedRuns;
        BindingList<RegressionPoint> RegressionList;
        GraphPane regressionPane;
        GraphPane snPane;
        GraphPane ionFluxPane;
        bool RunGridInitialized;
        string Path;
        string ParamPath;
        int QuantOrder;
        List<Modification> TMTIons;

        public SCPCompanionForm()
        {
            Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SCPCompanionFiles");
            if (!System.IO.Directory.Exists(Path)) { System.IO.Directory.CreateDirectory(Path); }

            ParamPath = Path + "\\params";
            if (!System.IO.Directory.Exists(ParamPath)) { System.IO.Directory.CreateDirectory(ParamPath); }

            InitializeComponent();
            InitializeRegressionPlot();
            InitializeSNPlot();
            InitializeIonFluxPlot();

            regressionDGV.AutoGenerateColumns = false;
            RegressionList = new BindingList<RegressionPoint>();
            regressionDGV.DataSource = RegressionList;
            InitializeRegressionList();
            PlotRegression();

            RunGridInitialized = false;
            InputFiles = new BindingList<string>();
            inputFileBox.DataSource = InputFiles;

            AnalyzedRuns = new BindingList<AnalyzedRun>();         
        }

        private void ms2Analysis_Click(object sender, EventArgs e)
        {
            string quantOrderString = quantComboBox.Text;
            int quantOrder = -1;
            if(quantOrderString == "MS2")
            {
                quantOrder = 2;

                if (!RunGridInitialized) { InitializeMS2RunGrid(); }
            }
            if (quantOrderString == "MS3")
            {
                quantOrder = 3;

                if (!RunGridInitialized) { InitializeMS3RunGrid(); }
            }

            QuantOrder = quantOrder;

            runAnalysis(quantOrder);  
        }

        private void clearFiles_Click(object sender, EventArgs e)
        {
            InputFiles.Clear();
        }

        private void updateAndSave_Click(object sender, EventArgs e)
        {
            List<RegressionPoint> newPoints = new List<RegressionPoint>();
            foreach(DataGridViewRow row in regressionDGV.Rows)
            {
                double carrierLevel = double.Parse(row.Cells[0].Value.ToString());
                double snValue = double.Parse(row.Cells[1].Value.ToString());

                newPoints.Add(new RegressionPoint(carrierLevel, snValue, double.Parse(snAdjTB.Text)));
            }

            RegressionList.Clear();
            foreach(RegressionPoint point in newPoints)
            {
                RegressionList.Add(point);
            }

            PlotRegression();

            //When we update we will want to write out a new summary file
            using (StreamWriter writer = new StreamWriter(Path + "\\SCPCompanionSummary_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv"))
            {
                if (QuantOrder == 2) { writer.WriteLine("RawFileName,Median MS2 IT,Median Sum SN,Median Ion Flux,Suggested SN Cutoff,Suggested Max IT,Carrier Proteome Level,SN Adjustment,Max IT Adjustment,Slope,yIntercept"); }
                else if (QuantOrder == 3) { writer.WriteLine("RawFileName,Median MS2 IT,Median MS3 IT,Median Sum SN,Median Ion Flux,Suggested SN Cutoff,Suggested Max IT,Carrier Proteome Level,SN Adjustment,Max IT Adjustment,Slope,yIntercept"); }

                foreach (AnalyzedRun analyzedRun in AnalyzedRuns)
                {
                    //Re-calculate the settings based on the new values
                    analyzedRun.CalculateSuggestedValues(double.Parse(carrierRatioTB.Text), double.Parse(slopeTB.Text), double.Parse(yInterTB.Text), double.Parse(maxITAdjTB.Text));                  

                    //Write the results to a file
                    writer.WriteLine(analyzedRun.PrintSummary() + "," + carrierRatioTB.Text + "," + snAdjTB.Text + "," + maxITAdjTB.Text + "," + slopeTB.Text + "," + yInterTB.Text);
                }
            }

            tmtFileGrid.Refresh();
        }

        private void runAnalysis(int quantOrder)
        {
            //Initialize which TMT ions will be looked at in this analysis
            List<Modification> tmtIons = InitializeTMT();

            Dictionary<string, string> RawFiles = new Dictionary<string, string>();
            Dictionary<string, string> ResultFiles = new Dictionary<string, string>();

            //For each of the input files
            foreach (string filename in InputFiles)
            {
                if (filename.Contains(".raw"))
                {
                    RawFiles.Add(filename, filename);
                }
                if (filename.Contains(".csv"))
                {
                    ResultFiles.Add(filename, filename);
                }
            }

            List<AnalyzedRun> analyzedRuns = new List<AnalyzedRun>();
            foreach (string rawFileName in RawFiles.Keys)
            {
                AnalyzedRun analyzedRun = AnalyzeRawFile(quantOrder, rawFileName, tmtIons);
                analyzedRuns.Add(analyzedRun);
            }

            //Add the lines that will be added to the grid
            //
            using (StreamWriter writer = new StreamWriter(Path + "\\SCPCompanionSummary_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv"))
            { 
                if(quantOrder == 2) { writer.WriteLine("RawFileName,Median MS2 IT,Median Sum SN,Median Ion Flux,Suggested SN Cutoff,Suggested Max IT,Carrier Proteome Level,SN Adjustment,Max IT Adjustment,Slope,yIntercept"); }
                else if (quantOrder == 3) { writer.WriteLine("RawFileName,Median MS2 IT,Median MS3 IT,Median Sum SN,Median Ion Flux,Suggested SN Cutoff,Suggested Max IT,Carrier Proteome Level,SN Adjustment,Max IT Adjustment,Slope,yIntercept"); }

                foreach (AnalyzedRun run in analyzedRuns)
                {
                    //Print the quant results in separate files
                    run.PrintQuantScans(Path, tmtIons);

                    //Write the summary for the analyzed run
                    writer.WriteLine(run.PrintSummary() + "," + carrierRatioTB.Text + "," + snAdjTB.Text + "," + maxITAdjTB.Text + "," + slopeTB.Text + "," + yInterTB.Text);

                    //Add the analyzed run to the list
                    AnalyzedRuns.Add(run);        
                }
            }

            //If we analyzed a run then update the TMT plots
            if(AnalyzedRuns.Count > 0)
            {
                UpdateTMTPlots(quantOrder, AnalyzedRuns[0]);
            }
        }

        private void loadParamButton_Click(object sender, EventArgs e)
        {
            LoadParams();

            PlotRegression();

            //When we update we will want to write out a new summary file
            using (StreamWriter writer = new StreamWriter(Path + "\\SCPCompanionSummary_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv"))
            {
                if (QuantOrder == 2) { writer.WriteLine("RawFileName,Median MS2 IT,Median Sum SN,Median Ion Flux,Suggested SN Cutoff,Suggested Max IT,Carrier Proteome Level,SN Adjustment,Max IT Adjustment,Slope,yIntercept"); }
                else if (QuantOrder == 3) { writer.WriteLine("RawFileName,Median MS2 IT,Median MS3 IT,Median Sum SN,Median Ion Flux,Suggested SN Cutoff,Suggested Max IT,Carrier Proteome Level,SN Adjustment,Max IT Adjustment,Slope,yIntercept"); }

                foreach (AnalyzedRun analyzedRun in AnalyzedRuns)
                {
                    //Re-calculate the settings based on the new values
                    analyzedRun.CalculateSuggestedValues(double.Parse(carrierRatioTB.Text), double.Parse(slopeTB.Text), double.Parse(yInterTB.Text), double.Parse(maxITAdjTB.Text));

                    //Write the results to a file
                    writer.WriteLine(analyzedRun.PrintSummary() + "," + carrierRatioTB.Text + "," + snAdjTB.Text + "," + maxITAdjTB.Text + "," + slopeTB.Text + "," + yInterTB.Text);
                }
            }

            tmtFileGrid.Refresh();
        }

        private AnalyzedRun AnalyzeRawFile(int quantOrder, string rawFileName, List<Modification> tmtIons)
        {
            AnalyzedRun analyzedRun = null;

            //Make sure you can open the raw file
            IRawDataPlus rawFile = RawFileReaderAdapter.FileFactory(rawFileName);
            if (!rawFile.IsOpen || rawFile.IsError)
            {
                Console.WriteLine(" Error: unable to access the RAW file using the RawFileReader class.");
                return analyzedRun;
            }

            //Select the instrument on the raw file
            rawFile.SelectInstrument(Device.MS, 1);

            //Cycle through the scans to extract the data
            int firstSpectrumNumber = rawFile.RunHeaderEx.FirstSpectrum;
            int lastSpectrumNumber = rawFile.RunHeaderEx.LastSpectrum;

            //Map the scans
            Dictionary<int, MSnScan> mappedScans = new Dictionary<int, MSnScan>();
            for (int i = firstSpectrumNumber; i <= lastSpectrumNumber; i++)
            {
                Console.WriteLine("Spectrum " + i);

                //The trailer data is stored in two different arrays...which is annoying. So here they are combined
                IScanFilter filter = rawFile.GetFilterForScanNumber(i);
                var trailerData = rawFile.GetTrailerExtraInformation(i);
                Dictionary<string, string> trailerDict = new Dictionary<string, string>();
                int l = 0;
                foreach (string label in trailerData.Labels)
                {
                    trailerDict.Add(trailerData.Labels.ElementAt(l), trailerData.Values.ElementAt(l));
                    l++;
                }

                if (filter.MSOrder == MSOrderType.Ms2 || filter.MSOrder == MSOrderType.Ms3)
                {
                    MSnScan addScan = new MSnScan();

                    //Do everything you would for both types
                    if (filter.MSOrder == MSOrderType.Ms2) { addScan.Order = 2; } else { addScan.Order = 3; }
                    addScan.ScanNumber = i;
                    addScan.InjectionTime = double.Parse(trailerDict["Ion Injection Time (ms):"]); //TODO: May not work with QE
                    addScan.RetentionTime = rawFile.RetentionTimeFromScanNumber(i);

                    addScan.ParentScanNumber = -1;
                    string outStr = "";
                    if (trailerDict.TryGetValue("Master Scan Number:", out outStr))
                    {
                        addScan.ParentScanNumber = int.Parse(trailerDict["Master Scan Number:"]);
                    }

                    //If it is an MS3 we will need to find the MS2 and then edit the child parameters
                    MSnScan parentScan = null;
                    if(mappedScans.TryGetValue(addScan.ParentScanNumber, out parentScan))
                    {
                        parentScan.ChildScanNumber = addScan.ScanNumber;
                        parentScan.ChildScan = addScan;

                        addScan.ParentScan = addScan;
                        addScan.ParentInjectionTime = parentScan.InjectionTime;
                    }

                    SortedList<double, double> lowMasses = new SortedList<double, double>();
                    var allData = rawFile.GetAdvancedPacketData(i);
                    CentroidStream centroidData = allData.CentroidData;
                    foreach (ICentroidPeak peak in centroidData.GetCentroids())
                    {
                        if (peak.Mass < 135)
                        {
                            lowMasses.Add(peak.Mass, peak.SignalToNoise);
                        }
                    }

                    //Search for the peaks...not a great solution right now. 
                    double sumSN = 0;
                    foreach (Modification tmtion in tmtIons)
                    {
                        MzRange tmtIonRange = new MzRange(tmtion.MonoisotopicMass, new Tolerance(ToleranceUnit.PPM, 15));

                        int index = ~Array.BinarySearch(lowMasses.Keys.ToArray(), tmtIonRange.Minimum);

                        double maxSN = 0;
                        if (index != 0 && index < lowMasses.Count)
                        {
                            while (lowMasses.ElementAt(index).Key < tmtIonRange.Maximum)
                            {
                                if (tmtIonRange.Contains(lowMasses.ElementAt(index).Key))
                                {
                                    if(lowMasses.ElementAt(index).Value > maxSN)
                                    {
                                        maxSN = lowMasses.ElementAt(index).Value;
                                    }
                                }

                                index++;

                                if (index >= lowMasses.Count)
                                {
                                    break;
                                }
                            }
                        }

                        addScan.SNDict.Add(tmtion.Name, maxSN);
                        sumSN += maxSN;
                    }

                    addScan.SumSN = sumSN;
                    addScan.IonFlux = sumSN / addScan.InjectionTime;

                    if(onlyScansWithSN.Checked && addScan.Order == quantOrder)
                    {
                        if (addScan.SumSN > 0)
                        {
                            mappedScans.Add(addScan.ScanNumber, addScan);
                        }
                    }
                    else
                    {
                        mappedScans.Add(addScan.ScanNumber, addScan);
                    }
                }
            }


            //Using the user inputs calculate the suggested values
            analyzedRun = new AnalyzedRun(quantOrder, rawFileName, mappedScans.Values.ToList());
            analyzedRun.CalculateSuggestedValues(double.Parse(carrierRatioTB.Text), double.Parse(slopeTB.Text), double.Parse(yInterTB.Text), double.Parse(maxITAdjTB.Text));

            return analyzedRun;
        }

        #region User Interface
        private void tmtFileGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (AnalyzedRuns.Count != 0)
            {
                int index = 0;
                if (tmtFileGrid.CurrentCell != null)
                {
                    index = tmtFileGrid.CurrentCell.RowIndex;
                }

                AnalyzedRun run = AnalyzedRuns[index];

                UpdateTMTPlots(QuantOrder, run);
            }
        }
        #endregion

        #region Plotting Code
        private void UpdateTMTPlots(int quantOrder, AnalyzedRun run)
        {
            PointPairList tmtIonFlux = null;
            PointPairList tmtSN = null;

            if(quantOrder == 2)
            {
                tmtIonFlux = Histogram(0, 20, 0.5, run.MS2IonFluxs);
                tmtSN = Histogram(0, 10000, 100, run.MS2SumSNs);
            }
            else if (quantOrder == 3)
            {
                tmtIonFlux = Histogram(0, 20, 0.5, run.MS3IonFluxs);
                tmtSN = Histogram(0, 10000, 100, run.MS3SumSNs);
            }
          
            ionFluxPane.CurveList.Clear();
            ionFluxPane.AddBar("Ion Flux", tmtIonFlux, Color.Blue);

            snPane.CurveList.Clear();
            snPane.AddBar("Signal to Noise", tmtSN, Color.Blue);

            tmtIonFluxgraphControl.AxisChange();
            tmtIonFluxgraphControl.Refresh();

            tmtSumSNgraphControl.AxisChange();
            tmtSumSNgraphControl.Refresh();
        }

        private PointPairList Histogram(double min, double max, double width, List<double> values)
        {
            PointPairList retList = new PointPairList();

            List<double> bins = new List<double>();
            Dictionary<double, int> binDict = new Dictionary<double, int>();
            for (double i = min; i <= max; i += width)
            {
                bins.Add(i);
                binDict.Add(i, 0);
            }

            foreach (double value in values)
            {
                foreach (double bin in bins)
                {
                    if (value >= bin && value <= bin + width)
                    {
                        binDict[bin] += 1;
                        break;
                    }
                }
            }


            foreach (KeyValuePair<double, int> kvp in binDict)
            {
                retList.Add(kvp.Key, kvp.Value);
            }

            return retList;
        }
        #endregion

        #region Regression Code
        private void InitializeRegressionList()
        {
            DataGridViewTextBoxColumn carrierLevelCol = new DataGridViewTextBoxColumn();
            carrierLevelCol.DataPropertyName = "CarrierLevel";
            carrierLevelCol.HeaderText = "Carrier Level";
            carrierLevelCol.Width = 50;
            regressionDGV.Columns.Add(carrierLevelCol);

            DataGridViewTextBoxColumn SNCol = new DataGridViewTextBoxColumn();
            SNCol.DataPropertyName = "SNCutoff";
            SNCol.HeaderText = "Median SN 20% CV";
            SNCol.Width = 50;
            regressionDGV.Columns.Add(SNCol);

            DataGridViewTextBoxColumn suggestSNCol = new DataGridViewTextBoxColumn();
            suggestSNCol.DataPropertyName = "SuggestedSNCutoff";
            suggestSNCol.HeaderText = "Suggested SN";
            suggestSNCol.Width = 60;
            suggestSNCol.ReadOnly = true;
            regressionDGV.Columns.Add(suggestSNCol);

            if(File.Exists(ParamPath + "\\defaultParams.csv"))
            {
                LoadParams();
            }
            else
            {
                double snAdjustment = double.Parse(snAdjTB.Text);
                RegressionPoint point1 = new RegressionPoint(5, 135, snAdjustment);
                RegressionPoint point2 = new RegressionPoint(20, 250, snAdjustment);
                RegressionPoint point3 = new RegressionPoint(75, 600, snAdjustment);
                RegressionPoint point4 = new RegressionPoint(300, 3000, snAdjustment);
                RegressionPoint point5 = new RegressionPoint(900, 8000, snAdjustment);

                RegressionList.Add(point1);
                RegressionList.Add(point2);
                RegressionList.Add(point3);
                RegressionList.Add(point4);
                RegressionList.Add(point5);

                SaveParams();
            }
        }

        private void InitializeRegressionPlot()
        {
            regressionPane = regressionGC.GraphPane;
            regressionPane.Title.Text = "Carrier Proteome vs. Suggested s/n";
            regressionPane.Title.FontSpec.Size = 25f;
            regressionPane.XAxis.Title.Text = "carrier proteome level";
            regressionPane.XAxis.Title.FontSpec.Size = 20f;
            regressionPane.YAxis.Title.Text = "suggested s/n cutoff";
            regressionPane.YAxis.Title.FontSpec.Size = 20f;
        }

        private void PlotRegression()
        {
            List<double> xList = new List<double>();
            List<double> yList = new List<double>();

            PointPairList points = new PointPairList();
            foreach (RegressionPoint point in RegressionList)
            {
                points.Add(new PointPair(double.Parse(point.CarrierLevel), double.Parse(point.SuggestedSNCutoff)));
                xList.Add(double.Parse(point.CarrierLevel));
                yList.Add(double.Parse(point.SuggestedSNCutoff));
            }

            double[] xdata = xList.ToArray();
            double[] ydata = yList.ToArray();

            Tuple<double, double> output = Fit.Line(xdata, ydata);
            double intercept = output.Item1;
            double slope = output.Item2;

            slopeTB.Text = Math.Round(slope, 2).ToString();
            yInterTB.Text = Math.Round(intercept, 2).ToString();

            regressionPane.CurveList.Clear();
            LineItem myCurve = regressionPane.AddCurve("", points, Color.Black, SymbolType.Circle);
            myCurve.Line.IsVisible = true;
            regressionGC.AxisChange();
            regressionGC.Refresh();
        }

        private void SaveParams()
        {
            using (StreamWriter writer = new StreamWriter(ParamPath + "\\" + paramNameTB.Text + ".csv"))
            {
                writer.WriteLine("Type,Val1,Val2");

                writer.WriteLine("CarrierLevel," + carrierRatioTB.Text);
                writer.WriteLine("SNAdjustment," + snAdjTB.Text);
                writer.WriteLine("MaxITAdjustment," + maxITAdjTB.Text);

                foreach (DataGridViewRow row in regressionDGV.Rows)
                {
                    double carrierLevel = double.Parse(row.Cells[0].Value.ToString());
                    double snValue = double.Parse(row.Cells[1].Value.ToString());

                    writer.WriteLine("RegressionPoint," + carrierLevel + "," + snValue);
                }
            }
        }

        private void LoadParams()
        {
            RegressionList.Clear();
            using (CsvReader reader = new CsvReader(new StreamReader(ParamPath + "\\" + paramNameTB.Text + ".csv"), true))
            {
                Dictionary<double, double> regressionPoints = new Dictionary<double, double>();
                while(reader.ReadNextRecord())
                {
                    if(reader["Type"] == "RegressionPoint")
                    {
                        double carrierLevel = double.Parse(reader["Val1"]);
                        double sn = double.Parse(reader["Val2"]);

                        regressionPoints.Add(carrierLevel, sn);
                    }
                    else if(reader["Type"] == "MaxITAdjustment")
                    {
                        maxITAdjTB.Text = reader["Val1"];
                    }
                    else if (reader["Type"] == "SNAdjustment")
                    {
                        snAdjTB.Text = reader["Val1"];
                    }
                    else if (reader["Type"] == "CarrierLevel")
                    {
                        carrierRatioTB.Text = reader["Val1"];
                    }
                }

                foreach(KeyValuePair<double, double> kvp in regressionPoints)
                {
                    RegressionList.Add(new RegressionPoint(kvp.Key, kvp.Value, double.Parse(snAdjTB.Text)));
                }
            }

            regressionDGV.Refresh();
        }
        #endregion

        #region Program Intializations
        private void InitializeSNPlot()
        {
            snPane = tmtSumSNgraphControl.GraphPane;
            snPane.Title.Text = "Sum s/n";
            snPane.Title.FontSpec.Size = 25f;
            snPane.XAxis.Title.Text = "sum s/n";
            snPane.XAxis.Title.FontSpec.Size = 20f;
            snPane.YAxis.Title.Text = "count";
            snPane.YAxis.Title.FontSpec.Size = 20f;
        }

        private void InitializeIonFluxPlot()
        {
            ionFluxPane = tmtIonFluxgraphControl.GraphPane;
            ionFluxPane.Title.Text = "Ion Flux";
            ionFluxPane.Title.FontSpec.Size = 25f;
            ionFluxPane.XAxis.Title.Text = "ion flux (s/n per ms)";
            ionFluxPane.XAxis.Title.FontSpec.Size = 20f;
            ionFluxPane.YAxis.Title.Text = "count";
            ionFluxPane.YAxis.Title.FontSpec.Size = 20f;
        }

        private void InitializeMS2RunGrid()
        {
            tmtFileGrid.DataSource = AnalyzedRuns;
            RunGridInitialized = true;

            tmtFileGrid.AutoGenerateColumns = true;

            tmtFileGrid.Columns.Clear();
            DataGridViewTextBoxColumn rawNameCol = new DataGridViewTextBoxColumn();
            rawNameCol.DataPropertyName = "RawFileName";
            rawNameCol.HeaderText = "Raw File";
            rawNameCol.ReadOnly = true;
            rawNameCol.Width = 350;
            tmtFileGrid.Columns.Add(rawNameCol);

            DataGridViewTextBoxColumn ms2ITCol = new DataGridViewTextBoxColumn();
            ms2ITCol.DataPropertyName = "MedianMS2IT";
            ms2ITCol.HeaderText = "Median MS2 IT";
            ms2ITCol.ReadOnly = true;
            ms2ITCol.Width = 120;
            tmtFileGrid.Columns.Add(ms2ITCol);

            DataGridViewTextBoxColumn ms2SNCol = new DataGridViewTextBoxColumn();
            ms2SNCol.DataPropertyName = "MedianMS2SN";
            ms2SNCol.HeaderText = "Median Sum SN";
            ms2SNCol.ReadOnly = true;
            ms2SNCol.Width = 120;
            tmtFileGrid.Columns.Add(ms2SNCol);

            DataGridViewTextBoxColumn ms2IonFluxCol = new DataGridViewTextBoxColumn();
            ms2IonFluxCol.DataPropertyName = "MedianMS2IonFlux";
            ms2IonFluxCol.HeaderText = "Median Ion Flux";
            ms2IonFluxCol.ReadOnly = true;
            ms2IonFluxCol.Width = 120;
            tmtFileGrid.Columns.Add(ms2IonFluxCol);

            DataGridViewTextBoxColumn ms2SuggSNCutoffCol = new DataGridViewTextBoxColumn();
            ms2SuggSNCutoffCol.DataPropertyName = "SuggestedSNCutoff";
            ms2SuggSNCutoffCol.HeaderText = "Suggested SN Cutoff";
            ms2SuggSNCutoffCol.ReadOnly = true;
            ms2SuggSNCutoffCol.Width = 130;
            tmtFileGrid.Columns.Add(ms2SuggSNCutoffCol);

            DataGridViewTextBoxColumn ms2SuggMaxITCol = new DataGridViewTextBoxColumn();
            ms2SuggMaxITCol.DataPropertyName = "SuggestedMaxIT";
            ms2SuggMaxITCol.HeaderText = "Suggested Max IT";
            ms2SuggMaxITCol.ReadOnly = true;
            ms2SuggMaxITCol.Width = 120;
            tmtFileGrid.Columns.Add(ms2SuggMaxITCol);
        }

        private void InitializeMS3RunGrid()
        {
            tmtFileGrid.DataSource = AnalyzedRuns;
            RunGridInitialized = true;

            tmtFileGrid.AutoGenerateColumns = true;

            tmtFileGrid.Columns.Clear();
            DataGridViewTextBoxColumn rawNameCol = new DataGridViewTextBoxColumn();
            rawNameCol.DataPropertyName = "RawFileName";
            rawNameCol.HeaderText = "Raw File";
            rawNameCol.ReadOnly = true;
            rawNameCol.Width = 350;
            tmtFileGrid.Columns.Add(rawNameCol);

            DataGridViewTextBoxColumn ms2ITCol = new DataGridViewTextBoxColumn();
            ms2ITCol.DataPropertyName = "MedianMS2IT";
            ms2ITCol.HeaderText = "Median MS2 IT";
            ms2ITCol.ReadOnly = true;
            ms2ITCol.Width = 120;
            tmtFileGrid.Columns.Add(ms2ITCol);

            DataGridViewTextBoxColumn ms3ITCol = new DataGridViewTextBoxColumn();
            ms3ITCol.DataPropertyName = "MedianMS3IT";
            ms3ITCol.HeaderText = "Median MS3 IT";
            ms3ITCol.ReadOnly = true;
            ms3ITCol.Width = 120;
            tmtFileGrid.Columns.Add(ms3ITCol);

            DataGridViewTextBoxColumn ms2SNCol = new DataGridViewTextBoxColumn();
            ms2SNCol.DataPropertyName = "MedianMS3SN";
            ms2SNCol.HeaderText = "Median Sum SN";
            ms2SNCol.ReadOnly = true;
            ms2SNCol.Width = 120;
            tmtFileGrid.Columns.Add(ms2SNCol);

            DataGridViewTextBoxColumn ms2IonFluxCol = new DataGridViewTextBoxColumn();
            ms2IonFluxCol.DataPropertyName = "MedianMS3IonFlux";
            ms2IonFluxCol.HeaderText = "Median Ion Flux";
            ms2IonFluxCol.ReadOnly = true;
            ms2IonFluxCol.Width = 120;
            tmtFileGrid.Columns.Add(ms2IonFluxCol);

            DataGridViewTextBoxColumn ms2SuggSNCutoffCol = new DataGridViewTextBoxColumn();
            ms2SuggSNCutoffCol.DataPropertyName = "SuggestedSNCutoff";
            ms2SuggSNCutoffCol.HeaderText = "Suggested SN Cutoff";
            ms2SuggSNCutoffCol.ReadOnly = true;
            ms2SuggSNCutoffCol.Width = 130;
            tmtFileGrid.Columns.Add(ms2SuggSNCutoffCol);

            DataGridViewTextBoxColumn ms2SuggMaxITCol = new DataGridViewTextBoxColumn();
            ms2SuggMaxITCol.DataPropertyName = "SuggestedMaxIT";
            ms2SuggMaxITCol.HeaderText = "Suggested Max IT";
            ms2SuggMaxITCol.ReadOnly = true;
            ms2SuggMaxITCol.Width = 120;
            tmtFileGrid.Columns.Add(ms2SuggMaxITCol);
        }

        private List<Modification> InitializeTMT()
        {
            List<Modification> tmt = new List<Modification>();

            tmt.Add(new Modification(monoMass: 126.127726, name: "TMT126", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 127.124761, name: "TMT127n", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 127.131081, name: "TMT127c", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 128.128116, name: "TMT128n", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 128.134436, name: "TMT128c", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 129.131471, name: "TMT129n", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 129.13779, name: "TMT129c", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 130.134825, name: "TMT130n", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 130.141145, name: "TMT130c", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 131.13818, name: "TMT131n", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 131.1445, name: "TMT131c", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 132.141535, name: "TMT132n", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 132.147855, name: "TMT132c", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 133.14489, name: "TMT133n", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 133.15121, name: "TMT133c", sites: ModificationSites.K | ModificationSites.NPep));
            tmt.Add(new Modification(monoMass: 134.148245, name: "TMT134n", sites: ModificationSites.K | ModificationSites.NPep));

            TMTIons = tmt;

            return tmt;
        }
        #endregion

        #region Input Code
        private void AddFiles(List<string> files)
        {
            foreach (string file in files)
            {
                InputFiles.Add(file);
            }
        }

        private void inputFileBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                AddFiles(files.ToList());
            }
        }

        private void inputFileBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }
        #endregion

        #region Unused
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tmtSumSNgraphControl_Load(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
