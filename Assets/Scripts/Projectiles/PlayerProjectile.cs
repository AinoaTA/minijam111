using System;
using Colors;
using UnityEngine;

namespace Projectiles
{
    public class PlayerProjectile : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed = 20f;
        [SerializeField] private float secondsToDestroy = 10f;
        
        [Header("Materials")]
        [SerializeField] private Material greenProjectileMaterial;
        [SerializeField] private Material blueProjectileMaterial;
        [SerializeField] private Material redProjectileMaterial;

        private ColorTypes _color;
        
        private void Awake()
        {

            if (!(Camera.main is null))
            {
                GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * projectileSpeed;
            }
            /*
                Vector3 bulletDirection;
                Transform cam = Camera.main.transform;
                Ray ray = new Ray(cam.position, cam.forward);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    bulletDirection = hit.point - transform.position;
                }
                else
                {
                    bulletDirection = (Camera.main.transform.position + cam.forward * 1000) - transform.position;
                }

                bulletDirection.Normalize();
                GetComponent<Rigidbody>().velocity = bulletDirection * projectileSpeed;
            }
            */
            
            

            _color = GetComponent<ColorEntity>().colorType;
            GetComponent<Renderer>().material = GetMaterial();

            DestroyAfterSeconds(secondsToDestroy);
        }

        private void DestroyAfterSeconds(float seconds)
        {
            Destroy(gameObject, secondsToDestroy);
        }
        
        private Material GetMaterial()
        {
            var material = _color switch
            {
                ColorTypes.Green => greenProjectileMaterial,
                ColorTypes.Blue => blueProjectileMaterial,
                ColorTypes.Red => redProjectileMaterial,
                _ => throw new ArgumentOutOfRangeException()
            };

            return material;
        }
    }
}
