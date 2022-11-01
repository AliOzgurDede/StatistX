/*
The MIT License(MIT)

Copyright(c) 2020 Ali Özgür Dede

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StatistX
{
    public partial class DescriptiveStatistics : Form
    {
        // definitions
        int N;
        double[] DataSet;
        double MinimumValue;
        double MaximumValue;
        double Range;
        double Mean;
        double StandartDeviation;
        double Z;
        bool DatasetNormal;
        double ChiSquareValue;
        double CriticalValue;
        int DegreeOfFreedom;

        Dictionary<double, double> ChiSquareTable = new Dictionary<double, double>();
        void GeneratingChiSquareTable()
        {
            // key=s.d. value=chisquare
            // significance degree = %1
            ChiSquareTable.Add(1, 6.635);
            ChiSquareTable.Add(2, 9.210);
            ChiSquareTable.Add(3, 11.345);
            ChiSquareTable.Add(4, 13.227);
            ChiSquareTable.Add(5, 15.086);
            ChiSquareTable.Add(6, 16.812);
            ChiSquareTable.Add(7, 18.475);
            ChiSquareTable.Add(8, 20.090);
            ChiSquareTable.Add(9, 21.666);
            ChiSquareTable.Add(10, 23.209);
            ChiSquareTable.Add(11, 24.725);
            ChiSquareTable.Add(12, 26.217);
            ChiSquareTable.Add(13, 27.688);
            ChiSquareTable.Add(14, 29.141);
            ChiSquareTable.Add(15, 30.578);
            ChiSquareTable.Add(16, 32.000);
            ChiSquareTable.Add(17, 33.409);
            ChiSquareTable.Add(18, 34.805);
            ChiSquareTable.Add(19, 36.191);
            ChiSquareTable.Add(20, 37.566);
            ChiSquareTable.Add(21, 38.932);
            ChiSquareTable.Add(22, 40.289);
            ChiSquareTable.Add(23, 41.638);
            ChiSquareTable.Add(24, 42.980);
            ChiSquareTable.Add(25, 44.314);
            ChiSquareTable.Add(26, 45.642);
            ChiSquareTable.Add(27, 46.963);
            ChiSquareTable.Add(28, 48.378);
            ChiSquareTable.Add(29, 49.588);
            ChiSquareTable.Add(30, 50.892);
        }

        void AddRowsToGrid()
        {
            int rows = Int32.Parse(textBox1.Text);

            for (int i = 0; i < rows - 1; i++)
            {
                dataGridView1.Rows.Add();
            }
        }

        void PasteFromClipboard()
        {
            string s = Clipboard.GetText();
            string[] lines = s.Split('\n');
            int row = dataGridView1.CurrentCell.RowIndex;
            int col = dataGridView1.CurrentCell.ColumnIndex;
            foreach (string line in lines)
            {
                string[] cells = line.Split('\t');
                int cellsSelected = cells.Length;
                if (row < dataGridView1.Rows.Count)
                {
                    for (int i = 0; i < cellsSelected; i++)
                    {
                        if (col + i < dataGridView1.Columns.Count)
                            dataGridView1[col + i, row].Value = cells[i];
                        else
                            break;
                    }
                    row++;
                }
                else
                {
                    break;
                }
            }
        }

        void TransferringData()
        {
            // determining the value of N
            N = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1[0, i].Value != null)
                {
                    N = N + 1;
                }
            }
            DataSet = new double[N];
            // filling data array from datagridview
            for (int i = 0; i < N; i++)
            {
                if (dataGridView1[0, i].Value != null)
                {
                    DataSet[i] = Double.Parse(dataGridView1[0, i].Value.ToString());
                }
            }
        }

        void PlottingHistogram()
        {
            // cleaning histogram
            this.Histogram.Series["Histogram"].Points.Clear();
            // sorting the data elements in increasing order
            Array.Sort(DataSet);
            // calculating range of data
            MaximumValue = DataSet[N - 1];
            MinimumValue = DataSet[0];
            Range = MaximumValue - MinimumValue;
            // determining number of data bins
            double d = Math.Sqrt(N);
            int numberOfBins = (int)d;
            // calculating bin width
            double binWidth = Range / numberOfBins;
            // determining bin intervals
            double[] binBorders = new double[numberOfBins];
            for (int i = 0; i < numberOfBins; i++)
            {
                if (i == 0)
                {
                    binBorders[i] = MinimumValue;
                }
                else
                {
                    binBorders[i] = binBorders[i - 1] + binWidth;
                }
            }
            // counting data frequency of each bin
            int[] binFrequency = new int[numberOfBins];
            for (int i = 0; i < numberOfBins; i++)
            {
                binFrequency[i] = 0;
            }
            for (int i = 0; i < N; i++)
            {
                for (int j = numberOfBins; j > 0; j--)
                {
                    if (DataSet[i] >= binBorders[j - 1])
                    {
                        binFrequency[j - 1] = binFrequency[j - 1] + 1;
                        break;
                    }
                }
            }
            // visualizing histogram columns
            for (int i = 0; i < numberOfBins; i++)
            {
                this.Histogram.Series["Histogram"].Points.AddXY(binBorders[i], binFrequency[i]);
            }
        }

        void CalculatingParameters()
        {
            // calculating mean
            Mean = 0;
            double Total = 0;
            for (int i = 0; i < N; i++)
            {
                Total = Total + DataSet[i];
            }
            Mean = Total / N;
            Mean = Math.Round(Mean, 2);
            label11.Text = Mean.ToString();

            // calculating standart deviation
            StandartDeviation = 0;
            double SumOfSquares = 0;
            for (int i = 0; i < N; i++)
            {
                SumOfSquares = SumOfSquares + Math.Pow(DataSet[i] - Mean, 2);
            }
            StandartDeviation = Math.Sqrt(SumOfSquares / N);
            StandartDeviation = Math.Round(StandartDeviation, 2);
            label9.Text = StandartDeviation.ToString();

            // range and size
            label10.Text = Range.ToString();
            label7.Text = N.ToString();

            // dsitribution test
            TestingNormalDistribution();
            if (DatasetNormal == true)
            {
                label8.Text = "Normal";
            }
            else
            {
                label8.Text = "Not Normal";
            }
        }

        void TestingNormalDistribution()
        {
            // defining test parameters
            DegreeOfFreedom = 5;
            CriticalValue = ChiSquareTable[DegreeOfFreedom];
            ChiSquareValue = 0;
            // calculating expected frequencies
            double[] ExpectedFrequency = new double[6];
            ExpectedFrequency[0] = 2.1 / 100 * N;
            ExpectedFrequency[1] = 13.6 / 100 * N;
            ExpectedFrequency[2] = 34.1 / 100 * N;
            ExpectedFrequency[3] = 34.1 / 100 * N;
            ExpectedFrequency[4] = 13.6 / 100 * N;
            ExpectedFrequency[5] = 2.1 / 100 * N;
            // gathering observed frequencies
            double[] ObservedFrequency = new double[6];
            ObservedFrequency[0] = 0;
            ObservedFrequency[1] = 0;
            ObservedFrequency[2] = 0;
            ObservedFrequency[3] = 0;
            ObservedFrequency[4] = 0;
            ObservedFrequency[5] = 0;
            for (int i = 0; i < N; i++)
            {
                Z = (DataSet[i] - Mean) / StandartDeviation;
                if (Z <= -2)
                {
                    ObservedFrequency[0] = ObservedFrequency[0] + 1;
                }
                else if (Z > -2 && Z <= -1)
                {
                    ObservedFrequency[1] = ObservedFrequency[1] + 1;
                }
                else if (Z > -1 && Z <= 0)
                {
                    ObservedFrequency[2] = ObservedFrequency[2] + 1;
                }
                else if (Z > 0 && Z <= 1)
                {
                    ObservedFrequency[3] = ObservedFrequency[3] + 1;
                }
                else if (Z > 1 && Z <= 2)
                {
                    ObservedFrequency[4] = ObservedFrequency[4] + 1;
                }
                else if (Z > 2)
                {
                    ObservedFrequency[5] = ObservedFrequency[5] + 1;
                }
            }
            // calculating chi square parameter
            for (int i = 0; i < 6; i++)
            {
                ChiSquareValue = ChiSquareValue + Math.Pow(ObservedFrequency[i] - ExpectedFrequency[i], 2) / ExpectedFrequency[i];
            }
            // comparing the calculated chi square parameter and critical chi square parameter
            if (ChiSquareValue <= CriticalValue)
            {
                DatasetNormal = true;
            }
            else
            {
                DatasetNormal = false;
            }
        }

        public DescriptiveStatistics()
        {
            InitializeComponent();
        }

        private void DescriptiveStatistics_Load(object sender, EventArgs e)
        {
            GeneratingChiSquareTable();
        }

        private void DescriptiveStatistics_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void BackButtonPic_Click(object sender, EventArgs e)
        {
            MainPage mainPage = new MainPage();
            mainPage.Show();
            this.Hide();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            MainPage mainPage = new MainPage();
            mainPage.Show();
            this.Hide();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            DescriptiveStatistics descriptiveStatistics = new DescriptiveStatistics();
            descriptiveStatistics.Show();
            this.Hide();
        }

        private void RefreshButtonPic_Click(object sender, EventArgs e)
        {
            DescriptiveStatistics descriptiveStatistics = new DescriptiveStatistics();
            descriptiveStatistics.Show();
            this.Hide();
        }

        private void InputButton_Click(object sender, EventArgs e)
        {
            if (this.InputPanel.Visible == false)
            {
                this.InputPanel.Visible = true;
            }
            else if (this.InputPanel.Visible == true)
            {
                this.InputPanel.Visible = false;
            }
        }

        private void InputButtonPic_Click(object sender, EventArgs e)
        {
            if (this.InputPanel.Visible == false)
            {
                this.InputPanel.Visible = true;
            }
            else if (this.InputPanel.Visible == true)
            {
                this.InputPanel.Visible = false;
            }
        }

        private void AnalyzeButtonPic_Click(object sender, EventArgs e)
        {
            try
            {
                TransferringData();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
                return;
            }
            finally
            {
                PlottingHistogram();
                CalculatingParameters();
            }
        }

        private void AnalyzeButton_Click(object sender, EventArgs e)
        {
            try
            {
                TransferringData();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
                return;
            }
            finally
            {
                PlottingHistogram();
                CalculatingParameters();
            }
        }

        private void PasteFromClipboardButton_Click(object sender, EventArgs e)
        {
            PasteFromClipboard();
        }

        private void AddRowsToGridButton_Click(object sender, EventArgs e)
        {
            AddRowsToGrid();
        }
    }
}
