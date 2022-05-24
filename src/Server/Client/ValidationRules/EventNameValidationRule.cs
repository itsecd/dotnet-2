using System.Globalization;
using System.Windows.Controls;

namespace Client.ValidationRules
{
    internal class EventNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = (string)value;
            return !string.IsNullOrWhiteSpace(val)  
                ? ValidationResult.ValidResult 
                : new ValidationResult(false, "Name not entered");
        }
    }
}
