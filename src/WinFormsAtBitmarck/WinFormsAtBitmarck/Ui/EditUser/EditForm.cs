using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using WinFormsAtBitmarck.MvvmFramework;

namespace WinFormsAtBitmarck.Ui.EditUser
{
    /// <summary>
    /// Edit form which uses the BindingSource for data binding
    /// </summary>
    public partial class EditForm : Form, IWindow
    {
        private readonly EditFormModel _formModel;

        [Obsolete("only for design time")]
        public EditForm()
        {
            InitializeComponent();
        }

        public EditForm(EditFormModel formModel)
        : this()
        {
            this.bindingSource.DataSource = formModel;
            _formModel = formModel ?? throw new ArgumentNullException(nameof(formModel));
        }

        public object FormModel => _formModel;
    }
}
