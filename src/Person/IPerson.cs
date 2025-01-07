namespace Person
{
    public interface IHaveUniqueIdentifier
    {
        int Id { get; set; }
    }

    public interface IAmPerson
         : IHaveUniqueIdentifier
    {
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}