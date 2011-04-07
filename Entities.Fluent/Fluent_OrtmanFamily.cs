using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace DemoWebApp.Models.Fluent
{
        [ExportDemo("reset - fluent")]
    public class Reset : IDemo
    {
        #region Implementation of IDemo

        public void Run()
        {
            Database.SetInitializer(new OrtmanFamilyInitializer());
            var context = new FamilyMembersWithFluentConfiguration();
            context.Database.Delete();
            context.Database.Initialize(true);
        }

        #endregion
    }
    [ExportDemo("insert kid (fluent)")]
    //[Export(typeof(IDemo))]
    //[ExportMetadata("DemoName","conventions")]
    public class InsertKidDemo : IDemo
    {
        public void Run()
        {
            Database.SetInitializer(new OrtmanFamilyInitializer());
            var context = new FamilyMembersWithFluentConfiguration();

            //since i know i've only just created 1 dad, i just hardcode the ID
            var chris = context.Dads.First(x => x.FirstName == "Chris");

            var clara = new Kid() {Name = "Clara", Birthday = DateTime.Parse("1/21/2009")};
            chris.Kids.Add(clara);

            var message = new Message("Data inserted via conventions demo");
            context.Messages.Add(message);

            context.SaveChanges();
        }
    }

    public class FamilyMembersWithFluentConfiguration : DbContext
    {
        public FamilyMembersWithFluentConfiguration() : base("FamilyMembers")
        {
        }

        public DbSet<Dad> Dads { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            var dadEntity = modelBuilder.Entity<Dad>();

            dadEntity
                .HasKey(x => x.ID);
            dadEntity.Property(x => x.FirstName);
            dadEntity.Property(x => x.DayOfBirth);
            //no way to say that address on dad is complex
            dadEntity.HasMany(x => x.Kids).WithRequired().Map(x => x.MapKey("Fluent_DadID"));
            
                
            var messageEntity = modelBuilder.Entity<Message>();
            messageEntity.HasKey(x => x.ID);
            messageEntity.Property(x => x.Text);

            var addressValueObject = modelBuilder.ComplexType<Address>();
            addressValueObject.Property(x => x.City);
            addressValueObject.Property(x => x.State);
            addressValueObject.Property(x => x.Zip);

            var kidEntity = modelBuilder.Entity<Kid>();
            kidEntity.HasKey(x => x.ID);
            kidEntity.Property(x => x.Name);
            kidEntity.Property(x => x.Birthday);

        }
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

        public int ID { get; set; }

        public string Text { get; set; }
    }
    
    public class Dad
    {
        public Dad()
        {
            Kids = new List<Kid>();
        }

        public int ID { get; set; }

        public string FirstName { get; set; }

        public DateTime DayOfBirth { get; set; }

        public Address Address { get; set; }


        public virtual List<Kid> Kids { get; set; }

        public override string ToString()
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
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

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

    public class Address
    {
        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public override string ToString()
        {
            return String.Format("{0},{1} {2}", City, State, Zip);
        }
    }

    public class OrtmanFamilyInitializer : DropCreateDatabaseAlways<FamilyMembersWithFluentConfiguration>
    {
        protected override void Seed(FamilyMembersWithFluentConfiguration context)
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
        }
    }

}