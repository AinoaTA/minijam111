using System.Collections;
using UI_and_Menus;
using UnityEngine;

namespace Others
{
    public class HealthSystem : MonoBehaviour
    {
         public int maxHealth = 3;
         public int currHealth { private set; get; }
        [SerializeField] private float cooldownTimer = 2f;
        [SerializeField] private Animator damage;
        
        private bool cooldown;
        
        private void Start()
        {
            currHealth = maxHealth;
        }

        public void TakeDamage()
        {
            if (cooldown)
                return;

            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/Damage", transform.position);
            damage.Play("Damage");
            StartCoroutine(Cooldown());
            currHealth--;
            GameManager.gameManager.hudController.UpdateHearts(currHealth,true);
            
            if (currHealth <= 0)
                Death();
        }

        IEnumerator Cooldown()
        {
            cooldown = true;
            yield return new WaitForSeconds(cooldownTimer);
            cooldown = false;
        }
        public void GetHealing()
        {
            if (currHealth < maxHealth)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Music/New Wave", transform.position);
                GameManager.gameManager.hudController.UpdateHearts(currHealth,false);
                currHealth++;
            }
        }

        private void Death()
        {
            GameManager.gameManager.hudController.GameOver();
        }

        public void RecoverAllLife()
        {
            for (int i = currHealth; i < 3; i++)
            {
                GetHealing();
            }
        }
    }
}
