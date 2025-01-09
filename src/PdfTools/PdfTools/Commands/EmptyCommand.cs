namespace PdfTools.Commands
{
    public class EmptyCommand : ICommand
    {
        public bool CanExecute(string[] context)
        {
            return true;
        }

        public void Execute(string[] context)
        {
        }
    }
}