namespace Person
{
    /********************************************************
     * Aufgabe:
     * 
     * In welche Schnittstellen könnte man die Klasse "Person"
     * schneiden. Überlegt euch Schnittstellen im Sinne des
     * Interface Segregation Principle (ISP)
     ********************************************************/


    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public void Load(string path)
        {
            // Lesen der Daten aus der Datei...
        }

        public void Save(string path)
        {
            // Speichern der Daten in die Datei...
        }

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
