namespace SCPCompanion
{
    partial class SCPCompanionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.inputFileBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tmtFileGrid = new System.Windows.Forms.DataGridView();
            this.tmtSumSNgraphControl = new ZedGraph.ZedGraphControl();
            this.tmtIonFluxgraphControl = new ZedGraph.ZedGraphControl();
            this.carrierRatioTB = new System.Windows.Forms.TextBox();
            this.snAdjTB = new System.Windows.Forms.TextBox();
            this.maxITAdjTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.regressionGC = new ZedGraph.ZedGraphControl();
            this.updateAndSave = new System.Windows.Forms.Button();
            this.clearFiles = new System.Windows.Forms.Button();
            this.ms2Analysis = new System.Windows.Forms.Button();
            this.paramNameTB = new System.Windows.Forms.TextBox();
            this.yInterTB = new System.Windows.Forms.TextBox();
            this.slopeTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.regressionDGV = new System.Windows.Forms.DataGridView();
            this.onlyScansWithSN = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.quantComboBox = new System.Windows.Forms.ComboBox();
            this.loadParamButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tmtFileGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.regressionDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // inputFileBox
            // 
            this.inputFileBox.AllowDrop = true;
            this.inputFileBox.FormattingEnabled = true;
            this.inputFileBox.ItemHeight = 16;
            this.inputFileBox.Location = new System.Drawing.Point(13, 31);
            this.inputFileBox.Margin = new System.Windows.Forms.Padding(4);
            this.inputFileBox.Name = "inputFileBox";
            this.inputFileBox.Size = new System.Drawing.Size(1384, 84);
            this.inputFileBox.TabIndex = 3;
            this.inputFileBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.inputFileBox_DragDrop);
            this.inputFileBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.inputFileBox_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Input Files";
            // 
            // tmtFileGrid
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tmtFileGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tmtFileGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tmtFileGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.tmtFileGrid.Location = new System.Drawing.Point(16, 576);
            this.tmtFileGrid.Margin = new System.Windows.Forms.Padding(4);
            this.tmtFileGrid.Name = "tmtFileGrid";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tmtFileGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.tmtFileGrid.Size = new System.Drawing.Size(1381, 200);
            this.tmtFileGrid.TabIndex = 9;
            this.tmtFileGrid.SelectionChanged += new System.EventHandler(this.tmtFileGrid_SelectionChanged);
            // 
            // tmtSumSNgraphControl
            // 
            this.tmtSumSNgraphControl.Location = new System.Drawing.Point(14, 138);
            this.tmtSumSNgraphControl.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.tmtSumSNgraphControl.Name = "tmtSumSNgraphControl";
            this.tmtSumSNgraphControl.ScrollGrace = 0D;
            this.tmtSumSNgraphControl.ScrollMaxX = 0D;
            this.tmtSumSNgraphControl.ScrollMaxY = 0D;
            this.tmtSumSNgraphControl.ScrollMaxY2 = 0D;
            this.tmtSumSNgraphControl.ScrollMinX = 0D;
            this.tmtSumSNgraphControl.ScrollMinY = 0D;
            this.tmtSumSNgraphControl.ScrollMinY2 = 0D;
            this.tmtSumSNgraphControl.Size = new System.Drawing.Size(600, 207);
            this.tmtSumSNgraphControl.TabIndex = 10;
            this.tmtSumSNgraphControl.UseExtendedPrintDialog = true;
            this.tmtSumSNgraphControl.Load += new System.EventHandler(this.tmtSumSNgraphControl_Load);
            // 
            // tmtIonFluxgraphControl
            // 
            this.tmtIonFluxgraphControl.Location = new System.Drawing.Point(16, 356);
            this.tmtIonFluxgraphControl.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.tmtIonFluxgraphControl.Name = "tmtIonFluxgraphControl";
            this.tmtIonFluxgraphControl.ScrollGrace = 0D;
            this.tmtIonFluxgraphControl.ScrollMaxX = 0D;
            this.tmtIonFluxgraphControl.ScrollMaxY = 0D;
            this.tmtIonFluxgraphControl.ScrollMaxY2 = 0D;
            this.tmtIonFluxgraphControl.ScrollMinX = 0D;
            this.tmtIonFluxgraphControl.ScrollMinY = 0D;
            this.tmtIonFluxgraphControl.ScrollMinY2 = 0D;
            this.tmtIonFluxgraphControl.Size = new System.Drawing.Size(600, 207);
            this.tmtIonFluxgraphControl.TabIndex = 11;
            this.tmtIonFluxgraphControl.UseExtendedPrintDialog = true;
            // 
            // carrierRatioTB
            // 
            this.carrierRatioTB.Location = new System.Drawing.Point(833, 174);
            this.carrierRatioTB.Margin = new System.Windows.Forms.Padding(4);
            this.carrierRatioTB.Name = "carrierRatioTB";
            this.carrierRatioTB.Size = new System.Drawing.Size(93, 22);
            this.carrierRatioTB.TabIndex = 18;
            this.carrierRatioTB.Text = "0";
            this.carrierRatioTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // snAdjTB
            // 
            this.snAdjTB.Location = new System.Drawing.Point(833, 207);
            this.snAdjTB.Margin = new System.Windows.Forms.Padding(4);
            this.snAdjTB.Name = "snAdjTB";
            this.snAdjTB.Size = new System.Drawing.Size(93, 22);
            this.snAdjTB.TabIndex = 19;
            this.snAdjTB.Text = "1.2";
            this.snAdjTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // maxITAdjTB
            // 
            this.maxITAdjTB.Location = new System.Drawing.Point(833, 240);
            this.maxITAdjTB.Margin = new System.Windows.Forms.Padding(4);
            this.maxITAdjTB.Name = "maxITAdjTB";
            this.maxITAdjTB.Size = new System.Drawing.Size(93, 22);
            this.maxITAdjTB.TabIndex = 20;
            this.maxITAdjTB.Text = "1.5";
            this.maxITAdjTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(638, 177);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 17);
            this.label2.TabIndex = 21;
            this.label2.Text = "Carrier Proteome Level";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(638, 210);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 17);
            this.label3.TabIndex = 22;
            this.label3.Text = "s/n Cuttoff Adjustment";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(638, 243);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 17);
            this.label4.TabIndex = 23;
            this.label4.Text = "Max IT Adjustment";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(639, 138);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(279, 24);
            this.label5.TabIndex = 24;
            this.label5.Text = "Carrier Proteome Adjustment";
            // 
            // regressionGC
            // 
            this.regressionGC.Location = new System.Drawing.Point(950, 207);
            this.regressionGC.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.regressionGC.Name = "regressionGC";
            this.regressionGC.ScrollGrace = 0D;
            this.regressionGC.ScrollMaxX = 0D;
            this.regressionGC.ScrollMaxY = 0D;
            this.regressionGC.ScrollMaxY2 = 0D;
            this.regressionGC.ScrollMinX = 0D;
            this.regressionGC.ScrollMinY = 0D;
            this.regressionGC.ScrollMinY2 = 0D;
            this.regressionGC.Size = new System.Drawing.Size(447, 313);
            this.regressionGC.TabIndex = 25;
            this.regressionGC.UseExtendedPrintDialog = true;
            // 
            // updateAndSave
            // 
            this.updateAndSave.Location = new System.Drawing.Point(791, 533);
            this.updateAndSave.Name = "updateAndSave";
            this.updateAndSave.Size = new System.Drawing.Size(134, 31);
            this.updateAndSave.TabIndex = 26;
            this.updateAndSave.Text = "Update and Save";
            this.updateAndSave.UseVisualStyleBackColor = true;
            this.updateAndSave.Click += new System.EventHandler(this.updateAndSave_Click);
            // 
            // clearFiles
            // 
            this.clearFiles.Location = new System.Drawing.Point(1134, 126);
            this.clearFiles.Name = "clearFiles";
            this.clearFiles.Size = new System.Drawing.Size(126, 31);
            this.clearFiles.TabIndex = 27;
            this.clearFiles.Text = "Clear Files";
            this.clearFiles.UseVisualStyleBackColor = true;
            this.clearFiles.Click += new System.EventHandler(this.clearFiles_Click);
            // 
            // ms2Analysis
            // 
            this.ms2Analysis.Location = new System.Drawing.Point(1272, 126);
            this.ms2Analysis.Name = "ms2Analysis";
            this.ms2Analysis.Size = new System.Drawing.Size(126, 31);
            this.ms2Analysis.TabIndex = 28;
            this.ms2Analysis.Text = "Analyze";
            this.ms2Analysis.UseVisualStyleBackColor = true;
            this.ms2Analysis.Click += new System.EventHandler(this.ms2Analysis_Click);
            // 
            // paramNameTB
            // 
            this.paramNameTB.Location = new System.Drawing.Point(641, 501);
            this.paramNameTB.Margin = new System.Windows.Forms.Padding(4);
            this.paramNameTB.Name = "paramNameTB";
            this.paramNameTB.Size = new System.Drawing.Size(285, 22);
            this.paramNameTB.TabIndex = 30;
            this.paramNameTB.Text = "defaultParams";
            // 
            // yInterTB
            // 
            this.yInterTB.Location = new System.Drawing.Point(1245, 532);
            this.yInterTB.Margin = new System.Windows.Forms.Padding(4);
            this.yInterTB.Name = "yInterTB";
            this.yInterTB.Size = new System.Drawing.Size(105, 22);
            this.yInterTB.TabIndex = 31;
            // 
            // slopeTB
            // 
            this.slopeTB.Location = new System.Drawing.Point(1041, 532);
            this.slopeTB.Margin = new System.Windows.Forms.Padding(4);
            this.slopeTB.Name = "slopeTB";
            this.slopeTB.Size = new System.Drawing.Size(106, 22);
            this.slopeTB.TabIndex = 32;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(995, 534);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 17);
            this.label6.TabIndex = 33;
            this.label6.Text = "slope";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1165, 534);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 17);
            this.label7.TabIndex = 34;
            this.label7.Text = "y-intercept";
            // 
            // regressionDGV
            // 
            this.regressionDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.regressionDGV.Location = new System.Drawing.Point(641, 281);
            this.regressionDGV.Name = "regressionDGV";
            this.regressionDGV.RowTemplate.Height = 24;
            this.regressionDGV.Size = new System.Drawing.Size(285, 213);
            this.regressionDGV.TabIndex = 35;
            this.regressionDGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // onlyScansWithSN
            // 
            this.onlyScansWithSN.AutoSize = true;
            this.onlyScansWithSN.Checked = true;
            this.onlyScansWithSN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.onlyScansWithSN.Location = new System.Drawing.Point(1236, 174);
            this.onlyScansWithSN.Name = "onlyScansWithSN";
            this.onlyScansWithSN.Size = new System.Drawing.Size(160, 21);
            this.onlyScansWithSN.TabIndex = 36;
            this.onlyScansWithSN.Text = "Only Analyze SN > 0";
            this.onlyScansWithSN.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(947, 175);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(150, 17);
            this.label8.TabIndex = 38;
            this.label8.Text = "Quantitation MS Order";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // quantComboBox
            // 
            this.quantComboBox.FormattingEnabled = true;
            this.quantComboBox.Items.AddRange(new object[] {
            "MS2",
            "MS3"});
            this.quantComboBox.Location = new System.Drawing.Point(1105, 172);
            this.quantComboBox.Name = "quantComboBox";
            this.quantComboBox.Size = new System.Drawing.Size(82, 24);
            this.quantComboBox.TabIndex = 39;
            this.quantComboBox.Text = "MS2";
            // 
            // loadParamButton
            // 
            this.loadParamButton.Location = new System.Drawing.Point(643, 534);
            this.loadParamButton.Name = "loadParamButton";
            this.loadParamButton.Size = new System.Drawing.Size(102, 30);
            this.loadParamButton.TabIndex = 40;
            this.loadParamButton.Text = "Load";
            this.loadParamButton.UseVisualStyleBackColor = true;
            this.loadParamButton.Click += new System.EventHandler(this.loadParamButton_Click);
            // 
            // SCPCompanionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1410, 795);
            this.Controls.Add(this.loadParamButton);
            this.Controls.Add(this.quantComboBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.onlyScansWithSN);
            this.Controls.Add(this.regressionDGV);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.slopeTB);
            this.Controls.Add(this.yInterTB);
            this.Controls.Add(this.paramNameTB);
            this.Controls.Add(this.ms2Analysis);
            this.Controls.Add(this.clearFiles);
            this.Controls.Add(this.updateAndSave);
            this.Controls.Add(this.regressionGC);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.maxITAdjTB);
            this.Controls.Add(this.snAdjTB);
            this.Controls.Add(this.carrierRatioTB);
            this.Controls.Add(this.tmtIonFluxgraphControl);
            this.Controls.Add(this.tmtSumSNgraphControl);
            this.Controls.Add(this.tmtFileGrid);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inputFileBox);
            this.Name = "SCPCompanionForm";
            this.Text = "SCPCompanion";
            ((System.ComponentModel.ISupportInitialize)(this.tmtFileGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.regressionDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox inputFileBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView tmtFileGrid;
        private ZedGraph.ZedGraphControl tmtSumSNgraphControl;
        private ZedGraph.ZedGraphControl tmtIonFluxgraphControl;
        private System.Windows.Forms.TextBox carrierRatioTB;
        private System.Windows.Forms.TextBox snAdjTB;
        private System.Windows.Forms.TextBox maxITAdjTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private ZedGraph.ZedGraphControl regressionGC;
        private System.Windows.Forms.Button updateAndSave;
        private System.Windows.Forms.Button clearFiles;
        private System.Windows.Forms.Button ms2Analysis;
        private System.Windows.Forms.TextBox paramNameTB;
        private System.Windows.Forms.TextBox yInterTB;
        private System.Windows.Forms.TextBox slopeTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView regressionDGV;
        private System.Windows.Forms.CheckBox onlyScansWithSN;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox quantComboBox;
        private System.Windows.Forms.Button loadParamButton;
    }
}

