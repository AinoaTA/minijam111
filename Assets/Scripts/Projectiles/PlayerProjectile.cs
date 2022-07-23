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
        
        private void Start()
        {
            if (Camera.main != null)
                GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * projectileSpeed;

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
