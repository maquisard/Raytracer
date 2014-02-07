using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth
{
    public class CoreAssembly
    {
        private static CoreAssembly current = null;
        private Assembly assembly;

        private CoreAssembly()
        {
            String currentPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            assembly = Assembly.LoadFile(currentPath + "\\edu.tamu.courses.imagesynth.core.dll");
        }

        public static CoreAssembly Current
        {
            get
            {
                if (current == null)
                {
                    current = new CoreAssembly();
                }
                return current;
            }
        }

        public Type GetType(String type)
        {
            return this.assembly.GetType(type);
        }
    }
}
