using UnityEngine;

namespace LogicalElements
{
    [CreateAssetMenu(fileName = "ColorEnum", menuName = "SO/ColorEnum", order = 0)]
    public class ColorEnum : ScriptableObject
    {
        public int ColorId;
        public Color Color;
    }
}