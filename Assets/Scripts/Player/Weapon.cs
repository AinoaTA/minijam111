using System;
using Colors;
using Projectiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private ColorType projectileColor = 0;
        [SerializeField] private bool shootFromFirePoint;
        
        [Header("Projectiles Data")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Material greenProjectileMaterial;
        [SerializeField] private Material blueProjectileMaterial;
        [SerializeField] private Material redProjectileMaterial;
        [SerializeField] private Transform firePoint;
        
        private Renderer _projectileRenderer;
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
            _projectileRenderer = projectilePrefab.GetComponent<Renderer>();

            ApplyMaterialToProjectile();
            
        }

        private void ApplyMaterialToProjectile()
        {
            var material = GetMaterial(); 
            _projectileRenderer.material = material;
        }

        public void ChangeWeaponColor()
        {
            projectileColor = GetNextColor(projectileColor);
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
                firePosition += _mainCamera.transform.forward * 0.5f;
            }
            
            var projectile = Instantiate(projectilePrefab, firePosition, Quaternion.identity);
            projectile.GetComponent<PlayerProjectile>().projectileColor = projectileColor;
        }

        private Material GetMaterial()
        {
            var material = projectileColor switch
            {
                ColorType.Green => greenProjectileMaterial,
                ColorType.Blue => blueProjectileMaterial,
                ColorType.Red => redProjectileMaterial,
                _ => throw new ArgumentOutOfRangeException()
            };

            return material;
        }

        private ColorType GetNextColor(ColorType color)
        {
            /*
            var colors = Enum.GetValues(typeof(Color));
            var colorIndex = (int)color + 1;
            
            if(colorIndex < colors.Length)
                return (Color) colors.GetValue(colorIndex);
            
            return (Color) colors.GetValue(0);
            */
            
            var colors = (ColorType[])Enum.GetValues(typeof(ColorType));
            var i = Array.IndexOf(colors, color) + 1;
            return (colors.Length==i) ? colors[0] : colors[i];
        }
    }
}
