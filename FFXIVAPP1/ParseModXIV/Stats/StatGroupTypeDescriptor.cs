using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ParseModXIV.Stats
{
    public abstract class StatGroupTypeDescriptor : CustomTypeDescriptor
    {
        protected StatGroup statGroup = null;

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var props = statGroup.Stats.Select(p => new StatGroupPropertyDescriptor(p.Name)).Cast<PropertyDescriptor>().ToList();
            props.Add(new StatGroupPropertyDescriptor("Name"));
            return new PropertyDescriptorCollection(props.ToArray());
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }
    }

    public class StatGroupPropertyDescriptor : PropertyDescriptor
    {
        public StatGroupPropertyDescriptor(string name) : base(name,null) {}

        #region Overrides of PropertyDescriptor

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            if (Name.ToLower() == "name") return ((StatGroup) component).Name;
            return ((StatGroup) component).GetStatValue(Name);
        }

        public override void ResetValue(object component)
        {
            var sg = (StatGroup) component;
            if(sg.Stats.HasStat(Name))
            {
                sg.Stats.GetStat(Name).Value = 0;
            }
        }

        public override void SetValue(object component, object value)
        {
            ((StatGroup) component).Stats.SetOrAddStat(Name, (Decimal)value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return typeof (StatGroup); }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get
            {
                if (Name.ToLower() == "name") return typeof (String); return typeof(Decimal);
            }
        }

        #endregion
    }
}
