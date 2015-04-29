using System;
using System.Windows.Forms;
using Omu.ValueInjecter;

namespace WinFormsSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var o = new MyEntity()
                        {
                            FirstName = "Jhon",
                            LastName = "Smith",
                            Phone = "12312312",
                        };
            this.InjectFrom<StringToTextBox>(o);

            txtResult.Text = string.Format("The form was filled with the obj o: {0},{1},{2}",
                                           o.FirstName, o.LastName, o.Phone);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var o = new MyEntity();
            o.InjectFrom<TextBoxToString>(this);

            txtResult.Text = string.Format("The object o got values from the form : {0},{1},{2}",
                                          o.FirstName, o.LastName, o.Phone);
        }
    }
}
