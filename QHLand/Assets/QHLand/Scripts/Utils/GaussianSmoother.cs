using UnityEngine;

namespace QHLand
{
    // Apply normal distribution in edges, for properly smooth steepness
    // GaussianSmoother works with an adapted version of the mathemathical equation "Gaussian/Normal distribution"
    public class GaussianSmoother
    {
        public Terrain terrain;

        static float firstGaussianVal;
        static float gaussExp;
        static float gaussianVal;
        static float normalizedVal;
        static float x;
        static bool firstIn;

        private GaussianSmoother()
        { }

        private static GaussianSmoother instance;

        public static GaussianSmoother Instance
        {
            get
            {
                firstGaussianVal = 0f;
                gaussExp = 0f;
                gaussianVal = 0f;
                normalizedVal = 0f;
                x = 0f;
                firstIn = true;

                if (instance == null)
                {
                    instance = new GaussianSmoother();
                }
                return instance;
            }
        }


        public float[,] BOTTOM(float[,] hts, float e, float ro, int k, int i, int startCoast, float baseHeight, float incr)
        {
            do
            {
                if (startCoast != 0 && hts[k + 1, i] <= hts[k - 3, i])
                    return hts;

                normalizedVal = GaussianValue(e, ro, x, firstIn);
                hts[k, i] = normalizedVal + baseHeight;
                --k;
                x += incr;
                firstIn = false;
            }
            while (normalizedVal > 0.001f && k > 0 + startCoast);

            return hts;
        }

        public float[,] LEFT(float[,] hts, float e, float ro, int k, int i, int startCoast, float baseHeight, float incr)
        {
            do
            {
                if (startCoast != 0 && hts[i, k + 1] <= hts[i, k - 3])
                    return hts;

                normalizedVal = GaussianValue(e, ro, x, firstIn);
                hts[i, k] = normalizedVal + baseHeight;
                --k;
                x += incr;
                firstIn = false;
            }
            while (normalizedVal > 0.001f && k > 0 + startCoast);

            return hts;
        }

        public float[,] UP(float[,] hts, float e, float ro, int k, int i, int startCoast, float baseHeight, float incr)
        {
            int htsTamY = hts.GetLength(0) - startCoast;

            do
            {
                if (startCoast != 0 && hts[k - 1, i] <= hts[k + 3, i])
                    return hts;

                normalizedVal = GaussianValue(e, ro, x, firstIn);
                hts[k, i] = normalizedVal + baseHeight;
                ++k;
                x += incr;
                firstIn = false;
            }
            while (normalizedVal > 0.001f && k < htsTamY);

            return hts;
        }

        public float[,] RIGHT(float[,] hts, float e, float ro, int k, int i, int startCoast, float baseHeight, float incr)
        {
            int htsTamX = hts.GetLength(1) - startCoast;

            do
            {
                if (startCoast != 0 && hts[i, k - 1] <= hts[i, k + 3])
                    return hts;

                normalizedVal = GaussianValue(e, ro, x, firstIn);
                hts[i, k] = normalizedVal + baseHeight;
                ++k;
                x += incr;
                firstIn = false;
            }
            while (normalizedVal > 0.001f && k < htsTamX);

            return hts;
        }

        private float GaussianValue(float e, float ro, float x, bool firstIn)
        {
            gaussExp = -0.5f * Mathf.Pow(x / ro, 2);
            gaussianVal = (Mathf.Pow(e, gaussExp) / Mathf.Sqrt(2 * Mathf.PI));

            if (firstIn)
                firstGaussianVal = gaussianVal * 1.0085f; //1.0085 es un valor para que suavice un poco el salto de altura

            // Ajusta el valor en función de la altura/hight inicial, que está en la variable "ro" (puesto que guardan relación)
            normalizedVal = (gaussianVal / firstGaussianVal) * ro;

            return normalizedVal;
        }
    }
}