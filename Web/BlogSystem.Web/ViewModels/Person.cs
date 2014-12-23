namespace BlogSystem.Web.ViewModels
{
    public class Person
    {
        public Person()
        {
            this.FullName = "JohnDow";
            this.Age = 34;
        }

        public string FullName { get; set; }

        public int Age { get; set; }
    }
}