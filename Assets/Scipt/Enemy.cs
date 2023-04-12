using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EAttack
    {
        None,
        Meteor,
        Circle,
        CircleRot,
        Rush,
        Target,
        CircleT,
        CircleRotT,
    }
    public EAttack eAttack;

    [Header("이동")]
    [SerializeField] int moveSpeed = 0;
    [SerializeField] Vector3 moveDirection = Vector3.zero;
    [SerializeField] int meteorRotSpeed = 0;

    [Header("체력")]
    public int currnetHP = 0;

    [Header("타격")]
    [SerializeField] Material[] hit = new Material[2];

    [Header("점수")]
    [SerializeField] int score = 0;

    [Header("공격")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject warningMark;
    [SerializeField] int bodyAttack = 0;
    [SerializeField] int circleCount = 0;
    [SerializeField] int circleRotCount = 0;
    GameObject warningMarkSummon;
    bool isRush = false;
    bool isT = false;
    bool isAttack = false;

    void Start()
    {
        if (eAttack != EAttack.Meteor)
            hit[0] = GetComponent<MeshRenderer>().material;
        else
            hit[0] = transform.GetChild(0).GetComponent<MeshRenderer>().material;

        Destroy(gameObject, 20);
    }

    void Update()
    {
        DIe();
        Move();
        Attack();
        RushAttack();
        TelapoteAttack();
    }

    void Move()
    {
        transform.position += moveSpeed * moveDirection * Time.deltaTime;

        if (eAttack == EAttack.Meteor)
            transform.GetChild(0).Rotate(new Vector3(meteorRotSpeed, meteorRotSpeed, -meteorRotSpeed), meteorRotSpeed * Time.deltaTime);
    }

    void DIe()
    {
        if (currnetHP <= 0)
        {
            GameManager.instance.currentScore += score;
            GameManager.instance.currentEnemyDie++;
            SpawnManager.instance.ItemSpawn(gameObject);
            Destroy(gameObject);
        }
    }

    void Attack()
    {
        switch (eAttack)
        {
            case EAttack.Circle:
                StartCoroutine(CirCleAttack(circleCount));
                break;

            case EAttack.CircleRot:
                StartCoroutine(CirCleRotAttack(circleRotCount));
                break;

            case EAttack.Rush:
                StartCoroutine(Rush());
                break;

            case EAttack.Target:
                StartCoroutine(Target());
                break;

            case EAttack.CircleT:
                StartCoroutine(TelapoteCircle(circleCount));
                break;

            case EAttack.CircleRotT:
                StartCoroutine(TelapoteCircleRot(circleRotCount));
                break;
        }
    }

    IEnumerator CirCleAttack(int count)
    {
        if (!isAttack)
        {
            isAttack = true;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 360; j += 360 / count)
                    Instantiate(bullet, transform.position, Quaternion.Euler(-90, 0, j), SpawnManager.instance.bulletVowel.transform);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    IEnumerator CirCleRotAttack(int count)
    {
        if (!isAttack)
        {
            isAttack = true;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 360; j += 360 / count)
                {
                    Instantiate(bullet, transform.position, Quaternion.Euler(-90, 0, j), SpawnManager.instance.bulletVowel.transform);
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    IEnumerator Rush()
    {
        if (!isAttack)
        {
            isAttack = true;

            while (true)
            {
                isRush = true;
                yield return new WaitForSeconds(3);
                isRush = false;
                yield return new WaitForSeconds(3);
            }
        }
    }

    void RushAttack()
    {
        if (!isRush)
            moveSpeed = 5;
        else
        {
            moveSpeed = 0;
            transform.position = Vector3.Lerp(transform.position, Player.instance.transform.position, 0.01f);
        }
    }

    IEnumerator Target()
    {
        if (transform.position.z <= 10)
        {
            moveSpeed = 0;
            transform.position = Vector3.Lerp(transform.position, new Vector3(Player.instance.transform.position.x, transform.position.y, transform.position.z), 0.01f);

            if (!isAttack)
            {
                isAttack = true;
                while (true)
                {
                    yield return new WaitForSeconds(0.5f);
                    Instantiate(bullet, transform.position, bullet.transform.rotation, SpawnManager.instance.bulletVowel.transform);
                }
            }
        }
    }

    IEnumerator TelapoteCircle(int count)
    {
        if (transform.position.z <= 10)
        {
            if (!isAttack)
            {
                isAttack = true;

                while (true)
                {
                    isT = true;
                    int randomPosX = Random.Range(-14, 24);
                    int randomPosZ = Random.Range(-10, 10);

                    Vector3 randomPos = new Vector3(randomPosX, -8, randomPosZ);

                    warningMarkSummon = Instantiate(warningMark, randomPos, warningMark.transform.rotation);
                    Destroy(warningMarkSummon, 1);

                    for (int i = 0; i < 360; i += 360 / count)
                        Instantiate(bullet, transform.position, Quaternion.Euler(-90, 0, i), SpawnManager.instance.bulletVowel.transform);

                    yield return new WaitForSeconds(1.5f);
                    isT = false;
                    yield return new WaitForSeconds(1.5f);
                }
            }
        }
    }

    IEnumerator TelapoteCircleRot(int count)
    {
        if (transform.position.z <= 10)
        {
            if (!isAttack)
            {
                isAttack = true;

                while (true)
                {
                    isT = true;
                    int randomPosX = Random.Range(-14, 24);
                    int randomPosZ = Random.Range(-10, 10);

                    Vector3 randomPos = new Vector3(randomPosX, -8, randomPosZ);

                    warningMarkSummon = Instantiate(warningMark, randomPos, warningMark.transform.rotation);
                    Destroy(warningMarkSummon, 1);

                    for (int i = 0; i < 360; i += 360 / count)
                    {
                        Instantiate(bullet, transform.position, Quaternion.Euler(-90, 0, i), SpawnManager.instance.bulletVowel.transform);
                        yield return new WaitForSeconds(0.1f);
                    }

                    yield return new WaitForSeconds(1.5f);
                    isT = false;
                    yield return new WaitForSeconds(1.5f);
                }
            }
        }
    }

    void TelapoteAttack()
    {
        if (isT && warningMarkSummon != null)
            transform.position = warningMarkSummon.transform.position;
    }


    IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Player.instance.currentHp - bodyAttack >= 0)
                Player.instance.currentHp -= bodyAttack;
            else
                Player.instance.currentHp = 0;

            Destroy(gameObject);
        }

        if (other.CompareTag("PlayerBullet"))
        {
            if (eAttack != EAttack.Meteor)
            {
                GetComponent<MeshRenderer>().material = hit[1];
                yield return new WaitForSeconds(0.05f);
                GetComponent<MeshRenderer>().material = hit[0];
            }

            else
            {
                transform.GetChild(0).GetComponent<MeshRenderer>().material = hit[1];
                yield return new WaitForSeconds(0.05f);
                transform.GetChild(0).GetComponent<MeshRenderer>().material = hit[0];
            }
        }
    }
}
