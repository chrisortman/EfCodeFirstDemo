using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Common;

namespace DemoWebApp.Models.Attributes
{
    [ExportDemo("validation with max length of name example")]
    public class ValidationWithMaxLengthOfNameExample : IDemo
    {
        #region Implementation of IDemo

        public IEnumerable<string> Run()
        {
             List<string> errorMessages = new List<string>();
            Database.SetInitializer(new DropCreateDatabaseAlways<FamilyMembersWithAttributes>());
            var context = new FamilyMembersWithAttributes();

            var chris = new Dad()
            {
                FirstName = "Chris this is a really long name........",
                Address = new Address()
            };

            try
            {
                context.Dads.Add(chris);

                errorMessages.AddRange(ValidationHelper.ExtractValidationMessages(context));

                context.SaveChanges();
            }
            catch(DbEntityValidationException ex)
            {
                errorMessages.Add("Exception thrown when trying to save dad " + ex.ToString());
            }

            return errorMessages;
        }

        #endregion
    }

      [ExportDemo("validation with max length of zip example (attributes)")]
    public class ValidationWithLengthOfZipCodeExample : IDemo
    {
        #region Implementation of IDemo

        public IEnumerable<string> Run()
        {
             List<string> errorMessages = new List<string>();
            Database.SetInitializer(new DropCreateDatabaseAlways<FamilyMembersWithAttributes>());
            var context = new FamilyMembersWithAttributes();

            var chris = new Dad()
            {
                FirstName = "Chris",
                Address = new Address()
                {
                    City = "Ethan",
                    State = "SD",
                    Zip = "9999999999"
                }
            };

            var otherDad = new Dad()
            {
                FirstName = "Dad with short zip code",
                Address = new Address()
                {
                    Zip = "1"
                }
            };

            
            
            try
            {
                context.Dads.Add(chris);
                context.Dads.Add(otherDad);
                
                errorMessages.AddRange(ValidationHelper.ExtractValidationMessages(context));

                context.SaveChanges();
            }
            catch(DbEntityValidationException ex)
            {
                errorMessages.Add("Exception thrown when trying to save dad " + ex.ToString());
            }

            return errorMessages;
        }

        #endregion
    }
}