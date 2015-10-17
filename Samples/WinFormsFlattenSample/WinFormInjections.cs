using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Flat;
using Omu.ValueInjecter.Injections;
using Omu.ValueInjecter.Utils;

namespace WinFormsFlattenSample
{
    public class SameNameFlat : ValueInjection
    {
        protected override void Inject(object source, object target)
        {
            foreach (var targetProp in target.GetType().GetProperties())
            {
                var endpoints = UberFlatter.Flat(targetProp.Name, source);
                if (endpoints.Count() == 0) continue;

                var desc = endpoints.First();

                var result = Convert.ChangeType(desc.Property.GetValue(desc.Component), targetProp.PropertyType);
                targetProp.SetValue(target, result);
            }
        }
    }

    public class SameNameUnflat : ValueInjection
    {
        protected override void Inject(object source, object target)
        {
            foreach (var sourceProp in source.GetType().GetProperties())
            {
                var endpoints = UberFlatter.Unflat(sourceProp.Name, target);
                if (endpoints.Count() == 0) continue;

                var desc = endpoints.First();

                var prop = sourceProp.GetValue(source);
                var result = prop == null ? prop : Convert.ChangeType(prop, desc.Property.PropertyType);
                desc.Property.SetValue(desc.Component, result);
            }
        }
    }

    public class TextBoxToString : KnownSourceInjection<Control>
    {
        protected override void Inject(Control request, object target)
        {
            foreach (var control in request.GetChildControls())
            {
                if (control.Text == string.Empty) continue;

                var endpoints = UberFlatter.Unflat(StrUtil.RemovePrefix(control.Name, "txt"), target);
                if(endpoints.Count() == 0) continue;

                var desc = endpoints.First();


                var c = TypeDescriptor.GetConverter(desc.Property.PropertyType);
                try
                {
                    desc.Property.SetValue(desc.Component, c.ConvertFrom(control.Text));
                }
                catch
                {
                    //add form validaton and remove this 
                }
            }
        }
    }

    public class StringToTextBox : KnownTargetInjection<Control>
    {
        protected override void Inject(object source, ref Control target)
        {
            foreach (var txt in target.GetChildControls())
            {
                var es = UberFlatter.Flat(StrUtil.RemovePrefix(txt.Name, "txt"), source);
                if (es.Count() == 0) continue;
                var desc = es.First();
                txt.Text = (desc.Property.GetValue(desc.Component) ?? "").ToString();
            }
        }
    }

    public class DateTimePickerToDateTime : KnownSourceInjection<Control>
    {
        protected override void Inject(Control request, object target)
        {
            foreach (DateTimePicker dt in request.GetChildControls<DateTimePicker>())
            {
                var es = UberFlatter.Unflat(StrUtil.RemovePrefix(dt.Name, "dt"), target);
                if(es.Count() == 0) continue;
                var desc = es.First();
                desc.Property.SetValue(desc.Component, dt.Value);
            }
        }
    }

    public class DateTimeToDateTimePicker : KnownTargetInjection<Control>
    {
        protected override void Inject(object source, ref Control target)
        {
            foreach (DateTimePicker dt in target.GetChildControls<DateTimePicker>())
            {
                var es = UberFlatter.Flat(dt.Name, source);
                if(es.Count() == 0) continue;
                var desc = es.First();
                dt.Value = (DateTime)desc.Property.GetValue(desc.Component);
            }
        }
    }
}