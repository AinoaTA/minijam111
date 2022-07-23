using Colors;
using UnityEngine;
using UnityEngine.UI;

namespace UI_and_Menus
{
    public class HUDController : MonoBehaviour
    {
        [Header("Health Parts")]
        public Image[] hearts;

        [Header("Projectile Parts")]
        [SerializeField] private GameObject currentProjectileColor;
        [SerializeField] private Sprite[] colorsSprites;
        [SerializeField] private Image[] allColorProjectile;
        [SerializeField] private Image selectedColor;
        [SerializeField] private Color selected;
        ColorTypes previousColor=(ColorTypes)1;

        public void UpdateHearts(int health, bool isDamaged)
        {
            hearts[health].gameObject.SetActive(!isDamaged);
        }

        public void UpdateColor(ColorTypes color)
        {
            selectedColor.sprite = colorsSprites[(int)color];
            allColorProjectile[(int)previousColor].color = Color.white;
            allColorProjectile[(int)color].color = selected;
            previousColor = color;
            switch (color)
            {
                case ColorTypes.Green:
                    
                    break;
                case ColorTypes.Blue:

                    break;

                case ColorTypes.Red:

                    break;
            }
        }
    }
}
