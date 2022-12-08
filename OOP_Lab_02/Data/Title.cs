namespace EmployeesFileWork.Data
{
    public class Title
    {
        public string StartDate { get; }
        public string EndDate { get; }  

        public Title() { }

        public Title(string startDate, string endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public override string ToString()
        {
            return $"{StartDate}, {EndDate}";
        }
    }
}
