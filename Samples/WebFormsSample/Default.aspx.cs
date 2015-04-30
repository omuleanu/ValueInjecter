using System;
using System.Web.UI;
using Omu.ValueInjecter;

namespace WebFormsSample
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var o = new MyWebEntity
                            {
                                FirstName = "Jimmy",
                                LastName = "Programmer",
                                Address = "Internetz"
                            };
                this.InjectFrom<StringToTextBox>(o);

                Response.Write(
                    string.Format("Hello, the values from the entity FirstName: <b>{0}</b>, LastName: <b>{1}</b>, Address <b>{2}</b> where injected into the page",
                    o.FirstName, o.LastName, o.Address));
                
            }

        }

        protected void btnSumbit_Click(object sender, EventArgs e)
        {
            var o = new MyWebEntity();
            o.InjectFrom<TextBoxToString>(this);

            Response.Write(
                string.Format("The values where injected into the entity: FirstName: <b>{0}</b>, LastName: <b>{1}</b>, Address: <b>{2}</b>", 
                o.FirstName,o.LastName, o.Address));
        }
    }
}
