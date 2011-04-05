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
            var context = new DemoEntities();
            var customer = new Customer {Name = "Chris", Birthday = new DateTime(1979, 12, 6)};
            context.Customers.Add(customer);

            context.SaveChanges();

            Console.WriteLine("All Done");
            Console.ReadLine();

        }
    }

    public class Customer
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}", ID, Name, Birthday);
        }
    }


    public class DemoEntities : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
    }

}
