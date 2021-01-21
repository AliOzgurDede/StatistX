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
        // tanımlar
        int SampleSize;
        double SampleMean;
        double SampleStandartDeviation;
        double PopulationMean;
        double PopulationStandartDeviation;
        double Significance;
        string TestType;
        Dictionary<double, double> Zvalues = new Dictionary<double, double>();
        bool Hypothesis;

        public InferentialStatistics()
        {
            InitializeComponent();
        }

        private void InferentialStatistics_Load(object sender, EventArgs e)
        {

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
            double TestStatistic = 0;
            TestStatistic = (SampleMean - PopulationMean) / (PopulationStandartDeviation / Math.Sqrt(SampleSize));
            double CriticalValue = 0;
            if (TestType == "One Tailed")
            {
                CriticalValue = Zvalues[Significance];
            }
            else if (TestType == "Two Tailed")
            {
                CriticalValue = Zvalues[Significance] / 2;
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

        private void button1_Click(object sender, EventArgs e)
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
                if (Hypothesis == true)
                {
                    MessageBox.Show("Population data are represented by sample data");
                }
                else if (Hypothesis == false)
                {
                    MessageBox.Show("Population data are not represented by sample data");
                }
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
    }
}
