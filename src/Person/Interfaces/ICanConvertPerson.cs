namespace Person
{
    public interface ICanConvertPerson
    {
        string ToString(Person person);
        Person FromString(string json);
    }
}
