using Colors;
using UnityEngine;
using UnityEngine.UI;

namespace UI_and_Menus
{
    public class HUDController : MonoBehaviour
    {
        [Header("Health Parts")]
        public Image[] hearts;
        public Sprite broken, heal;

        [Header("Projectile Parts")]
        [SerializeField] private GameObject currentProjectileColor;

        public void UpdateHearts(int health, bool isDamaged)
        {
            if (isDamaged)
                hearts[health].sprite = broken;
            else
                hearts[health].sprite = heal;
        }

        public void UpdateColor(ColorTypes color)
        {
            switch (color)
            {
                case ColorTypes.Green:

                    break;
                case ColorTypes.Red:

                    break;
                case ColorTypes.Blue:

                    break;
            }
        }
    }
}
