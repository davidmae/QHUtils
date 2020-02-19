using UnityEngine;
using System.Collections;

namespace QHLand
{

    // ###################################################################################################################
    // ################ Class for doing noise over the terrain. Works with 2D noise. #####################################
    // ###################################################################################################################

    public class NoiseAlgorithm
    {

        public int repeat;

        public NoiseAlgorithm(int repeat = -1)
        {
            this.repeat = repeat;
        }

        public float FractionalBrownianMotion(float x, float y, int octaves, float persistence, float lacunarity, float frequency)
        {
            float total = 0;
            float amplitude = 1;

            for (int i = 0; i < octaves; i++)
            {
                total += noise2D(x * frequency, y * frequency) * amplitude;
                frequency *= lacunarity;
                amplitude *= persistence;
            }

            return total;
        }

        public float RidgedMultiFractal(float x, float y, int octaves, float persistence, float lacunarity, float frequency)
        {
            float signal = 0.0f;
            float total = 0.0f;
            float weight = 1.0f;
            float offset = 1f;

            for (int i = 0; i < octaves; i++)
            {
                signal = noise2D(x * frequency, y * frequency);

                signal = offset - Mathf.Abs(signal);
                signal *= signal;
                signal *= weight;

                weight = signal * persistence;
                if (weight > 1.0f)
                {
                    weight = 1.0f;
                }
                if (weight < 0.0f)
                {
                    weight = 0.0f;
                }

                total += signal;

                frequency *= lacunarity;
            }

            return total;
        }

        public float Billow(float x, float y, int octaves, float persistence, float lacunarity, float frequency)
        {
            float signal = 0.0f;
            float total = 0.0f;
            float curPersistence = 1.0f;

            for (int i = 0; i < octaves; i++)
            {
                signal = noise2D(x * frequency, y * frequency);
                signal = 2.0f * Mathf.Abs(signal) - 1.0f;
                total += signal * persistence;

                frequency *= lacunarity;
                curPersistence *= persistence;
            }

            return (total + 0.5f);
        }

        private static readonly int[] permutation = { 151,160,137,91,90,15,					// Hash lookup table as defined by Ken Perlin.  This is a randomly
	131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,	    // arranged array of all numbers from 0-255 inclusive.
	190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
	88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
	77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
	102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
	135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
	5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
	223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
	129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
	251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
	49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
	138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
    };

        private static readonly int[] p; 													// floatd permutation to avoid overflow

        static NoiseAlgorithm()
        {
            p = new int[512];
            for (int x = 0; x < 512; x++)
            {
                p[x] = permutation[x % 256];
            }
        }

        public float noise2D(float x, float y)
        {
            if (repeat > 0)
            {									// If we have any repeat on, change the coordinates to their "local" repetitions
                x = x % repeat;
                y = y % repeat;
            }

            int xi = (int)x & 255;
            int yi = (int)y & 255;

            float u = x - (int)x;
            float v = y - (int)y;

            int aa, ab, ba, bb;
            aa = p[p[xi] + yi];
            ab = p[p[xi] + inc(yi)];
            ba = p[p[inc(xi)] + yi];
            bb = p[p[inc(xi)] + inc(yi)];

            float i1, i2;

            float r00 = grad(aa, u, v);
            float r10 = grad(ba, u - 1, v);
            float r01 = grad(ab, u, v - 1);
            float r11 = grad(bb, u - 1, v - 1);

            float uFade = fade(u);
            float vFade = fade(v);

            i1 = lerp(r00, r10, uFade);
            i2 = lerp(r01, r11, uFade);

            return (lerp(i1, i2, vFade) + 1) / 2;
        }

        public int inc(int num)
        {
            num++;
            if (repeat > 0) num %= repeat;

            return num;
        }

        public static float grad(int hash, float x, float y)
        {
            //int h = hash & 7;
            //float u = h < 4 ? x : y;
            //float v = h < 4 ? y : x;
            //return (((h & 1) != 0) ? -u : u) + (((h & 2) != 0) ? -2.0f * v : 2.0f * v);

            switch (hash & 0x7)
            {
                case 0x0: return x + y;
                case 0x1: return -x + y;
                case 0x2: return x - y;
                case 0x3: return -x - y;
                case 0x4: return x;
                case 0x5: return -x;
                case 0x6: return -y;
                case 0x7: return y;
                default: return 0; // never happens
            }

        }

        public static float fade(float x)
        {
            // Fade function as defined by Ken Perlin.  This eases coordinate values
            // so that they will "ease" towards integral values.  This ends up smoothing
            // the final output.
            return x * x * x * (x * (x * 6 - 15) + 10);			// 6x^5 - 15x^4 + 10x^3
        }

        public static float lerp(float a, float b, float x)
        {
            return a + x * (b - a);
        }

    }
}