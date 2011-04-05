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
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DemoEntities>());

            var context = new DemoEntities();
            var customer = new Person
            {
                Name = "Chris", 
                Birthday = new DateTime(1979, 12, 6),
                Address = new Address()
                {
                    City = "Ethan",
                    State = "SD",
                    Zip = "57334"
                }
            };

            context.Customers.Add(customer);

            context.SaveChanges();

            Console.WriteLine("======Customers======");
            foreach(var c in context.Customers)
            {
                Console.WriteLine(c.ToString());
            }
            
            Console.WriteLine("All Done");
            Console.ReadLine();

        }
    }

    public class Person
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public Address Address { get; set; }

        public override string ToString()
        {
            return String.Format("[{0}] {1} was born on {2} and lives at {3}", ID, Name, Birthday, Address);
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


    public class DemoEntities : DbContext
    {
        public DbSet<Person> Customers { get; set; }
    }

}
