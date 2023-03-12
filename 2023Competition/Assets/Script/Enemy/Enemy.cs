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
        CircleDelay
    }
    public EAttack eAttack;

    [Header("이동")]
    [SerializeField] int moveSpeed = 0;
    [SerializeField] Vector3 moveDirection = Vector3.zero;

    [Header("공격")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject meteorAmong;
    public int attack = 0;
    public int score = 0;

    [Space(10)]
    [SerializeField] int circleCount = 0;

    [Space(5)]
    [SerializeField] int circleDelayCount = 0;
    [SerializeField] bool isDelay = false;

    [Header("체력")]
    public int hp = 0;

    void Start()
    {
        Invoke("Attack", 1);
    }

    void Update()
    {
        Hp();
        MeteorRotation();
        Move();
    }

    void Move()
    {
        transform.position += moveSpeed * moveDirection * Time.deltaTime;
    }

    void Hp()
    {
        if (hp <= 0)
        {
            RandomItem();
            GameManager.instance.currentScore += score;
            GameManager.instance.dieEnemyCount++;
            Destroy(gameObject);
        }
    }

    #region 공격
    void Attack()
    {
        switch (eAttack)
        {
            case EAttack.Circle:
                StartCoroutine(AttackCircle(circleCount));
                break;

            case EAttack.CircleDelay:
                StartCoroutine(AttackCircleDelay(circleDelayCount));
                break;
        }
    }

    IEnumerator AttackCircle(int count) // 원형 공격
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 360; j += 360 / count)
                Instantiate(bullet, transform.position, Quaternion.Euler(90, 0, j));

            yield return new WaitForSeconds(2);
        }
    }

    IEnumerator AttackCircleDelay(int count) // 딜레이 있는 원형 공격
    {
        for (int i = 0; i < 3; i++)
        {
            isDelay = false;

            for (int j = 0; j < 360; j += 360 / count)
            {
                Instantiate(bullet, transform.position, Quaternion.Euler(90, 0, j));
                yield return new WaitForSeconds(0.1f);
            }

            isDelay = true;

            yield return new WaitForSeconds(2);
        }
    }

    #endregion

    void MeteorRotation()
    {
        int speed = 10;

        switch (eAttack)
        {
            case EAttack.Meteor:
                meteorAmong.transform.Rotate(new Vector3(speed, speed, -speed) * Time.deltaTime * 2 * speed);
                break;
        }
    }

    void RandomItem()
    {
        int randomItem = Random.Range(0, 100);
        int itemNum = 0;

        // 꽝 : 30%, 강화 : 10%, 무적 : 15%, 연료 : 25%, 수리 : 10%, 스코어 : 10%

        if (0 < randomItem || randomItem > 31)
            itemNum = 0;

        if (30 < randomItem || randomItem > 46)
            itemNum = 1;

        if (45 < randomItem || randomItem > 56)
            itemNum = 2;

        if (55 < randomItem || randomItem > 81)
            itemNum = 3;

        if (80 < randomItem || randomItem > 91)
            itemNum = 4;

        if (90 < randomItem || randomItem > 101)
            itemNum = 5;

        if (itemNum != 0)
            Instantiate(SpawnManager.instance.itemList[itemNum - 1], transform.position, SpawnManager.instance.itemList[itemNum - 1].transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
            Destroy(gameObject);
    }
}
