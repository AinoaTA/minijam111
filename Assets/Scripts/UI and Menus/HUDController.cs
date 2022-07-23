using Colors;
using UnityEngine;
using UnityEngine.UI;

namespace UI_and_Menus
{
    public class HUDController : MonoBehaviour
    {
        [Header("HUD Parts")] 
        public Image[] hearts;
        public Sprite broken, heal;

        [SerializeField] private GameObject currentColor;


        public void UpdateHearts(int health)
        {
            hearts[health].sprite = broken;
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
