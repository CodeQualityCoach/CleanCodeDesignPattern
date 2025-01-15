namespace WinFormsAtBitmarck.Data;

public interface IGreetingRepository
{
    string GetGreeting(IDateTimeProvider dateTime);

}