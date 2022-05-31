using System.Globalization;
using System.Windows.Controls;

namespace TaskClientWPF.ValidationRules
{
    public class TaskHeaderValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrEmpty((string)value) ?
                new ValidationResult(false, "Заполните поле") : ValidationResult.ValidResult;
        }
    }
}
