using System;
using Colors;
using UnityEngine;

namespace Player
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private ColorTypes projectileColor = 0;
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
                firePosition += _mainCamera.transform.forward * 0.1f;
            }
            
            var projectile = Instantiate(projectilePrefab, firePosition, Quaternion.identity);
            
            projectile.GetComponent<ColorEntity>().ColorType = projectileColor;
        }

        private Material GetMaterial()
        {
            var material = projectileColor switch
            {
                ColorTypes.Green => greenProjectileMaterial,
                ColorTypes.Blue => blueProjectileMaterial,
                ColorTypes.Red => redProjectileMaterial,
                _ => throw new ArgumentOutOfRangeException()
            };

            return material;
        }

        private ColorTypes GetNextColor(ColorTypes color)
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
            return (colors.Length==i) ? colors[0] : colors[i];
        }
    }
}
