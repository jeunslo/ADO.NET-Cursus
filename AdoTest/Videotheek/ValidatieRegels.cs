using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Videotheek
{
    public class GetalRuleGroterDan0 : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            NumberStyles style = NumberStyles.Currency;
            decimal prijs = 0;
            if (string.IsNullOrEmpty(value.ToString()))
                return new ValidationResult(false, "Getal moet ingevuld zijn");
            if (!decimal.TryParse(value.ToString(), style, cultureInfo, out prijs))
                return new ValidationResult(false, "Waarde moet een getal zijn");
            if (prijs <= 0)
                return new ValidationResult(false, "Getal moet groter zijn dan 0");
            else
                return ValidationResult.ValidResult;
        }
    }

    public class GetalRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Int32 getal = 0;
            if (string.IsNullOrEmpty(value.ToString()))
                return new ValidationResult(false, "Getal moet ingevuld zijn");
            if (!Int32.TryParse(value.ToString(), out getal))
                return new ValidationResult(false, "Waarde moet een getal zijn");
            else
                return ValidationResult.ValidResult;
        }
    }

    public class TextRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult(false, "waarde moet ingevuld zijn");
            else
                return ValidationResult.ValidResult;
        }
    }

    public class GenreCBRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult(false, "Genre moet gekozen worden");
            else
                return ValidationResult.ValidResult;
        }
    }
}
