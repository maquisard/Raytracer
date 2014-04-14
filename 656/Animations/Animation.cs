using edu.tamu.courses.imagesynth.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.Animations
{
    public abstract class Animation<T>
    {
        public virtual void Update(Animable animable, String property, float t)
        {
            this.UpdateValue(animable, null, property, t);
        }

        public virtual void UpdateValue(Object source, Object obj, String propertyName, float t)
        {
            String[] names = propertyName.Split('.');
            if (names.Length > 1)
            {
                int index = propertyName.IndexOf(".");
                String new_name = propertyName.Substring(index + 1);
                String old_name = propertyName.Substring(0, index);
                PropertyInfo property = source.GetType().GetProperty(old_name);
                obj = property.GetValue(source);
                if (property != null)
                {
                    UpdateValue(obj, property, new_name, t);
                }
                else
                {
                    FieldInfo field = obj.GetType().GetField(new_name);
                    UpdateValue(obj, field, new_name, t);
                }
            }
            else if(names.Length == 1)
            {
                String name = names[0];
                PropertyInfo property = source.GetType().GetProperty(name);
                if (property != null)
                {
                    T currentValue = (T)property.GetValue(source);
                    T newValue = this.Update(currentValue, t);
                    property.SetValue(source, newValue);
                }
                else
                {
                    FieldInfo field = source.GetType().GetField(name);
                    T currentValue = (T)field.GetValue(source);
                    T newValue = this.Update(currentValue, t);
                    field.SetValue(source, newValue);
                }
            }
        }

        public abstract T Update(T currentValue, float t);
    }
}
