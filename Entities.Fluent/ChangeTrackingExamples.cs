using System;
using System.Collections.Generic;

namespace DemoWebApp.Models.Fluent
{
    [ExportDemo("change tracking (fluent)")]
    public class ChangeTrackingExamples : IDemo
    {
        private FamilyMembersWithFluentConfiguration _context;
        private List<string> _messages = new List<string>();
        #region Implementation of IDemo

        public IEnumerable<string> Run()
        {

            _context = new FamilyMembersWithFluentConfiguration();

            var kids = _context.Kids;
            foreach(var kid in kids)
            {
                if(kid.Birthday.HasValue)
                {
                    kid.Birthday = kid.Birthday.Value.AddDays(20);
                }
            }

            //so by adding 20 days whose birth day changed?
            foreach(var k in _context.Kids)
            {
                var birthDayProperty = _context.Entry(k).Property(x => x.Birthday);
                if(birthDayProperty.IsModified)
                {
                    
                    if(birthDayProperty.OriginalValue.Value.Month != birthDayProperty.CurrentValue.Value.Month)
                    {
                        Log("Birthday month changed for {0} from {1} to {2}", k.Name, birthDayProperty.OriginalValue,
                            birthDayProperty.CurrentValue);

                        
                    }
                }
            }

            return _messages;
        }

        private void Log(string message, params object[] args)
        {
            _messages.Add(String.Format(message, args));
        }

        #endregion
    }
}