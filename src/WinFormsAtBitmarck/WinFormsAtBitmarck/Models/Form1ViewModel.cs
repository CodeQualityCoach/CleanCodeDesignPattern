using System.ComponentModel;
using System.Runtime.CompilerServices;
using Caliburn.Micro;
using MediatR;
using WinFormsAtBitmarck.Data;
using WinFormsAtBitmarck.Views;

namespace WinFormsAtBitmarck.Models;

public class Form1ViewModel : INotifyPropertyChanged
{
    private readonly IGreetingRepository _greetings;
    private readonly IDateTimeProvider _dateTime;
    private readonly IWindowManager _windowManager;
    private readonly IEventAggregator _eventAggregator;
    private string _name;

    public Form1ViewModel(
        IGreetingRepository greetings, 
        IDateTimeProvider dateTime, 
        IWindowManager windowManager,
        IEventAggregator eventAggregator)
    {
        _greetings = greetings ?? throw new ArgumentNullException(nameof(greetings));
        _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
        _windowManager = windowManager ?? throw new ArgumentNullException(nameof(windowManager));
        _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

        Name =  "Thomas";
    }

    public string Name
    {
        get => _name;
        set
        {
            if (value == _name) return;
            _name = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanDoSomethingElse));
        }
    }

    public void Save()
    {
        _eventAggregator.PublishOnUIThreadAsync(new LogMessage() {Message = $"{_dateTime.Now}\t{_greetings.GetGreeting(_dateTime)} {this.Name}"});
        MessageBox.Show($"{_greetings.GetGreeting(_dateTime)} {this.Name}");
    }

    public void DoSomethingElse(object context)
    {
        MessageBox.Show($"Your name is {Name}");
    }

    public bool CanDoSomethingElse(object context)
    {
        return !string.IsNullOrWhiteSpace(Name);
    }

    public void OpenProtocolForm(object context)
    {
        // irgendeine Abhängigkeit wird benötigt ... in der Regel das ViewModel
        _windowManager.Show<ProtocollForm>();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}