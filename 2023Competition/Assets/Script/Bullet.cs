using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet instance;
    void Awake() => instance = this;

    public enum EBullet
    {
        Player,
        Enemy,
    }
    public EBullet eBullet;

    [Header("이동")]
    [SerializeField] int moveSpeed = 0;
    [SerializeField] Vector3 moveDirection = Vector3.zero;

    [Header("공격")]
    public int attack = 0;

    void Update()
    {
        Move();
    }

    void Move()
    {
        switch (eBullet)
        {
            case EBullet.Player:
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                break;

            case EBullet.Enemy:
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        switch (eBullet)
        {
            case EBullet.Player:
                if (other.CompareTag("Enemy"))
                {
                    other.GetComponent<Enemy>().hp -= attack;
                    Destroy(gameObject);
                }
                break;

            case EBullet.Enemy:
                if (other.CompareTag("Player"))
                {
                    Player.instance.currentHp -= attack;
                    Destroy(gameObject);
                }
                break;
        }

        if(other.CompareTag("DestroyWall"))
            Destroy(gameObject);
    }
}
