namespace WinFormsAtBitmarck.Core.Services;

public interface IGreetingRepository
{
    string GetGreeting(IDateTimeProvider dateTime);

}