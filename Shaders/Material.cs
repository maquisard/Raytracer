﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public class Material : Phong //in the ray trace, if this is a material then if reflective
    {
        public bool IsReflective { get; set; }
        public bool IsRefractive { get; set; }

        public float Kra { get; set; } //Index of refraction
    }
}
