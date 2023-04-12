using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    void Awake() => instance = this;

    [Header("인게임 UI")]
    [SerializeField] float timer = 0;
    [SerializeField] GameObject ingameSound;
    [SerializeField] Text timerText;
    [SerializeField] Text scoreText;
    [SerializeField] Text levelText;

    [Header("게이지")]
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider fuelSlider;
    [SerializeField] Text hpValue;
    [SerializeField] Text fuelValue;

    [Header("메뉴")]
    [SerializeField] GameObject menuWindow;
    [SerializeField] Button backBtn;
    [SerializeField] Button rePlayBtn;
    [SerializeField] Button titleBtn;
    bool isMenu = false;

    [Header("게임오버")]
    [SerializeField] GameObject gameOverWindow;
    [SerializeField] GameObject gameOverSound;
    [SerializeField] Text overTime;
    [SerializeField] Text overScore;
    [SerializeField] Text overEnemyDie;
    [SerializeField] Text overHp;
    [SerializeField] Text overTotal;
    [SerializeField] Button overReplay;
    [SerializeField] Button overTitle;
    bool isGameOVer = false;

    [Header("클리어")]
    [SerializeField] GameObject clearWindow;
    [SerializeField] GameObject clearSound;
    [SerializeField] Text clearScore;
    [SerializeField] Text clearTime;
    [SerializeField] Text clearHp;
    [SerializeField] Text clearTotal;
    [SerializeField] Button nextBtn;
    bool isClear = false;

    [Header("스테이지 화면")]
    [SerializeField] CanvasGroup stageWindow;
    bool isStage = false;

    void Start()
    {
        Btns();
    }

    void Update()
    {
        Menu();
        Gauge();
        IngameUI();
        GameOverWindow();
        StartCoroutine(StageWidnow());
    }

    IEnumerator StageWidnow()
    {
        if (SpawnManager.instance.eStage != SpawnManager.EStage.Tutorial)
        {
            if (!isStage)
            {
                isStage = true;
                float fadeCount = 1;
                stageWindow.alpha = fadeCount;
                while (fadeCount >= 0)
                {
                    fadeCount -= 0.01f;
                    stageWindow.alpha = fadeCount;
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }

        else
        {
            if (TutorialManager.instance.isTutorial && !isStage)
            {
                isStage = true;
                float fadeCount = 1;
                stageWindow.alpha = fadeCount;
                while (fadeCount >= 0)
                {
                    fadeCount -= 0.01f;
                    stageWindow.alpha = fadeCount;
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }
    }

    void IngameUI()
    {
        GameManager.instance.currentTime += Time.deltaTime;
        timer += Time.deltaTime;

        timerText.text = "Time : " + (int)timer;
        scoreText.text = "Score : " + GameManager.instance.currentScore;
        if (GameManager.instance.currentLevel == 3)
            levelText.text = "Level : MAX";
        else
            levelText.text = "Level : " + (GameManager.instance.currentLevel + 1);
    }

    void Gauge()
    {
        float currentHp = Player.instance.currentHp;
        float currentFuel = Player.instance.currentFuel;
        int maxHp = Player.instance.maxHp;
        int maxFuel = Player.instance.maxFuel;

        hpSlider.value = Mathf.Lerp(hpSlider.value, currentHp / maxHp, 0.05f);
        fuelSlider.value = Mathf.Lerp(fuelSlider.value, currentFuel / maxFuel, 0.05f);

        Player.instance.currentFuel -= Time.deltaTime;

        hpValue.text = ((int)Player.instance.currentHp).ToString();
        fuelValue.text = ((int)Player.instance.currentFuel).ToString();
    }

    void Menu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SpawnManager.instance.eStage != SpawnManager.EStage.Tutorial)
            {
                if (!isMenu)
                {
                    isMenu = true;
                    Time.timeScale = 0;
                }

                else
                {
                    isMenu = false;
                    Time.timeScale = 1;
                }
            }

            else
            {
                if (TutorialManager.instance.isTutorial)
                {
                    if (!isMenu)
                    {
                        isMenu = true;
                        Time.timeScale = 0;
                    }

                    else
                    {
                        isMenu = false;
                        Time.timeScale = 1;
                    }
                }
            }
        }

        if (!isMenu)
            menuWindow.transform.localPosition = Vector3.Lerp(menuWindow.transform.localPosition, new Vector3(0, 1100, 0), 0.1f);

        else
            menuWindow.transform.localPosition = Vector3.Lerp(menuWindow.transform.localPosition, Vector3.zero, 0.1f);
    }

    void GameOverWindow()
    {
        if (Player.instance.currentHp <= 0 || Player.instance.currentFuel <= 0)
        {
            if (!isGameOVer)
            {
                isGameOVer = true;
                Time.timeScale = 0;
                ingameSound.SetActive(false);
                gameOverSound.SetActive(true);
                GameManager.instance.totalScore = (GameManager.instance.currentScore - (int)GameManager.instance.currentTime + GameManager.instance.currentEnemyDie + (int)Player.instance.currentHp);
            }

            gameOverWindow.transform.localPosition = Vector3.Lerp(gameOverWindow.transform.localPosition, Vector3.zero, 0.1f);

            overScore.text = "Score : " + GameManager.instance.currentScore;
            overTime.text = "Time : " + (int)GameManager.instance.currentTime;
            overEnemyDie.text = "EnemyDie : " + GameManager.instance.currentEnemyDie;
            overHp.text = "HP : " + (int)Player.instance.currentHp;

            overTotal.text = "Total : " + GameManager.instance.totalScore;
        }
    }

    public void ClearWindow()
    {
        if (!isClear)
        {
            isClear = true;

            Time.timeScale = 0;
            ingameSound.SetActive(false);
            clearSound.SetActive(true);
            GameManager.instance.totalScore = (GameManager.instance.currentScore - (int)timer + (int)Player.instance.currentHp);
        }

        clearWindow.transform.localPosition = Vector3.Lerp(clearWindow.transform.localPosition, Vector3.zero, 0.1f);

        clearScore.text = "Score : " + GameManager.instance.currentScore;
        clearTime.text = "Time : " + (int)timer;
        clearHp.text = "HP : " + (int)Player.instance.currentHp;

        clearTotal.text = "Total : " + (GameManager.instance.currentScore + -(int)timer + (int)Player.instance.currentHp);

    }

    void Btns()
    {
        backBtn.onClick.AddListener(() =>
        {
            isMenu = false;
            Time.timeScale = 1;
        });

        rePlayBtn.onClick.AddListener(() =>
        {
            GameManager.instance.GameReset();
            SceneManager.LoadScene("Tutorial");
        });

        titleBtn.onClick.AddListener(() =>
        {
            GameManager.instance.GameReset();
            SceneManager.LoadScene("Title");
        });

        overReplay.onClick.AddListener(() =>
        {
            GameManager.instance.GameReset();
            SceneManager.LoadScene("Tutorial");
        });

        overTitle.onClick.AddListener(() =>
        {
            Ranking.instance.AddRanking();
            SceneManager.LoadScene("Title");
        });

        nextBtn.onClick.AddListener(() =>
        {
            GameManager.instance.currentScore = GameManager.instance.totalScore;

            switch (SceneManager.GetActiveScene().name)
            {
                case "Tutorial":
                    SceneManager.LoadScene("Ingame2");
                    break;

                case "Ingame2":
                    SceneManager.LoadScene("Ingame3");
                    break;

                case "Ingame3":
                    SceneManager.LoadScene("Ending");
                    break;
            }
        });
    }
}
