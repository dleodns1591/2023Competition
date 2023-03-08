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

    void Start()
    {

    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DestroyWall"))
            Destroy(gameObject);

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
    }
}
