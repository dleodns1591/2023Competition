using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Boss : MonoBehaviour
{
    public static Boss instance;
    void Awake() => instance = this;

    public enum EBoss
    {
        None,
        Boss01,
        Boss02,
        Boss03,
    }
    public EBoss eBoss;

    [Header("체력")]
    public float currentHp = 0;
    [SerializeField] int maxHp = 0;
    [SerializeField] Slider hpSlider;
    [SerializeField] Text hpValue;
    bool isDie = false;

    [Header("이동")]
    [SerializeField] int moveCool = 0;
    int randomMove = 0;
    bool isMove = false;

    [Header("스코어")]
    [SerializeField] int score = 0;

    [Header("공격")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject rushBullet;
    [SerializeField] GameObject bigBullet;
    [SerializeField] GameObject skyBullet;
    [SerializeField] GameObject warningMisile;
    [SerializeField] int circleCount = 0;
    [SerializeField] int circleRotCount = 0;
    [SerializeField] int bigCircleCount = 0;
    [SerializeField] int missileCount = 0;
    bool isAttack = false;


    void Start()
    {
        currentHp = maxHp;
    }

    void Update()
    {
        Gauge();
        Die();
        StartCoroutine(Attack());
        StartCoroutine(Move());
    }

    void Die()
    {
        if (currentHp <= 0)
        {
            if (!isDie)
            {
                isDie = true;
                GameManager.instance.currentScore += score;
            }
            UIManager.instance.ClearWindow();
        }
    }
    IEnumerator Move()
    {
        if (!isMove)
        {
            isMove = true;
            randomMove = Random.Range(0, 3);
            yield return new WaitForSeconds(moveCool);
            isMove = false;
            yield return new WaitForSeconds(moveCool);

        }

        switch (randomMove)
        {
            case 0:
                transform.position = Vector3.Lerp(transform.position, new Vector3(4, -8, 7), 0.01f);
                break;

            case 1:
                transform.position = Vector3.Lerp(transform.position, new Vector3(-11, -8, 7), 0.01f);
                break;

            case 2:
                transform.position = Vector3.Lerp(transform.position, new Vector3(20, -8, 7), 0.01f);
                break;
        }
    }

    IEnumerator Attack()
    {
        if (!isAttack)
        {
            isAttack = true;
            yield return new WaitForSeconds(2);

            switch (eBoss)
            {
                case EBoss.Boss01:
                    int randomAttack01 = Random.Range(0, 2);
                    switch (randomAttack01)
                    {
                        case 0:
                            StartCoroutine(CirCleAttack(circleCount));
                            break;

                        case 1:
                            StartCoroutine(CirCleRotAttack(circleRotCount));
                            break;
                    }
                    break;

                case EBoss.Boss02:
                    int randomAttack02 = Random.Range(0, 4);
                    switch (randomAttack02)
                    {
                        case 0:
                            StartCoroutine(CirCleAttack(circleCount));
                            break;

                        case 1:
                            StartCoroutine(CirCleRotAttack(circleRotCount));
                            break;

                        case 2:
                            RushAttakc();
                            break;

                        case 3:
                            StartCoroutine(BigCircleAttack(bigCircleCount));
                            break;
                    }

                    break;

                case EBoss.Boss03:
                    int randomAttack03 = Random.Range(0, 6);
                    switch (randomAttack03)
                    {
                        case 0:
                            StartCoroutine(CirCleAttack(circleCount));
                            break;

                        case 1:
                            StartCoroutine(CirCleRotAttack(circleRotCount));
                            break;

                        case 2:
                            RushAttakc();
                            break;

                        case 3:
                            StartCoroutine(BigCircleAttack(bigCircleCount));
                            break;

                        case 4:
                            SkyAttack(missileCount);
                            break;

                        case 5:
                            SkyAttack(missileCount);
                            break;
                    }

                    break;
            }

        }
    }
    IEnumerator CirCleAttack(int count)
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 360; j += 360 / count)
                Instantiate(bullet, transform.position, Quaternion.Euler(-90, 0, j), SpawnManager.instance.bulletVowel.transform);
            yield return new WaitForSeconds(0.5f);
        }

        isAttack = false;
    }

    IEnumerator CirCleRotAttack(int count)
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 360; j += 360 / count)
            {
                Instantiate(bullet, transform.position, Quaternion.Euler(-90, 0, j), SpawnManager.instance.bulletVowel.transform);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
        }

        isAttack = false;
    }

    void RushAttakc()
    {
        for (int i = 0; i < 2; i++)
            Instantiate(rushBullet, transform.position, rushBullet.transform.rotation, SpawnManager.instance.bulletVowel.transform);

        isAttack = false;
    }

    IEnumerator BigCircleAttack(int count)
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 360; j += 360 / count)
                Instantiate(bigBullet, transform.position, Quaternion.Euler(bullet.transform.position.x, 0, j), SpawnManager.instance.bulletVowel.transform);
            yield return new WaitForSeconds(0.5f);
        }

        isAttack = false;
    }

    void SkyAttack(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randomPosX = Random.Range(-14, 24);
            int randomPosZ = Random.Range(-10, 10);

            GameObject warningMark = Instantiate(warningMisile, new Vector3(randomPosX, -10, randomPosZ), warningMisile.transform.rotation);
            Instantiate(skyBullet, new Vector3(randomPosX, 30, randomPosZ), skyBullet.transform.rotation);

            Destroy(warningMark, 1);
        }
    }


    void Gauge()
    {
        hpValue.text = "Boss : " + (int)currentHp;
        hpSlider.value = Mathf.Lerp(hpSlider.value, currentHp / maxHp, 0.01f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            currentHp -= other.GetComponent<Bullet>().attack;
            Destroy(other.gameObject);
        }
    }
}
