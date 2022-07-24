using UnityEngine;
using UI_and_Menus;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; set; }

    public HUDController hudController { get; set; }
    public List<string> bossesKilled = new List<string>();

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
    }
}
