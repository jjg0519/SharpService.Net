using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SharpService.Utilities
{
    public static class ValidateUtil
    {
        public static bool ValidateEntity<T>(T model, out string msg)
        {
            msg = string.Empty;

            string validationString = string.Empty;
            var b = ValidationModel(new ValidationContext(model, null, null), out validationString);
            if (!b)
            {
                msg = validationString;
                return false;
            }
            return true;
        }

        public static bool ValidationModel(ValidationContext validationObj, out string msg)
        {
            msg = string.Empty;

            var valResult = new List<ValidationResult>();
            var b = Validator.TryValidateObject(validationObj.ObjectInstance, validationObj, valResult, true);
            if (b == false)
            {
                msg = String.Join("\n", valResult.Select(o => o.ErrorMessage));

            }
            return b;
        }

    }
}
