using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace P3AddNewFunctionalityDotNetCore.Attributes
{
    public class IntegerAttribute : ValidationAttribute
    {
        public override bool IsValid(object source)
        {
            var value = source as string;

            value = value.Replace(',', '.').Trim();

            if (!int.TryParse(
                    value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out int result))
            {
                return false;
            }

            return true;
        }
    }
}
