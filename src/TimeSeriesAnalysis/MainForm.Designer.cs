﻿using System.Windows.Forms;
using MathLib.DrawEngine;

namespace TimeSeriesAnalysis
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.openFileBtn = new System.Windows.Forms.Button();
            this.plotBtn = new System.Windows.Forms.Button();
            this.fileNameLbl = new System.Windows.Forms.Label();
            this.saveBtn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.waveletGroup = new System.Windows.Forms.GroupBox();
            this.wav_plotPBox = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.wav_colorMapCombo = new System.Windows.Forms.ComboBox();
            this.wav_nameCombo = new System.Windows.Forms.ComboBox();
            this.wav_dtNum = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.wav_fEndNum = new System.Windows.Forms.NumericUpDown();
            this.wav_fStartNum = new System.Windows.Forms.NumericUpDown();
            this.waveletNameLbl = new System.Windows.Forms.Label();
            this.waveletCheckbox = new System.Windows.Forms.CheckBox();
            this.fourierGroup = new System.Windows.Forms.GroupBox();
            this.fft_dtNum = new System.Windows.Forms.NumericUpDown();
            this.fft_logCbox = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.fft_fEndNum = new System.Windows.Forms.NumericUpDown();
            this.fft_fStartNum = new System.Windows.Forms.NumericUpDown();
            this.fourierCheckbox = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.le_jakobian_radio = new System.Windows.Forms.RadioButton();
            this.label19 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lyapunovRedrawBtn = new System.Windows.Forms.Button();
            this.lyap_calc_Rad_kantz = new System.Windows.Forms.RadioButton();
            this.le_scaleMaxNum = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.le_pStartNum = new System.Windows.Forms.NumericUpDown();
            this.le_pEndNum = new System.Windows.Forms.NumericUpDown();
            this.lyap_k_Grp = new System.Windows.Forms.GroupBox();
            this.le_kantz_slopeCombo = new System.Windows.Forms.ComboBox();
            this.lyap_k_Lbl_scales = new System.Windows.Forms.Label();
            this.le_kantz_scalesNum = new System.Windows.Forms.NumericUpDown();
            this.startBtn = new System.Windows.Forms.Button();
            this.le_scaleMinNum = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.lyap_calc_Rad_rosenstein = new System.Windows.Forms.RadioButton();
            this.le_resultText = new System.Windows.Forms.RichTextBox();
            this.le_tauNum = new System.Windows.Forms.NumericUpDown();
            this.dimLbl = new System.Windows.Forms.Label();
            this.tauLbl = new System.Windows.Forms.Label();
            this.lyap_r_Grp = new System.Windows.Forms.GroupBox();
            this.le_ros_stepsNum = new System.Windows.Forms.NumericUpDown();
            this.rosStepsLbl = new System.Windows.Forms.Label();
            this.rosDistanceLbl = new System.Windows.Forms.Label();
            this.le_ros_distanceNum = new System.Windows.Forms.NumericUpDown();
            this.le_dimNum = new System.Windows.Forms.NumericUpDown();
            this.le_wolf_radio = new System.Windows.Forms.RadioButton();
            this.lyap_w_Grp = new System.Windows.Forms.GroupBox();
            this.le_wolf_stepNum = new System.Windows.Forms.NumericUpDown();
            this.stepLbl = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.le_wolf_evolveStepsNum = new System.Windows.Forms.NumericUpDown();
            this.sourcePrefGBox = new System.Windows.Forms.GroupBox();
            this.sourceStepLbl = new System.Windows.Forms.Label();
            this.sourceStepTxt = new System.Windows.Forms.RichTextBox();
            this.useTimeCheckbox = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.endPointNum = new System.Windows.Forms.NumericUpDown();
            this.startPointNum = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pointsNum = new System.Windows.Forms.NumericUpDown();
            this.sourceColumnNum = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.numPreviewWidth = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.numPreviewHeight = new System.Windows.Forms.NumericUpDown();
            this.lyap_log_text = new System.Windows.Forms.RichTextBox();
            this.chartPoincare = new MathLib.DrawEngine.MathChart();
            this.chartSignal = new MathLib.DrawEngine.MathChart();
            this.chartFft = new MathLib.DrawEngine.MathChart();
            this.chartLyapunov = new MathLib.DrawEngine.MathChart();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.waveletGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wav_plotPBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wav_dtNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wav_fEndNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wav_fStartNum)).BeginInit();
            this.fourierGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fft_dtNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fft_fEndNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fft_fStartNum)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.le_scaleMaxNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_pStartNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_pEndNum)).BeginInit();
            this.lyap_k_Grp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.le_kantz_scalesNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_scaleMinNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_tauNum)).BeginInit();
            this.lyap_r_Grp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.le_ros_stepsNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_ros_distanceNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_dimNum)).BeginInit();
            this.lyap_w_Grp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.le_wolf_stepNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_wolf_evolveStepsNum)).BeginInit();
            this.sourcePrefGBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endPointNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startPointNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointsNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceColumnNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPreviewWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPreviewHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartPoincare)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSignal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartFft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLyapunov)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileBtn
            // 
            this.openFileBtn.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.openFileBtn.Location = new System.Drawing.Point(1073, 12);
            this.openFileBtn.Name = "openFileBtn";
            this.openFileBtn.Size = new System.Drawing.Size(75, 23);
            this.openFileBtn.TabIndex = 0;
            this.openFileBtn.Text = "Open";
            this.openFileBtn.UseVisualStyleBackColor = true;
            this.openFileBtn.Click += new System.EventHandler(this.openFileBtn_Click);
            // 
            // plotBtn
            // 
            this.plotBtn.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.plotBtn.Location = new System.Drawing.Point(797, 6);
            this.plotBtn.Name = "plotBtn";
            this.plotBtn.Size = new System.Drawing.Size(95, 23);
            this.plotBtn.TabIndex = 2;
            this.plotBtn.Text = "Build charts";
            this.plotBtn.UseVisualStyleBackColor = true;
            this.plotBtn.Click += new System.EventHandler(this.plotBtn_Click);
            // 
            // fileNameLbl
            // 
            this.fileNameLbl.AutoSize = true;
            this.fileNameLbl.BackColor = System.Drawing.Color.Khaki;
            this.fileNameLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileNameLbl.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fileNameLbl.Location = new System.Drawing.Point(4, 5);
            this.fileNameLbl.Name = "fileNameLbl";
            this.fileNameLbl.Size = new System.Drawing.Size(218, 16);
            this.fileNameLbl.TabIndex = 14;
            this.fileNameLbl.Text = "Please, select file with timeseries";
            // 
            // saveBtn
            // 
            this.saveBtn.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.saveBtn.Location = new System.Drawing.Point(1069, 601);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(79, 23);
            this.saveBtn.TabIndex = 1;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(906, 612);
            this.tabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Silver;
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.waveletGroup);
            this.tabPage1.Controls.Add(this.fourierGroup);
            this.tabPage1.Controls.Add(this.plotBtn);
            this.tabPage1.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(898, 585);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Charts";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chartPoincare);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(8, 309);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(340, 268);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Poincare pseudosection";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chartSignal);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(8, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(340, 269);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Signal";
            // 
            // waveletGroup
            // 
            this.waveletGroup.Controls.Add(this.wav_plotPBox);
            this.waveletGroup.Controls.Add(this.label10);
            this.waveletGroup.Controls.Add(this.wav_colorMapCombo);
            this.waveletGroup.Controls.Add(this.wav_nameCombo);
            this.waveletGroup.Controls.Add(this.wav_dtNum);
            this.waveletGroup.Controls.Add(this.label11);
            this.waveletGroup.Controls.Add(this.label15);
            this.waveletGroup.Controls.Add(this.label14);
            this.waveletGroup.Controls.Add(this.wav_fEndNum);
            this.waveletGroup.Controls.Add(this.wav_fStartNum);
            this.waveletGroup.Controls.Add(this.waveletNameLbl);
            this.waveletGroup.Controls.Add(this.waveletCheckbox);
            this.waveletGroup.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.waveletGroup.Location = new System.Drawing.Point(360, 309);
            this.waveletGroup.Name = "waveletGroup";
            this.waveletGroup.Size = new System.Drawing.Size(532, 268);
            this.waveletGroup.TabIndex = 22;
            this.waveletGroup.TabStop = false;
            this.waveletGroup.Text = "Wavelet";
            // 
            // wav_plotPBox
            // 
            this.wav_plotPBox.BackColor = System.Drawing.Color.White;
            this.wav_plotPBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wav_plotPBox.Cursor = System.Windows.Forms.Cursors.Cross;
            this.wav_plotPBox.Location = new System.Drawing.Point(6, 21);
            this.wav_plotPBox.Name = "wav_plotPBox";
            this.wav_plotPBox.Size = new System.Drawing.Size(320, 240);
            this.wav_plotPBox.TabIndex = 25;
            this.wav_plotPBox.TabStop = false;
            this.wav_plotPBox.DoubleClick += new System.EventHandler(this.waveletPBox_DoubleClick);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(355, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 14);
            this.label10.TabIndex = 31;
            this.label10.Text = "Color map:";
            // 
            // wav_colorMapCombo
            // 
            this.wav_colorMapCombo.DisplayMember = "haar";
            this.wav_colorMapCombo.FormattingEnabled = true;
            this.wav_colorMapCombo.Items.AddRange(new object[] {
            "pink",
            "parula"});
            this.wav_colorMapCombo.Location = new System.Drawing.Point(437, 49);
            this.wav_colorMapCombo.Name = "wav_colorMapCombo";
            this.wav_colorMapCombo.Size = new System.Drawing.Size(90, 22);
            this.wav_colorMapCombo.TabIndex = 30;
            this.wav_colorMapCombo.Text = "pink";
            // 
            // wav_nameCombo
            // 
            this.wav_nameCombo.DisplayMember = "haar";
            this.wav_nameCombo.FormattingEnabled = true;
            this.wav_nameCombo.Items.AddRange(new object[] {
            "haar",
            "sym2",
            "gaus8",
            "cgau8"});
            this.wav_nameCombo.Location = new System.Drawing.Point(436, 21);
            this.wav_nameCombo.Name = "wav_nameCombo";
            this.wav_nameCombo.Size = new System.Drawing.Size(90, 22);
            this.wav_nameCombo.TabIndex = 29;
            this.wav_nameCombo.Text = "gaus8";
            // 
            // wav_dtNum
            // 
            this.wav_dtNum.DecimalPlaces = 5;
            this.wav_dtNum.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.wav_dtNum.Location = new System.Drawing.Point(438, 135);
            this.wav_dtNum.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.wav_dtNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.wav_dtNum.Name = "wav_dtNum";
            this.wav_dtNum.Size = new System.Drawing.Size(90, 22);
            this.wav_dtNum.TabIndex = 28;
            this.wav_dtNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(406, 137);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(25, 14);
            this.label11.TabIndex = 27;
            this.label11.Text = "Δt:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(361, 108);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(70, 14);
            this.label15.TabIndex = 25;
            this.label15.Text = "Ending ω:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(355, 79);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 14);
            this.label14.TabIndex = 24;
            this.label14.Text = "Starting ω:";
            // 
            // wav_fEndNum
            // 
            this.wav_fEndNum.DecimalPlaces = 2;
            this.wav_fEndNum.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.wav_fEndNum.Location = new System.Drawing.Point(438, 106);
            this.wav_fEndNum.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.wav_fEndNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.wav_fEndNum.Name = "wav_fEndNum";
            this.wav_fEndNum.Size = new System.Drawing.Size(90, 22);
            this.wav_fEndNum.TabIndex = 23;
            this.wav_fEndNum.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // wav_fStartNum
            // 
            this.wav_fStartNum.DecimalPlaces = 2;
            this.wav_fStartNum.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.wav_fStartNum.Location = new System.Drawing.Point(437, 77);
            this.wav_fStartNum.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.wav_fStartNum.Name = "wav_fStartNum";
            this.wav_fStartNum.Size = new System.Drawing.Size(90, 22);
            this.wav_fStartNum.TabIndex = 22;
            // 
            // waveletNameLbl
            // 
            this.waveletNameLbl.AutoSize = true;
            this.waveletNameLbl.Location = new System.Drawing.Point(335, 24);
            this.waveletNameLbl.Name = "waveletNameLbl";
            this.waveletNameLbl.Size = new System.Drawing.Size(95, 14);
            this.waveletNameLbl.TabIndex = 20;
            this.waveletNameLbl.Text = "Wavelet type:";
            // 
            // waveletCheckbox
            // 
            this.waveletCheckbox.AutoSize = true;
            this.waveletCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.waveletCheckbox.Location = new System.Drawing.Point(470, 244);
            this.waveletCheckbox.Name = "waveletCheckbox";
            this.waveletCheckbox.Size = new System.Drawing.Size(56, 18);
            this.waveletCheckbox.TabIndex = 14;
            this.waveletCheckbox.Text = "Build";
            this.waveletCheckbox.UseVisualStyleBackColor = true;
            // 
            // fourierGroup
            // 
            this.fourierGroup.Controls.Add(this.chartFft);
            this.fourierGroup.Controls.Add(this.fft_dtNum);
            this.fourierGroup.Controls.Add(this.fft_logCbox);
            this.fourierGroup.Controls.Add(this.label16);
            this.fourierGroup.Controls.Add(this.label9);
            this.fourierGroup.Controls.Add(this.label8);
            this.fourierGroup.Controls.Add(this.fft_fEndNum);
            this.fourierGroup.Controls.Add(this.fft_fStartNum);
            this.fourierGroup.Controls.Add(this.fourierCheckbox);
            this.fourierGroup.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fourierGroup.Location = new System.Drawing.Point(360, 35);
            this.fourierGroup.Name = "fourierGroup";
            this.fourierGroup.Size = new System.Drawing.Size(532, 268);
            this.fourierGroup.TabIndex = 20;
            this.fourierGroup.TabStop = false;
            this.fourierGroup.Text = "Fourier power spectrum";
            // 
            // fft_dtNum
            // 
            this.fft_dtNum.DecimalPlaces = 5;
            this.fft_dtNum.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.fft_dtNum.Location = new System.Drawing.Point(436, 104);
            this.fft_dtNum.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.fft_dtNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.fft_dtNum.Name = "fft_dtNum";
            this.fft_dtNum.Size = new System.Drawing.Size(90, 22);
            this.fft_dtNum.TabIndex = 26;
            this.fft_dtNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // fft_logCbox
            // 
            this.fft_logCbox.AutoSize = true;
            this.fft_logCbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.fft_logCbox.Location = new System.Drawing.Point(352, 22);
            this.fft_logCbox.Name = "fft_logCbox";
            this.fft_logCbox.Size = new System.Drawing.Size(98, 18);
            this.fft_logCbox.TabIndex = 25;
            this.fft_logCbox.Text = "Logarithmic";
            this.fft_logCbox.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(405, 106);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(25, 14);
            this.label16.TabIndex = 23;
            this.label16.Text = "Δt:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(360, 78);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 14);
            this.label9.TabIndex = 21;
            this.label9.Text = "Ending ω:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(354, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 14);
            this.label8.TabIndex = 20;
            this.label8.Text = "Starting ω:";
            // 
            // fft_fEndNum
            // 
            this.fft_fEndNum.DecimalPlaces = 2;
            this.fft_fEndNum.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.fft_fEndNum.Location = new System.Drawing.Point(436, 76);
            this.fft_fEndNum.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.fft_fEndNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.fft_fEndNum.Name = "fft_fEndNum";
            this.fft_fEndNum.Size = new System.Drawing.Size(90, 22);
            this.fft_fEndNum.TabIndex = 6;
            this.fft_fEndNum.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // fft_fStartNum
            // 
            this.fft_fStartNum.DecimalPlaces = 2;
            this.fft_fStartNum.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.fft_fStartNum.Location = new System.Drawing.Point(436, 47);
            this.fft_fStartNum.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.fft_fStartNum.Name = "fft_fStartNum";
            this.fft_fStartNum.Size = new System.Drawing.Size(90, 22);
            this.fft_fStartNum.TabIndex = 5;
            // 
            // fourierCheckbox
            // 
            this.fourierCheckbox.AutoSize = true;
            this.fourierCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.fourierCheckbox.Location = new System.Drawing.Point(470, 244);
            this.fourierCheckbox.Name = "fourierCheckbox";
            this.fourierCheckbox.Size = new System.Drawing.Size(56, 18);
            this.fourierCheckbox.TabIndex = 0;
            this.fourierCheckbox.Text = "Build";
            this.fourierCheckbox.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Silver;
            this.tabPage2.Controls.Add(this.lyap_log_text);
            this.tabPage2.Controls.Add(this.le_jakobian_radio);
            this.tabPage2.Controls.Add(this.chartLyapunov);
            this.tabPage2.Controls.Add(this.label19);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.lyapunovRedrawBtn);
            this.tabPage2.Controls.Add(this.lyap_calc_Rad_kantz);
            this.tabPage2.Controls.Add(this.le_scaleMaxNum);
            this.tabPage2.Controls.Add(this.label17);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.le_pStartNum);
            this.tabPage2.Controls.Add(this.le_pEndNum);
            this.tabPage2.Controls.Add(this.startBtn);
            this.tabPage2.Controls.Add(this.le_scaleMinNum);
            this.tabPage2.Controls.Add(this.label18);
            this.tabPage2.Controls.Add(this.lyap_calc_Rad_rosenstein);
            this.tabPage2.Controls.Add(this.le_resultText);
            this.tabPage2.Controls.Add(this.le_tauNum);
            this.tabPage2.Controls.Add(this.dimLbl);
            this.tabPage2.Controls.Add(this.tauLbl);
            this.tabPage2.Controls.Add(this.lyap_r_Grp);
            this.tabPage2.Controls.Add(this.le_dimNum);
            this.tabPage2.Controls.Add(this.le_wolf_radio);
            this.tabPage2.Controls.Add(this.lyap_w_Grp);
            this.tabPage2.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(898, 585);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Lyapunov exponents";
            // 
            // le_jakobian_radio
            // 
            this.le_jakobian_radio.AutoSize = true;
            this.le_jakobian_radio.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_jakobian_radio.Location = new System.Drawing.Point(176, 6);
            this.le_jakobian_radio.Name = "le_jakobian_radio";
            this.le_jakobian_radio.Size = new System.Drawing.Size(80, 18);
            this.le_jakobian_radio.TabIndex = 27;
            this.le_jakobian_radio.Text = "Jakobian";
            this.le_jakobian_radio.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label19.Location = new System.Drawing.Point(6, 231);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(174, 14);
            this.label19.TabIndex = 25;
            this.label19.Text = "Lyapunov exponent slope:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(135, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 14);
            this.label4.TabIndex = 14;
            this.label4.Text = "Max scale:";
            // 
            // lyapunovRedrawBtn
            // 
            this.lyapunovRedrawBtn.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lyapunovRedrawBtn.Location = new System.Drawing.Point(526, 227);
            this.lyapunovRedrawBtn.Name = "lyapunovRedrawBtn";
            this.lyapunovRedrawBtn.Size = new System.Drawing.Size(100, 23);
            this.lyapunovRedrawBtn.TabIndex = 14;
            this.lyapunovRedrawBtn.Text = "Adjust";
            this.lyapunovRedrawBtn.UseVisualStyleBackColor = true;
            this.lyapunovRedrawBtn.Click += new System.EventHandler(this.lyapunovRedrawBtn_Click);
            // 
            // lyap_calc_Rad_kantz
            // 
            this.lyap_calc_Rad_kantz.AutoSize = true;
            this.lyap_calc_Rad_kantz.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lyap_calc_Rad_kantz.Location = new System.Drawing.Point(109, 6);
            this.lyap_calc_Rad_kantz.Name = "lyap_calc_Rad_kantz";
            this.lyap_calc_Rad_kantz.Size = new System.Drawing.Size(61, 18);
            this.lyap_calc_Rad_kantz.TabIndex = 16;
            this.lyap_calc_Rad_kantz.Text = "Kantz";
            this.lyap_calc_Rad_kantz.UseVisualStyleBackColor = true;
            // 
            // le_scaleMaxNum
            // 
            this.le_scaleMaxNum.DecimalPlaces = 7;
            this.le_scaleMaxNum.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_scaleMaxNum.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.le_scaleMaxNum.Location = new System.Drawing.Point(214, 129);
            this.le_scaleMaxNum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.le_scaleMaxNum.Name = "le_scaleMaxNum";
            this.le_scaleMaxNum.Size = new System.Drawing.Size(100, 22);
            this.le_scaleMaxNum.TabIndex = 11;
            this.le_scaleMaxNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label17.Location = new System.Drawing.Point(236, 230);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(40, 14);
            this.label17.TabIndex = 6;
            this.label17.Text = "from:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(139, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 14);
            this.label3.TabIndex = 12;
            this.label3.Text = "Min scale:";
            // 
            // le_pStartNum
            // 
            this.le_pStartNum.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_pStartNum.Location = new System.Drawing.Point(282, 227);
            this.le_pStartNum.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.le_pStartNum.Name = "le_pStartNum";
            this.le_pStartNum.Size = new System.Drawing.Size(100, 22);
            this.le_pStartNum.TabIndex = 8;
            // 
            // le_pEndNum
            // 
            this.le_pEndNum.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_pEndNum.Location = new System.Drawing.Point(420, 227);
            this.le_pEndNum.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.le_pEndNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.le_pEndNum.Name = "le_pEndNum";
            this.le_pEndNum.Size = new System.Drawing.Size(100, 22);
            this.le_pEndNum.TabIndex = 9;
            this.le_pEndNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lyap_k_Grp
            // 
            this.lyap_k_Grp.Controls.Add(this.le_kantz_slopeCombo);
            this.lyap_k_Grp.Controls.Add(this.lyap_k_Lbl_scales);
            this.lyap_k_Grp.Controls.Add(this.le_kantz_scalesNum);
            this.lyap_k_Grp.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lyap_k_Grp.Location = new System.Drawing.Point(6, 105);
            this.lyap_k_Grp.Name = "lyap_k_Grp";
            this.lyap_k_Grp.Size = new System.Drawing.Size(243, 84);
            this.lyap_k_Grp.TabIndex = 20;
            this.lyap_k_Grp.TabStop = false;
            this.lyap_k_Grp.Text = "Kantz";
            // 
            // le_kantz_slopeCombo
            // 
            this.le_kantz_slopeCombo.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_kantz_slopeCombo.FormattingEnabled = true;
            this.le_kantz_slopeCombo.Location = new System.Drawing.Point(60, 48);
            this.le_kantz_slopeCombo.Name = "le_kantz_slopeCombo";
            this.le_kantz_slopeCombo.Size = new System.Drawing.Size(177, 22);
            this.le_kantz_slopeCombo.TabIndex = 30;
            // 
            // lyap_k_Lbl_scales
            // 
            this.lyap_k_Lbl_scales.AutoSize = true;
            this.lyap_k_Lbl_scales.Location = new System.Drawing.Point(79, 23);
            this.lyap_k_Lbl_scales.Name = "lyap_k_Lbl_scales";
            this.lyap_k_Lbl_scales.Size = new System.Drawing.Size(52, 14);
            this.lyap_k_Lbl_scales.TabIndex = 16;
            this.lyap_k_Lbl_scales.Text = "Scales:";
            // 
            // le_kantz_scalesNum
            // 
            this.le_kantz_scalesNum.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_kantz_scalesNum.Location = new System.Drawing.Point(137, 21);
            this.le_kantz_scalesNum.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.le_kantz_scalesNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.le_kantz_scalesNum.Name = "le_kantz_scalesNum";
            this.le_kantz_scalesNum.Size = new System.Drawing.Size(100, 22);
            this.le_kantz_scalesNum.TabIndex = 12;
            this.le_kantz_scalesNum.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // startBtn
            // 
            this.startBtn.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.startBtn.Location = new System.Drawing.Point(792, 229);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(100, 23);
            this.startBtn.TabIndex = 13;
            this.startBtn.Text = "Calculate";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // le_scaleMinNum
            // 
            this.le_scaleMinNum.DecimalPlaces = 7;
            this.le_scaleMinNum.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_scaleMinNum.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.le_scaleMinNum.Location = new System.Drawing.Point(214, 102);
            this.le_scaleMinNum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.le_scaleMinNum.Name = "le_scaleMinNum";
            this.le_scaleMinNum.Size = new System.Drawing.Size(100, 22);
            this.le_scaleMinNum.TabIndex = 10;
            this.le_scaleMinNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label18.Location = new System.Drawing.Point(389, 229);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(25, 14);
            this.label18.TabIndex = 7;
            this.label18.Text = "to:";
            // 
            // lyap_calc_Rad_rosenstein
            // 
            this.lyap_calc_Rad_rosenstein.AutoSize = true;
            this.lyap_calc_Rad_rosenstein.Checked = true;
            this.lyap_calc_Rad_rosenstein.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lyap_calc_Rad_rosenstein.Location = new System.Drawing.Point(8, 6);
            this.lyap_calc_Rad_rosenstein.Name = "lyap_calc_Rad_rosenstein";
            this.lyap_calc_Rad_rosenstein.Size = new System.Drawing.Size(95, 18);
            this.lyap_calc_Rad_rosenstein.TabIndex = 15;
            this.lyap_calc_Rad_rosenstein.TabStop = true;
            this.lyap_calc_Rad_rosenstein.Text = "Rosenstein";
            this.lyap_calc_Rad_rosenstein.UseVisualStyleBackColor = true;
            // 
            // le_resultText
            // 
            this.le_resultText.BackColor = System.Drawing.Color.Khaki;
            this.le_resultText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.le_resultText.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_resultText.Location = new System.Drawing.Point(632, 256);
            this.le_resultText.MaxLength = 100;
            this.le_resultText.Name = "le_resultText";
            this.le_resultText.ReadOnly = true;
            this.le_resultText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.le_resultText.ShowSelectionMargin = true;
            this.le_resultText.Size = new System.Drawing.Size(260, 58);
            this.le_resultText.TabIndex = 4;
            this.le_resultText.Text = "";
            // 
            // le_tauNum
            // 
            this.le_tauNum.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_tauNum.Location = new System.Drawing.Point(214, 75);
            this.le_tauNum.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.le_tauNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.le_tauNum.Name = "le_tauNum";
            this.le_tauNum.Size = new System.Drawing.Size(100, 22);
            this.le_tauNum.TabIndex = 9;
            this.le_tauNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // dimLbl
            // 
            this.dimLbl.AutoSize = true;
            this.dimLbl.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dimLbl.Location = new System.Drawing.Point(58, 50);
            this.dimLbl.Name = "dimLbl";
            this.dimLbl.Size = new System.Drawing.Size(150, 14);
            this.dimLbl.TabIndex = 6;
            this.dimLbl.Text = "Embedding dimension:";
            // 
            // tauLbl
            // 
            this.tauLbl.AutoSize = true;
            this.tauLbl.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tauLbl.Location = new System.Drawing.Point(65, 78);
            this.tauLbl.Name = "tauLbl";
            this.tauLbl.Size = new System.Drawing.Size(143, 14);
            this.tauLbl.TabIndex = 7;
            this.tauLbl.Text = "Reconstruction delay:";
            // 
            // lyap_r_Grp
            // 
            this.lyap_r_Grp.Controls.Add(this.le_ros_stepsNum);
            this.lyap_r_Grp.Controls.Add(this.rosStepsLbl);
            this.lyap_r_Grp.Controls.Add(this.rosDistanceLbl);
            this.lyap_r_Grp.Controls.Add(this.le_ros_distanceNum);
            this.lyap_r_Grp.Controls.Add(this.lyap_k_Grp);
            this.lyap_r_Grp.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lyap_r_Grp.Location = new System.Drawing.Point(366, 6);
            this.lyap_r_Grp.Name = "lyap_r_Grp";
            this.lyap_r_Grp.Size = new System.Drawing.Size(260, 195);
            this.lyap_r_Grp.TabIndex = 20;
            this.lyap_r_Grp.TabStop = false;
            this.lyap_r_Grp.Text = "Rosenstein/Kantz";
            // 
            // le_ros_stepsNum
            // 
            this.le_ros_stepsNum.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_ros_stepsNum.Location = new System.Drawing.Point(149, 21);
            this.le_ros_stepsNum.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.le_ros_stepsNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.le_ros_stepsNum.Name = "le_ros_stepsNum";
            this.le_ros_stepsNum.Size = new System.Drawing.Size(100, 22);
            this.le_ros_stepsNum.TabIndex = 19;
            this.le_ros_stepsNum.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // rosStepsLbl
            // 
            this.rosStepsLbl.AutoSize = true;
            this.rosStepsLbl.Location = new System.Drawing.Point(69, 23);
            this.rosStepsLbl.Name = "rosStepsLbl";
            this.rosStepsLbl.Size = new System.Drawing.Size(74, 14);
            this.rosStepsLbl.TabIndex = 18;
            this.rosStepsLbl.Text = "Iterations:";
            // 
            // rosDistanceLbl
            // 
            this.rosDistanceLbl.AutoSize = true;
            this.rosDistanceLbl.Location = new System.Drawing.Point(36, 50);
            this.rosDistanceLbl.Name = "rosDistanceLbl";
            this.rosDistanceLbl.Size = new System.Drawing.Size(107, 14);
            this.rosDistanceLbl.TabIndex = 16;
            this.rosDistanceLbl.Text = "Theiler window:";
            // 
            // le_ros_distanceNum
            // 
            this.le_ros_distanceNum.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_ros_distanceNum.Location = new System.Drawing.Point(149, 48);
            this.le_ros_distanceNum.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.le_ros_distanceNum.Name = "le_ros_distanceNum";
            this.le_ros_distanceNum.Size = new System.Drawing.Size(100, 22);
            this.le_ros_distanceNum.TabIndex = 12;
            // 
            // le_dimNum
            // 
            this.le_dimNum.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_dimNum.Location = new System.Drawing.Point(214, 48);
            this.le_dimNum.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.le_dimNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.le_dimNum.Name = "le_dimNum";
            this.le_dimNum.Size = new System.Drawing.Size(100, 22);
            this.le_dimNum.TabIndex = 8;
            this.le_dimNum.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // le_wolf_radio
            // 
            this.le_wolf_radio.AutoSize = true;
            this.le_wolf_radio.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_wolf_radio.Location = new System.Drawing.Point(262, 6);
            this.le_wolf_radio.Name = "le_wolf_radio";
            this.le_wolf_radio.Size = new System.Drawing.Size(52, 18);
            this.le_wolf_radio.TabIndex = 14;
            this.le_wolf_radio.Text = "Wolf";
            this.le_wolf_radio.UseVisualStyleBackColor = true;
            // 
            // lyap_w_Grp
            // 
            this.lyap_w_Grp.Controls.Add(this.le_wolf_stepNum);
            this.lyap_w_Grp.Controls.Add(this.stepLbl);
            this.lyap_w_Grp.Controls.Add(this.label5);
            this.lyap_w_Grp.Controls.Add(this.le_wolf_evolveStepsNum);
            this.lyap_w_Grp.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lyap_w_Grp.Location = new System.Drawing.Point(632, 6);
            this.lyap_w_Grp.Name = "lyap_w_Grp";
            this.lyap_w_Grp.Size = new System.Drawing.Size(260, 86);
            this.lyap_w_Grp.TabIndex = 12;
            this.lyap_w_Grp.TabStop = false;
            this.lyap_w_Grp.Text = "Wolf";
            // 
            // le_wolf_stepNum
            // 
            this.le_wolf_stepNum.DecimalPlaces = 7;
            this.le_wolf_stepNum.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_wolf_stepNum.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.le_wolf_stepNum.Location = new System.Drawing.Point(149, 21);
            this.le_wolf_stepNum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.le_wolf_stepNum.Name = "le_wolf_stepNum";
            this.le_wolf_stepNum.Size = new System.Drawing.Size(100, 22);
            this.le_wolf_stepNum.TabIndex = 19;
            this.le_wolf_stepNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // stepLbl
            // 
            this.stepLbl.AutoSize = true;
            this.stepLbl.Location = new System.Drawing.Point(118, 23);
            this.stepLbl.Name = "stepLbl";
            this.stepLbl.Size = new System.Drawing.Size(25, 14);
            this.stepLbl.TabIndex = 18;
            this.stepLbl.Text = "Δt:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 14);
            this.label5.TabIndex = 16;
            this.label5.Text = "Evolution step size:";
            // 
            // le_wolf_evolveStepsNum
            // 
            this.le_wolf_evolveStepsNum.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.le_wolf_evolveStepsNum.Location = new System.Drawing.Point(149, 48);
            this.le_wolf_evolveStepsNum.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.le_wolf_evolveStepsNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.le_wolf_evolveStepsNum.Name = "le_wolf_evolveStepsNum";
            this.le_wolf_evolveStepsNum.Size = new System.Drawing.Size(100, 22);
            this.le_wolf_evolveStepsNum.TabIndex = 12;
            this.le_wolf_evolveStepsNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // sourcePrefGBox
            // 
            this.sourcePrefGBox.Controls.Add(this.sourceStepLbl);
            this.sourcePrefGBox.Controls.Add(this.sourceStepTxt);
            this.sourcePrefGBox.Controls.Add(this.useTimeCheckbox);
            this.sourcePrefGBox.Controls.Add(this.label7);
            this.sourcePrefGBox.Controls.Add(this.label6);
            this.sourcePrefGBox.Controls.Add(this.endPointNum);
            this.sourcePrefGBox.Controls.Add(this.startPointNum);
            this.sourcePrefGBox.Controls.Add(this.label2);
            this.sourcePrefGBox.Controls.Add(this.label1);
            this.sourcePrefGBox.Controls.Add(this.pointsNum);
            this.sourcePrefGBox.Controls.Add(this.sourceColumnNum);
            this.sourcePrefGBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.sourcePrefGBox.Location = new System.Drawing.Point(912, 82);
            this.sourcePrefGBox.Name = "sourcePrefGBox";
            this.sourcePrefGBox.Size = new System.Drawing.Size(236, 202);
            this.sourcePrefGBox.TabIndex = 19;
            this.sourcePrefGBox.TabStop = false;
            this.sourcePrefGBox.Text = "Signal settings";
            // 
            // sourceStepLbl
            // 
            this.sourceStepLbl.AutoSize = true;
            this.sourceStepLbl.Location = new System.Drawing.Point(105, 51);
            this.sourceStepLbl.Name = "sourceStepLbl";
            this.sourceStepLbl.Size = new System.Drawing.Size(25, 14);
            this.sourceStepLbl.TabIndex = 24;
            this.sourceStepLbl.Text = "Δt:";
            // 
            // sourceStepTxt
            // 
            this.sourceStepTxt.BackColor = System.Drawing.Color.Khaki;
            this.sourceStepTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sourceStepTxt.Location = new System.Drawing.Point(135, 49);
            this.sourceStepTxt.MaxLength = 20;
            this.sourceStepTxt.Name = "sourceStepTxt";
            this.sourceStepTxt.ReadOnly = true;
            this.sourceStepTxt.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.sourceStepTxt.ShowSelectionMargin = true;
            this.sourceStepTxt.Size = new System.Drawing.Size(90, 21);
            this.sourceStepTxt.TabIndex = 23;
            this.sourceStepTxt.Text = "";
            // 
            // useTimeCheckbox
            // 
            this.useTimeCheckbox.AutoSize = true;
            this.useTimeCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.useTimeCheckbox.Location = new System.Drawing.Point(6, 26);
            this.useTimeCheckbox.Name = "useTimeCheckbox";
            this.useTimeCheckbox.Size = new System.Drawing.Size(142, 18);
            this.useTimeCheckbox.TabIndex = 1;
            this.useTimeCheckbox.Text = "Time in 1st column";
            this.useTimeCheckbox.UseVisualStyleBackColor = true;
            this.useTimeCheckbox.CheckedChanged += new System.EventHandler(this.useTimeCheckbox_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(59, 136);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 14);
            this.label7.TabIndex = 22;
            this.label7.Text = "End point:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(52, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 14);
            this.label6.TabIndex = 21;
            this.label6.Text = "Start point:";
            // 
            // endPointNum
            // 
            this.endPointNum.Location = new System.Drawing.Point(135, 134);
            this.endPointNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.endPointNum.Name = "endPointNum";
            this.endPointNum.Size = new System.Drawing.Size(90, 22);
            this.endPointNum.TabIndex = 4;
            this.endPointNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // startPointNum
            // 
            this.startPointNum.Location = new System.Drawing.Point(135, 105);
            this.startPointNum.Name = "startPointNum";
            this.startPointNum.Size = new System.Drawing.Size(90, 22);
            this.startPointNum.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 14);
            this.label2.TabIndex = 19;
            this.label2.Text = "Each N points:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 14);
            this.label1.TabIndex = 18;
            this.label1.Text = "SIgnal column #:";
            // 
            // pointsNum
            // 
            this.pointsNum.Location = new System.Drawing.Point(135, 163);
            this.pointsNum.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.pointsNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pointsNum.Name = "pointsNum";
            this.pointsNum.Size = new System.Drawing.Size(90, 22);
            this.pointsNum.TabIndex = 5;
            this.pointsNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pointsNum.ValueChanged += new System.EventHandler(this.pointsNum_ValueChanged);
            // 
            // sourceColumnNum
            // 
            this.sourceColumnNum.Location = new System.Drawing.Point(135, 76);
            this.sourceColumnNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.sourceColumnNum.Name = "sourceColumnNum";
            this.sourceColumnNum.Size = new System.Drawing.Size(90, 22);
            this.sourceColumnNum.TabIndex = 2;
            this.sourceColumnNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(941, 349);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(101, 14);
            this.label20.TabIndex = 26;
            this.label20.Text = "Preview width:";
            // 
            // numPreviewWidth
            // 
            this.numPreviewWidth.Location = new System.Drawing.Point(1047, 347);
            this.numPreviewWidth.Maximum = new decimal(new int[] {
            1920,
            0,
            0,
            0});
            this.numPreviewWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPreviewWidth.Name = "numPreviewWidth";
            this.numPreviewWidth.Size = new System.Drawing.Size(90, 22);
            this.numPreviewWidth.TabIndex = 23;
            this.numPreviewWidth.Value = new decimal(new int[] {
            215,
            0,
            0,
            0});
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(935, 377);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(107, 14);
            this.label21.TabIndex = 25;
            this.label21.Text = "Preview Height:";
            // 
            // numPreviewHeight
            // 
            this.numPreviewHeight.Location = new System.Drawing.Point(1047, 375);
            this.numPreviewHeight.Maximum = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            this.numPreviewHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPreviewHeight.Name = "numPreviewHeight";
            this.numPreviewHeight.Size = new System.Drawing.Size(90, 22);
            this.numPreviewHeight.TabIndex = 24;
            this.numPreviewHeight.Value = new decimal(new int[] {
            160,
            0,
            0,
            0});
            // 
            // lyap_log_text
            // 
            this.lyap_log_text.BackColor = System.Drawing.Color.Khaki;
            this.lyap_log_text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lyap_log_text.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lyap_log_text.Location = new System.Drawing.Point(632, 320);
            this.lyap_log_text.MaxLength = 100;
            this.lyap_log_text.Name = "lyap_log_text";
            this.lyap_log_text.ReadOnly = true;
            this.lyap_log_text.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.lyap_log_text.ShowSelectionMargin = true;
            this.lyap_log_text.Size = new System.Drawing.Size(260, 256);
            this.lyap_log_text.TabIndex = 28;
            this.lyap_log_text.Text = "";
            // 
            // chartPoincare
            // 
            this.chartPoincare.Location = new System.Drawing.Point(9, 22);
            this.chartPoincare.Name = "chartPoincare";
            this.chartPoincare.Size = new System.Drawing.Size(320, 239);
            this.chartPoincare.TabIndex = 13;
            this.chartPoincare.Text = "chart1";
            this.chartPoincare.DoubleClick += new System.EventHandler(this.chartPoincare_DoubleClick);
            // 
            // chartSignal
            // 
            this.chartSignal.Location = new System.Drawing.Point(9, 21);
            this.chartSignal.Name = "chartSignal";
            this.chartSignal.Size = new System.Drawing.Size(320, 240);
            this.chartSignal.TabIndex = 0;
            this.chartSignal.Text = "mathChart1";
            this.chartSignal.DoubleClick += new System.EventHandler(this.chartSignal_DoubleClick);
            // 
            // chartFft
            // 
            this.chartFft.Location = new System.Drawing.Point(6, 22);
            this.chartFft.Name = "chartFft";
            this.chartFft.Size = new System.Drawing.Size(320, 239);
            this.chartFft.TabIndex = 13;
            this.chartFft.Text = "chart1";
            this.chartFft.DoubleClick += new System.EventHandler(this.chartFft_DoubleClick);
            // 
            // chartLyapunov
            // 
            this.chartLyapunov.Location = new System.Drawing.Point(8, 256);
            this.chartLyapunov.Name = "chartLyapunov";
            this.chartLyapunov.Size = new System.Drawing.Size(618, 320);
            this.chartLyapunov.TabIndex = 26;
            this.chartLyapunov.Text = "chart1";
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.ClientSize = new System.Drawing.Size(1160, 635);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.numPreviewWidth);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.numPreviewHeight);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.fileNameLbl);
            this.Controls.Add(this.sourcePrefGBox);
            this.Controls.Add(this.openFileBtn);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Timeseries analyser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.waveletGroup.ResumeLayout(false);
            this.waveletGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wav_plotPBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wav_dtNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wav_fEndNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wav_fStartNum)).EndInit();
            this.fourierGroup.ResumeLayout(false);
            this.fourierGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fft_dtNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fft_fEndNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fft_fStartNum)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.le_scaleMaxNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_pStartNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_pEndNum)).EndInit();
            this.lyap_k_Grp.ResumeLayout(false);
            this.lyap_k_Grp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.le_kantz_scalesNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_scaleMinNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_tauNum)).EndInit();
            this.lyap_r_Grp.ResumeLayout(false);
            this.lyap_r_Grp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.le_ros_stepsNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_ros_distanceNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_dimNum)).EndInit();
            this.lyap_w_Grp.ResumeLayout(false);
            this.lyap_w_Grp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.le_wolf_stepNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.le_wolf_evolveStepsNum)).EndInit();
            this.sourcePrefGBox.ResumeLayout(false);
            this.sourcePrefGBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endPointNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startPointNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointsNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceColumnNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPreviewWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPreviewHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartPoincare)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSignal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartFft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLyapunov)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button openFileBtn;
        private Button plotBtn;
        private Label fileNameLbl;
        private Button saveBtn;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private GroupBox fourierGroup;
        private NumericUpDown fft_fEndNum;
        private NumericUpDown fft_fStartNum;
        private CheckBox fourierCheckbox;
        private GroupBox sourcePrefGBox;
        private CheckBox useTimeCheckbox;
        private Label label7;
        private Label label6;
        private NumericUpDown endPointNum;
        private NumericUpDown startPointNum;
        private Label label2;
        private Label label1;
        private NumericUpDown pointsNum;
        private NumericUpDown sourceColumnNum;
        private TabPage tabPage2;
        private GroupBox lyap_w_Grp;
        private Label label5;
        private NumericUpDown le_wolf_evolveStepsNum;
        private Label label4;
        private NumericUpDown le_scaleMaxNum;
        private Label label3;
        private NumericUpDown le_scaleMinNum;
        private RichTextBox le_resultText;
        private Button startBtn;
        private Label dimLbl;
        private NumericUpDown le_dimNum;
        private NumericUpDown le_tauNum;
        private Label tauLbl;
        private Label label9;
        private Label label8;
        private GroupBox waveletGroup;
        private Label waveletNameLbl;
        private CheckBox waveletCheckbox;
        private PictureBox wav_plotPBox;
        private Button lyapunovRedrawBtn;
        private Label label17;
        private NumericUpDown le_pStartNum;
        private NumericUpDown le_pEndNum;
        private Label label18;
        private NumericUpDown wav_fEndNum;
        private NumericUpDown wav_fStartNum;
        private Label label15;
        private Label label14;
        private Label label16;
        private NumericUpDown le_wolf_stepNum;
        private Label stepLbl;
        private GroupBox lyap_r_Grp;
        private NumericUpDown le_ros_stepsNum;
        private Label rosStepsLbl;
        private Label rosDistanceLbl;
        private NumericUpDown le_ros_distanceNum;
        private RadioButton lyap_calc_Rad_rosenstein;
        private RadioButton le_wolf_radio;
        private Label sourceStepLbl;
        private RichTextBox sourceStepTxt;
        private GroupBox lyap_k_Grp;
        private Label lyap_k_Lbl_scales;
        private NumericUpDown le_kantz_scalesNum;
        private RadioButton lyap_calc_Rad_kantz;
        private NumericUpDown fft_dtNum;
        private CheckBox fft_logCbox;
        private NumericUpDown wav_dtNum;
        private Label label11;
        private ComboBox wav_nameCombo;
        private ComboBox le_kantz_slopeCombo;
        private Label label19;
        private Label label10;
        private ComboBox wav_colorMapCombo;
        private Label label20;
        private NumericUpDown numPreviewWidth;
        private Label label21;
        private NumericUpDown numPreviewHeight;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private MathChart chartPoincare;
        private MathChart chartFft;
        private MathChart chartLyapunov;
        private MathChart chartSignal;
        private RadioButton le_jakobian_radio;
        private RichTextBox lyap_log_text;
    }
}

