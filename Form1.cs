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
        private void LoadDataFromCSV(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if (values.Length >= 13)
                    {
                        string name = values[1];
                        List<string> pokemonStats = new List<string>();

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
        #region Stat Changes
        private void CalculateFinalStats()
        {
            int stat;
            double lv = Convert.ToDouble(pokemonLv.Text);
            double VSlv = Convert.ToDouble(VSpokemonLV.Text);
            var statCalculator = new BaseStatCalculation();
            for (int i = 1; i <= 12; i++)
            {
                if (i == 1 || i == 7)
                {
                    TextBox ivTextBox = Controls.Find($"iv{i}", true)[0] as TextBox;
                    NumericUpDown evNumericUpDown = Controls.Find($"ev{i}", true)[0] as NumericUpDown;
                    TextBox baseTextBox = Controls.Find($"baseStat{i}", true)[0] as TextBox;
                    TextBox finalStatLabel = Controls.Find($"finalStatTotal{i}", true)[0] as TextBox;

                    double iv = double.Parse(ivTextBox.Text);
                    double ev = (double)evNumericUpDown.Value;
                    double @base = double.Parse(baseTextBox.Text);
                    if (i == 1)
                    {
                        stat = statCalculator.CalcHPStat(@base, iv, ev, lv);
                    }
                    else
                    {
                        stat = statCalculator.CalcHPStat(@base, iv, ev, VSlv);
                    }
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
                    if (i <= 6)
                    {
                        stat = statCalculator.CalcOtherStat(@base, iv, ev, lv);
                    }
                    else
                    {
                        stat = statCalculator.CalcOtherStat(@base, iv, ev, VSlv);
                    }
                    finalStatLabel.Text = stat.ToString();
                }
            }
        }
        private void statTextBox(object sender, EventArgs e)
        {
            for (int i = 1; i <= 12; i++)
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

            if (VSpokemonLV.Text == "")
                VSpokemonLV.Text = "1";
            if (Convert.ToInt32(VSpokemonLV.Text) > 100)
                VSpokemonLV.Text = "100";
            CalculateFinalStats();
        }
        private void evChange(object sender, EventArgs e)
        {
            CalculateFinalStats();
        }
        #endregion Stat Changes
        #region Pokemon Changer-Inator
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

        private void VSpokemon_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPokemon = VSpokemonName.SelectedItem.ToString();
            if (pokemonDataMap.ContainsKey(selectedPokemon))
            {
                List<string> pokemonStats = pokemonDataMap[selectedPokemon];
                if (pokemonStats.Count >= 6)
                {
                    for (int i = 1; i <= 6; i++)
                    {
                        TextBox baseTextBox = Controls.Find($"baseStat{i+6}", true)[0] as TextBox;
                        baseTextBox.Text = pokemonStats[i - 1];
                    }
                    VStype1.Text = pokemonStats[6];
                    VStype2.Text = pokemonStats[7];
                }

            }
        }
        #endregion Pokemon Changer-Inator
        private void moveChange(object sender, EventArgs e)
        {
            ComboBox moveComboBox = sender as ComboBox;
            if (moveComboBox != null)
            {
                int moveIndex = int.Parse(moveComboBox.Name.Replace("move", ""));

                TextBox bpTextBox = Controls.Find($"BP{moveIndex}", true)[0] as TextBox;
                ComboBox typeTextBox = Controls.Find($"moveType{moveIndex}", true)[0] as ComboBox;
                ComboBox categoryTextBox = Controls.Find($"moveCategory{moveIndex}", true)[0] as ComboBox;
                TextBox damageOutput = Controls.Find($"damageOutput{moveIndex}", true).FirstOrDefault() as TextBox;

                if (damageOutput != null)
                {
                    string selectedMove = moveComboBox.SelectedItem.ToString();

                    if (pokemonMoveDataMap.ContainsKey(selectedMove))
                    {
                        List<string> moveData = pokemonMoveDataMap[selectedMove];
                        if (moveData.Count >= 3)
                        {
                            string basePowerStr = moveData[2];
                            int basePower;
                            if (int.TryParse(basePowerStr, out basePower))
                            {
                                bpTextBox.Text = basePower.ToString();
                            }
                            else
                            {
                                bpTextBox.Text = "0";
                            }
                            typeTextBox.Text = moveData[0];
                            categoryTextBox.Text = moveData[1];

                            double level = Convert.ToDouble(pokemonLv.Text);
                            string moveType = moveComboBox.Text;
                            double basePowerValue = Convert.ToDouble(bpTextBox.Text);
                            string moveCategory = categoryTextBox.Text;

                            DamageCalculation damageCalculator = new DamageCalculation();

                            TextBox attackStatTextBox = null;
                            TextBox defenseStatTextBox = null;

                            if (moveCategory == "Physical")
                            {
                                attackStatTextBox = Controls.Find($"finalStatTotal2", true).FirstOrDefault() as TextBox;
                                defenseStatTextBox = Controls.Find($"finalStatTotal9", true).FirstOrDefault() as TextBox;
                            }
                            else if (moveCategory == "Special")
                            {
                                attackStatTextBox = Controls.Find($"finalStatTotal4", true).FirstOrDefault() as TextBox;
                                defenseStatTextBox = Controls.Find($"finalStatTotal11", true).FirstOrDefault() as TextBox;
                            }

                            if (attackStatTextBox != null && defenseStatTextBox != null)
                            {
                                double attackStat = Convert.ToDouble(attackStatTextBox.Text);
                                double defenseStat = Convert.ToDouble(defenseStatTextBox.Text);

                                if (moveCategory == "Status")
                                {
                                    // Display message for Status moves (does 0 damage)
                                    damageOutput.Text = "This move does not cause damage.";
                                }
                                else
                                {
                                    // Calculate damage for Physical or Special moves
                                    double maxDamage = damageCalculator.calcMaxDamage(level, basePowerValue, type1.Text, type2.Text, VStype1.Text, VStype2.Text, moveType, attackStat, defenseStat);
                                    double minDamage = damageCalculator.calcMinDamage(level, basePowerValue, type1.Text, type2.Text, VStype1.Text, VStype2.Text, moveType, attackStat, defenseStat);

                                    damageOutput.Text = $"Damage Range: {minDamage:N0} - {maxDamage:N0}";
                                }
                            }
                        }
                        else
                        {
                            bpTextBox.Text = "";
                            typeTextBox.Text = "";
                            categoryTextBox.Text = "";
                            damageOutput.Text = "";
                        }
                    }
                    else
                    {
                        bpTextBox.Text = "";
                        typeTextBox.Text = "";
                        categoryTextBox.Text = "";
                        damageOutput.Text = "";
                    }
                }
            }
        }

    }
}

