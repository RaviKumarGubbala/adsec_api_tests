using System.Text;
using Oasys.AdSec;
using Oasys.AdSec.DesignCode;
using Oasys.AdSec.IO.Serialization;
using Oasys.AdSec.Reinforcement;
using Oasys.AdSec.Reinforcement.Groups;
using Oasys.AdSec.Reinforcement.Layers;
using Oasys.AdSec.StandardMaterials;
using Oasys.Profiles;
using Oasys.Units;
using System.Diagnostics;
using UnitsNet;
using Newtonsoft.Json;

namespace TestWithFormsUI
{
    public partial class Form1 : Form
    {
        Dictionary<string, List<ISection>> parsed_sections = new Dictionary<string, List<ISection>>();
        List<string> failed_files = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "AdSec API tester - Select an AdSec 10 file";
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fileDialog.Filter = "AdSec 10 JSON files (*.Ads)|*.ads";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.filePathBox.Text = fileDialog.FileName;
            }
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            this.infoTextBox.Text = String.Empty;
            bool isExtactFromFolder = true;
            if (isExtactFromFolder)
            {
                var rootDir = @"C:\Users\ravikumar.gubbala\OneDrive - Arup\Documents\Take_away\AdSec Test files\API_Test";
                string[] files =
                    Directory.GetFiles(rootDir, "*.ads", SearchOption.TopDirectoryOnly);


                foreach (string filePath in files)
                {
                    ExtractData(filePath);
                }
            }
            else
            {
                ExtractData(this.filePathBox.Text);
            }
        }

        private void ExtractData(string filePath)
        {
            bool skipKnownIssues = false;
            if (skipKnownIssues && (filePath.Contains("1038") || filePath.Contains("1067") || filePath.Contains("687") || filePath.Contains("API group") || filePath.Contains("Deck") || filePath.Contains("MoreStand") || filePath.Contains("RE sect") || filePath.Contains("RE sect") ||
    filePath.Contains("RftMult") || filePath.Contains("User Material")))
            {
                failed_files.Add(filePath);
                return;
            }

            this.infoTextBox.AppendText(Environment.NewLine + filePath + Environment.NewLine);
            string fileJSONstring = File.ReadAllText(filePath);

            try
            {
                dynamic parsedFile = JsonConvert.DeserializeObject(fileJSONstring);
                if (parsedFile != null)
                {
                    var concreteCode = parsedFile["codes"].concrete.Value;

                    var parsed = JsonParser.Deserialize(fileJSONstring);
                    foreach (var warning in parsed.Warnings)
                    {
                        var warningText = AdSec_API_Utils.Textify.Text(warning);
                        this.infoTextBox.AppendText(warningText);
                        this.infoTextBox.AppendText(Environment.NewLine +
                            "-------------------------------------------" +
                            Environment.NewLine);
                    }

                    foreach (var section in parsed.Sections)
                    {
                        if (parsed_sections.ContainsKey(concreteCode))
                        {
                            parsed_sections[concreteCode].Add(section);
                        }
                        else
                        {
                            parsed_sections[concreteCode] = new List<ISection>();
                            parsed_sections[concreteCode].Add(section);
                        }

                        var sectionText = AdSec_API_Utils.Textify.Text(section);
                        this.infoTextBox.AppendText(sectionText);
                        this.infoTextBox.AppendText(Environment.NewLine +
                            "-------------------------------------------" +
                            Environment.NewLine);
                    }

                    foreach (var sectionsPair in this.parsed_sections)
                    {
                        if (sectionsPair.Key.Contains("EC"))
                        {
                            Oasys.AdSec.IO.Serialization.JsonConverter jsonConverter = new Oasys.AdSec.IO.Serialization.JsonConverter(EN1992.Part1_1.Edition_2004.NationalAnnex.GB.Edition_2014);
                            foreach (var section in sectionsPair.Value)
                            {
                                string sectionJson = jsonConverter.SectionToJson(section);
                                if (sectionJson != null)
                                {
                                    var file = AdSec_API_Utils.FileHelper.GetAdsFileNamewithCurrentTime();
                                    using (StreamWriter streamWriter = File.AppendText(file))
                                    {
                                        streamWriter.Write(sectionJson);
                                    }
                                }
                            }

                        }
                    }

                    this.parsed_sections.Clear();
                }
            }
            catch (System.Exception ex)
            {
                var oex = ex.InnerException as Oasys.Exceptions.ValidationException;
                if (oex != null)
                {
                    this.infoTextBox.AppendText(Environment.NewLine + ex.InnerException.Message +
                            Environment.NewLine);
                    //MessageBox.Show(ex.InnerException.Message);
                }
                else
                {
                    MessageBox.Show(filePath + "\n" + ex.Message);
                }
            }
        }
    }
}