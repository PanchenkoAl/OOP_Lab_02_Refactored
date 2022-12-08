using EmployeesFileWork.Data;
using EmployeesFileWork.Serializers;
using System.Xml.Xsl;

namespace OOP_Lab_02
{
    public partial class Form1 : Form
    {
        private string _fileLocation = "";
        private List<Employee> _employeeList;
        private string _tagPhrase;

        private const int COLUMNS_NUMBER = 7;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AddColumns();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Deserializes with SAX
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            _employeeList = Deserialize(new SAXSerializer());
            FillDataGridView(_employeeList);
        }

        /// <summary>
        /// Deserializes with DOM
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            _employeeList = Deserialize(new DOMSerializer());
            FillDataGridView(_employeeList);
        }

        /// <summary>
        /// Deserializes with LINQ
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            _employeeList = Deserialize(new LINQSerializer());
            FillDataGridView(_employeeList);
        }

        /// <summary>
        /// Opens File
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "xml files (*.xml)|*.xml";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(openFileDialog.FileName);
                _fileLocation = openFileDialog.FileName;
                reader.Close();
            }
            textBox1.Text = _fileLocation;
        }

        /// <summary>
        /// Finds with SAX
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {
            ResetDataGridView();
            var resultList = FindByTagPhrase(new SAXSerializer());
            FillDataGridView(resultList);
        }

        /// <summary>
        /// Finds with DOM
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            ResetDataGridView();
            var resultList = FindByTagPhrase(new DOMSerializer());
            FillDataGridView(resultList);
        }

        /// <summary>
        /// Finds with LINQ
        /// </summary>
        private void button7_Click(object sender, EventArgs e)
        {
            ResetDataGridView();
            var resultList = FindByTagPhrase(new LINQSerializer());            
            FillDataGridView(resultList);
        }              

        /// <summary>
        /// Shows all deserialized data
        /// </summary>
        private void button8_Click(object sender, EventArgs e)
        {
            ResetDataGridView();
            FillDataGridView(_employeeList);
        }

        /// <summary>
        /// Transforms to HTML
        /// </summary>
        private void button9_Click(object sender, EventArgs e)
        {
            //change for work directory, what if there is no D:\ drive? 
            //Exception: Not found xsl file
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(@"D:\MyXslt.xsl");

            if (string.IsNullOrEmpty(_fileLocation))
            {
                xslt.Transform(_fileLocation, "EmployeesT.html");
                MessageBox.Show(
                    "Complete",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "File Location is incorrect!",
                    "Error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Shows help.
        /// </summary>
        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Author's name Panchenko Olexandr, gr. K-25. \n" +
                "Open .xml file with 'Open File' button. \n" +
                "- To Deserialize, press SAX/DOM/LINQ Deserialize button. \n" +
                "- To find something by phrase, type phrase in the texbox in the right corner. " +
                "\t Use 'phrase(Attribute)' (For example Alex(Name), 1234(Id), etc.). \n" +
                "Press 'Convert to HTML' button to get .html file, using special .xsl file, \n" +
                "that you can find in the projects folder (file name MyXslt.xsl). form.");
        }

        #region Helpers
        private List<Employee> Deserialize(ISerializer serializer)
        {
            if (string.IsNullOrEmpty(_fileLocation))
            {
                MessageBox.Show(
                    "File path is empty. Open file before serializing",
                    "Error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return new List<Employee>();
            }

            serializer.Deserialize(_fileLocation, out List<Employee> employees);

            if (employees != null)
            {
                MessageBox.Show(
                    $"File was successfully serialized by {serializer.GetType()}",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }            

            return employees;
        }

        private List<Employee> FindByTagPhrase(ISerializer serializer)
        {
            if (string.IsNullOrEmpty(_fileLocation))
            {
                MessageBox.Show(
                    "File path is empty. Open file before serializing",
                    "Error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return new List<Employee>();
            }

            _tagPhrase = textBox2.Text.Trim();
            string[] splittedTag = _tagPhrase.Split('&');
            List<Employee> employees = new List<Employee>();
            foreach (string tag in splittedTag)
            {
                for(int i = 0; i < serializer.FindByTag(_fileLocation, tag).Count; i ++)
                employees.Add(serializer.FindByTag(_fileLocation, tag)[i]);
            }
            if (employees == null)
            {
                MessageBox.Show(
                    $"Search by {serializer.GetType()} failed.",
                    "Error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return employees;
        }

        private void ResetDataGridView()
        {
            for (int i = 1; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < COLUMNS_NUMBER; j++)
                {
                    dataGridView1[j, i].Value = "";
                }
            }
            for (int i = dataGridView1.Rows.Count - 1; i > 0; i--)
            {
                dataGridView1.Rows.RemoveAt(i);
            }
        }

        private void FillDataGridView(List<Employee> employees)
        {
            if (employees != null)
            {
                for (int RowIndex = 1; dataGridView1.Rows.Count < employees.Count; RowIndex++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[RowIndex - 1].HeaderCell.Value = RowIndex.ToString();
                }

                for (int i = 0; i < employees.Count; i++)
                {
                    dataGridView1[0, i].Value = employees[i].Id;
                    dataGridView1[1, i].Value = employees[i].Name;
                    dataGridView1[2, i].Value = employees[i].Faculty?.Department;
                    dataGridView1[3, i].Value = employees[i].Faculty?.Part;
                    dataGridView1[4, i].Value = employees[i].Laboratory;
                    dataGridView1[5, i].Value = employees[i].Title?.StartDate;
                    dataGridView1[6, i].Value = employees[i].Title?.EndDate;
                }
            }
            else
            {
                MessageBox.Show("Error. Input List is null");
            }
        }

        private void AddColumns()
        {
            for (int i = 0; i < COLUMNS_NUMBER; i++)
            {
                dataGridView1.Columns.Add(Name, ISerializer.TAGS[(ISerializer.TAGS_ENUM)i]);
            }

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[COLUMNS_NUMBER - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[COLUMNS_NUMBER - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }
        #endregion

    }
}