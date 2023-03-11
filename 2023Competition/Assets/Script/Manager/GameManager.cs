using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentScore = 0; // 현재 스코어

    public int currentLevel = 1;
    int levelMax = 4; // 최대 레벨

    Player player;

    void Start()
    {
        player = Player.instance;
    }

    void Update()
    {
        Cheat();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
   

    void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.F1)) // 맵 내에 모든 적 유닛 제거
            Destroy(SpawnManager.instance.transform.GetChild(SpawnManager.instance.transform.childCount - 1));

        if (Input.GetKeyDown(KeyCode.F2)) // 공격 업그레이드를 최고 단계로 상승
        {
            for (int i = currentLevel; i < levelMax; i++)
                Player.instance.transform.GetChild(i).gameObject.SetActive(true);

            currentLevel = levelMax;
        }

        if (Input.GetKeyDown(KeyCode.F3)) // 스킬의 쿨타임 및 횟수를 초기화 시킨다
        {
            player.hpCurrentCoolTime = 0;
            player.bombCurrentCoolTime = 0;
        }

        if (Input.GetKeyDown(KeyCode.F4)) // 내구도 초기화
            player.currentHp = player.maxHp;

        if (Input.GetKeyDown(KeyCode.F5)) // 연료 초기화
            player.currentFuel = player.maxFuel;

        //if(Input.GetKeyDown(KeyCode.F6)) //스테이지 이동

    }
}
