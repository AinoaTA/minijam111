using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI_and_Menus
{
    public class PauseMenu : MonoBehaviour
    {
        CanvasGroup canvas;

        private void Awake()
        {
            canvas = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.gameManager.hudController.isGameOver)
            {
                GameManager.gameManager.hudController.isPause = true;
                GameManager.gameManager.hudController.Unlock();
                GameManager.gameManager.hudController.ShowCanvasGroup(canvas);
            }
        }
        public void OnResumePressed()
        {
            GameManager.gameManager.hudController.HideCanvasGroup(canvas);
            GameManager.gameManager.hudController.Lock();
            GameManager.gameManager.hudController.isPause = false;
            Time.timeScale = 1;
        }

        public void OnRestartGame()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(1);
        }

        public void OnQuitPressed()
        {
            Application.Quit(0);
        }

        //SETTINGS

        public void OnMasterVolumeChanged(float value)
        {

        }

        public void OnMusicVolumeChanged(float value)
        {

        }

        public void OnEffectsVolumeChanged(float value)
        {

        }

       
    }
}
