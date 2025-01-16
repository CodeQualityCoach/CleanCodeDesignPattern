using WinFormsAtBitmarck.Exceptions;
using WinFormsAtBitmarck.MvvmFramework;
using WinFormsAtBitmarck.Ui.EditUser;
using WinFormsAtBitmarck.Ui.Main;

namespace WinFormsAtBitmarck
{
    public partial class MainForm : Form
    {
        [Obsolete("only for design time")]
        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(MainFormModel mainFormModel, IWindowManager windowManager) : this()
        {

            helloRalfButton.Command = new FormModelAsParameterCommandDecorator(new HelloRalfCommand(), mainFormModel);

            editUserButton.Command = new RelayCommand((ctx) =>
            {
                windowManager.Show<EditForm>();
            }, (ctx) => true);
            editUserButton.CommandParameter = mainFormModel; // so the context is set properly

            textBox1.DataBindings.Add(
                nameof(textBox1.Text),
                mainFormModel,
                nameof(mainFormModel.Name),
                true,
                DataSourceUpdateMode.OnPropertyChanged);




            button2.Command = new FormModelAsParameterCommandDecorator(new RelayCommand(
                mainFormModel.DoSomethingElse,
                mainFormModel.CanDoSomethingElse), mainFormModel);




            // Verbinden der UI mit dem Formularmodel
            button1.Click += (sender, args) => mainFormModel.Save();
            button3.Command = new RelayCommand(mainFormModel.OpenProtocolForm, (ctx) => true);
            textBox2.DataBindings.Add("Text", mainFormModel, nameof(mainFormModel.Name), true, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}