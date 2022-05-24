using System;
using System.Globalization;
using System.Windows.Controls;

namespace Client.ValidationRules
{
    internal class DateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = (string)value;
            return (!string.IsNullOrWhiteSpace(val) && !string.IsNullOrEmpty(val) && DateTime.TryParse(val, out var _)) 
                ? ValidationResult.ValidResult 
                : new ValidationResult(false, "The entered value is not a date and time");
        }
    }
}
