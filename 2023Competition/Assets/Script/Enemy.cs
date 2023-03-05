using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EAttack
    {
        Circle,
        Meteor,
    }
    public EAttack eAttack;

    [Header("이동")]
    [SerializeField] int moveSpeed = 0;
    [SerializeField] Vector3 moveDirection = Vector3.zero;

    [Header("공격")]
    [SerializeField] GameObject bullet;
    [SerializeField] int attack = 0;
    [SerializeField] int score = 0;

    [Header("체력")]
    public int hp = 0;

    void Start()
    {
        Attack();
    }

    void Update()
    {
        Move();
        Hp();
    }

    void Move()
    {
        transform.position += moveSpeed * moveDirection * Time.deltaTime;
    }

    void Hp()
    {
        if (hp == 0)
        {
            GameManager.instance.currentScore += score;
            Destroy(gameObject);
        }
    }

    void Attack()
    {
        switch (eAttack)
        {
            case EAttack.Circle:
                break;
            case EAttack.Meteor:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
            Destroy(gameObject);

        else if (other.CompareTag("PlayerBullet"))
            hp -= Bullet.instance.attack;
    }
}
