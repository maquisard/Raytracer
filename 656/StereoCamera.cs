using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth
{
    public class StereoCamera : Camera
    {
        public Camera LeftCamera { get; set; }
        public Camera RightCamera { get; set; }

        public float Shift { get; set; }

        public StereoCamera() 
        {
            this.LeftCamera = new Camera();
            this.RightCamera = new Camera();
        }

        public override void updateValues()
        {
            base.updateValues();
            this.LeftCamera.D = this.RightCamera.D = this.D;
            this.LeftCamera.Xmax = this.RightCamera.Xmax = this.Xmax;
            this.LeftCamera.Ymax = this.RightCamera.Ymax = this.Ymax;
            this.LeftCamera.Up = this.RightCamera.Up = this.Up;
            this.LeftCamera.Sx = this.RightCamera.Sx = this.Sx;
            
            this.LeftCamera.Pe = this.Pe + Shift * this.N0;
            this.RightCamera.Pe = this.Pe - Shift * this.N0;
            this.LeftCamera.View = this.View + Shift * this.N0;
            this.RightCamera.View = this.View - Shift * this.N0;

            this.LeftCamera.updateValues();
            this.RightCamera.updateValues();
        }
    }
}
