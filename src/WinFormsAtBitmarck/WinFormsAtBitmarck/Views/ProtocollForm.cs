using WinFormsAtBitmarck.Models;

namespace WinFormsAtBitmarck.Views
{
    public partial class ProtocollForm : Form
    {
        private readonly ProtocollFormViewModel _viewModel;

        public ProtocollForm()
        {
            InitializeComponent();
        }

        public ProtocollForm(ProtocollFormViewModel viewModel) : this()
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _viewModel.PropertyChanged += (sender, args) => textBox1.Text = _viewModel.Log;

            // Verbinden der UI mit dem Formularmodel
            textBox1.DataContext = _viewModel;
            textBox1.DataBindings.Add(nameof(textBox1.Text), _viewModel, nameof(_viewModel.Log), true, DataSourceUpdateMode.OnPropertyChanged);
        }
    }

}
