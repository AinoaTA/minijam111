using System.Collections;
using UI_and_Menus;
using UnityEngine;

namespace Others
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] private HUDController hudController; 
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private int currHealth;
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
            hudController.UpdateHearts(currHealth);
            
            //CALL HUDCONTROLLER
            
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
                hudController.UpdateHearts(currHealth);
            }
        }

        private void Death()
        {
            Time.timeScale = 0;
        }
    }
}
