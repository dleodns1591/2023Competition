using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("플레이어 정보")]
    public int currentScore = 0;
    public int totalScore = 0;
    public float currentTime = 0;
    public int currentEnemyDie = 0;
    public int currentLevel = 0;
    public string PlayerName;

    [Header("스킬")]
    public int healUseCount = 0;
    public int bombUseCount = 0;

    [Header("사운드")]
    public bool isBGM = false;
    public bool isEffect = false;

    void Start()
    {

    }

    void Update()
    {
        Cheat();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void GameReset()
    {
        currentScore = 0;
        totalScore = 0;
        currentTime = 0;
        currentEnemyDie = 0;
        currentLevel = 0;

        PlayerName = "";

        healUseCount = 0;
        bombUseCount = 0;
    }


    void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            for (int i = 0; i < SpawnManager.instance.spawnEnemy.transform.childCount; i++)
                Destroy(SpawnManager.instance.spawnEnemy.transform.GetChild(i).gameObject);
        }

        if (Input.GetKeyDown(KeyCode.F2))
            currentLevel = 3;

        if (Input.GetKeyDown(KeyCode.F3))
        {
            Player.instance.bombCoolTime = 0;
            Player.instance.healCoolTime = 0;
        }

        if (Input.GetKeyDown(KeyCode.F4))
            Player.instance.currentHp = Player.instance.maxHp;

        if (Input.GetKeyDown(KeyCode.F5))
            Player.instance.currentFuel = Player.instance.maxFuel;

        if (Input.GetKeyDown(KeyCode.F6))
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Tutorial":
                    SceneManager.LoadScene("Ingame2");
                    break;

                case "Ingame2":
                    SceneManager.LoadScene("Ingame3");
                    break;

                case "Ingame3":
                    SceneManager.LoadScene("Tutorial");
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.F7))
            SpawnManager.instance.isCheatBossSummon = true;

        if(Input.GetKeyDown(KeyCode.F8))
            SceneManager.LoadScene("Ending");

        if (Input.GetKeyDown(KeyCode.F9))
        {
            Ranking.instance.AddRanking();
            SceneManager.LoadScene("Title");
        }

    }
}
