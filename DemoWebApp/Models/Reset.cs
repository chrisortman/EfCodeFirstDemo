using System;
using System.Data.Entity;
using DemoWebApp.Models.Conventions;

namespace DemoWebApp.Models
{
    [ExportDemo("reset")]
    public class Reset : IDemo
    {
        #region Implementation of IDemo

        public void Run()
        {
            Database.SetInitializer(new OrtmanFamilyInitializer());
            var context = new FamilyMembers();
            context.Database.Delete();
            context.Database.Initialize(true);
        }

        #endregion
    }
}