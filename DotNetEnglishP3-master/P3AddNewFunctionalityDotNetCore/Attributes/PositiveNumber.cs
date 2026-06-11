using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace P3AddNewFunctionalityDotNetCore.Attributes
{
    public class PositiveNumber : ValidationAttribute
    {
        public override bool IsValid(object source)
        {
            var value = source as string;

            value = value.Replace(',', '.');

            if (float.TryParse(
                    value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out float result)
                && result >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
