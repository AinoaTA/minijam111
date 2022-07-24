using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI_and_Menus
{
    public class PauseMenu : MonoBehaviour
    {
        CanvasGroup canvas;
        FMOD.Studio.Bus Music;
        FMOD.Studio.EventInstance PauseAmb;
        float MusicVolume = 0.8f;


        private void Awake()
        {
            canvas = GetComponent<CanvasGroup>();
            Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
            PauseAmb = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/Pause Menu");
        }

        public void MusicVolumeLevel(float newMusicVolume)
        {
            MusicVolume = newMusicVolume;
        }
        private void Update()
        {
            Music.setVolume(MusicVolume);
            if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.gameManager.hudController.isGameOver)
            {
                MusicVolume = 0f;
                GameManager.gameManager.hudController.isPause = !GameManager.gameManager.hudController.isPause;
                GameManager.gameManager.hudController.UnlockMouse(GameManager.gameManager.hudController.isPause);
                if(GameManager.gameManager.hudController.isPause)
                    GameManager.gameManager.hudController.ShowCanvasGroup(canvas);
                else
                    GameManager.gameManager.hudController.HideCanvasGroup(canvas);

                Time.timeScale = GameManager.gameManager.hudController.isPause ? 0 : 1;
            }
        }
        public void OnResumePressed()
        {
            MusicVolume = 0.8f;
            GameManager.gameManager.hudController.HideCanvasGroup(canvas);
            GameManager.gameManager.hudController.Lock();
            GameManager.gameManager.hudController.isPause = false;
            Time.timeScale = 1;
        }

        public void OnRestartGame()
        {
            Time.timeScale = 1;
            GameManager.gameManager.bossesKilled.Clear();
            SceneManager.LoadScene(1);
        }
        public void OnCheckPoint()
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
