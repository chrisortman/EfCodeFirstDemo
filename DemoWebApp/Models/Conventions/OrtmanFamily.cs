using System;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using SqlMapper;

namespace DemoWebApp.Models.Conventions
{
    
    [ExportDemo("conventions")]
    //[Export(typeof(IDemo))]
    //[ExportMetadata("DemoName","conventions")]
    public class Demo : IDemo
    {
        public void Run()
        {
            Database.SetInitializer(new OrtmanFamilyInitializer());
            var context = new FamilyMembers();

            //since i know i've only just created 1 dad, i just hardcode the ID
            var chris = context.Dads.First(x => x.FirstName == "Chris");

            var clara = new Kid() {Name = "Clara", Birthday = DateTime.Parse("1/21/2009")};
            chris.Kids.Add(clara);

            context.SaveChanges();
        }
    }

    public class FamilyMembers : DbContext
    {
        public DbSet<Dad> Dads { get; set; }
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

    public class OrtmanFamilyInitializer : DropCreateDatabaseAlways<FamilyMembers>
    {
        protected override void Seed(FamilyMembers context)
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