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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace StatisticsApp
{
    public partial class TimeSeries : Form
    {
        // tanımlar
        int N;
        double[] DataSet;
        string[] horizontalAxis;
        DateTime StartDate;
        string TimeUnit;
        int step;
        int[] timePeriod;
        double slope;
        double intercept;

        public TimeSeries()
        {
            InitializeComponent();
        }

        private void TimeSeries_Load(object sender, EventArgs e)
        {
            panel3.Visible = false;
            checkBox1.Visible = false;
            checkBox2.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
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
                DataSet[i] = Double.Parse(dataGridView1[0, i].Value.ToString());
            }
        }

        void VisualizingData()
        {
            // taking user options
            TimeUnit = listBox1.SelectedItem.ToString();
            StartDate = new DateTime();
            StartDate = dateTimePicker1.Value.Date;

            // determining the elements of horizontal axis 
            horizontalAxis = new string[N];
            DateTime NextDate = new DateTime();
            NextDate = StartDate;
            if (TimeUnit == "Day")
            {
                horizontalAxis[0] = StartDate.ToString();
                for (int i = 1; i < N; i++)
                {
                    NextDate = NextDate.AddDays(1);
                    horizontalAxis[i] = NextDate.ToString();
                }
            }
            else if (TimeUnit == "Week")
            {
                horizontalAxis[0] = StartDate.ToString();
                for (int i = 1; i < N; i++)
                {
                    NextDate = NextDate.AddDays(7);
                    horizontalAxis[i] = NextDate.ToString();
                }
            }
            else if (TimeUnit == "Month")
            {
                horizontalAxis[0] = StartDate.ToString();
                for (int i = 1; i < N; i++)
                {
                    NextDate = NextDate.AddMonths(1);
                    horizontalAxis[i] = NextDate.ToString();
                }
            }
            else if (TimeUnit == "Year")
            {
                horizontalAxis[0] = StartDate.ToString();
                for (int i = 1; i < N; i++)
                {
                    NextDate = NextDate.AddYears(1);
                    horizontalAxis[i] = NextDate.ToString();
                }
            }

            // plotting on graphic
            for (int i = 0; i < N; i++)
            {
                this.Grafik.Series["Data"].Points.AddXY(horizontalAxis[i], DataSet[i]);
            }
        }

        void MovingAverages()
        {
            // calculating moving averages for each data element
            double[] MA = new double[N];
            for (int i = 0; i < step; i++)
            {
                MA[i] = 0;
            }
            for (int i = step; i < N; i++)
            {
                double sum = 0;
                for (int j = i - step; j < i; j++)
                {
                    sum = sum + DataSet[j];
                }
                MA[i] = sum / step;
            }
            // plotting moving averages on graphic
            for (int i = 0; i < N; i++)
            {
                this.Grafik.Series["Moving Averages"].Points.AddXY(horizontalAxis[i], MA[i]);
            }
        }

        void RegressionModelSetup()
        {
            // regression line | y = a + b*x

            // determining of x: independent variable
            timePeriod = new int[N];
            for (int i = 0; i < N; i++)
            {
                timePeriod[i] = i + 1;
            }

            // calculation of b: slope
            double xBar = 0;
            double ttl = 0;
            for (int i = 0; i < N; i++)
            {
                ttl = ttl + timePeriod[i];
            }
            xBar = ttl / N;
            xBar = Math.Round(xBar, 2);

            double yBar = 0;
            double Total = 0;
            for (int i = 0; i < N; i++)
            {
                Total = Total + DataSet[i];
            }
            yBar = Total / N;
            yBar = Math.Round(yBar, 2);

            double sumOfXcrossY = 0;
            for (int i = 0; i < N; i++)
            {
                sumOfXcrossY = sumOfXcrossY + (DataSet[i] * timePeriod[i]);
            }

            double sumOfXsquare = 0;
            for (int i = 0; i < N; i++)
            {
                sumOfXsquare = sumOfXsquare + (Math.Pow(timePeriod[i], 2));
            }

            slope = (sumOfXcrossY - (N * xBar * yBar)) / (sumOfXsquare - (N * xBar * xBar));

            // calculation of a: intercept
            intercept = yBar - (slope * xBar);

            // visualizing linear regression line
            for (int i = 0; i < N; i++)
            {
                this.Grafik.Series["Regression Line"].Points.AddXY(horizontalAxis[i], Regression(timePeriod[i]));
            }
        }

        double Regression(double x)
        {
            double y;
            y = intercept + (slope * x);
            return y;
        }

        int SettingCounter(DateTime TargetTime)
        {
            StartDate = dateTimePicker1.Value.Date;
            DateTime TargetDate = new DateTime();
            TargetDate = dateTimePicker2.Value.Date;
            TimeSpan span = new TimeSpan();
            int interval = 0;

            if (TargetDate.CompareTo(StartDate) == 1)
            {
                if (TimeUnit == "Day")
                {
                    span = TargetDate - StartDate;
                    interval = span.Days;
                }
                else if (TimeUnit == "Week")
                {
                    span = TargetDate - StartDate;
                    interval = span.Days / 7;
                }
                else if (TimeUnit == "Month")
                {
                    span = TargetDate - StartDate;
                    interval = span.Days / 31;
                }
                else if (TimeUnit == "Year")
                {
                    span = TargetDate - StartDate;
                    interval = span.Days / 366;
                }
            }
            return interval;
        }

        void Estimation()
        {
            if (dateTimePicker2.Value.Date.CompareTo(StartDate) == 1)
            {
                double estimate;
                estimate = Regression(SettingCounter(dateTimePicker2.Value.Date));
                estimate = Math.Round(estimate, 2);
                string output;
                output = estimate.ToString();
                string title;
                title = "Estimated Value";
                MessageBox.Show(title + "\n" + output);
            }
            else
            {
                MessageBox.Show("Invalid Target Date Choice");
            }
        }

        void RefreshingPage()
        {
            TimeSeries yeniForm = new TimeSeries();
            yeniForm.Show();
            this.Hide();
        }

        void ReturningToMainPage()
        {
            MainPage yeniForm = new MainPage();
            yeniForm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ReturningToMainPage();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RefreshingPage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Missing information, number of the rows");
            }
            else
            {
                AddRowsToGrid();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PasteFromClipboard();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Time unit is not selected!");
            }
            else
            {
                this.Grafik.Series["Data"].Points.Clear();
                TransferringData();
                VisualizingData();
                checkBox1.Visible = true;
                checkBox2.Visible = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                this.Grafik.Series["Moving Averages"].Points.Clear();
                string input = Interaction.InputBox("Enter the step parameter for moving averages");
                step = Int32.Parse(input);
                MovingAverages();
            }
            else if (checkBox2.Checked == false)
            {
                this.Grafik.Series["Moving Averages"].Points.Clear();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                label5.Visible = true;
                label6.Visible = true;
                panel3.Visible = true;
                this.Grafik.Series["Regression Line"].Points.Clear();
                RegressionModelSetup();
                label6.Text = "y = " + Math.Round(slope,2).ToString() + "x + " + Math.Round(intercept, 2).ToString();
                MessageBox.Show("Regression model is constructed. Regression line is visualized on the graph");
            }
            else if (checkBox1.Checked == false)
            {
                label5.Visible = false;
                label6.Visible = false;
                panel3.Visible = false;
                this.Grafik.Series["Regression Line"].Points.Clear();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Estimation();
        }

        
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
    }
}
