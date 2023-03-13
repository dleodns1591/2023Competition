using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public enum EBoss
    {
        Boss1,
        Boss2,
        Boss3
    }
    public EBoss eBoss;

    [SerializeField] GameObject shild;
    [SerializeField] int shildSpeed = 0;

    [Header("체력")]
    [SerializeField] float currentHp;
    [SerializeField] int maxHp;
    [SerializeField] int hpDownSpeed = 0;
    [SerializeField] Slider bossSlider;

    [Header("이동")]
    [SerializeField] int moveTime = 0;
    int randomMove = 0;
    bool isMove = false;

    [Header("죽음")]
    [SerializeField] GameObject dieParticle;
    bool isDie = false;
    bool isDieBossMove = false;

    [Header("공격")]
    [SerializeField] GameObject bullet;
    [SerializeField] int circleCount = 0;
    [SerializeField] float circleCoolTime = 0;

    [Space(5)]
    [SerializeField] int delayCircleCount = 0;
    [SerializeField] float delayCircleTime = 0;
    bool isDelay = false;

    bool isAttack = false;

    void Start()
    {
        currentHp = maxHp;
    }

    void Update()
    {
        shild.transform.Rotate(new Vector3(0, shildSpeed * Time.deltaTime, 0));

        BossHP();
        StartCoroutine(Attack());
        StartCoroutine(Move());
        StartCoroutine(Die());
    }

    void BossHP()
    {
        bossSlider.value = Mathf.Lerp(bossSlider.value, currentHp / maxHp, Time.deltaTime * hpDownSpeed);
    }

    IEnumerator Move()
    {
        Vector3 leftPos = new Vector3(-6, -7, 4);
        Vector3 amongPos = new Vector3(2, -7, 4);
        Vector3 RightPos = new Vector3(13, -7, 4);

        if (!isMove)
        {
            isMove = true;
            yield return new WaitForSeconds(3);

            while (true)
            {
                yield return new WaitForSeconds(moveTime);
                randomMove = Random.Range(0, 3);
            }
        }

        switch (randomMove)
        {
            case 0:
                transform.position = Vector3.Lerp(transform.position, leftPos, 0.01f);
                break;
            case 1:
                transform.position = Vector3.Lerp(transform.position, amongPos, 0.01f);
                break;
            case 2:
                transform.position = Vector3.Lerp(transform.position, RightPos, 0.01f);
                break;
        }

    }

    IEnumerator Attack()
    {
        if (currentHp > 0 && !isAttack)
        {
            isAttack = true;

            yield return new WaitForSeconds(2);
            switch (eBoss)
            {
                case EBoss.Boss1:
                    int randomAttack = Random.Range(0, 2);

                    switch (randomAttack)
                    {
                        case 0:
                            StartCoroutine(AttackCircle(circleCount));
                            break;
                        case 1:
                            StartCoroutine(AttackCircleDelay(delayCircleCount));
                            break;
                    }

                    break;
                case EBoss.Boss2:
                    break;
                case EBoss.Boss3:
                    break;
            }
        }
    }

    IEnumerator AttackCircle(int count) // 원형 공격
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 360; j += 360 / count)
                Instantiate(bullet, transform.position, Quaternion.Euler(90, 0, j), SpawnManager.instance.bulletVowel.gameObject.transform);

            yield return new WaitForSeconds(circleCoolTime);
        }

        isAttack = false;
    }

    IEnumerator AttackCircleDelay(int count) // 딜레이 있는 원형 공격
    {
        for(int i = 0; i < 5; i++)
        {
            isDelay = false;

            for(int j = 0; j< 360; j+= 360 / count)
            {
                Instantiate(bullet, transform.position, Quaternion.Euler(90, 0, j), SpawnManager.instance.bulletVowel.gameObject.transform);
                yield return new WaitForSeconds(delayCircleTime);
            }

            isDelay = true;
            yield return new WaitForSeconds(delayCircleTime);
        }

        isAttack = false;
    }

    IEnumerator Die()
    {
        Vector3 dieParticlePos = new Vector3(transform.position.x, -3, transform.position.z);
        Vector3 dieBossPos = new Vector3(transform.position.x, -25, transform.position.z);

        if (isDieBossMove)
            transform.position = Vector3.Lerp(transform.position, dieBossPos, 0.01f);

        if (currentHp < 0 && !isDie)
        {
            isDie = true;
            GameObject particle = Instantiate(dieParticle, dieParticlePos, Quaternion.identity);
            isDieBossMove = true;

            yield return new WaitForSeconds(3f);

            Time.timeScale = 0;

            Destroy(particle);
            UIManager.instance.clearWindow.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            currentHp -= Bullet.instance.attack;
            Destroy(other.gameObject);
        }
    }
}
