using System.Windows.Forms;
using Omu.ValueInjecter.Injections;

namespace WinFormsSample
{
    public class TextBoxToString : KnownSourceInjection<Form>
    {
        protected override void Inject(Form form, object target)
        {
            var targetType = target.GetType();
            foreach (var control in form.Controls)
            {
                if (control.GetType() != typeof(TextBox)) continue;
                var txt = control as TextBox;

                var targetProp = targetType.GetProperty(txt.Name.Replace("txt", ""));
                if (targetProp == null || targetProp.PropertyType != typeof(string)) continue;

                targetProp.SetValue(target, txt.Text);
            }
        }
    }

    public class StringToTextBox : KnownTargetInjection<Form>
    {
        protected override void Inject(object source, ref Form form)
        {
            var sourceProps = source.GetType().GetProperties();
            foreach (var sourceProp in sourceProps)
            {
                var textBox = form.Controls["txt" + sourceProp.Name] as TextBox;
                if (textBox == null) continue;

                textBox.Text = (string)sourceProp.GetValue(source);
            }
        }
    }
}
