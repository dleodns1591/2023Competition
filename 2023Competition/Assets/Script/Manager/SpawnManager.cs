using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [Header("적 소환")]
    [SerializeField] List<GameObject> enemyList = new List<GameObject>();
    [SerializeField] int spawnTime = 0;
    bool isSpawn = false;

    [Header("보스 소환")]
    [SerializeField] float shakeTime = 0;
    [SerializeField] float shakeIntensity = 0;

    [SerializeField] Image bossSpawnFade;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject star1;
    [SerializeField] GameObject star2;

    bool isBossSpawn = false;
    bool isBossDirector = false;

    [Header("아이템")]
    public List<GameObject> itemList = new List<GameObject>();

    void Start()
    {
    }

    void Update()
    {
        StartCoroutine(BossSpawn());
        BossDirector();
    }

    void Awake()
    {
        instance = this;

        if (!isSpawn)
            StartCoroutine("Spawn");
    }

    IEnumerator Spawn() // 적 스폰 함수
    {
        while (true)
        {
            float posX = Random.Range(-10, 15);
            int enemyRandom = Random.Range(0, enemyList.Count);

            Instantiate(enemyList[enemyRandom], new Vector3(posX, Player.instance.transform.position.y, 17), enemyList[enemyRandom].transform.rotation, gameObject.transform);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    IEnumerator BossSpawn() // 보스스폰 함수
    {
        if (UIManager.instance.timer > 3 && !isBossSpawn)
        {
            isSpawn = true;
            isBossSpawn = true;
            StopCoroutine("Spawn"); // 적 스폰 중지

            yield return new WaitForSeconds(3); // 3초 후 보스 연출

            StartCoroutine(ShakeCamer()); // 카메라 흔들림
            isBossDirector = true; // 보스연출 시작

            yield return new WaitForSeconds(1); // 1초 후

            float fadeCount = 0;

            while (fadeCount < 1.0f) // FadeIn
            {
                fadeCount += 0.02f;
                yield return new WaitForSeconds(0.01f);
                bossSpawnFade.color = new Color(255, 255, 255, fadeCount);
            }

            isBossDirector = false; // 보스연출 중단
            Destroy(star1); // 보스연출 삭제
            Destroy(star2); // 보스연출 삭제

            yield return new WaitForSeconds(1); // 1초 후

            // 보스 소환
            Instantiate(boss, new Vector3(2, -7, 4), boss.transform.rotation);

            while (fadeCount >= 0) // FadeOut
            {
                fadeCount -= 0.02f;
                yield return new WaitForSeconds(0.01f);
                bossSpawnFade.color = new Color(255, 255, 255, fadeCount);

            }
        }
    }

    void BossDirector() // 보스연출 함수
    {
        if (isBossDirector)
        {
            star1.GetComponent<BackGround>().moveSpeed = 0;
            star2.GetComponent<BackGround>().moveSpeed = 0;

            for (int i = 0; i < star1.transform.childCount; i++)
            {
                Vector3 starTarget = new Vector3(2, star1.transform.GetChild(i).transform.position.y, 0);

                star1.transform.GetChild(i).transform.position = Vector3.Lerp(star1.transform.GetChild(i).transform.position, starTarget, 0.01f);
                star2.transform.GetChild(i).transform.position = Vector3.Lerp(star2.transform.GetChild(i).transform.position, starTarget, 0.01f);
            }
        }
    }

    IEnumerator ShakeCamer() // 카메라 흔들림 함수
    {
        Vector3 camerPos = Camera.main.transform.position;

        while(shakeTime > 0.0f)
        {
            Camera.main.transform.position = camerPos + Random.insideUnitSphere * shakeIntensity;
            shakeTime -= Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = camerPos;
    }
}
