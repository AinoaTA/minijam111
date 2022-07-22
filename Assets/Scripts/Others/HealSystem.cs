using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealSystem : MonoBehaviour
{
    public int maxHealth = 3;
    public int currHealth;
    public float cooldownTimer = 2f;

    [Header("UI")]
    public Image[] hearts;
    public Sprite broken, heal;
    public Animator damage;
    bool cooldown;
    private void Start()
    {
        currHealth = maxHealth;
    }

    public void TakeDamage()
    {
        if (currHealth <= 0)
            Death();
        else
        {
            if (cooldown)
                return;
            damage.Play("Damage");
            StartCoroutine(Cooldown());
            hearts[currHealth - 1].sprite = broken;
            currHealth--;
        }
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
            hearts[currHealth - 1].sprite = heal;
            currHealth++;
        }
    }

    private void Death()
    {
        Time.timeScale = 0;
    }
}
