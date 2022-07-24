using Others;
using UnityEngine;

namespace Others {
    public class HealingObject : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private GameObject up, down;
        bool used;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !used)
            {
                particles.Play();
                up.SetActive(false);
                down.SetActive(false);
                used = true;
                other.GetComponent<HealthSystem>().GetHealing();
                Destroy(gameObject, 1);
            }
        }
    }
}
