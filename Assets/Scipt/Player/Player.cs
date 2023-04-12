using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player instance;
    void Awake() => instance = this;

    [Header("이동")]
    [SerializeField] int moveSpeed = 0;
    [SerializeField] Vector3 moveDirection = Vector3.zero;

    [Header("스테이터스")]
    public float currentHp = 0;
    public int maxHp = 0;
    public float currentFuel = 0;
    public int maxFuel = 0;

    [Header("공격")]
    [SerializeField] GameObject bullet;
    [SerializeField] float attackRange;
    [SerializeField] GameObject bulletTarget1_1;
    [SerializeField] GameObject bulletTarget1_2;
    [SerializeField] GameObject bulletTarget2_1;
    [SerializeField] GameObject bulletTarget2_2;
    [SerializeField] GameObject bulletTarget3_1;
    [SerializeField] GameObject bulletTarget3_2;
    [SerializeField] GameObject bulletTarget4_1;
    [SerializeField] GameObject bulletTarget4_2;

    [Header("타격")]
    [SerializeField] GameObject hitWinndow;
    [SerializeField] GameObject barrier;

    [Header("레벨")]
    [SerializeField] GameObject level;

    [Header("스킬")]
    [SerializeField] Image healSkill;
    [SerializeField] Image bombSkill;
    [SerializeField] GameObject healParticle;
    [SerializeField] GameObject bombParticle;
    public float healCoolTime = 0;
    public float bombCoolTime = 0;
    [SerializeField] int healMaxCoolTime = 0;
    [SerializeField] int bombMaxCoolTime = 0;
    [SerializeField] GameObject heal;
    [SerializeField] GameObject bomb;
    [SerializeField] CanvasGroup notSkillText;

    [SerializeField] Text healCoolText;
    [SerializeField] Text bombCoolText;
    [SerializeField] Text healUseCount;
    [SerializeField] Text bombUseCount;
    GameObject healSummon;
    GameObject bombSummon;
    bool isHealUse = false;
    bool isBombUse = false;

    [Header("방어막 아이템")]
    [SerializeField] GameObject shild;

    [Header("사운드")]
    [SerializeField] GameObject attackSound;

    void Start()
    {
        currentHp = maxHp;
        currentFuel = maxFuel;
    }

    void Update()
    {
        Move();
        Attack();
        Skill();
        Leve();
        MoveRot();
    }

    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -14, 24), -8, Mathf.Clamp(transform.position.z, -10, 10));
    }

    void Move()
    {
        if (SpawnManager.instance.eStage != SpawnManager.EStage.Tutorial)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector3(x, 0, y);

            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        else
        {
            if (TutorialManager.instance.isTutorial)
            {
                float x = Input.GetAxisRaw("Horizontal");
                float y = Input.GetAxisRaw("Vertical");

                moveDirection = new Vector3(x, 0, y);

                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }
    }

    void MoveRot()
    {
        switch (moveDirection.x)
        {
            case -1:
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 30), 0.05f);
                break;

            case 0:
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 0.05f);
                break;

            case 1:
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -30), 0.05f);
                break;
        }
    }

    void Attack()
    {
        if (SpawnManager.instance.eStage != SpawnManager.EStage.Tutorial)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine("BulletSummon");
                attackSound.SetActive(true);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                StopCoroutine("BulletSummon");
                attackSound.SetActive(false);
            }
        }

        else
        {
            if (TutorialManager.instance.isTutorial)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartCoroutine("BulletSummon");
                    attackSound.SetActive(true);
                }
                else if (Input.GetKeyUp(KeyCode.Space))
                {
                    StopCoroutine("BulletSummon");
                    attackSound.SetActive(false);
                }
            }
        }

    }

    void Leve()
    {
        for (int i = 0; i <= GameManager.instance.currentLevel; i++)
            level.transform.GetChild(i).gameObject.SetActive(true);
    }


    public void ItemShild()
    {
        StopCoroutine("Shild");
        StartCoroutine("Shild");
    }

    IEnumerator Shild()
    {
        tag = "PlayerGod";
        shild.SetActive(true);
        yield return new WaitForSeconds(2);
        shild.SetActive(false);
        tag = "Player";
    }

    #region 스킬
    void Skill()
    {
        if (SpawnManager.instance.eStage != SpawnManager.EStage.Tutorial)
        {
            StartCoroutine(HealSkill());
            StartCoroutine(BombSkill());
        }

        else
        {
            if (TutorialManager.instance.isTutorial)
            {
                StartCoroutine(HealSkill());
                StartCoroutine(BombSkill());
            }
        }
    }

    IEnumerator HealSkill()
    {
        healSkill.fillAmount = Mathf.Lerp(healSkill.fillAmount, healCoolTime / healMaxCoolTime, 0.01f);
        healUseCount.text = GameManager.instance.healUseCount.ToString();
        healCoolText.text = ((int)healCoolTime).ToString();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!isHealUse)
            {
                isHealUse = true;
                healSkill.fillAmount = 1;
                healCoolTime = healMaxCoolTime;
                GameManager.instance.healUseCount++;
                StartCoroutine(HealCool());

                healSummon = Instantiate(heal, new Vector3(0, 4, -30), heal.transform.rotation);
                Destroy(healSummon, 0.5f);

                yield return new WaitForSeconds(0.5f);

                if (currentHp + 20 <= maxHp)
                    currentHp += 20;
                else
                    currentHp = maxHp;

                GameObject particle = Instantiate(healParticle, new Vector3(4, -15, 0), healParticle.transform.rotation);
                Destroy(particle, 1);
            }

            else
            {
                StopCoroutine("NotSkill");
                StartCoroutine("NotSkill");
            }
        }

        if (healCoolTime <= 0)
            isHealUse = false;

        if (!isHealUse)
            healCoolText.gameObject.SetActive(false);

        else
            healCoolText.gameObject.SetActive(true);

        if (healSummon != null)
            healSummon.transform.position = Vector3.Lerp(healSummon.transform.position, new Vector3(4, -15, 0), 0.05f);
    }

    IEnumerator BombSkill()
    {
        bombSkill.fillAmount = Mathf.Lerp(bombSkill.fillAmount, bombCoolTime / bombMaxCoolTime, 0.01f);
        bombUseCount.text = GameManager.instance.bombUseCount.ToString();
        bombCoolText.text = ((int)bombCoolTime).ToString();

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!isBombUse)
            {
                isBombUse = true;
                bombSkill.fillAmount = 1;
                bombCoolTime = bombMaxCoolTime;
                GameManager.instance.bombUseCount++;
                StartCoroutine(BombCool());

                bombSummon = Instantiate(bomb, new Vector3(0, 4, -30), bomb.transform.rotation);
                Destroy(bombSummon, 0.5f);

                yield return new WaitForSeconds(0.5f);
                if (Boss.instance != null)
                {
                    GameObject particle = Instantiate(bombParticle, Boss.instance.transform.position, bombParticle.transform.rotation);
                    Boss.instance.currentHp -= 200;
                    Destroy(particle, 1);
                }

                for (int i = 0; i < SpawnManager.instance.spawnEnemy.transform.childCount; i++)
                {
                    GameObject particle = Instantiate(bombParticle, SpawnManager.instance.spawnEnemy.transform.GetChild(i).position, bombParticle.transform.rotation);
                    SpawnManager.instance.spawnEnemy.transform.GetChild(i).GetComponent<Enemy>().currnetHP -= 200;
                    Destroy(particle, 1);
                }

                for (int i = 0; i < SpawnManager.instance.bulletVowel.transform.childCount; i++)
                {
                    GameObject particle = Instantiate(bombParticle, SpawnManager.instance.bulletVowel.transform.GetChild(i).position, bombParticle.transform.rotation);
                    Destroy(SpawnManager.instance.bulletVowel.transform.GetChild(i).gameObject);
                    Destroy(particle, 1);
                }
            }

            else
            {
                StopCoroutine("NotSkill");
                StartCoroutine("NotSkill");
            }
        }

        if (bombCoolTime <= 0)
            isBombUse = false;

        if (!isBombUse)
            bombCoolText.gameObject.SetActive(false);

        else
            bombCoolText.gameObject.SetActive(true);

        if (bombSummon != null)
            bombSummon.transform.position = Vector3.Lerp(bombSummon.transform.position, new Vector3(4, -15, 0), 0.05f);
    }

    IEnumerator HealCool()
    {
        while (healCoolTime >= 0)
        {
            healCoolTime -= 1;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator BombCool()
    {
        while (bombCoolTime >= 0)
        {
            bombCoolTime -= 1;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator NotSkill()
    {
        float fadeCount = 1;
        notSkillText.alpha = fadeCount;

        while (fadeCount >= 0)
        {
            fadeCount -= 0.02f;
            notSkillText.alpha = fadeCount;
            yield return new WaitForSeconds(0.01f);
        }
    }
    #endregion

    IEnumerator BulletSummon()
    {
        while (true)
        {
            if (GameManager.instance.currentLevel >= 0)
            {
                Instantiate(bullet, bulletTarget1_1.transform.position, bullet.transform.rotation);
                Instantiate(bullet, bulletTarget1_2.transform.position, bullet.transform.rotation);
            }

            if (GameManager.instance.currentLevel >= 1)
            {
                Instantiate(bullet, bulletTarget2_1.transform.position, bullet.transform.rotation);
                Instantiate(bullet, bulletTarget2_2.transform.position, bullet.transform.rotation);
            }

            if (GameManager.instance.currentLevel >= 2)
            {
                Instantiate(bullet, bulletTarget3_1.transform.position, bullet.transform.rotation);
                Instantiate(bullet, bulletTarget3_2.transform.position, bullet.transform.rotation);
            }

            if (GameManager.instance.currentLevel >= 3)
            {
                Instantiate(bullet, bulletTarget4_1.transform.position, bullet.transform.rotation);
                Instantiate(bullet, bulletTarget4_2.transform.position, bullet.transform.rotation);
            }

            yield return new WaitForSeconds(attackRange);
        }
    }

    IEnumerator Hit()
    {
        tag = "PlayerGod";
        hitWinndow.SetActive(true);
        barrier.SetActive(true);
        StartCoroutine(ShakeCamer(1, 1));
        yield return new WaitForSeconds(2);
        tag = "Player";
        barrier.SetActive(false);
        hitWinndow.SetActive(false);
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

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Player"))
        {
            if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet") || other.CompareTag("Boss"))
                StartCoroutine(Hit());
        }
    }
}
