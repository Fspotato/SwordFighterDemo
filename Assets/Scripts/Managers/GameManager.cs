using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    int width = 1366; // 解析度寬度
    int height = 768; // 解析度高度

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        CheckScreen();
    }

    void CheckScreen()
    {
        if (Screen.width != width)
        {
            if (Screen.fullScreen)
            {
                Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
            }
            else
            {
                Screen.SetResolution(width, height, FullScreenMode.Windowed);
            }
        }
    }
}
