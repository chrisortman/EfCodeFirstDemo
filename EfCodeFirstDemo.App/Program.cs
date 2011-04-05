using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace EfCodeFirstDemo.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<FamilyMembers>());

            var context = new FamilyMembers();
            var ethan = new Address()
            {
                City = "Ethan",
                State = "SD",
                Zip = "57334"
            };
            var customer = new Dad
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

            context.Dads.Add(customer);

            context.SaveChanges();

            Console.WriteLine("======Customers======");
            foreach(var c in context.Dads)
            {
                Console.WriteLine(c.ToString());
            }
            
            Console.WriteLine("All Done");
            Console.ReadLine();

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

        public List<Kid> Kids { get; set; }

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
