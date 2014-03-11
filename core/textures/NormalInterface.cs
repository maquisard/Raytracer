using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core.textures
{
    public interface NormalInterface
    {
        Vector3 GetNormalVector(Vector3 iNormal, Vector3 iPoint, Vector texCoordinates);
        Vector3 GetCartesianCoordinates(float x, float y);

        NormalMap NormalMap { get; set; }
    }
}
