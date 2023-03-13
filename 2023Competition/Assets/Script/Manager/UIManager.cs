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
    [SerializeField] Text timerText;
    public float timer = 0;

    [Header("게이지")]
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider fuelSlider;
    [SerializeField] Text hpText;
    [SerializeField] Text fuelText;
    [SerializeField] float fuelDownSpeed = 0;
    [SerializeField] float hpDownSpeed = 0;

    [Header("메뉴화면")]
    [SerializeField] GameObject menuWindow;
    [SerializeField] Button backBtn;
    [SerializeField] Button reStartBtn;
    [SerializeField] Button outBtn;
    bool isMenuCheck = false;

    [Header("게임오버")]
    public GameObject gameoverWindow;
    public Text overScore;
    [SerializeField] Button overOutBtn;

    [Header("클리어 화면")]
    public GameObject clearWindow;
    public Text clearScore;
    public Text currentTime;
    public Text dieEnemy;
    public Text totalText;
    public Button nextBtn;

    void Start()
    {
        MenuBtns();
    }

    void Update()
    {
        Menu();
        Slider();
        IngameText();
        ClearWindow();
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

    void IngameText()
    {
        timer += Time.deltaTime;

        scoreText.text = "Score : " + GameManager.instance.currentScore;
        timerText.text = "Time : " + (int)timer;
    }

    void Slider()
    {
        float currentHp = Player.instance.currentHp;
        float maxHp = Player.instance.maxHp;

        float currentFuel = Player.instance.currentFuel;
        float maxFuel = Player.instance.maxFuel;

        hpText.text = ((int)currentHp).ToString();
        fuelText.text = ((int)currentFuel).ToString();

        if (SpawnManager.instance.isBossSpawn)
            Player.instance.currentFuel -= Time.deltaTime * fuelDownSpeed;

        hpSlider.value = Mathf.Lerp(hpSlider.value, currentHp / maxHp, Time.unscaledDeltaTime * hpDownSpeed);
        fuelSlider.value = Mathf.Lerp(fuelSlider.value, currentFuel / maxFuel, Time.deltaTime * fuelDownSpeed);
    }

    void ClearWindow()
    {
        clearScore.text = "점수 : " + GameManager.instance.currentScore;
        currentTime.text = "시간 : " + (int)timer;
        dieEnemy.text = "죽은 적 : " + GameManager.instance.dieEnemyCount;

        totalText.text = "토탈 : " + (GameManager.instance.currentScore - (int)timer + GameManager.instance.dieEnemyCount);
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

        nextBtn.onClick.AddListener(() =>
        {
            GameManager.instance.MoveScene();
        });
    }
}
