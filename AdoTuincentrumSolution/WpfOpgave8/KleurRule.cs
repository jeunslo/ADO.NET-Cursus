using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfOpgave8
{
    public class KleurRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex regex = new Regex("^[a-z|A-Z]+$");
            if(string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult(false, "Veld moet ingevuld zijn");
            if (!regex.IsMatch(value.ToString()))
                return new ValidationResult(false, "Voer een geldige kleur in");
            else
                return ValidationResult.ValidResult;
        }
    }
}
