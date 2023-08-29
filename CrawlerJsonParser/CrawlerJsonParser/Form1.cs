
using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CrawlerJsonParser
{
    public partial class MainForm : Form
    {
        #region FilePath 
        private string sourceJsonFilePath;
        private string destCSVFilePath;
        #endregion

        #region Parsing Data
        private List<SpinData> spinDatas = new List<SpinData>();
        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        private void AddSpinData(JsonObject jsonObj)
        {
            if (!jsonObj.ContainsKey("_id"))
            {
                AddLogOnListBox("[ERROR]", "Source Json doesn't has a _id key value.");
                Debug.Assert(false); 
                return;
            }
            
            SpinData spinData = new SpinData();
            foreach (string _key in Enum.GetNames(typeof(SpinDataKey))) 
            {                
                if(_key  ==  null){
                    continue;
                }

                JsonNode _node;
                if(jsonObj.TryGetPropertyValue(_key, out _node))
                {                    
                    spinData.datum[_key] = jsonObj[_key].ToString();
                }
                else
                {
                    //string msg = string.Format("id: {0}, key: {1} is not found on source file.", jsonObj["_id"], _key );
                    //AddLogOnListBox("[Src Data:Warnning]", msg);
                }
            }

            spinDatas.Add(spinData);
        }

        private string GetTargetFilePath()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Title = "Crawler  CSV Parser";
            fileDialog.FileName = string.Empty;
            fileDialog.Filter = "Crawler Json File(*.json) | *.json";

            DialogResult dialogResult = fileDialog.ShowDialog();
            if(dialogResult == DialogResult.OK) 
            {
                string fileName = fileDialog.SafeFileName;
                string fullFilePath = fileDialog.FileName;
                string onlyFilePath = fullFilePath.Replace(fileName, "");

                return fullFilePath;
            }

            return string.Empty;
        }

        private string SetTargetFilePath()
        {
            string timeMark = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            string fileName = string.Format("spinData_{0}.csv", timeMark);
            return fileName;
        }

        private bool WriteCSVFile(string filePath)
        {
            using (StreamWriter file = new StreamWriter(filePath, false, System.Text.Encoding.GetEncoding("utf-8")))
            {                
                foreach(SpinData data in spinDatas)
                {
                    string lineData = data.ToCSVString();
                    file.WriteLine(lineData);
                }
            }

            return true;
        }

        private void SetSourceJsonFilePath(object sender, EventArgs e)
        {
            sourceJsonFilePath = string.Empty;
            sourceJsonFilePath = GetTargetFilePath();

            if(sourceJsonFilePath == string.Empty)
            {
                AddLogOnListBox("[Source Json Path]", "An Error has occured.");
                return;
            }

            this.textBox1.Text = sourceJsonFilePath;
            AddLogOnListBox("[Source Json Path]", sourceJsonFilePath);
            
            string content = string.Empty;

            try
            {
                content = File.ReadAllText(sourceJsonFilePath);
            }
            catch (Exception ex)
            {
                AddLogOnListBox("[Error]", ex.Message);
                return;
            }

            var items = JsonNode.Parse(content);

            if (items != null)
            {
                spinDatas.Clear();
                AddLogOnListBox("[Source Json: Spin Data Count] ", items.AsArray().Count.ToString());

                for (int index =0; index < items.AsArray().Count; index++)
                {
                    JsonObject _obj = items[index].AsObject();
                    
                    if (_obj != null)
                    {
                        AddSpinData(_obj);
                    }
                    else
                    {
                        AddLogOnListBox("[Source Json:ERROR][Json Index]", index.ToString());
                    }
                }
            }
        }

        private void SetDestCSVFilePath(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "Crawler  CSV Parser";            
            fileDialog.FileName = SetTargetFilePath();
            fileDialog.Filter = "CSV(*.csv) | *.csv";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                destCSVFilePath = fileDialog.FileName;
                this.textBox2.Text = destCSVFilePath;
                AddLogOnListBox("[Dest][csv]", destCSVFilePath);

                if (WriteCSVFile(destCSVFilePath))
                {
                    AddLogOnListBox("[Dest][csv]", "file write is done.");
                }
            }
        }

        private void AddLogOnListBox(string desc, string message)
        {
            if(message == string.Empty)
            {
                return;
            }

            string logMessage = string.Format("{0} : {1}", desc, message);
            this.ResultListBox.Items.Add(logMessage);
        }
    }
}