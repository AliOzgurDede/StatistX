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

namespace StatisticsApp
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

        public InferentialStatistics()
        {
            InitializeComponent();
        }

        private void InferentialStatistics_Load(object sender, EventArgs e)
        {
            pictureBox5.Visible = false;
            pictureBox6.Visible = false;
            InputTabOpen = false;
            panel7.Visible = false;
        }

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

        private void button4_Click(object sender, EventArgs e)
        {
            MainPage yeniForm = new MainPage();
            yeniForm.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            InferentialStatistics yeniForm = new InferentialStatistics();
            yeniForm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainPage yeniForm = new MainPage();
            yeniForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InferentialStatistics yeniForm = new InferentialStatistics();
            yeniForm.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == " " || textBox2.Text == " " || textBox3.Text == " " || textBox4.Text == " " || textBox5.Text == " " || listBox1.SelectedIndex == -1 || listBox2.SelectedIndex == -1)
                {
                    MessageBox.Show("There are missing inputs");
                }
                else
                {
                    GeneratingZvalues();
                    TakingUserOptions();
                    Ztest();
                    label12.Text = CriticalValue.ToString();
                    label13.Text = TestStatistic.ToString();
                    if (Hypothesis == true)
                    {
                        label15.Text = "Hypothesis Accepted";
                        pictureBox5.Visible = true;
                        pictureBox6.Visible = false;
                    }
                    else if (Hypothesis == false)
                    {
                        label15.Text = "Hypothesis Rejected";
                        pictureBox5.Visible = false;
                        pictureBox6.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error, please refresh the page and try again");
            }
        }
        
        private void button7_Click(object sender, EventArgs e)
        {
            if (InputTabOpen == true)
            {
                panel7.Visible = false;
                InputTabOpen = false;
                this.pictureBox5.Location = new System.Drawing.Point(334, 23);
                this.pictureBox6.Location = new System.Drawing.Point(334, 23);
            }
            else if (InputTabOpen == false)
            {
                panel7.Visible = true;
                InputTabOpen = true;
                this.pictureBox5.Location = new System.Drawing.Point(230, 23);
                this.pictureBox6.Location = new System.Drawing.Point(230, 23);
            }
        }
    }
}
