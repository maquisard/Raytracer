using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.tests
{
    public class VectorTest
    {
        public static void SimpleTest()
        {
            Vector3 i = new Vector3(1f, 0f, 0f);
            Vector3 j = new Vector3(0f, 1f, 0f);
            Vector3 k = new Vector3(0f, 0f, 1f);

            if (i % j == 0f && j % k == 0f && k % i == 0f)
            {
                Console.WriteLine("Simple Dot Product Test: OK");
            }
            else
            {
                Console.WriteLine("Simple Dot Product Test: FAIL");
            }
            if ((i ^ j) == k && (j ^ k) == i && (k ^ i) == j)
            {
                Console.WriteLine("Simple Cross Product Test: OK");
            }
            else
            {
                Console.WriteLine("Simple Cross Product Test: FAIL");
            }

            if (i + j + k == new Vector3(1, 1, 1))
            {
                Console.WriteLine("Simple Vector Sum Test: OK");
            }
            else
            {
                Console.WriteLine("Simple Vector Sum Test: FAIL");
            }

            if ((i + 3) + (j + 3) + (k + 3) == new Vector3(10, 10, 10))
            {
                Console.WriteLine("Simple Vector-Number Sum Test: OK");
            }
            else
            {
                Console.WriteLine("Simple Vector-Number Sum Test: FAIL");
            }

            if (i * j * k == Vector3.Zero)
            {
                Console.WriteLine("Simple Vector Product Test: OK");
            }
            else
            {
                Console.WriteLine("Simple Vector Product Test: FAIL");
            }

            if (i * j * k == Vector3.Zero)
            {
                Console.WriteLine("Simple Vector Product Test: OK");
            }
            else
            {
                Console.WriteLine("Simple Vector Product Test: FAIL");
            }

            if (3f * i == new Vector3(3f, 0f, 0f))
            {
                Console.WriteLine("Simple Vector-Num Product Test: OK");
            }
            else
            {
                Console.WriteLine("Simple Vector-Num Product Test: FAIL");
            }

            if ( ((i / 2f ) * 2f) == i  )
            {
                Console.WriteLine("Simple Division Product Test: OK");
            }
            else
            {
                Console.WriteLine("Simple Division Product Test: FAIL");
            }

            if ((i + j + k) / 3f == (new Vector3(1f, 1f, 1f) / new Vector3(3f, 3f, 3f)))
            {
                Console.WriteLine("Simple Division Test: OK");
            }
            else
            {
                Console.WriteLine("Simple Division Test: FAIL");
            }

            if(i.Norm == i.Normalize())
            {
                Console.WriteLine("Simple Norm Test: OK");
            }
            else
            {
                Console.WriteLine("Simple Norm Test: FAIL");
            }
        }
    }
}
