using System;
using Colors;
using UnityEngine;

namespace Player
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private ColorTypes projectileColor = 0;
        [SerializeField] private bool shootFromFirePoint;
        [SerializeField] private Animator weaponAnimator;
        [Header("Projectiles Data")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;

        [Header("Materials")]
        [SerializeField] private Material greenProjectileMaterial;
        [SerializeField] private Material blueProjectileMaterial;
        [SerializeField] private Material redProjectileMaterial;


        private Renderer _weaponRenderer;
        private Camera _mainCamera;

        private void Start()
        {
            weaponAnimator.Play("Idle");
            _mainCamera = Camera.main;
            GameManager.gameManager.hudController.UpdateColor(projectileColor);
            //ApplyMaterialToProjectile();
        }

        //private void ApplyMaterialToProjectile()
        //{
        //    var material = GetMaterial();
        //    _weaponRenderer.material = material;
        //}

        public void ChangeWeaponColor()
        {
            projectileColor = ColorEntity.GetNextColor(projectileColor);
            GameManager.gameManager.hudController.UpdateColor(projectileColor);
            // ApplyMaterialToProjectile();
        }

        public void SetVelocityIdle(float speed)
        {
            if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                weaponAnimator.speed = speed;
            else
                weaponAnimator.speed = 1;
        }
        public void InstantiateProjectile()
        {
            weaponAnimator.Play("Shoot");

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
        
            projectile.GetComponent<ColorEntity>().colorType = projectileColor;
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
    }
}
