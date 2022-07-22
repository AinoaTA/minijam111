using System;
using Colors;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private ColorsController.Color projectileColor = 0;
        [SerializeField] private bool shootFromFirePoint;
        
        [Header("Projectiles Data")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Material greenProjectileMaterial;
        [SerializeField] private Material blueProjectileMaterial;
        [SerializeField] private Material redProjectileMaterial;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float projectileSpeed = 10f;

        private GameObject _coloredProjectile;
        private Renderer _coloredProjectileRenderer;
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
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
            Vector3 firePosition;
            if (shootFromFirePoint)
            {
                firePosition = firePoint.position;
            }
            else
            {
                var centerScreen = new Vector3(Screen.height / 2, Screen.width / 2, 0);
                firePosition = _mainCamera.ScreenToWorldPoint(centerScreen);
                firePosition += _mainCamera.transform.forward;
            }
            
            var projectile = Instantiate(_coloredProjectile, firePosition, Quaternion.identity);
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
