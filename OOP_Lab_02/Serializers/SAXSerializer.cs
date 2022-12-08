using System.Xml;
using System.Xml.Serialization;
using EmployeesFileWork.Data;

namespace EmployeesFileWork.Serializers
{
    internal class SAXSerializer : ISerializer
    {
        public override bool Serialize(out string xmlUrl, List<Employee> items)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Employee[]));

            using (FileStream fileStream = new FileStream("Employee.xml", FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fileStream, items);
                Console.WriteLine("Object was Serialized");
            }
            xmlUrl = "serialized";

            return true;
        }

        public override bool Deserialize(string xmlUrl, out List<Employee> items)
        {
            items = new List<Employee>();

            using (XmlReader xmlReader = XmlReader.Create(xmlUrl))
            {
                var tableObject = new Dictionary<TAGS_ENUM, string>();
                string xmlElement = "";                

                var title = new Title();
                var faculty = new Faculty();

                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        xmlElement = xmlReader.Name;
                    }
                    else if (xmlReader.NodeType == XmlNodeType.Text)
                    {
                        TAGS_ENUM key = TAGS.Where(tag => tag.Value == xmlElement).First().Key;
                        tableObject.TryAdd(key, xmlReader.Value);                       
                    }
                    else if ((xmlReader.NodeType == XmlNodeType.EndElement))
                    {
                        xmlElement = xmlReader.Name;
                        switch (xmlElement)
                        {
                            case "Title":
                                if (tableObject.TryGetValue(TAGS_ENUM.StartDate, out string startDate)
                                    && tableObject.TryGetValue(TAGS_ENUM.EndDate, out string endDate))
                                {
                                    title = new Title(startDate, endDate);
                                }                                
                                break;

                            case "Faculty":
                                if (tableObject.TryGetValue(TAGS_ENUM.Department, out string department)
                                    && tableObject.TryGetValue(TAGS_ENUM.Part, out string part))
                                {
                                    faculty = new Faculty(department, part);
                                }
                                break;
                            case "Employee":
                                {
                                    if (tableObject.TryGetValue(TAGS_ENUM.Id, out string id)
                                    && tableObject.TryGetValue(TAGS_ENUM.Name, out string name)
                                    && tableObject.TryGetValue(TAGS_ENUM.Laboratory, out string laboratory))
                                    {
                                        items.Add(new Employee(id, name, faculty, laboratory, title));
                                    }
                                    
                                    tableObject.Clear();
                                    break;
                                }
                        }
                    }
                }
            }
            return true;
        }

        public override List<Employee> FindByTag(string xmlUrl, string tag)
        {
            if (xmlUrl is null)
            {
                MessageBox.Show("No file given.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Employee>();
            }

            if (tag is null)
            {
                MessageBox.Show("Tag is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Employee>();
            }

            Dictionary<string, string> attributeValueDictionary = ProcessTagPhrase(tag);

            var employees = new List<Employee>();
            try
            {
                using (XmlReader xmlReader = XmlReader.Create(xmlUrl))
                {
                    bool isMatchTag = false;
                    var tableObject = new Dictionary<TAGS_ENUM, string>();

                    string xmlElement = "";     

                    Title title = new Title();
                    Faculty faculty = new Faculty();

                    while (xmlReader.Read())
                    {

                        if (xmlReader.NodeType == XmlNodeType.Element)
                        {
                            xmlElement = xmlReader.Name;
                        }
                        else if (xmlReader.NodeType == XmlNodeType.Text)
                        {
                            TAGS_ENUM key = TAGS.Where(tag => tag.Value == xmlElement).First().Key;
                            tableObject.TryAdd(key, xmlReader.Value);

                            isMatchTag = isMatchTag || attributeValueDictionary.Any(tag => xmlElement == tag.Key
                                && xmlReader.Value.Contains(tag.Value));                            
                        }
                        else if ((xmlReader.NodeType == XmlNodeType.EndElement))
                        {
                            xmlElement = xmlReader.Name;
                            switch (xmlElement)
                            {
                                case "Title":
                                    if (tableObject.TryGetValue(TAGS_ENUM.StartDate, out string startDate)
                                        && tableObject.TryGetValue(TAGS_ENUM.EndDate, out string endDate))
                                    {
                                        title = new Title(startDate, endDate);
                                    }
                                    break;

                                case "Faculty":
                                    if (tableObject.TryGetValue(TAGS_ENUM.Department, out string department)
                                        && tableObject.TryGetValue(TAGS_ENUM.Part, out string part))
                                    {
                                        faculty = new Faculty(department, part);
                                    }
                                    break;

                                case "Employee":
                                    {
                                        if (isMatchTag)
                                        {
                                            if (tableObject.TryGetValue(TAGS_ENUM.Id, out string id)
                                                && tableObject.TryGetValue(TAGS_ENUM.Name, out string name)
                                                && tableObject.TryGetValue(TAGS_ENUM.Laboratory, out string laboratory))
                                            {
                                                employees.Add(new Employee(id, name, faculty, laboratory, title));
                                            }

                                        }
                                        tableObject.Clear();
                                        isMatchTag = false;
                                    }
                                    break;
                            }
                        }
                    }
                }
                return employees;
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Unable to find.");
            }

            return new List<Employee>();
        }        
    }
}
