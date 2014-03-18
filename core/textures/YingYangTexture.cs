using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core.textures
{
    public class YingYangTexture : Texture
    {
        public float Height { get; set; }
        public float Width { get; set; }

        public Vector2 Origin { get; set; }

        public Color Color1 { get; set; }
        public Color Color2 { get; set; }

        private Circle leftCircle;
        private Circle rightCircle;
        private Circle topCircle;
        private Circle bottomCircle;

        public YingYangTexture()
        {
            Height = 5f;
            Width = 5f;
            Color1 = new Color(1, 1, 1);
            Color2 = new Color(0, 0, 0);
        }

        public override void PostLoad()
        {
            leftCircle = new Circle(new Vector2(Width / 4f, Height / 2f), Height / 4f);
            rightCircle = new Circle(new Vector2((3f * Width) / 4f, Height / 2f), Height / 4f);
            topCircle = new Circle(new Vector2(Width / 2f, Height / 4f), Height / 4f);
            bottomCircle = new Circle(new Vector2(Width / 2f, (3f * Height) / 4f), Height / 4f);
        }

        public override Color ComputeColor(Vector uvcoordinates, Vector3 iPoint)
        {
            Vector2 uv = uvcoordinates as Vector2;
            float x = uv.X * Width;
            float y = uv.Y * Height;

            if (leftCircle.Inside(x, y) || topCircle.Inside(x, y))
            {
                return Color2;
            }
            else if (rightCircle.Inside(x, y) || bottomCircle.Inside(x, y))
            {
                return Color1;
            }
            else if (x <= Width)
            {
                return Color1;
            }
            else if (x > Width)
            {
                return Color2;
            }
            else
            {
                return Color.BLACK;
            }
        }

        public class Circle
        {
            public Vector2 Center { get; set; }
            public float Radius { get; set; }

            public Circle(Vector2 center, float radius)
            {
                this.Center = center;
                this.Radius = radius;
            }

            public bool Inside(Vector2 point)
            {
                Vector2 diff = point - Center;
                float value = diff.X * diff.X + diff.Y * diff.Y - Radius * Radius;
                return Math.Abs(value) <= 0.001;
            }

            public bool Inside(float x, float y)
            {
                Vector2 point = new Vector2(x, y);
                Vector2 diff = point - Center;
                float value = diff.X * diff.X + diff.Y * diff.Y - Radius * Radius;
                return  value <= 0f;
            }
        }
    }
}
