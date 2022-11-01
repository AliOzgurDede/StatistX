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

namespace StatistX
{
    public partial class InferentialStatistics : Form
    {
        // definitions
        int SampleSize;
        double SampleMean;
        double SampleStandartDeviation;
        double PopulationMean;
        double PopulationStandartDeviation;
        double Significance;
        string TestType;
        Dictionary<double, double> Zvalues = new Dictionary<double, double>();
        bool Hypothesis;
        bool InputTabOpen;
        double TestStatistic;
        double CriticalValue;

        void GeneratingZvalues()
        {
            Zvalues.Add(0.10, 1.28);
            Zvalues.Add(0.05, 1.65);
            Zvalues.Add(0.025, 1.96);
            Zvalues.Add(0.01, 2.31);
            Zvalues.Add(0.005, 2.56);
        }

        void TakingUserOptions()
        {
            SampleSize = Int32.Parse(textBox1.Text);
            SampleMean = Double.Parse(textBox2.Text);
            SampleStandartDeviation = Double.Parse(textBox3.Text);
            PopulationMean = Double.Parse(textBox4.Text);
            PopulationStandartDeviation = Double.Parse(textBox5.Text);
            Significance = Double.Parse(listBox1.SelectedItem.ToString());
            TestType = listBox2.SelectedItem.ToString();
        }

        void Ztest()
        {
            TestStatistic = 0;
            TestStatistic = (SampleMean - PopulationMean) / (PopulationStandartDeviation / Math.Sqrt(SampleSize));
            CriticalValue = 0;
            if (TestType == "One Tailed")
            {
                CriticalValue = Zvalues[Significance];
            }
            else if (TestType == "Two Tailed")
            {
                CriticalValue = Zvalues[Significance / 2];
            }
            if (Math.Abs(TestStatistic) > Math.Abs(CriticalValue))
            {
                Hypothesis = false;
            }
            else
            {
                Hypothesis = true;
            }
        }

        public InferentialStatistics()
        {
            InitializeComponent();
        }

        private void InferentialStatistics_Load(object sender, EventArgs e)
        {
            GeneratingZvalues();
        }

        private void InferentialStatistics_FormClosed(object sender, FormClosedEventArgs e)
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
            InferentialStatistics ınferentialStatistics = new InferentialStatistics();
            ınferentialStatistics.Show();
            this.Hide();
        }

        private void RefreshButtonPic_Click(object sender, EventArgs e)
        {
            InferentialStatistics ınferentialStatistics = new InferentialStatistics();
            ınferentialStatistics.Show();
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

        private void AnalyzeButton_Click(object sender, EventArgs e)
        {
            try
            {
                TakingUserOptions();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
                return;
            }
            finally
            {
                Ztest();
                label15.Text = CriticalValue.ToString();
                label16.Text = TestStatistic.ToString();
                if (Hypothesis == true)
                {
                    label17.Text = "Hypothesis Accepted";
                    this.HypothesisTruePicture.Visible = true;
                    this.HypothesisFalsePicture.Visible = false;
                }
                else if (Hypothesis == false)
                {
                    label17.Text = "Hypothesis Rejected";
                    this.HypothesisTruePicture.Visible = false;
                    this.HypothesisFalsePicture.Visible = true;
                }
            }
        }

        private void AnalyzeButtonPic_Click(object sender, EventArgs e)
        {
            try
            {
                TakingUserOptions();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
                return;
            }
            finally
            {
                Ztest();
                label15.Text = CriticalValue.ToString();
                label16.Text = TestStatistic.ToString();
                if (Hypothesis == true)
                {
                    label17.Text = "Hypothesis Accepted";
                    this.HypothesisTruePicture.Visible = true;
                    this.HypothesisFalsePicture.Visible = false;
                }
                else if (Hypothesis == false)
                {
                    label17.Text = "Hypothesis Rejected";
                    this.HypothesisTruePicture.Visible = false;
                    this.HypothesisFalsePicture.Visible = true;
                }
            }
        }
    }
}
