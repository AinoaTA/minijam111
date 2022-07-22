using System;
using UnityEngine;

namespace Colors
{
    public enum ColorType
    {
        Green = 0,
        Red,
        Blue
    }
    public interface IColor
    {
        public ColorType ColorType { get; }
        void RunInteraction(ColorType entityFromColor, ColorType entityToColor);
    }
}
