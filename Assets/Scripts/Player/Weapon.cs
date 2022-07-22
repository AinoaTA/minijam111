using System;
using Colors;
using UnityEngine;

namespace Player
{
    public class Weapon : MonoBehaviour
    {
        
        [SerializeField] private ColorsController.Color projectileColor = 0;
        
        [Header("Projectiles Data")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Material greenProjectileMaterial;
        [SerializeField] private Material blueProjectileMaterial;
        [SerializeField] private Material redProjectileMaterial;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float projectileSpeed = 10f;

        private GameObject _coloredProjectile;
        private Renderer _coloredProjectileRenderer;

        private void Start()
        {
            _coloredProjectile = projectilePrefab;
            _coloredProjectileRenderer = _coloredProjectile.GetComponent<Renderer>();

            ApplyMaterialToProjectile();
            
        }

        private void ApplyMaterialToProjectile()
        {
            var material = GetMaterial(); 
            _coloredProjectileRenderer.material = material;
        }

        public void ChangeWeaponColor()
        {
            projectileColor = ColorsController.Instance.GetNextColor(projectileColor);
            ApplyMaterialToProjectile();
            Debug.Log(projectileColor);
        }
        
        public void InstantiateProjectile()
        {
            var projectile = Instantiate(_coloredProjectile, firePoint.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = firePoint.forward * projectileSpeed;

        }

        private Material GetMaterial()
        {
            var material = projectileColor switch
            {
                ColorsController.Color.Green => greenProjectileMaterial,
                ColorsController.Color.Blue => blueProjectileMaterial,
                ColorsController.Color.Red => redProjectileMaterial,
                _ => throw new ArgumentOutOfRangeException()
            };

            return material;
        }
    }
}
