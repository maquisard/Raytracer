using edu.tamu.courses.imagesynth.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.Animations
{
    public class DampedVerticalFall : Animation<Sphere>
    {
        public float CurrentSpeed { get; set; }
        public float Gravity { get; set; }
        public float Loss { get; set; }


        public DampedVerticalFall(float currentSpeed, float gravity, float loss)
        {
            this.CurrentSpeed = currentSpeed;
            this.Gravity = gravity;
            this.Loss = loss;
        }

        public override Sphere Update(Sphere currentPosition, float t)
        {
            float currentValue = currentPosition.Center.Y;
            if (currentValue >= -0.2f)
            {
                CurrentSpeed = -Loss * CurrentSpeed;
            }
            CurrentSpeed = -0.5f * Gravity * t + CurrentSpeed;
            float newValue = CurrentSpeed * t  + currentValue;
            currentPosition.Center.Y = newValue >= -0.2f ? -0.2f : newValue;
            return currentPosition;
        }

    }
}
