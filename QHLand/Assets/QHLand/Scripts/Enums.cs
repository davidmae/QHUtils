using UnityEngine;
using System.Collections;

namespace QHLand
{
    //public enum eBiome { GREEN, SNOW, DESERT, SABANA, NONE };
    public enum eSizes { FLAT, PLAIN, ROUGHT, HILLS, HIGHLAND, CUSTOM };
    public enum eSides { LEFT = 0, BOTTOM = 1, RIGHT = 2, UP = 3 };
    public enum eFadeMenus { NOISE, EDGES, WATER, SAVELOAD, SMOOTH };
    public enum eShaderType { Shader_OriginalMode, Shader_NoTransition };
    public enum eNoise { FRACTAL, FBM, BILLOW };
}