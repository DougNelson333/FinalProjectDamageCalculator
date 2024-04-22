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
        private readonly Dictionary<string, List<string>> pokemonMoveDataMap = new Dictionary<string, List<string>>();
        private readonly BaseStatCalculation statCalculator;
        public damageCalculator()
        {
            InitializeComponent();
            CalculateFinalStats();
            AttachNumericInputHandlers();
            LoadDataFromCSV("pokemon.csv");
            LoadDataFromCSV("pokemonMoves.csv");
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

                    // Assuming the CSV has columns: number,name,type1,type2,total,hp,attack,defense,sp_attack,sp_defense,speed,generation,legendary
                    if (values.Length >= 13) // Assuming you have at least 13 columns
                    {
                        string name = values[1];
                        List<string> pokemonStats = new List<string>();

                        // Extracting base stats (columns 6 to 11)
                        for (int i = 5; i <= 10; i++)
                        {
                            pokemonStats.Add(values[i]);
                        }
                        pokemonStats.Add(values[2]);
                        pokemonStats.Add(values[3]);
                        pokemonDataMap[name] = pokemonStats;
                    }
                    else if (values.Length >= 8)
                    {
                        string name = values[1];
                        List<string> moveData = new List<string>();
                        moveData.Add(values[2]);
                        moveData.Add(values[3]);
                        moveData.Add(values[5]);
                        pokemonMoveDataMap[name] = moveData;
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
            string selectedPokemon = pokemonName.SelectedItem.ToString();
            if (pokemonDataMap.ContainsKey(selectedPokemon))
            {
                List<string> pokemonStats = pokemonDataMap[selectedPokemon];
                if (pokemonStats.Count >= 6)
                {
                    for (int i = 1; i <= 6; i++)
                    {
                        TextBox baseTextBox = Controls.Find($"baseStat{i}", true)[0] as TextBox;
                        baseTextBox.Text = pokemonStats[i-1];
                    }
                    type1.Text = pokemonStats[6];
                    type2.Text = pokemonStats[7];
                }

            }
        }
    }
}

