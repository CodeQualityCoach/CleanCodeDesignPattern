using WinFormsAtBitmarck.Exceptions;
using WinFormsAtBitmarck.Models;

namespace WinFormsAtBitmarck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Form1(Form1ViewModel form1ViewModel, IWindowManager windowManager) : this()
        {
            // Verbinden der UI mit dem Formularmodel
            button1.Click += (sender, args) => form1ViewModel.Save();
            button2.Command = new RelayCommand(form1ViewModel.DoSomethingElse, form1ViewModel.CanDoSomethingElse, form1ViewModel);
            button3.Command = new RelayCommand(form1ViewModel.OpenProtocolForm, (ctx) => true, form1ViewModel);
            textBox1.DataBindings.Add("Text", form1ViewModel, nameof(form1ViewModel.Name), true, DataSourceUpdateMode.OnPropertyChanged);
            textBox2.DataBindings.Add("Text", form1ViewModel, nameof(form1ViewModel.Name), true, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}
