using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player instance;
    void Awake() => instance = this;

    public GameObject shild;
    public GameObject Barrier;
    public bool isShild = false;

    [Header("이동")]
    [SerializeField] int moveSpeed = 0;
    [SerializeField] Vector3 moveDirection = Vector3.zero;

    [Header("공격")]
    [SerializeField] GameObject bullet;
    [SerializeField] float attackRange = 0;

    [Space(10)]
    [SerializeField] GameObject levelTarget1;
    [SerializeField] GameObject levelTarget2_1;
    [SerializeField] GameObject levelTarget2_2;
    [SerializeField] GameObject levelTarget3_1;
    [SerializeField] GameObject levelTarget3_2;
    [SerializeField] GameObject levelTarget4_1;
    [SerializeField] GameObject levelTarget4_2;

    [Header("맞음")]
    [SerializeField] GameObject hit;
    [SerializeField] float shakeTime = 0;
    [SerializeField] float shakeIntensity = 0;

    [Header("내구도 / 연료")]
    public float currentHp = 0;
    public float currentFuel = 0;
    public int maxHp = 0;
    public int maxFuel = 0;

    [Header("스킬")]
    [SerializeField] float hpCoolTime = 0;
    public float hpCurrentCoolTime = 0;
    [SerializeField] float bombCoolTime = 0;
    public float bombCurrentCoolTime = 0;

    [SerializeField] GameObject bombPrefab;
    [SerializeField] GameObject enemyDieParticle;

    [SerializeField] Image hpSkill;
    [SerializeField] Image bombSkill;

    [SerializeField] Text notSkillText;

    GameObject putBomb;

    bool isHpSkillUse = false;
    bool isBombSkillUse = false;

    void Start()
    {
        currentHp = maxHp;
        currentFuel = maxFuel;
    }

    void Update()
    {
        Attack();
        GameOver();
        StartCoroutine(Skill());
        Level();
        StartCoroutine(ShildItem());

        if (putBomb != null)
            putBomb.transform.position = Vector3.Slerp(putBomb.transform.position, new Vector3(-1, -7, -1), 0.05f);

    }

    void FixedUpdate()
    {
        Move();
    }

    void LateUpdate()
    {
        // 플레이어가 화면 범위 바깥으로 못나가게 함.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -10, 15),
                                         Mathf.Clamp(transform.position.y, -7, -7),
                                         Mathf.Clamp(transform.position.z, -10, 7));

    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(x, 0, y);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (x == 1)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, -20)), 0.1f);
        else if (x == -1)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 20)), 0.1f);
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), 0.1f);
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine("BulletSummon");
        else if (Input.GetKeyUp(KeyCode.Space))
            StopCoroutine("BulletSummon");
    }

    #region 스킬
    IEnumerator Skill()
    {
        hpSkill.fillAmount = Mathf.Lerp(hpSkill.fillAmount, hpCurrentCoolTime / hpCoolTime, Time.deltaTime * 10);
        bombSkill.fillAmount = Mathf.Lerp(bombSkill.fillAmount, bombCurrentCoolTime / bombCoolTime, Time.deltaTime * 10);

        // 내구도 스킬
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!isHpSkillUse)
            {
                isHpSkillUse = true;
                hpSkill.fillAmount = 1;
                hpCurrentCoolTime = hpCoolTime;
                StartCoroutine(HpCoolTime());

                if (currentHp + 10 <= maxHp)
                    currentHp += 10;
                else
                    currentHp = maxHp;
            }

            else
            {
                StopCoroutine("NotSkill");
                StartCoroutine("NotSkill");
            }
        }

        if (hpCurrentCoolTime == 0)
            isHpSkillUse = false;

        // 폭탄
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!isBombSkillUse)
            {
                isBombSkillUse = true;
                bombSkill.fillAmount = 1;
                bombCurrentCoolTime = bombCoolTime;
                StartCoroutine(BombCoolTime());

                putBomb = Instantiate(bombPrefab, new Vector3(-1, -1, -20), bombPrefab.transform.rotation);
                yield return new WaitForSeconds(0.5f);

                for (int i = 0; i < SpawnManager.instance.transform.childCount; i++)
                {
                    GameManager.instance.currentScore += SpawnManager.instance.transform.GetChild(i).GetComponent<Enemy>().score;

                    GameObject particle = Instantiate(enemyDieParticle, SpawnManager.instance.transform.GetChild(i).position, Quaternion.identity);
                    Destroy(putBomb);
                    Destroy(particle, 3);
                    Destroy(SpawnManager.instance.transform.GetChild(i).gameObject);
                }

                for(int i = 0; i < SpawnManager.instance.bulletVowel.transform.childCount; i++)
                    Destroy(SpawnManager.instance.bulletVowel.transform.GetChild(i).gameObject);

            }

            else
            {
                StopCoroutine("NotSkill");
                StartCoroutine("NotSkill");
            }
        }

        if (bombCurrentCoolTime == 0)
            isBombSkillUse = false;
    }

    IEnumerator HpCoolTime()
    {
        while (hpCurrentCoolTime > 0)
        {
            yield return new WaitForSeconds(1);
            hpCurrentCoolTime -= 1;
        }
        yield break;
    }

    IEnumerator BombCoolTime()
    {
        while (bombCurrentCoolTime > 0)
        {
            yield return new WaitForSeconds(1);
            bombCurrentCoolTime -= 1;
        }
        yield break;
    }

    IEnumerator NotSkill()
    {
        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            notSkillText.color = new Color(255, 255, 255, fadeCount);
        }

        yield return new WaitForSeconds(1);

        while (fadeCount >= 0)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            notSkillText.color = new Color(255, 255, 255, fadeCount);
        }
    }
    #endregion

    void Level()
    {
        transform.GetChild(GameManager.instance.currentLevel - 1).gameObject.SetActive(true);
    }

    void GameOver()
    {
        if (currentFuel <= 0 || currentHp <= 0)
        {
            Time.timeScale = 0;
            UIManager.instance.gameoverWindow.SetActive(true);
            UIManager.instance.overScore.text = "Score : " + GameManager.instance.currentScore;
        }
    }

    public IEnumerator Hit()
    {
        tag = "Invincibility";
        Barrier.SetActive(true);

        yield return new WaitForSeconds(2);

        Barrier.SetActive(false);
        tag = "Player";
    }

    public IEnumerator ShildItem()
    {
        if (isShild)
        {
            isShild = false;
            tag = "Invincibility";
            shild.SetActive(true);

            yield return new WaitForSeconds(2);

            shild.SetActive(false);
            tag = "Player";
        }

    }

    IEnumerator BulletSummon()
    {
        while (true)
        {
            Instantiate(bullet, levelTarget1.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));

            if (transform.GetChild(1).gameObject.activeSelf)
            {
                Instantiate(bullet, levelTarget2_1.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
                Instantiate(bullet, levelTarget2_2.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
            }

            if (transform.GetChild(2).gameObject.activeSelf)
            {
                Instantiate(bullet, levelTarget3_1.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
                Instantiate(bullet, levelTarget3_2.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
            }

            if (transform.GetChild(3).gameObject.activeSelf)
            {
                Instantiate(bullet, levelTarget4_1.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
                Instantiate(bullet, levelTarget4_2.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
            }


            yield return new WaitForSeconds(attackRange);
        }
    }


    public IEnumerator ShakeCmaer()
    {
        Vector3 camerPos = Camera.main.transform.position;
        float shake = shakeTime;

        hit.SetActive(true);
        while (shake > 0.0f)
        {
            Camera.main.transform.position = camerPos + Random.insideUnitSphere * shakeIntensity;
            shake -= Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = camerPos;
        hit.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && gameObject.CompareTag("Player"))
        {
            currentHp -= other.GetComponent<Enemy>().attack;
            StartCoroutine(ShakeCmaer());
            Destroy(other.gameObject);

            StartCoroutine(Hit());
        }
    }
}
