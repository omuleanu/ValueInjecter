using Omu.ValueInjecter;

namespace DALSample
{
    public class WhereInjection : KnownTargetValueInjection<string>
    {
        protected override void Inject(object source, ref string target)
        {
            var sourceProps = source.GetProps();
            for (var i = 0; i < sourceProps.Count; i++)
            {
                if (i != 0) target += " and ";

                var p = sourceProps[i];
                target += p.Name + "=@" + p.Name;
            }
        }
    }
}