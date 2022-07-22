using UnityEngine;
using UnityEngine.UI;

public class HealSystem : MonoBehaviour
{
    public int maxHealth = 3;
    public int currHealth;
    public Image[] hearts;
    public Sprite broken, heal;

    private void Start()
    {
        currHealth = maxHealth;
    }

    public void TakeDamage()
    {
        if (currHealth <= 0)
        {
            Death();
        }
        else
        {
            hearts[currHealth - 1].sprite = broken;
            currHealth--;
        }
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
