using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DemoWebApp.Models.Fluent
{
    public class FullOrtmanFamilyInitializer : DropCreateDatabaseAlways<FamilyMembersWithFluentConfiguration>
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
                    },
                    new Kid()
                    {
                        Name = "Clara",
                        Birthday = DateTime.Parse("1/21/2009")
                    },
                    new Kid()
                    {
                        Name = "Lincoln",
                        Birthday = DateTime.Parse("3/25/2010"),
                    },
                    new Kid()
                    {
                        Name = "?????????",

                    }
                }
            };

            var charles = new Dad()
            {
                FirstName = "Charles",
                DayOfBirth = new DateTime(1948, 5, 21),
                Address = new Address()
                {
                    City = "Canistota",
                    State = "SD",
                    Zip = "57012"
                },
                Kids = new List<Kid>()
                {
                    new Kid()
                    {
                        Name = "Chris",
                        Birthday = DateTime.Parse("12/6/1979")
                    },
                    new Kid()
                    {
                        Name = "Sam",
                        Birthday = DateTime.Parse("5/18/1982")
                    }
                }
            };

            var clarence = new Dad()
            {
                FirstName = "Clarence",
                DayOfBirth = new DateTime(1909, 1, 1),
                Address = new Address()
                {
                    City = "Canistota",
                    State = "SD",
                    Zip = "57012",
                },
                Kids = new List<Kid>()
                {
                    new Kid() {Name = "Charles", Birthday = DateTime.Parse("5/21/1948")}
                }
            };

            var amon = new Dad()
            {
                FirstName = "Amon",
                DayOfBirth = new DateTime(1890, 1, 1),
                Address = new Address()
                {
                    City = "Canistota",
                    State = "SD",
                    Zip = "57012"
                },
                Kids = new List<Kid>()
                {
                    new Kid() {Name = "Ervin", Birthday = DateTime.Parse("2/1/1910")},
                    new Kid() {Name = "Herb", Birthday = DateTime.Parse("5/1/1912")},
                    new Kid() {Name = "Lester", Birthday = DateTime.Parse("11/1/1914")},
                    new Kid() {Name = "Clarence", Birthday = DateTime.Parse("2/1/1916")},
                    new Kid() {Name = "Irene", Birthday = DateTime.Parse("8/1/1917")},
                }
            };
            context.Dads.Add(chris);
            context.Dads.Add(charles);
            context.Dads.Add(clarence);
            context.Dads.Add(amon);
        }
    }
}