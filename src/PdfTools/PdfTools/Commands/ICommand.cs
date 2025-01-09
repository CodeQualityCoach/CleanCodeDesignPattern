namespace PdfTools.Commands
{
    public interface ICommand
    {
        bool CanExecute(string[] context);
        void Execute(string[] context);
    }
}