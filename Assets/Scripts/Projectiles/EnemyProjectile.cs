using System.Collections;
using UnityEngine;
using Others;

namespace Projectiles
{
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float secondToDestroy;
        IEnumerator routine;
        public void InitializedProjectile(Vector3 dir)
        {
            GetComponent<Rigidbody>().velocity = dir * speed;
            StartCoroutine(routine=DestroyAfterSeconds());
        }

        IEnumerator DestroyAfterSeconds()
        {
            yield return new WaitForSeconds(secondToDestroy);
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                StopCoroutine(routine);
                collision.collider.GetComponent<HealthSystem>().TakeDamage();
                Destroy(gameObject);
            }
        }
    }
}
