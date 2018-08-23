using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfOpgave8
{
    public class PrijsRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            NumberStyles style = NumberStyles.Currency;
            decimal prijs = 0;
            if(!decimal.TryParse(value.ToString(), style, cultureInfo, out prijs))
                return new ValidationResult(false, "Getal moet ingevuld zijn");
            if (prijs <= 0)
                return new ValidationResult(false, "Getal moet groter zijn dan 0");
            else
                return ValidationResult.ValidResult;
        }
    }
}
