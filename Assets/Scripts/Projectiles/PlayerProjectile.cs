using Colors;
using UnityEngine;

namespace Projectiles
{
    public class PlayerProjectile : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed = 20f;
        [SerializeField] private float secondsToDestroy = 10f;

        public ColorType projectileColor = 0;
        private void Start()
        {
            if (Camera.main != null)
                GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * projectileSpeed;

            DestroyAfterSeconds(secondsToDestroy);
        }

        private void DestroyAfterSeconds(float seconds)
        {
            Destroy(gameObject, secondsToDestroy);
        }
    }
}
