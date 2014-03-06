using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.lights
{
    public class LightProjection : Light
    {
        public float Distance { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Up { get; set; }
        public float Sx { get; set; }
        public float Sy { get; set; }
        public String ImageTexturePath { get; set; }

        private Vector3 Nx { get; set; }
        private Vector3 Ny { get; set; }
        private Vector3 P00 { get; set; }
        private Texture Texture { get; set; }

        public override Color ComputeFinalLightColor(Vector3 P)
        {
            //shoot the ray from here.
            Vector3 L = this.ComputeLightVector(P);
            L.Normalize();

            float t = -1f;
            float denom = L % Normal;
            if (denom == 0f) return Color.BLACK;

            t = (((Position + Normal * Distance) - P) % Normal) / denom;
            //Now check that the plane is within the bounds of the plane
            Vector3 Ph = P + t * L;
            float u = ((Ph - P00) % Nx) / Sx;
            float v = ((Ph - P00) % Ny) / Sy;
            if (!(u <= 1f && v <= 1f))
            {
                return Color.BLACK;
            }

            return Texture.ComputeColor(new Vector2(u, v));
        }

        public override void PostLoad()
        {
            Up.Normalize();
            Normal.Normalize();

            Vector3 P0 = Position + Normal * Distance;
            Nx = Normal ^ Up;
            Ny = Nx ^ Normal;

            Nx.Normalize();
            Ny.Normalize();

            P00 = P0 - 0.5f * (Sx * Nx + Sy * Ny);
            this.CreateTexture();
        }

        protected virtual void CreateTexture()
        {
            Texture = new ImageTexture();
            ((ImageTexture)Texture).FileName = ImageTexturePath;
            ((ImageTexture)Texture).PostLoad();
        }

    }
}
