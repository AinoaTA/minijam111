using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI_and_Menus;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; set; }


    public HUDController hudController { get; set; }


    private void Awake()
    {
        gameManager = this;
    }
}
