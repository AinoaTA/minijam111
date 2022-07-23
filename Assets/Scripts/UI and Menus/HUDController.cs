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

        [Header("Menus Parts")]
        [SerializeField] private CanvasGroup pauseMenu;
        [SerializeField] private CanvasGroup gameOver;

        [HideInInspector] public bool isPause;
        [HideInInspector] public bool isGameOver;
        private void Awake()
        {
            GameManager.gameManager.hudController = this;
        }
        public void ShowCanvasGroup(CanvasGroup canvas) 
        {
            canvas.alpha = 1;
            canvas.blocksRaycasts = true;
            canvas.interactable = true;
        }

        public void HideCanvasGroup(CanvasGroup canvas)
        {
            canvas.alpha = 0;
            canvas.blocksRaycasts = false;
            canvas.interactable =false;
        }
        public void UpdateHearts(int health, bool isDamaged)
        {
            hearts[health].gameObject.SetActive(!isDamaged);
        }
        public void Lock()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void Unlock() 
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
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
        public void GameOver()
        {
            ShowCanvasGroup(gameOver);
            Unlock();
            Time.timeScale = 0;
            isGameOver = true;
        }
    }
}
