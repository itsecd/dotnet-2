using System.Globalization;
using System.Windows.Controls;

namespace Client.ValidationRules
{
    internal class EventFrequencyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = (string)value;
            return (!string.IsNullOrWhiteSpace(val) && !string.IsNullOrEmpty(val) && int.TryParse(val, out var data) && data > 0 && data < 8) 
                ? ValidationResult.ValidResult 
                : new ValidationResult(false, "The entered value is not an integer or does not fall within the range of values 1-7");
        }
    }
}
