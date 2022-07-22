using System;
using UnityEngine;

namespace Colors
{
    public class ColorsController : MonoBehaviour
    {
        public static ColorsController Instance;
        public enum Color
        {
            Green = 0,
            Red,
            Blue
        }

        private void Awake()
        {
            Instance = this;
        }

        public void ResolveColorCollision(Color projectileColor, Color entityColor)
        {
            if (projectileColor == entityColor)
            {
                //KILL ENTITY
            }
            else
            {
                //POWER UP ENTITY
            }
        }

        public Color GetNextColor(Color color)
        {
            /*
            var colors = Enum.GetValues(typeof(Color));
            var colorIndex = (int)color + 1;
            
            if(colorIndex < colors.Length)
                return (Color) colors.GetValue(colorIndex);
            
            return (Color) colors.GetValue(0);
            */
            
            var colors = (Color[])Enum.GetValues(typeof(Color));
            var i = Array.IndexOf(colors, color) + 1;
            return (colors.Length==i) ? colors[0] : colors[i];
        }
    }
}
