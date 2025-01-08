namespace Person
{
    public class Prog
    {
        public void Main()
        {
            // laden einer person
            //var factory = new PersonFactory(new FilePersister(), new JsonPersonConverter());
            var factory = new PersonFactory(new DatabasePersister(), new XmlPersonConverter());
            var person =factory.LoadFromSomewhereAsSomewhat("c:/foo.json");

        }
    }
}
