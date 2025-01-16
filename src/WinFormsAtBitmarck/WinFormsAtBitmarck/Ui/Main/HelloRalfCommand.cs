using System.Windows.Input;

namespace WinFormsAtBitmarck;

public class HelloRalfCommand : ICommand
{
    bool _canExecute = true;

    public bool CanExecute(object? parameter)
    {
        return _canExecute;
    }

    public void Execute(object? parameter)
    {
        MessageBox.Show("Hello Ralf");

        _canExecute = false;
        CanExecuteChanged(this, EventArgs.Empty);
    }

    public event EventHandler? CanExecuteChanged;
}