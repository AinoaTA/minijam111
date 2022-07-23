using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;

namespace Colors
{
    public class ColorEntity : MonoBehaviour, IColor
    {
        public ColorTypes colorType;
        public void RunInteraction(GameObject otherEntity)
        {
            var otherEntityColor = otherEntity.GetComponent<ColorEntity>().colorType;
            if ( colorType == otherEntityColor)
            {
                Destroy(gameObject);
                
                if (otherEntity.CompareTag("Enemy"))
                {
                    otherEntity.GetComponent<IHit>().Attacked();
                }
            }
            else
            {
                
                //POWER UP ENTITY
            }
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out ColorEntity coloredEntity) && collision.gameObject.CompareTag("Enemy"))
            {
                RunInteraction(collision.gameObject);
            }
        }
        
        public static ColorTypes GetNextColor(ColorTypes color)
        {

            /*
            var colors = Enum.GetValues(typeof(Color));
            var colorIndex = (int)color + 1;
            
            if(colorIndex < colors.Length)
                return (Color) colors.GetValue(colorIndex);
            
            return (Color) colors.GetValue(0);
            */

            var colors = (ColorTypes[])Enum.GetValues(typeof(ColorTypes));
            var i = Array.IndexOf(colors, color) + 1;

            return (colors.Length == i) ? colors[0] : colors[i];
        }
    }
}
