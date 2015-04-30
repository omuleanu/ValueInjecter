using System.Collections.Generic;
using System.Windows.Forms;

namespace WinFormsFlattenSample
{
    public static class  Extensions
    {
        public static IEnumerable<Control> GetChildControls(this Control parent)
        {
            var result = new List<Control>();
            foreach (Control control in parent.Controls)
            {
                result.Add(control);
                result.AddRange(control.GetChildControls());
            }
            return result;
        }

        public static IEnumerable<Control> GetChildControls<T>(this Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if(control.GetType().Equals(typeof(T)))
                    yield return control;
                foreach(var c in control.GetChildControls<T>()) yield return c;
            }
        }
    }
}
