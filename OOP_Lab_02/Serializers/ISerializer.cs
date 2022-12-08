using EmployeesFileWork.Data;
using System.Text.RegularExpressions;

namespace EmployeesFileWork.Serializers
{
    internal class ISerializer
    {
        public enum TAGS_ENUM
        {
            Id,
            Name,
            Department,
            Part,
            Laboratory,
            StartDate,
            EndDate,
            Faculty,
            Title,
            Employee,
        }

        public static readonly Dictionary<TAGS_ENUM, string> TAGS = new Dictionary<TAGS_ENUM, string> () 
        {
            {TAGS_ENUM.Id, "Id"},
            {TAGS_ENUM.Name, "Name"},
            {TAGS_ENUM.Department,"Department"},
            {TAGS_ENUM.Part,"Part"},
            {TAGS_ENUM.Laboratory, "Laboratory"},
            {TAGS_ENUM.StartDate, "StartDate"},
            {TAGS_ENUM.EndDate, "EndDate"},
            {TAGS_ENUM.Faculty, "Faculty"},
            {TAGS_ENUM.Title, "Title"},
            {TAGS_ENUM.Employee, "Employee"},
        };

        /// <summary>
        /// Serializes <see cref="Employee"/> to XML string
        /// </summary>
        public virtual bool Serialize(out string xmlURL, List<Employee> items)
        {
            xmlURL = string.Empty;
            return false;
        }

        /// <summary>
        /// Deserializes from XML file
        /// </summary>
        public virtual bool Deserialize(string xmlURL, out List<Employee> items)
        {
            items = null;
            return false;
        }

        /// <summary>
        /// Looks for certain tag in items
        /// </summary>
        public virtual List<Employee> FindByTag(string xmlURL, string tag)
        {
            return null;
        }

        const string ValueGroupName = "Value";
        const string AttributeGroupName = "Attribute";

        const string IdValuePattern = "\\d+";
        const string NameValuePattern = "[A-Za-z_]+";
        const string DateValuePattern = "\\d\\d/\\d\\d/\\d\\d"; // dd\mm\yy

        const string FullPattern = $"((?<Value>{IdValuePattern})\\((?<{AttributeGroupName}>Id)\\))" +
            $"|((?<{ValueGroupName}>{NameValuePattern})\\((?<{AttributeGroupName}>Name)\\))" +
            $"|((?<{ValueGroupName}>{NameValuePattern})\\((?<{AttributeGroupName}>Department)\\))" +
            $"|((?<{ValueGroupName}>{NameValuePattern})\\((?<{AttributeGroupName}>Part)\\))" +
            $"|((?<{ValueGroupName}>{NameValuePattern})\\((?<{AttributeGroupName}>Laboratory)\\))" +
            $"|((?<{ValueGroupName}>{DateValuePattern})\\((?<{AttributeGroupName}>StartDate)\\))" +
            $"|((?<{ValueGroupName}>{DateValuePattern})\\((?<{AttributeGroupName}>EndDate)\\))";

        /// <summary>
        /// Returns dictionary of attribute and value to look for
        /// </summary>
        protected Dictionary<string, string> ProcessTagPhrase(string tag)
        {
            Regex regex = new Regex(FullPattern);

            var result = new Dictionary<string, string>();

            foreach (Match match in regex.Matches(tag))
            {
                Group valueGroup = match.Groups[ValueGroupName];
                Group attributeGroup = match.Groups[AttributeGroupName];

                result.TryAdd(attributeGroup.Value, valueGroup.Value);
            }
            return result;
        }
    }
}
