using UnityEngine;
using UI_and_Menus;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; set; }

    public HUDController hudController { get; set; }
    [HideInInspector]public List<string> bossesKilled = new List<string>();
    public bool firstTime { get; set; }
    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameManager);
        }
        else
        {
            Destroy(this.gameObject);
        }
        firstTime=true;
    }

    public void CheckBossesKilled()
    {
        if (bossesKilled.Count == 3)
        {
            SceneManager.LoadScene(2);
        }
    }
}
