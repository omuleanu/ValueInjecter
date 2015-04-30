using System.Web.UI;
using System.Web.UI.WebControls;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

namespace WebFormsSample
{
    public class TextBoxToString : KnownSourceInjection<Control>
    {
        protected override void Inject(Control request, object target)
        {
            var targetProps = target.GetType().GetProperties();
            foreach (var tp in targetProps)
            {
                if (tp.PropertyType != typeof(string)) continue;

                var control = request.FindControl("txt" + tp.Name);
                if (control == null || control.GetType() != typeof(TextBox)) continue;

                tp.SetValue(target, ((TextBox)control).Text);
            }
        }
    }

    public class StringToTextBox : KnownTargetInjection<Control>
    {
        protected override void Inject(object source, ref Control target)
        {
            var sourceProps = source.GetType().GetProperties();
            foreach (var sourceProp in sourceProps)
            {
                if (sourceProp.PropertyType != typeof(string)) continue;

                var control = target.FindControl("txt" + sourceProp.Name);
                if (control == null || control.GetType() != typeof(TextBox)) continue;

                ((TextBox) control).Text = (string) sourceProp.GetValue(source);
            }
        }
    }
}