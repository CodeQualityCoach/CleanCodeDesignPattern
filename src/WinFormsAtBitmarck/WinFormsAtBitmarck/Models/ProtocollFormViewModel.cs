using System.ComponentModel;
using System.Runtime.CompilerServices;
using Caliburn.Micro;
using MediatR;

namespace WinFormsAtBitmarck.Models;

public class ProtocollFormViewModel : INotifyPropertyChanged, IHandle<LogMessage>
{
    private readonly IEventAggregator _eventAggregator;

    public ProtocollFormViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

        _eventAggregator.SubscribeOnUIThread(this);
    }

    private string _log = string.Empty;
    public event PropertyChangedEventHandler? PropertyChanged = (sender, args) => { };

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string Log
    {
        get => _log;
        set
        {
            if (value == _log) return;
            _log = value;
            OnPropertyChanged();
        } 
    }

    public Task HandleAsync(LogMessage message, CancellationToken cancellationToken)
    {
        Log = string.Join(Environment.NewLine, message.Message, Log);
        return Task.CompletedTask;
    }
}

public class LogMessage : INotification
{
    public string Message { get; set; }
}