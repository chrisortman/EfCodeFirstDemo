using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace DemoWebApp.Models.Fluent
{
    [ExportDemo("insert dad with required birthday (fluent)")]
    public class ValidationExampleWithBirthDayRequired : IDemo
    {
        #region Implementation of IDemo

        public IEnumerable<string> Run()
        {
            List<string> errorMessages = new List<string>();
            Database.SetInitializer(new DropCreateDatabaseAlways<FamilyMembersWithValidation>());
            var context = new FamilyMembersWithValidation();

            var chris = new Dad()
            {
                FirstName = "Chris",
                Address = new Address()
            };

            try
            {
                context.Dads.Add(chris);

                var errors = context.GetValidationErrors();
                foreach(var error in errors)
                {
                    errorMessages.Add("Validation error on " + error.Entry.Entity.GetType().FullName);
                    foreach(var errorMessage in error.ValidationErrors)
                    {
                        errorMessages.Add(
                            "Property " + errorMessage.PropertyName + " " + errorMessage.ErrorMessage);
                    }
                }
                errorMessages.Add("Inserted dad [charles] without setting a birthday");

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

    public class FamilyMembersWithValidation : FamilyMembersWithFluentConfiguration
    {
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dad>()
                .Property(x => x.DayOfBirth).IsRequired();
        }
    }
}