using AForge.Video.FFMPEG;
using edu.tamu.courses.imagesynth.core.imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace video
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int width = 640;
            int height = 640;
            VideoFileWriter writer = new VideoFileWriter();
            writer.Open("pointlight.anim.avi", width, height, 25, VideoCodec.MPEG4, 1000000);
            DirectoryInfo animFolder = new DirectoryInfo("../../animations/pointlight");
            SortedList<String, String> files = new SortedList<string, string>();
            foreach (FileInfo file in animFolder.GetFiles())
            {
                files.Add(file.FullName, file.FullName);
            }

            foreach (String filename in files.Keys)
            {
                Bitmap frame = edu.tamu.courses.imagesynth.core.imaging.Image.FromFile(filename);
                writer.WriteVideoFrame(frame);
            }
            writer.Close();
        }
    }
}
