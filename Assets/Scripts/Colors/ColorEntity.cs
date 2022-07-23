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
    }
}
