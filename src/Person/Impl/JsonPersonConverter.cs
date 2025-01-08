namespace Person
{
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
}
