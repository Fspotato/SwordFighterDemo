using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour
{
    [Header("角色")]
    [SerializeField] Player _player;
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject Result;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        Player player = Instantiate(_player);
        player.transform.position = new Vector3(2.5f, 1.5f, 0f);
        player.gameObject.layer = LayerMask.NameToLayer("Player1"); //改寫物理碰撞曾讓角色不會撞在一起
        player.HpBar = GameObject.Find("Player1_Hpbar").GetComponent<Slider>();
        Player player2 = Instantiate(_player);
        player2.transform.position = new Vector3(20.5f, 1.5f, 0f);
        player2.gameObject.layer = LayerMask.NameToLayer("Player2"); //改寫物理碰撞曾讓角色不會撞在一起
        player2.HpBar = GameObject.Find("Player2_Hpbar").GetComponent<Slider>();
        player2.GetComponent<Renderer>().material.color = ColorUtility.TryParseHtmlString("#FF6200", out Color newColor) ? newColor : Color.white;
        player2.GetComponent<SpriteRenderer>().flipX = true;
        player2.LeftKey = KeyCode.LeftArrow;
        player2.RightKey = KeyCode.RightArrow;
        player2.JumpKey = KeyCode.UpArrow;
        player2.DownKey = KeyCode.DownArrow;
        player2.SwiftKey = KeyCode.Keypad2;
        player2.AttackKey = KeyCode.Keypad1;
        player.Dead.AddListener(p1dead);
        player2.Dead.AddListener(p2dead);
    }

    void p1dead()
    {
        Result.SetActive(true);
        Result.transform.GetChild(0).GetComponent<Text>().text = "Player2 Wins!!";
        Time.timeScale = 0;
    }

    void p2dead()
    {
        Result.SetActive(true);
        Result.transform.GetChild(0).GetComponent<Text>().text = "Player1 Wins!!";
        Time.timeScale = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Menu.activeSelf == true)
            {
                CancelMenu();
            }
            else
            {
                CallMenu();
            }
        }
    }

    public void Restart()
    {
        Result.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("Level");
    }

    public void CallMenu()
    {
        Time.timeScale = 0;
        Menu.SetActive(true);
    }

    public void CancelMenu()
    {
        Time.timeScale = 1;
        Menu.SetActive(false);
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("Menu");
    }
}
