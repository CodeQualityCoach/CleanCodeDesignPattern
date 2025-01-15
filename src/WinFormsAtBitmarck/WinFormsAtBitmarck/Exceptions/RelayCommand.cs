using System.ComponentModel;
using System.Windows.Input;

namespace WinFormsAtBitmarck.Exceptions;

public class RelayCommand : ICommand, INotifyPropertyChanged
{
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    public RelayCommand(Action<object> execute, Predicate<object> canExecute, INotifyPropertyChanged parent)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        parent.PropertyChanged += (sender, args) => CanExecuteChanged?.Invoke(this, args);
    }

    public bool CanExecute(object? context)
    {
        return _canExecute(context);
    }

    public void Execute(object? context)
    {
        _execute(context);
    }

    public event EventHandler? CanExecuteChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
}