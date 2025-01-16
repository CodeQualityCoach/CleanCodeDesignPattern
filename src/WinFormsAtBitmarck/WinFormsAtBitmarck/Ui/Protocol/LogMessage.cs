using MediatR;

namespace WinFormsAtBitmarck.Ui.Protocol;

public class LogMessage : INotification
{
    public string Message { get; set; }
}