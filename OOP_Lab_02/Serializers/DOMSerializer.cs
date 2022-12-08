using EmployeesFileWork.Data;
using System.Xml;

namespace EmployeesFileWork.Serializers
{
    internal class DOMSerializer : ISerializer
    {
        public override bool Serialize(out string xmlURL, List<Employee> items)
        {
            return base.Serialize(out xmlURL, items);
        }
        public override bool Deserialize(string xmlURL, out List<Employee> employees)
        {
            employees = new List<Employee>();
            XmlDocument doc = new XmlDocument();

            doc.Load(xmlURL);

            for (int i = 0; i < GetXMLObjectsCount(doc); i ++)
            {
                var employee = new Employee(GetAttributeValue(TAGS[TAGS_ENUM.Id], doc, i),
                                            GetAttributeValue(TAGS[TAGS_ENUM.Name], doc, i),
                                            new Faculty(GetAttributeValue(TAGS[TAGS_ENUM.Department], doc, i),
                                                        GetAttributeValue(TAGS[TAGS_ENUM.Part], doc, i)),
                                            GetAttributeValue(TAGS[TAGS_ENUM.Laboratory], doc, i),
                                            new Title(GetAttributeValue(TAGS[TAGS_ENUM.StartDate], doc, i),
                                                      GetAttributeValue(TAGS[TAGS_ENUM.EndDate], doc, i)));
                employees.Add(employee);
            }

            return employees is not null;
        }

        public override List<Employee> FindByTag(string xmlURL, string tag)        
        {
            if (xmlURL == null)
            {
                MessageBox.Show("No file given.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }

            if (tag is null)
            {
                MessageBox.Show("Tag is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new List<Employee>();
            }

            var result = new List<Employee>();             
            XmlDocument doc = new XmlDocument();
            List<int> indexes = new List<int>();

            doc.Load(xmlURL);

            DivideTag(tag, out string tAttribute, out string tPhrase);

            for (int i = 0; i < 7; i++)
            {
                if(TAGS[(TAGS_ENUM)i] == tAttribute)
                {                   
                    List<string> Collection = new List<string>();
                    SetCollection(TAGS[(TAGS_ENUM)i], doc, out Collection);
                    for(int k = 0; k < Collection.Count; k++)
                    {
                        if (Collection[k].Contains(tPhrase))
                        {
                            indexes.Add(k);
                        }
                    }
                }
            }

            foreach(int index in indexes)
            {
                var employee = new Employee(GetAttributeValue(TAGS[TAGS_ENUM.Id], doc, index),
                                            GetAttributeValue(TAGS[TAGS_ENUM.Name], doc, index),
                                            new Faculty(GetAttributeValue(TAGS[TAGS_ENUM.Department], doc, index),
                                                        GetAttributeValue(TAGS[TAGS_ENUM.Part], doc, index)),
                                            GetAttributeValue(TAGS[TAGS_ENUM.Laboratory], doc, index),
                                            new Title(GetAttributeValue(TAGS[TAGS_ENUM.StartDate], doc, index),
                                                      GetAttributeValue(TAGS[TAGS_ENUM.EndDate], doc, index)));
                result.Add(employee);
            }

            return result;
        }

        /// <summary>
        /// Returns amount of objects in XML document
        /// </summary>
        private int GetXMLObjectsCount(XmlDocument doc)
        {
            int[] CollectionsCount = new int[7];

            for (int i = 0; i < CollectionsCount.Length; i++)
            {
                SetCollection(TAGS[(TAGS_ENUM)i], doc, out List<string> Collection);
                CollectionsCount[i] = Collection.Count;
            }

            return CollectionsCount.Max();
        }


        /// <summary>
        /// Divides tag to tAttribute and tPhrase strings
        /// </summary>
        private void DivideTag(string tag, out string tAttribute, out string tPhrase)
        {
            tAttribute = "";
            tPhrase = "";
            for(int i = 0; i < tag.Length; i++)
            {
                if(tag[i] == '(')
                {
                    for(int j = 0; j < i; j++)
                    {
                        tPhrase += tag[j];
                    }
                    int k = i + 1;
                    while(tag[k] != ')')
                    {
                        tAttribute += tag[k];
                        k++;
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Returns a specific XML Node Value by attribute
        /// </summary>
        private string GetAttributeValue(string attributeName, XmlDocument doc, int index)
        {
            string result;
            List<string> values = new List<string>();
            SetCollection(attributeName, doc, out values);
            result = values[index];
            return result;
        }

        /// <summary>
        /// Sets a List of XML Node Values by attribute
        /// </summary>
        private void SetCollection(string attributeName, XmlDocument doc, out List<string> tableObject)
        {
            int i = 0;
            tableObject = new List<string>();
            while (i < doc.GetElementsByTagName(attributeName).Count && doc.GetElementsByTagName(attributeName)[i].ChildNodes[0].Value != null)
            {
                string temp = doc.GetElementsByTagName(attributeName)[i].ChildNodes[0].Value;
                tableObject.Add(temp);
                i++;
            }
        }
    }
}
