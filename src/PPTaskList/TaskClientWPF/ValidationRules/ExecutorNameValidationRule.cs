using System;
using System.Globalization;
using System.Windows.Controls;

namespace TaskClientWPF.ValidationRules
{
    public class ExecutorNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return String.IsNullOrEmpty((string)value) ?
                new ValidationResult(false, "Заполните поле") : ValidationResult.ValidResult;
        }
    }
}
