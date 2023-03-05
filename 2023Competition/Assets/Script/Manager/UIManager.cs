using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    void Awake() => instance = this;

    [SerializeField] Text scoreText;

    [Header("메뉴화면")]
    [SerializeField] GameObject menuWindow;
    [SerializeField] Button backBtn;
    [SerializeField] Button reStartBtn;
    [SerializeField] Button outBtn;
    bool isMenuCheck = false;

    [Header("게임오버")]
    [SerializeField] GameObject gameoverWindow;
    [SerializeField] Text overScore;
    [SerializeField] Button overOutBtn;

    void Start()
    {
        MenuBtns();
    }

    void Update()
    {
        Menu();
        Score();
        GameOver();
    }

    void Menu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isMenuCheck)
            {
                isMenuCheck = true;
                menuWindow.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                isMenuCheck = false;
                menuWindow.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    void GameOver()
    {
        if(Player.instance.currentHp <= 0)
        {
            Time.timeScale = 0;
            overScore.text = "Score : " + GameManager.instance.currentScore;
            gameoverWindow.SetActive(true);
        }
    }

    void Score()
    {
        scoreText.text = "Score : " + GameManager.instance.currentScore;
    }

    void MenuBtns()
    {
        backBtn.onClick.AddListener(() =>
        {
            isMenuCheck = false;
            menuWindow.SetActive(false);
            Time.timeScale = 1;
        });

        reStartBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Ingame");
            Time.timeScale = 1;
        });

        outBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Title");
            Time.timeScale = 1;
        });

        overOutBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Title");
            Time.timeScale = 1;
        });
    }
}
