using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public enum EStage
    {
        None,
        Tutorial,
        Ingame2,
        Ingame3,
    }
    public EStage eStage;

    public GameObject spawnEnemy;
    public GameObject bulletVowel;

    [Header("적 스폰")]
    [SerializeField] List<GameObject> enemySpawn = new List<GameObject>();
    [SerializeField] float spawnTime = 0;
    bool isEnemySpawn = false;

    [Header("보스 스폰")]
    [SerializeField] GameObject boss;
    [SerializeField] GameObject stageBGM;
    [SerializeField] GameObject warningSound;
    [SerializeField] int bossSpawnTime = 0;
    public bool isCheatBossSummon = false;
    bool isBoss = false;

    [Header("보스 연출")]
    [SerializeField] Image fade;
    [SerializeField] GameObject star01;
    [SerializeField] GameObject star02;
    [SerializeField] GameObject ring;
    [SerializeField] GameObject gas;
    bool isDirector = false;

    [Header("아이템 스폰")]
    [SerializeField] GameObject[] itemSpawn = new GameObject[5];
    int itemNum = 0;


    void Start()
    {
        if (eStage != EStage.Tutorial)
            Time.timeScale = 1;
    }

    void Update()
    {
        BossDirector();
        StartCoroutine(BossSpawn());
    }

    void Awake()
    {
        instance = this;

        StartCoroutine("EnemySpawn");
    }

    IEnumerator EnemySpawn()
    {
        if (!isEnemySpawn)
        {
            while (true)
            {
                int randomEnemy = Random.Range(0, enemySpawn.Count);
                int randomPosX = Random.Range(-14, 23);
                Vector3 enemySpawnPos = new Vector3(randomPosX, -8, 22);

                Instantiate(enemySpawn[randomEnemy], enemySpawnPos, enemySpawn[randomEnemy].transform.rotation, spawnEnemy.transform);
                yield return new WaitForSeconds(spawnTime);
            }
        }
    }

    IEnumerator ShakeCamer(float shakeTime, float shakeDamage)
    {
        Vector3 cameraPos = Camera.main.transform.position;

        while (shakeTime >= 0)
        {
            shakeTime -= 0.01f;
            Camera.main.transform.position = cameraPos + Random.insideUnitSphere * shakeDamage;
            yield return new WaitForSeconds(0.01f);
        }

        Camera.main.transform.position = cameraPos;
    }

    IEnumerator BossSpawn()
    {
        if ((GameManager.instance.currentTime >= GameManager.instance.currentTime + bossSpawnTime || isCheatBossSummon) && !isBoss)
        {
            isBoss = true;
            isDirector = true;
            isEnemySpawn = true;
            StopCoroutine("EnemySpawn");

            stageBGM.SetActive(false);
            warningSound.SetActive(true);

            StartCoroutine(ShakeCamer(2, 2));

            float fadeCount = 0;
            fade.color = new Color(255, 255, 255, fadeCount);

            while (fadeCount < 1)
            {
                fadeCount += 0.01f;
                fade.color = new Color(255, 255, 255, fadeCount);
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(1);

            isDirector = false;
            switch (eStage)
            {
                case EStage.Tutorial:
                    Destroy(star01);
                    Destroy(star02);
                    break;

                case EStage.Ingame2:
                    Destroy(ring);
                    break;

                case EStage.Ingame3:
                    Destroy(gas);
                    break;
            }

            stageBGM.SetActive(true);
            warningSound.SetActive(false);
            Instantiate(boss, new Vector3(4, -8, 7), boss.transform.rotation);

            while (fadeCount >= 0)
            {
                fadeCount -= 0.01f;
                fade.color = new Color(255, 255, 255, fadeCount);
                yield return new WaitForSeconds(0.01f);
            }

            isEnemySpawn = false;
            StartCoroutine("EnemySpawn");
        }
    }

    void BossDirector()
    {
        if (isDirector)
        {
            switch (eStage)
            {
                case EStage.Tutorial:
                    for (int i = 0; i < star01.transform.childCount; i++)
                    {
                        star01.transform.GetChild(i).position = Vector3.Lerp(star01.transform.GetChild(i).position, new Vector3(4, -8, 0), 0.03f);
                        star02.transform.GetChild(i).position = Vector3.Lerp(star02.transform.GetChild(i).position, new Vector3(4, -8, 0), 0.03f);
                    }
                    break;

                case EStage.Ingame2:
                    ring.SetActive(true);
                    ring.transform.position = Vector3.Lerp(ring.transform.position, new Vector3(4, -20, 5), 0.03f);
                    ring.transform.Rotate(new Vector3(10, 10, -10) * 10 * Time.deltaTime);
                    break;

                case EStage.Ingame3:
                    gas.SetActive(true);
                    break;
            }
        }
    }

    public void ItemSpawn(GameObject target)
    {
        int randomItem = Random.Range(0, 100);

        if (20 <= randomItem || randomItem > 35)
            itemNum = 0;

        if (35 <= randomItem || randomItem > 45)
            itemNum = 1;

        if (45 <= randomItem || randomItem > 65)
            itemNum = 2;

        if (65 <= randomItem || randomItem > 90)
            itemNum = 3;

        if (90 <= randomItem || randomItem > 100)
            itemNum = 4;

        Instantiate(itemSpawn[itemNum], target.transform.position, itemSpawn[itemNum].transform.rotation);
    }
}
