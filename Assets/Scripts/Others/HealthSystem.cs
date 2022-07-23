using System.Collections;
using UI_and_Menus;
using UnityEngine;

namespace Others
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 3;
        private int currHealth;
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
                currHealth++;
                GameManager.gameManager.hudController.UpdateHearts(currHealth,false);
            }
        }

        private void Death()
        {
            GameManager.gameManager.hudController.GameOver();
        }
    }
}
