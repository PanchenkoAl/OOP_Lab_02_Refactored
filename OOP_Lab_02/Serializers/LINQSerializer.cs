using EmployeesFileWork.Data;
using System.Xml.Linq;

namespace EmployeesFileWork.Serializers
{
    internal class LINQSerializer : ISerializer
    {

        public override bool Serialize(out string xmlURL, List<Employee> items)
        {
            return base.Serialize(out xmlURL, items);
        }

        public override bool Deserialize(string xmlURL, out List<Employee> items)
        {
            items = new List<Employee>();

            if (xmlURL == null)
            {
                MessageBox.Show("No file given.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                XDocument xmlDocument = XDocument.Load(xmlURL);
                XElement employees = xmlDocument.Element("ArrayOfEmployee");

                if (employees is not null)
                {
                    foreach (XElement employeeElement in employees.Elements(TAGS[TAGS_ENUM.Employee]))
                    {
                        if (employeeElement is not null)
                        {
                            XElement id = employeeElement.Element(TAGS[TAGS_ENUM.Id]);
                            XElement name = employeeElement.Element(TAGS[TAGS_ENUM.Name]);

                            XElement facultyElement = employeeElement.Element(TAGS[TAGS_ENUM.Faculty]);
                            XElement laboratory = employeeElement.Element(TAGS[TAGS_ENUM.Laboratory]);

                            XElement titleElement = employeeElement.Element(TAGS[TAGS_ENUM.Title]);

                            XElement department = facultyElement?.Element(TAGS[TAGS_ENUM.Department]);
                            XElement part = facultyElement?.Element(TAGS[TAGS_ENUM.Part]);


                            XElement startDate = titleElement?.Element(TAGS[TAGS_ENUM.StartDate]);
                            XElement endDate = titleElement?.Element(TAGS[TAGS_ENUM.EndDate]);

                            Title title = new Title(startDate?.Value, endDate?.Value);
                            Faculty faculty = new Faculty(department?.Value, part?.Value);
                            Employee employee = new Employee(id?.Value, name?.Value, faculty, laboratory?.Value, title);

                            items.Add(employee);
                        }
                    }
                }
                return true;
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Unable to find");
                return false;
            }
        }

        public override List<Employee> FindByTag(string xmlURL, string tag)
        {
            if (xmlURL == null)
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
                XDocument xmlDocument = XDocument.Load(xmlURL);
                XElement employeeElements = xmlDocument.Element("ArrayOfEmployee");
                if (employeeElements is not null)
                {
                    foreach (XElement employeeElement in employeeElements.Elements(TAGS[TAGS_ENUM.Employee]))
                    {
                        bool isMatchTag = attributeValueDictionary.Any(tag => employeeElement.Element(tag.Key) != null 
                            && employeeElement.Element(tag.Key).Value.Contains(tag.Value));

                        if (isMatchTag && employeeElement is not null)
                        {
                            XElement id = employeeElement.Element(TAGS[TAGS_ENUM.Id]);
                            XElement name = employeeElement.Element(TAGS[TAGS_ENUM.Name]);

                            XElement facultyElement = employeeElement.Element(TAGS[TAGS_ENUM.Faculty]);
                            XElement laboratory = employeeElement.Element(TAGS[TAGS_ENUM.Laboratory]);

                            XElement titleElement = employeeElement.Element(TAGS[TAGS_ENUM.Title]);

                            XElement department = facultyElement?.Element(TAGS[TAGS_ENUM.Department]);
                            XElement part = facultyElement?.Element(TAGS[TAGS_ENUM.Part]);

                            
                            XElement startDate = titleElement?.Element(TAGS[TAGS_ENUM.StartDate]);
                            XElement endDate = titleElement?.Element(TAGS[TAGS_ENUM.EndDate]);

                            Title title = new Title(startDate?.Value, endDate?.Value);
                            Faculty faculty = new Faculty(department?.Value, part?.Value);
                            Employee employee = new Employee(id?.Value, name?.Value, faculty, laboratory?.Value, title);

                            employees.Add(employee);
                        }
                    }
                }
            }
            catch(ArgumentException)
            {
                MessageBox.Show("Unable to find");
            }            

            return employees;
        }
    }
}
