using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Data;
using AdoGemeenschap;

namespace AdoWPF
{
    public class PostcodeRangeRule0 : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int postcode = 0;
            if (value is BindingGroup)
                postcode = ((Brouwer)(value as BindingGroup).Items[0]).Postcode;
            else
            {
                if (!int.TryParse(value.ToString(), out postcode))
                    return new ValidationResult(false, "Gelieve een getal in te geven");
            }
            if (postcode == 0)
                return ValidationResult.ValidResult;
            if ((postcode < 1000) || (postcode > 9999))
                return new ValidationResult(false, "de postcode moet tussen 1000 en 9999 ligggen");
            else
                return ValidationResult.ValidResult;
        }
    }
}
