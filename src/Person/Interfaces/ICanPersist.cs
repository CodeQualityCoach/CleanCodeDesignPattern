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
}
