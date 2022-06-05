using System.Globalization;
using System.Windows.Controls;

namespace GeoAppAtmClient.ValidationRules
{
    public class AtmBalanceValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse((string)value, out var balance))
            {
                return balance < 0 ? new ValidationResult(false, "Баланс должен быть положительным") : ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Введенное значение не является числом");
        }
    }
}
