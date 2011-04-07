using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoWebApp.Models.Fluent
{
    [ExportDemo("query find by id (fluent)")]
    public class FindByIdExample : IDemo
    {
        #region Implementation of IDemo

        public IEnumerable<string> Run()
        {
            var context = new FamilyMembersWithFluentConfiguration();
            //the old way you used to do it
            var chris_first = (from d in context.Dads
                             where d.ID == 1
                             select d).First();

            var secondContext = new FamilyMembersWithFluentConfiguration();
            var chris_by_id = secondContext.Dads.Find(1);


            var thirdContext = new FamilyMembersWithFluentConfiguration();
            //var chris_by_sql = thirdContext.Dads.SqlQuery("select * from dads inner join kids on dads.id = kids.fluent_dadid where dads.ID = 1");

            //var xxx = chris_by_sql.ToArray();
            return Enumerable.Empty<string>();
        }

        #endregion
    }
}