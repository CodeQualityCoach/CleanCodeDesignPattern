namespace Person
{

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
}
