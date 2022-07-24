using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI_and_Menus
{
    public class PauseMenu : MonoBehaviour
    {
        CanvasGroup canvas;
        //FMOD.Studio.Bus Music;
        //float MusicVolume = 0.8f;
        FMOD.Studio.EventInstance Night;


        private void Awake()
        {
            canvas = GetComponent<CanvasGroup>();
            //Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
            Night = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/Nightmare");
        }

        /*public void MusicVolumeLevel(float newMusicVolume)
        {
            MusicVolume = newMusicVolume;
        }*/
        private void Update()
        {
            //Music.setVolume(MusicVolume);
            if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.gameManager.hudController.isGameOver)
            {
                Night.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Click", transform.position);
                //MusicVolume = 0.2f;
                GameManager.gameManager.hudController.isPause = !GameManager.gameManager.hudController.isPause;
                GameManager.gameManager.hudController.UnlockMouse(GameManager.gameManager.hudController.isPause);
                if(GameManager.gameManager.hudController.isPause)
                    GameManager.gameManager.hudController.ShowCanvasGroup(canvas);
                else
                {
                    Night.start();
                    //MusicVolume = 0.8f;
                    GameManager.gameManager.hudController.HideCanvasGroup(canvas);
                }

                Time.timeScale = GameManager.gameManager.hudController.isPause ? 0 : 1;
            }
        }
        public void OnResumePressed()
        {
            //MusicVolume = 0.8f;
            Night.start();
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
