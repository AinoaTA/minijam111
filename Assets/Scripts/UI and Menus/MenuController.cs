using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {
        [Header("Menu Containers")] 
        [SerializeField] private GameObject mainMenuContainer;
        [SerializeField] private GameObject settingsContainer;

        [Header("Volume Sliders")] 
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider effectsVolumeSlider;
        
        private void Start()
        {
            mainMenuContainer.SetActive(true);
            settingsContainer.SetActive(false);
            
            masterVolumeSlider.onValueChanged.AddListener (delegate {OnMasterVolumeChanged(masterVolumeSlider.value);});
            musicVolumeSlider.onValueChanged.AddListener (delegate {OnMusicVolumeChanged(musicVolumeSlider.value);});
            effectsVolumeSlider.onValueChanged.AddListener (delegate {OnEffectsVolumeChanged(effectsVolumeSlider.value);});
        }

        //MAIN MENU
        public void OnPlayPressed()
        {
            SceneManager.LoadScene(1);
        }
        
        public void OnSettingsPressed()
        {
            mainMenuContainer.SetActive(false);
            settingsContainer.SetActive(true);
        }
        
        public void OnExitPressed()
        {
            Application.Quit(0);
        }
        
        //SETTINGS

        private void OnMasterVolumeChanged(float value)
        {
            
        }
        
        private void OnMusicVolumeChanged(float value)
        {
            
        }
        
        private void OnEffectsVolumeChanged(float value)
        {
            
        }
        
        public void OnBackPressed()
        {
            mainMenuContainer.SetActive(true);
            settingsContainer.SetActive(false);
        }
    }
}
