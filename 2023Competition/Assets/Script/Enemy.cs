using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EMove
    {
        None,
        UpDown,
    }

    public enum EAttack
    {
        None,
        Circle,
        Meteor,
    }
    public EMove eMove;
    public EAttack eAttack;

    [Header("이동")]
    [SerializeField] int moveSpeed = 0;
    [SerializeField] Vector3 moveDirection = Vector3.zero;

    [Header("공격")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject meteorAmong;
    [SerializeField] int attack = 0;
    public int score = 0;

    [Header("체력")]
    public int hp = 0;

    void Start()
    {
        StartCoroutine(Attack());
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
            Destroy(gameObject);
        }
    }

    IEnumerator Attack()
    {
        switch (eAttack)
        {
            case EAttack.Circle:
                while (true)
                {
                    for (int i = 0; i < 360; i += 13)
                    {
                        GameObject temp = Instantiate(bullet);
                        temp.transform.position = transform.position;
                        temp.transform.rotation = Quaternion.Euler(90, 0, i);
                    }
                    yield return new WaitForSeconds(1);
                }
                break;
        }
    }

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

        else if (other.CompareTag("PlayerBullet"))
            hp -= Bullet.instance.attack;

        else if (other.CompareTag("Player"))
        {
            Player.instance.currentHp -= attack;
            Destroy(gameObject);
        }
    }
}
