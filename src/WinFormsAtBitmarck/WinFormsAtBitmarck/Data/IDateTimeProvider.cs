namespace WinFormsAtBitmarck.Data;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}