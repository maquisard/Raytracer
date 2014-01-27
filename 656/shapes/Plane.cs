﻿using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shapes
{
    public class Plane : Shape
    {
        public Vector3 Point { get; set; }
        public Vector3 Normal { get; set; }

        public Plane() { this.Point = Vector3.Zero; this.Normal = new Vector3(0.0f, 1.0f, 0.0f); }

        public Plane(Vector3 point, Vector3 normal)
        {
            this.Point = point;
            this.Normal = normal;
        }

        public Plane(Vector3 normal)
        {
            this.Normal = normal;
            this.Point = Vector3.Zero;
        }

        public override float Intersect(Vector3 pe, Vector3 npe)
        {
            throw new NotImplementedException();
        }
    }
}
