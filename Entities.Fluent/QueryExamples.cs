using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DemoWebApp.Models.Fluent
{
    [ExportDemo("query examples (fluent)")]
    public class QueryExamples : IDemo
    {
        private FamilyMembersWithFluentConfiguration _context;

        #region Implementation of IDemo

        public IEnumerable<string> Run()
        {
            _context = new FamilyMembersWithFluentConfiguration();

            var kidsWhosNamesBeginWithC = from d in _context.Dads
                                          from k in d.Kids
                                          where k.Name.StartsWith("C")
                                          select k.Name;

            Log("There are {0} kids whose names begin with 'C' ({1})",kidsWhosNamesBeginWithC.Count(),String.Join(",",kidsWhosNamesBeginWithC));

            var kidsWhoAreAlsoDads = from d in _context.Dads
                                     join k in _context.Kids on d.FirstName equals k.Name
                                     select d.FirstName;

            Log("There are {0} kids who are dads ({1})",kidsWhoAreAlsoDads.Count(),String.Join(",",kidsWhoAreAlsoDads));



            Log("Inefficient count of kids per dad");
            foreach(var dad in _context.Dads)
            {
                Log("{0} has {1} kids", dad.FirstName, dad.Kids.Count);
            }

            Log("Efficient count of kids per dad");
            foreach(var dad in _context.Dads.Include(x => x.Kids))
            {
                Log("{0} has {1} kids", dad.FirstName, dad.Kids.Count);
            }
            _context.SaveChanges();
            return Enumerable.Empty<string>();
        }

        private void LogKidQuery(IEnumerable<string> results)
        {
            
        }
        private void Log(string message,params object[] args)
        {
            _context.Messages.Add(new Message(String.Format(message,args)));
        }

        #endregion
    }

    [ExportDemo("insert full family (fluent)")]
    public class InsertFullFamilyDemo : IDemo
    {
        #region Implementation of IDemo

        public IEnumerable<string> Run()
        {
                Database.SetInitializer(new FullOrtmanFamilyInitializer());
            var context = new FamilyMembersWithFluentConfiguration();
            context.Database.Initialize(true);
            return Enumerable.Empty<string>();
        }

        #endregion
    }
}