namespace EmployeesFileWork.Data
{
    public class Employee
    {
        public string Id { get; }
        public string Name { get; }
        public Faculty Faculty { get; }
        public string Laboratory { get; }
        public Title Title { get; }

        public Employee() { }   

        public Employee(string id, string name, Faculty faculty, string laboratory, Title title)
        {
            Id = id;
            Name = name;
            Faculty = faculty;
            Laboratory = laboratory;
            Title = title;
        }        
    }
}
