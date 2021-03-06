using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using Entities.Attributes;

namespace DemoWebApp.Models.Attributes
{
    [ExportDemo("reset - attributes")]
    public class Reset : IDemo
    {
        #region Implementation of IDemo

        public IEnumerable<string> Run()
        {
            Database.SetInitializer(new OrtmanFamilyInitializer());
            var context = new FamilyMembersWithAttributes();
            context.Database.Delete();
            context.Database.Initialize(true);

            return Enumerable.Empty<string>();
        }

        #endregion
    }

    [ExportDemo("insert kid (attributes)")]
    public class InsertKidDemo : IDemo
    {
        public IEnumerable<string> Run()
        {
            Database.SetInitializer(new OrtmanFamilyInitializer());
            var context = new FamilyMembersWithAttributes();

            //since i know i've only just created 1 dad, i just hardcode the ID
            var chris = context.Dads.First(x => x.FirstName == "Chris");

            var clara = new Kid() {Name = "Clara", Birthday = DateTime.Parse("1/21/2009")};
            chris.Kids.Add(clara);

            var message = new Message("Data inserted via attributes demo");
            context.Messages.Add(message);
            context.SaveChanges();

            return Enumerable.Empty<string>();
        }
    }

    public class FamilyMembersWithAttributes : DbContext
    {
        public FamilyMembersWithAttributes() : base("FamilyMembers")
        {
        }

        public DbSet<Dad> Dads { get; set; }

        public DbSet<Message> Messages { get; set; }
    }

    public class Message
    {
        public Message()
        {
        }

        public Message(string text)
        {
            Text = text;
        }

        [Key]
        public int ID { get; set; }

        [Column]
        public string Text { get; set; }
    }
    [Table("Dads")]
    public class Dad
    {
        public Dad()
        {
            Kids = new List<Kid>();
        }

        [Key]
        public int ID { get; set; }

        [Column]
        [MaxLength(5, ErrorMessage = "First name can only be 5 characters")]
        public string FirstName { get; set; }

        [Column]        
        public DateTime DayOfBirth { get; set; }

        public Address Address { get; set; }

        
        public virtual List<Kid> Kids { get; set; }

        public override string ToString()
        {
            return String.Format("[{0}] - {1}", ID, FirstName);
        }

        public string LongToString()
        {
            var s = String.Format("[{0}] {1} was born on {2} and lives at {3}\n", ID, FirstName, DayOfBirth, Address);

            s += String.Format("\t{0} has {1} kids", FirstName, Kids.Count);
            if(Kids.Count > 0)
            {
                s += String.Format(" their names are {0}", String.Join(",", Kids.Select(x => x.ToString())));
            }
            return s;
        }
    }

    public class Kid
    {
        [Key]
        public int ID { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public DateTime Birthday { get; set; }

        [Column("Attributes_DadID")]
        public int DadID { get; set; }

        [ForeignKey("DadID")]
        public virtual Dad MyDad { get; set; }

        public TimeSpan Age
        {
            get { return DateTime.Now - Birthday; }
        }

        public override string ToString()
        {
            int remainder;
            int years = Math.DivRem(Convert.ToInt32(Age.TotalDays), 365, out remainder);
            return String.Format("{0} who is {1} years and {2} days old", Name, years, remainder);
        }
    }

    [ComplexType]
    public class Address
    {
        [Column]
        public string City { get; set; }

        [Column]
        public string State { get; set; }

        [Column]
        [MinLength(5,ErrorMessageResourceName = "FiveDigitZipCodePlease",ErrorMessageResourceType = typeof(ErrorMessages))]
        [MaxLength(5,ErrorMessageResourceName = "FiveDigitZipCodePlease",ErrorMessageResourceType = typeof(ErrorMessages))]        
        //[StringLength(5,MinimumLength = 5,ErrorMessageResourceName = "FiveDigitZipCodePlease",ErrorMessageResourceType = typeof(ErrorMessages))]
        public string Zip { get; set; }

        public override string ToString()
        {
            return String.Format("{0},{1} {2}", City, State, Zip);
        }
    }

    public class OrtmanFamilyInitializer : DropCreateDatabaseAlways<FamilyMembersWithAttributes>
    {
        protected override void Seed(FamilyMembersWithAttributes context)
        {
            var chris = context.Dads.Include(x => x.Kids).FirstOrDefault(x => x.FirstName == "Chris");
            if(chris != null)
            {
                chris.Kids.Clear();
                context.Dads.Remove(chris);
            }

            context.SaveChanges();

            var ethan = new Address()
            {
                City = "Ethan",
                State = "SD",
                Zip = "57334"
            };

            chris = new Dad
            {
                FirstName = "Chris",
                DayOfBirth = new DateTime(1979, 12, 6),
                Address = ethan,
                Kids = new List<Kid>
                {
                    new Kid()
                    {
                        Name = "Damon",
                        Birthday = DateTime.Parse("12/4/2003"),
                    },
                    new Kid()
                    {
                        Name = "Mason",
                        Birthday = DateTime.Parse("2/27/2005"),
                    }
                }
            };

            context.Dads.Add(chris);

            base.Seed(context);
        }
    }
}