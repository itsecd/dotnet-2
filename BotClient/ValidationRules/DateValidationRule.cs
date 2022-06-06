using System;
using System.Globalization;
using System.Windows.Controls;

namespace BotClient.ValidationRules
{
    class DateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = (string)value;
            return !string.IsNullOrWhiteSpace(val) && DateTime.TryParse(val, out _)
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "The entered value is not a date and time");
        }
    }
}
