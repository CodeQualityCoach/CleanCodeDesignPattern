using System;
using PdfTools.PdfServices;

namespace PdfTools.Commands
{
    public class AddCodeCommand : ICommand
    {
        public bool CanExecute(string[] context)
        {
            return string.Equals(context[0], "addcode", StringComparison.CurrentCultureIgnoreCase) && context.Length == 4;
        }

        public void Execute(string[] context)
        {
            var enhancer = new PdfCodeEnhancer(context[2]);

            enhancer.AddTextAsCode(context[1]);

            if (context.Length == 5)
                enhancer.SaveAs(context[4]);
            else
                enhancer.SaveAs(context[2]);
        }
    }
}