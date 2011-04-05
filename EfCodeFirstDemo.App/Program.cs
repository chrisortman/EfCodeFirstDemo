using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using HibernatingRhinos.Profiler.Appender.EntityFramework;

namespace EfCodeFirstDemo.App
{
    public class OrtmanFamilyInitializer : IDatabaseInitializer<FamilyMembers>
    {
        #region Implementation of IDatabaseInitializer<in FamilyMembers>

        /// <summary>
        /// Executes the strategy to initialize the database for the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void InitializeDatabase(FamilyMembers context)
        {
            Seed(context);
            context.SaveChanges();
        }

        #endregion

        protected void Seed(FamilyMembers context)
        {
            var chris = context.Dads.Include(x => x.Kids).FirstOrDefault(x => x.Name == "Chris");
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
                Name = "Chris", 
                Birthday = new DateTime(1979, 12, 6),
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
			EntityFrameworkProfiler.Initialize();

            //have to do this b4 profiler, since profiler doesn't like create / delete database
            Database.SetInitializer(new OrtmanFamilyInitializer());
            
             PrintDads();

            Console.WriteLine("========= after initialization ===========");
            var context = new FamilyMembers();
            
            //since i know i've only just created 1 dad, i just hardcode the ID
            var chris = context.Dads.First(x => x.Name == "Chris");

            var clara = new Kid() {Name = "Clara", Birthday = DateTime.Parse("1/19/2010")};
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

    public class Dad
    {
        public Dad()
        {
            Kids = new List<Kid>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }
        
        public Address Address { get; set; }

        public virtual List<Kid> Kids { get; set; }

        public override string ToString()
        {
            var s=  String.Format("[{0}] {1} was born on {2} and lives at {3}\n", ID, Name, Birthday, Address);
            s += String.Format("\t{0} has {1} kids", Name, Kids.Count);
            if(Kids.Count > 0)
            {
                s += String.Format(" their names are {0}", String.Join(",", Kids.Select(x => x.Name)));
            }
            return s;
        }
    }

    public class Kid
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
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


    public class FamilyMembers : DbContext
    {
        public DbSet<Dad> Dads { get; set; }
    }

}
