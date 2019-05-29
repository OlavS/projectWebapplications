using System;
using System.ComponentModel;

namespace Model.ViewModels
{
    public class ChangeLogVM
    {
        [DisplayName("Entity name:")]
        public string EntityName { get; set; }

        [DisplayName("Property name:")]
        public string PropertyName { get; set; }

        [DisplayName("Primary key value:")]
        public string PrimaryKeyValue { get; set; }

        [DisplayName("Old value/NA:")]
        public string OldValue { get; set; }

        [DisplayName("New value/Event:")]
        public string NewValue { get; set; }

        [DisplayName("Date changed:")]
        public String DateChanged { get; set; }

        public override string ToString()
        {
            return EntityName + " " + PropertyName + " " + PrimaryKeyValue + " " + OldValue + " " +
                   NewValue + " " + DateChanged;
        }
    }
}