using System;
using UnityEngine;

namespace Colors
{
    public class ColorEntity : MonoBehaviour, IColor
    {
        public ColorType ColorType { get; }
        public void RunInteraction(ColorType entityFromColor, ColorType entityToColor)
        {
            if (entityFromColor == entityToColor)
            {
                //KILL ENTITIES
            }
            else
            {
                //POWER UP ENTITY
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out IColor coloredEntity))
            {
                coloredEntity.RunInteraction(ColorType, coloredEntity.ColorType);
            }
        }
    }
}
