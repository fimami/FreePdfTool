using FreePdfTool.Converter.Services;

namespace FreePdfTool.UI
{
    public partial class Form1 : Form
    {
        private readonly IPdfOperations _pdfOperations;

        public Form1(IPdfOperations pdfOperations)
        {
            InitializeComponent();
            _pdfOperations = pdfOperations;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select File to merge";
            openFileDialog1.Filter = "PDF Files (*.pdf)|*.pdf";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                listBox1.Items.Add(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                var helperList = new List<object>();
                foreach (var item in listBox1.SelectedItems)
                {
                    helperList.Add(item);
                }
                foreach (var item in helperList)
                {
                    listBox1.Items.Remove(item);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "PDF Files|*.pdf";
                saveFileDialog1.Title = "Save merged PDF";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.ShowDialog();

                if (saveFileDialog1.FileName != "")
                {
                    var pathList = new List<string>();
                    foreach (string item in listBox1.Items)
                    {
                        pathList.Add(item);
                    }
                    _pdfOperations.ConcatenateDocuments(pathList, saveFileDialog1.FileName, true);
                }
            }
        }

        // TODO: Add arrow button logic

        // TODO: Add page numbers

        // TODO: Better visuals
    }
}