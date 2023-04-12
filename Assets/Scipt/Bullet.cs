using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum EBullet
    {
        None,
        Player,
        Enemy,
    }
    public EBullet eBullet;

    [Header("이동")]
    [SerializeField] int moveSpeed = 0;
    [SerializeField] Vector3 moveDirection = Vector3.zero;

    [Header("공격")]
    public int attack = 0;
    [SerializeField] GameObject hitParticle;

    [Header("유도")]
    [SerializeField] bool isRush = false;
    [SerializeField] bool isRushUse = false;

    [Header("삼각형")]
    [SerializeField] bool isSam = false;

    void Start()
    {
        StartCoroutine(RushCheck());
        Destroy(gameObject, 10);
    }

    void Update()
    {
        Move();
        Rush();
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

        if (isSam)
            transform.Rotate(new Vector3(0, 10, 0) * 10 * Time.deltaTime);
    }

    void Hit(GameObject target)
    {
        Instantiate(hitParticle, target.transform.position, hitParticle.transform.rotation);
    }

    void Rush()
    {
        if (isRushUse)
        {
            if (isRush)
            {
                moveSpeed = 0;
                transform.position = Vector3.Lerp(transform.position, Player.instance.transform.position, 0.01f);
            }

            else
                moveSpeed = 15;
        }
    }

    IEnumerator RushCheck()
    {
        while (true)
        {
            isRush = true;
            yield return new WaitForSeconds(2);
            isRush = false;
            yield return new WaitForSeconds(3);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (eBullet)
        {
            case EBullet.Player:
                if (other.CompareTag("Enemy"))
                {
                    other.GetComponent<Enemy>().currnetHP -= attack;
                    Hit(gameObject);
                    Destroy(gameObject);
                }

                if (other.CompareTag("Boss"))
                    Hit(gameObject);
                break;

            case EBullet.Enemy:
                if (other.CompareTag("Player"))
                {
                    if (Player.instance.currentHp - attack >= 0)
                        Player.instance.currentHp -= attack;
                    else
                        Player.instance.currentHp = 0;

                    Destroy(gameObject);
                }
                break;
        }
    }
}
