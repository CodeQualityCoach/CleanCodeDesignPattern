namespace WinFormsAtBitmarck.Data;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    //public DateTime Now { get; } = DateTime.Now;
}