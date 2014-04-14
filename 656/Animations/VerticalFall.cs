using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.Animations
{
    public class VerticalFall : Animation<float>
    {
        public float CurrentSpeed { get; set; }
        public float Gravity { get; set; }

        public VerticalFall(float currentSpeed, float gravity)
        {
            this.CurrentSpeed = currentSpeed;
            this.Gravity = gravity;
        }

        public override float Update(float currentValue, float t)
        {
            if (currentValue >= 0.0f || currentValue < -4f)
            {
                CurrentSpeed = -CurrentSpeed;
            }
            CurrentSpeed = -0.5f * Gravity * t + CurrentSpeed;
            float newValue = CurrentSpeed * t  + currentValue;
            return newValue >= 0.0f ? 0.0f : newValue < -4f ? 4f : newValue;
        }
    }
}
