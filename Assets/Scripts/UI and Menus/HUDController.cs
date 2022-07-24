using Colors;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI_and_Menus
{
    public class HUDController : MonoBehaviour
    {
        //FMOD.Studio.Bus Music;
        //float MusicVolume = 0.8f;

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
        [SerializeField] private CanvasGroup tutorial;

        [HideInInspector] public bool isPause;
        [HideInInspector] public bool isGameOver;
        private void Awake()
        {
            //Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
            GameManager.gameManager.hudController = this;
        }
        private void Start()
        {
            if (GameManager.gameManager.firstTime)
            {
                StartCoroutine(TutorialDelay());
            }
        }
        /*public void MusicVolumeLevel(float newMusicVolume)
        {
            MusicVolume = newMusicVolume;
        }*/
        public void ShowCanvasGroup(CanvasGroup canvas) 
        {
            canvas.alpha = 1;
            canvas.blocksRaycasts = true;
            canvas.interactable = true;
        }

        public void HideCanvasGroup(CanvasGroup canvas)
        {
            //MusicVolume = 0.8f;
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

        public void UnlockMouse(bool state) 
        {
            if (state)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true; 
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false; 
            }
            
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
            //MusicVolume = 0f;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/Game Over T", GetComponent<Transform>().position);
            ShowCanvasGroup(gameOver);
            UnlockMouse(true);
            Time.timeScale = 0;
            isGameOver = true;
        }

        IEnumerator TutorialDelay()
        {
            ShowCanvasGroup(tutorial);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1));
            HideCanvasGroup(tutorial);
            GameManager.gameManager.firstTime = false;
        }
    }
}
