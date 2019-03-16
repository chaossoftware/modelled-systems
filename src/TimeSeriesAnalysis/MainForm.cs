﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using MathLib.IO;
using MathLib.MathMethods.Lyapunov;
using System.Globalization;
using MathLib.Data;

namespace TimeSeriesAnalysis {
    public partial class MainForm : Form
    {
        private Routines routines = new Routines();

        public MainForm()
        {
            InitializeComponent();
        }

        //File open and read
        private void openFileBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files|*.*|Time series data|*.dat *.txt *.csv";
            openFileDialog.ShowDialog();
            string fName = openFileDialog.FileName;

            if (string.IsNullOrEmpty(fName))
                return;

            CleanUp();

            try
            {
                routines.sourceData = new SourceData(fName);
                FillUiWithData();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Unable to read file file:" + ex.Message);
            }
        }

        //Perform calculation in a separate thread
        private void startBtn_Click(object sender, EventArgs e)
        {
            if (RefreshTimeSeries())
            {
                le_resultText.Text = StringData.Calculating;
                le_resultText.BackColor = Color.OrangeRed;

                new Thread(CalculateLyapunovExponent)
                    .Start();
            }
        }

        private bool RefreshTimeSeries()
        {
            if (routines.sourceData == null)
            {
                MessageBox.Show(StringData.MsgEmptyFile);
                return false;
            }

            routines.sourceData.SetTimeSeries(
                sourceColumnNum.ToInt() - 1,
                startPointNum.ToInt(),
                endPointNum.ToInt(),
                pointsNum.ToInt(),
                useTimeCheckbox.Checked
            );

            return true;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (routines.sourceData == null)
            {
                MessageBox.Show(StringData.MsgEmptyFile);
                return;
            }

            var outDir = Path.Combine(routines.sourceData.Folder, routines.sourceData.FileName + "_rez");
            string fName = Path.Combine(outDir, routines.sourceData.FileName);

            if (!Directory.Exists(outDir))
                Directory.CreateDirectory(outDir);

            if (signal_plotPBox.Image != null && poincareMapPBox.Image != null)
            {
                signal_plotPBox.Image.Save(fName + "_plot.png", ImageFormat.Png);
                poincareMapPBox.Image.Save(fName + "_poincare.png", ImageFormat.Png);
                DataWriter.CreateDataFile(fName + "_signal", routines.sourceData.GetTimeSeriesString(false));
            }

            if (fft_plotPBox.Image != null)
                fft_plotPBox.Image.Save(fName + "_fourier.png", ImageFormat.Png);

            if (wav_plotPBox.Image != null)
                wav_plotPBox.Image.Save(fName + "_wavelet.png", ImageFormat.Png);

            if (routines.lyapunov != null)
                DataWriter.CreateDataFile(fName + "_lyapunov.txt", routines.lyapunov.GetInfoFull());

            if (le_plotPBox.Image != null)
                le_plotPBox.Image.Save(fName + "_lyapunovInTime.png", ImageFormat.Png);
        }

        private void FillUiWithData()
        {
            fileNameLbl.Text = routines.sourceData.ToString().Replace("\n", " ");

            sourceColumnNum.Maximum = routines.sourceData.ColumnsCount;
            sourceColumnNum.Minimum = 1;

            startPointNum.Maximum = routines.sourceData.Length - 1;

            endPointNum.Maximum = routines.sourceData.Length;
            endPointNum.Value = routines.sourceData.Length;
        }

        #region "CHARTS"

        private void plotBtn_Click(object sender, EventArgs e)
        {
            if (!RefreshTimeSeries())
                return;

            routines.DeleteTempFiles();

            signal_plotPBox.Image = routines
                .GetSignalPlot(signal_plotPBox.Size, 1, useTimeCheckbox.Checked, startPointNum.ToInt(), endPointNum.ToInt())
                .Plot();

            poincareMapPBox.Image = routines
                .GetPoincarePlot(poincareMapPBox.Size, 1)
                .Plot();

            if (fourierCheckbox.Checked)
            {
                try
                {
                    fft_plotPBox.Image = routines.GetFourierPlot(
                        signal_plotPBox.Size, 
                        1, 
                        fft_fStartNum.ToDouble(), 
                        fft_fEndNum.ToDouble(), 
                        fft_dtNum.ToDouble(), 
                        fft_logCbox.Checked)
                        .Plot();
                }
                catch (Exception ex)
                {
                    fft_plotPBox.Image = null;
                    MessageBox.Show($"Unable to build {StringData.Fourier}:\n" + ex.Message);
                }
            }

            if (waveletCheckbox.Checked)
            {
                wav_plotPBox.Image = null;
                double tStart = routines.sourceData.TimeSeries.Min.X;
                double tEnd = routines.sourceData.TimeSeries.Max.X;

                try
                {
                    routines.BuildWavelet(StringData.WaveletFile, wav_nameCombo.Text, tStart, tEnd, wav_fStartNum.ToDouble(), wav_fEndNum.ToDouble(), wav_dtNum.ToDouble(), wav_colorMapCombo.Text, wav_plotPBox.Width, wav_plotPBox.Height);
                    GetImageFromFile(wav_plotPBox, StringData.WaveletFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to build {StringData.Wavelet}:\n" + ex.Message);
                }
            }
        }

        private void lyapunovRedrawBtn_Click(object sender, EventArgs e) {
            try
            {

                string res;
                if (routines.lyapunov is KantzMethod)
                    ((KantzMethod)routines.lyapunov).SetSlope(this.le_kantz_slopeCombo.Text);

                le_plotPBox.Image = routines.GetLyapunovPlot(le_plotPBox.Size, 1, le_pStartNum.ToInt(), le_pEndNum.ToInt(), le_wolf_radio.Checked, out res).Plot();

                if (routines.lyapunov is KantzMethod || routines.lyapunov is RosensteinMethod)
                    le_resultText.Text = res;
            }
            catch (Exception ex) {
                MessageBox.Show("Error plotting Lyapunov slope: " + ex.Message); 
            }
        }

        private void poincareMapPBox_DoubleClick(object sender, EventArgs e)
        {
            if (poincareMapPBox.Image != null)
            {
                var pf = GetPreviewForm(StringData.Poincare);
                var pp = routines.GetPoincarePlot(pf.previewPBox.Size, 1);
                pf.previewPBox.Image = pp.Plot();
                pf.Plot = pp;
            }
        }

        private void signal_plotPBox_DoubleClick(object sender, EventArgs e)
        {
            if (signal_plotPBox.Image != null)
            {
                var pf = GetPreviewForm(StringData.Signal);

                var sp = routines.GetSignalPlot(
                    pf.previewPBox.Size, 
                    1, 
                    useTimeCheckbox.Checked, 
                    startPointNum.ToInt(), 
                    endPointNum.ToInt());

                pf.previewPBox.Image = sp.Plot();
                pf.Plot = sp;
            }
        }

        private void lyapunovPBox_DoubleClick(object sender, EventArgs e)
        {
            if (le_plotPBox.Image != null)
            {
                string tmp;
                var pf = GetPreviewForm(StringData.LeInTime);
                var sp = routines.GetLyapunovPlot(pf.previewPBox.Size, 1, le_pStartNum.ToInt(), le_pEndNum.ToInt(), le_wolf_radio.Checked, out tmp);
                pf.previewPBox.Image = sp.Plot();
                pf.Plot = sp;
            }
        }

        private void fft_plotPBox_DoubleClick(object sender, EventArgs e)
        {
            if (fft_plotPBox.Image != null)
            {
                var pf = GetPreviewForm(StringData.Fourier);
                var sp = routines.GetFourierPlot(pf.previewPBox.Size, 1, fft_fStartNum.ToDouble(), fft_fEndNum.ToDouble(), fft_dtNum.ToDouble(), fft_logCbox.Checked);
                pf.previewPBox.Image = sp.Plot();
                pf.Plot = sp;
            }
        }

        private void waveletPBox_DoubleClick(object sender, EventArgs e)
        {
            var pf = GetPreviewForm(this.wav_nameCombo.Text + " " + StringData.Wavelet);

            try
            {
                routines.BuildWavelet(
                    StringData.WaveletPreviewFile, 
                    wav_nameCombo.Text,
                    routines.sourceData.TimeSeries.Min.X,
                    routines.sourceData.TimeSeries.Max.X, 
                    wav_fStartNum.ToDouble(), 
                    wav_fEndNum.ToDouble(), 
                    wav_dtNum.ToDouble(), 
                    wav_colorMapCombo.Text, 
                    wav_plotPBox.Width, 
                    wav_plotPBox.Height);

                GetImageFromFile(pf.previewPBox, StringData.WaveletPreviewFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to build {StringData.Fourier}:\n" + ex.Message);
            }
        }

        private void GetImageFromFile(PictureBox pb, string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                pb.Image = Image.FromStream(fs);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        #endregion

        #region lyapunovRelated

        private void CalculateLyapunovExponent()
        {
            int dim = le_dimNum.ToInt();
            int tau = le_tauNum.ToInt();
            double scaleMin = le_scaleMinNum.ToDouble();

            if (le_wolf_radio.Checked)
                routines.lyapunov = new WolfMethod(
                    routines.sourceData.TimeSeries.YValues,
                    dim,
                    tau,
                    le_wolf_stepNum.ToDouble(),
                    scaleMin,
                    le_scaleMaxNum.ToDouble(), 
                    le_wolf_evolveStepsNum.ToInt()
                );

            if (lyap_calc_Rad_rosenstein.Checked)
                routines.lyapunov = new RosensteinMethod(
                    routines.sourceData.TimeSeries.YValues,
                    dim,
                    tau, 
                    le_ros_stepsNum.ToInt(), 
                    le_ros_distanceNum.ToInt(),
                    scaleMin
                );

            if (lyap_calc_Rad_kantz.Checked)
                routines.lyapunov = new KantzMethod(
                    routines.sourceData.TimeSeries.YValues,
                    dim,
                    tau,
                    le_kantz_maxIterNum.ToInt(),
                    le_kantz_windowNum.ToInt(),
                    scaleMin,
                    le_scaleMaxNum.ToDouble(),
                    le_kantz_scalesNum.ToInt()
                );

            try
            {
                routines.lyapunov.Calculate();
                var mi = new MethodInvoker(this.SetLyapunovResult);
                this.BeginInvoke(mi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to calculate LE:\n" + ex.Message);
                le_resultText.Text = "Error";
            }
        }

        private void SetLyapunovResult()
        {
            le_resultText.BackColor = Color.Khaki;
            string result = string.Empty;

            if (routines.lyapunov is WolfMethod)
            {
                result = string.Format("{0:F5}", ((WolfMethod)routines.lyapunov).rezult);
                le_resultText.Text = result;
            }

            if (routines.lyapunov is KantzMethod)
            {
                this.le_kantz_slopeCombo.Items.Clear();
                string[] items = new string[((KantzMethod)routines.lyapunov).SlopesList.Count];
                ((KantzMethod)routines.lyapunov).SlopesList.Keys.CopyTo(items, 0);
                this.le_kantz_slopeCombo.Items.AddRange(items);
                this.le_kantz_slopeCombo.SelectedIndex = 0;
                ((KantzMethod)routines.lyapunov).SetSlope(this.le_kantz_slopeCombo.Text);
            }

            if (routines.lyapunov.Slope.Length > 1)
            {
                le_resultText.Text = routines.lyapunov.GetInfoShort();
                le_pEndNum.Value = routines.lyapunov.Slope.Length - 1;

                try
                {
                    le_plotPBox.Image = routines.GetLyapunovPlot(
                        le_plotPBox.Size, 
                        1, 
                        le_pStartNum.ToInt(), 
                        le_pEndNum.ToInt(), 
                        le_wolf_radio.Checked, 
                        out result)
                        .Plot();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error plotting Lyapunov slope: " + ex.Message);
                    result = StringData.NoValue;
                }
            }
            else
            {
                result = StringData.NoValue;
            }
               
            if (routines.lyapunov is KantzMethod || routines.lyapunov is RosensteinMethod)
            {
                le_resultText.Text = result;
            }
        }

        #endregion

        private void useTimeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.useTimeCheckbox.Checked)
            {
                RefreshTimeSeries();
                sourceStepTxt.Text = string.Format(CultureInfo.InvariantCulture, "{0:F8}", routines.sourceData.Step);
            }
            else
            {
                sourceStepTxt.Text = string.Empty;
            }
        }

        private void CleanUp()
        {
            routines.sourceData = null;
            signal_plotPBox.Image = null;
            poincareMapPBox.Image = null;
            wav_plotPBox.Image = null;
            fft_plotPBox.Image = null;
            le_plotPBox.Image = null;
            routines.lyapunov = null;
            routines.DeleteTempFiles();
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e) =>
            CleanUp();

        private void pointsNum_ValueChanged(object sender, EventArgs e)
        {
            if (this.useTimeCheckbox.Checked)
            {
                RefreshTimeSeries();
                sourceStepTxt.Text = string.Format(CultureInfo.InvariantCulture, "{0:F8}", routines.sourceData.Step);
            }
        }

        private PreviewForm GetPreviewForm(string title)
        {
            var form = new PreviewForm(
                title,
                this.numPreviewWidth.ToInt(),
                this.numPreviewHeight.ToInt());

            form.Show();

            return form;
        }
    }
}
