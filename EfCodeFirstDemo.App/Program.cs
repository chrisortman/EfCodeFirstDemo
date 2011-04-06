using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using HibernatingRhinos.Profiler.Appender.EntityFramework;

namespace EfCodeFirstDemo.App
{

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

    class Program
    {
        static void Main(string[] args)
        {
			//EntityFrameworkProfiler.Initialize();

            //have to do this b4 profiler, since profiler doesn't like create / delete database
            Database.SetInitializer(new OrtmanFamilyInitializer());
            
             PrintDads();

            Console.WriteLine("========= after initialization ===========");
            var context = new FamilyMembers();

            //since i know i've only just created 1 dad, i just hardcode the ID
            var chris = context.Dads.First(x => x.FirstName == "Chris");

            var clara = new Kid() {Name = "Clara", Birthday = DateTime.Parse("1/21/2009")};
            chris.Kids.Add(clara);

            context.SaveChanges();

            PrintDads();

            

            Console.WriteLine("All Done");
            Console.ReadLine();

        }

        private static void PrintDads()
        {
            Console.WriteLine("======Dads======");
            var family = new FamilyMembers();
            foreach(var c in family.Dads.Include(x => x.Kids))
            {
                Console.WriteLine(c.ToString());
            }
        }
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

        [Column("Name")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Column("Birthday")]
        public DateTime DayOfBirth { get; set; }
                
        public Address Address { get; set; }

        
        public virtual List<Kid> Kids { get; set; }

        public override string ToString()
        {
            var s=  String.Format("[{0}] {1} was born on {2} and lives at {3}\n", ID, FirstName, DayOfBirth, Address);
            s += String.Format("\t{0} has {1} kids", FirstName, Kids.Count);
            if(Kids.Count > 0)
            {
                s += String.Format(" their names are {0}", String.Join(",", Kids.Select(x => x.ToString())));
            }
            return s;
        }
    }

    [Table("Kids")]
    public class Kid
    {
        [Key]
        public int ID { get; set; }
        
        [Column]
        public string Name { get; set; }
        
        [Column]
        public DateTime Birthday { get; set; }

        public TimeSpan Age
        {
            get { return DateTime.Now - Birthday; }
        }

        public override string ToString()
        {
            int remainder;
            int years = Math.DivRem(Convert.ToInt32(Age.TotalDays), 365, out remainder);
            return String.Format("{0} who is {1} years and {2} days old", Name, years,remainder);
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
        public string Zip { get; set; }

        public override string ToString()
        {
            return String.Format("{0},{1} {2}", City, State, Zip);
        }
    }


    public class FamilyMembers : DbContext
    {
        public DbSet<Dad> Dads { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dad>()
                .Property(x => x.DayOfBirth).HasColumnName("Birthday");

        }
    }

}
