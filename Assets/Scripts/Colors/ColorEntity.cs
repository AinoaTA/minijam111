using System;
using System.Security.Cryptography;
using UnityEngine;

namespace Colors
{
    public class ColorEntity : MonoBehaviour, IColor
    {
        public ColorTypes ColorType;
        public void RunInteraction(GameObject otherEntity)
        {
            var otherEntityColor = otherEntity.GetComponent<ColorEntity>().ColorType;
            if ( ColorType == otherEntityColor)
            {
                Destroy(otherEntity);
                Destroy(gameObject);
            }
            else
            {
                if (gameObject.CompareTag("Enemy"))
                {
                    GetComponent<IHit>().Attacked();
                }
                //POWER UP ENTITY
            }
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out ColorEntity coloredEntity))
            {
                RunInteraction(collision.gameObject);
            }
        }
    }
}
