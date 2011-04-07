using System.Collections.Generic;
using System.Data.Entity;

namespace Common
{
    public static class ValidationHelper
    {
        public static IEnumerable<string> ExtractValidationMessages(DbContext context)
        {
            var errors = context.GetValidationErrors();
            foreach(var error in errors)
            {
                yield return "Validation error on " + error.Entry.Entity.GetType().FullName + "{" + error.Entry.Entity.ToString() + "}";
                foreach(var errorMessage in error.ValidationErrors)
                {
                    yield return "Property " + errorMessage.PropertyName + " " + errorMessage.ErrorMessage;
                }
            }
        }
    }
}