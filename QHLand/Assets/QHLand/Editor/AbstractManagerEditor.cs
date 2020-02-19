using UnityEditor;

namespace QHLand
{
    public abstract class AbstractManagerEditor : Editor
    {
        public static AbstractManager manager { get; set; }
    }
}