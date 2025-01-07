namespace Person
{
    /********************************************************
     * Aufgabe:
     * 
     * In welche Schnittstellen könnte man die Klasse "Person"
     * schneiden. Überlegt euch Schnittstellen im Sinne des
     * Interface Segregation Principle (ISP)
     ********************************************************/

    public interface ICanPersist
    {
        string Load(string path);
        void Save(string path, string serializedString);
    }

    public class FilePersister : ICanPersist
    {
        public string Load(string path)
        {
            throw new System.NotImplementedException();
        }

        public void Save(string path, string serializedString)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface ICanConvertPerson
    {
        string ToString(Person person);
        Person FromString(string json);
    }

    public class JsonPersonConverter : ICanConvertPerson
    {
        public Person FromString(string json)
        {
            // konvertiert json nach person
            var x = new Person();
            return x;
        }

        public string ToString(Person person)
        {
            throw new System.NotImplementedException();
        }
    }

    public class PersonFactory
    {
        private readonly ICanPersist persister;
        private readonly ICanConvertPerson personConverter;

        public PersonFactory(ICanPersist persister, ICanConvertPerson personConverter)
        {
            this.persister = persister;
            this.personConverter = personConverter;
        }


        // somewhere datei oder www, somewhat xml oder json
        public Person LoadFromSomewhereAsSomewhat(string persistentLocationPathOrWWW)
        {
            var srcString = persister.Load(persistentLocationPathOrWWW);
            var result = personConverter.FromString(srcString);

            return result;
        }
    }

    public class Person : IAmPerson
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ToJsonString()
        {
            // Zurückliefern der Daten als JSON-Zeichenkette...
            var jsonString = "...";
            return jsonString;
        }

        public string ToXmlString()
        {
            // Zurückliefern der Daten als XML-Zeichenkette...
            var xmlString = "...";
            return xmlString;
        }
    }

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
