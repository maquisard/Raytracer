using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core.textures
{
    public interface UVInterface
    {
        Vector2 UVCoordinates(Vector3 p);
    }
}
