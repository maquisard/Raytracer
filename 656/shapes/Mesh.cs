using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.textures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shapes
{
    public class Mesh : Shape, UVInterface
    {
        public String FileName { get; set; }
        public List<Vector3> Points { get; set; }
        public List<Vector3> SpacePoints { get; set; }
        public List<Vector3> Normals { get; set; }
        public List<Vector2> UVs { get; set; }

        public List<Face> Faces { get; set; }

        private Face intersected = null;

        public Mesh()
        {
            Points = new List<Vector3>();
            SpacePoints = new List<Vector3>();
            Normals = new List<Vector3>();
            UVs = new List<Vector2>();
            Faces = new List<Face>();
        }

        private void ParsePrimtitive(String line)
        {
            String[] pieces = line.Split(' ');
            if (pieces[0].ToLower() == "v")
            {
                float x = float.Parse(pieces[1]);
                float y = float.Parse(pieces[2]);
                float z = float.Parse(pieces[3]);

                Points.Add(new Vector3(x, y, z));
            }
            else if (pieces[0].ToLower() == "vt")
            {
                float u = float.Parse(pieces[1]);
                float v = float.Parse(pieces[2]);

                UVs.Add(new Vector2(u, v));
            }
            else if (pieces[0].ToLower() == "vn")
            {
                float x = float.Parse(pieces[1]);
                float y = float.Parse(pieces[2]);
                float z = float.Parse(pieces[3]);

                Vector3 n = new Vector3(x, y, z);
                n.Normalize();
                Normals.Add(n);
            }
            else if (pieces[0].ToLower() == "vp")
            {
                float x = float.Parse(pieces[1]);
                float y = float.Parse(pieces[2]);
                float z = float.Parse(pieces[3]);

                SpacePoints.Add(new Vector3(x, y, z));
            }
        }


        public override void PostLoad()
        {
            this.LoadFromFile();
        }

        public void LoadFromFile()
        {
            using (StreamReader filereader = new StreamReader(FileName))
            {
                String line = filereader.ReadLine().Trim();
                while (line != null)
                {
                    line = line.Trim();
                    if (line != "" && line[0] != '#')//ignore comment
                    {
                        if (line[0] == 'f')
                        {
                            String[] pieces = line.Split();
                            Face face = new Face();
                            face.V0 = this.CreateVertex(pieces[1]);
                            face.V1 = this.CreateVertex(pieces[2]);
                            face.V2 = this.CreateVertex(pieces[3]);
                            Faces.Add(face);
                        }
                        else
                        {
                            this.ParsePrimtitive(line);
                        }
                    }
                    line = filereader.ReadLine();
                }
            }
        }

        private Vertex CreateVertex(String entry)
        {
            Vertex v = new Vertex();
            String[] entries = entry.Split('/');
            int pIndex = int.Parse(entries[0]);
            pIndex--;
            v.Point = new Vector3(Points[pIndex].X, Points[pIndex].Y, Points[pIndex].Z);
            if (entries.Length > 1)
            {
                if (entries[1] != "")
                {
                    int tIndex = int.Parse(entries[1]);
                    tIndex--;
                    v.UV = new Vector2(UVs[tIndex].X, UVs[tIndex].Y);
                }
                else
                {
                    v.UV = Vector2.Zero;
                }

                if (entries.Length > 2)
                {
                    int nIndex = int.Parse(entries[2]);
                    nIndex--;
                    v.Normal = new Vector3(Normals[nIndex].X, Normals[nIndex].Y, Normals[nIndex].Z);
                    //v.Normal.Normalize();
                }
            }
            return v;
        }

        public override Vector3 NormalAt(Vector3 p)
        {
            return intersected.NormalAt(p);
        }

        public override float Intersect(Vector3 pe, Vector3 npe)
        {
            SortedDictionary<float, Face> candidates = new SortedDictionary<float, Face>();
            foreach (Face face in Faces)
            {
                float t = face.Intersect(pe, npe);
                if (t >= 0 && !candidates.ContainsKey(t))
                {
                    candidates[t] = face;
                }
            }

            if (candidates.Count == 0) return -1f;
            float tmin = candidates.Keys.ElementAt(0);
            intersected = candidates[tmin];
            return tmin;
        }

        public Vector2 UVCoordinates(Vector3 p)
        {
            return intersected.UVCoordinates(p);
        }
    }
}
