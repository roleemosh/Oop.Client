namespace Oop.Client.Model
{
    public record Person
    {
        public Guid EmployeeID { get; set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public byte AccessLevel { get; private set; }
        public Person(Guid employeeID, string firstName, string lastName, byte accessLevel)
        {
            this.EmployeeID = employeeID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.AccessLevel = accessLevel;
        }
    }
}
