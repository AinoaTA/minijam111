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
                HealthSystem hp = other.GetComponent<HealthSystem>();
                if (hp.currHealth >= hp.maxHealth)
                    return;

                particles.Play();
                up.SetActive(false);
                down.SetActive(false);
                used = true;
                hp.GetHealing();
                Destroy(gameObject, 1);
            }
        }
    }
}
