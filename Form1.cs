using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DamageCalculator
{
    public partial class damageCalculator : Form
    {
        private readonly Dictionary<string, List<string>> pokemonDataMap = new Dictionary<string, List<string>>();
        private readonly BaseStatCalculation statCalculator;
        public damageCalculator()
        {
            InitializeComponent();
            CalculateFinalStats();
            AttachNumericInputHandlers();
            LoadDataFromCSV("pokemon.csv");
        }
        #region Prevent Letters
        private void AttachNumericInputHandlers()
        {
            foreach (Control control in Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.KeyPress += preventLetters;
                }
            }
        }
        private void preventLetters(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Disallow non-numeric characters
            }
        }
        #endregion Prevent Letters
        private void LoadDataFromCSV(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    // Assuming the CSV has columns: Name, ..., HP Stat, ...
                    if (values.Length >= 13) // Assuming you have at least 13 columns
                    {
                        string name = values[1];
                        List<string> pokemonStats = new List<string>();

                        pokemonStats.Add(values[2]);
                        pokemonStats.Add(values[3]);
                        // Extracting only the relevant columns (6th column is HP Stat)
                        pokemonStats.Add(values[5]); // HP Stat
                        pokemonStats.Add(values[6]);
                        pokemonStats.Add(values[7]);
                        pokemonStats.Add(values[8]);
                        pokemonStats.Add(values[9]);
                        pokemonStats.Add(values[10]);

                        pokemonDataMap[name] = pokemonStats;
                    }
                }
            }
        }

        private void CalculateFinalStats()
        {
            double lv = Convert.ToDouble(pokemonLv.Text);
            var statCalculator = new BaseStatCalculation();
            for (int i = 1; i <= 6; i++)
            {
                if (i == 1)
                {
                    TextBox ivTextBox = Controls.Find($"iv{i}", true)[0] as TextBox;
                    NumericUpDown evNumericUpDown = Controls.Find($"ev{i}", true)[0] as NumericUpDown;
                    TextBox baseTextBox = Controls.Find($"baseStat{i}", true)[0] as TextBox;
                    TextBox finalStatLabel = Controls.Find($"finalStatTotal{i}", true)[0] as TextBox;

                    double iv = double.Parse(ivTextBox.Text);
                    double ev = (double)evNumericUpDown.Value;
                    double @base = double.Parse(baseTextBox.Text);
                    int stat = statCalculator.CalcHPStat(@base, iv, ev, lv);
                    finalStatLabel.Text = stat.ToString();
                }
                else
                {
                    TextBox ivTextBox = Controls.Find($"iv{i}", true)[0] as TextBox;
                    NumericUpDown evNumericUpDown = Controls.Find($"ev{i}", true)[0] as NumericUpDown;
                    TextBox baseTextBox = Controls.Find($"baseStat{i}", true)[0] as TextBox;
                    TextBox finalStatLabel = Controls.Find($"finalStatTotal{i}", true)[0] as TextBox;

                    double iv = double.Parse(ivTextBox.Text);
                    double ev = (double)evNumericUpDown.Value;
                    double @base = double.Parse(baseTextBox.Text);
                    int stat = statCalculator.CalcOtherStat(@base, iv, ev, lv);
                    finalStatLabel.Text = stat.ToString();
                }
            }
        }
        private void statTextBox(object sender, EventArgs e)
        {
            for (int i = 1; i <= 6; i++)
            {
                TextBox ivTextBox = Controls.Find($"iv{i}", true)[0] as TextBox;
                TextBox baseTextBox = Controls.Find($"baseStat{i}", true)[0] as TextBox;

                if (ivTextBox.Text == "")
                {
                    ivTextBox.Text = "0";
                    ivTextBox.Select(ivTextBox.Text.Length, 0);
                }  
                if (Convert.ToInt32(ivTextBox.Text) > 31)
                {
                    ivTextBox.Text = "31";
                    ivTextBox.Select(ivTextBox.Text.Length, 0);
                }
                if (baseTextBox.Text == "" || Convert.ToInt32(baseTextBox.Text) <= 0)
                {
                    baseTextBox.Text = "1";
                    baseTextBox.Select(baseTextBox.Text.Length, 0);
                }
                if (Convert.ToInt32(baseTextBox.Text) > 255)
                {
                    baseTextBox.Text = "255";
                    baseTextBox.Select(baseTextBox.Text.Length, 0);
                }
            }
            if (pokemonLv.Text == "")
                pokemonLv.Text = "1";
            if (Convert.ToInt32(pokemonLv.Text) > 100)
                pokemonLv.Text = "100";

            CalculateFinalStats();
        }
        private void evChange(object sender, EventArgs e)
        {
            CalculateFinalStats();
        }

        private void pokemonName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

