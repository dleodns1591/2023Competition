using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum EItem
    {
        Strong,
        Shild,
        Repair,
        Fuel,
        Score,
    }
    public EItem eItem;

    [Header("이동")]
    [SerializeField] int moveSpeed = 0;
    [SerializeField] Vector3 moveDirection = Vector3.zero;

    Player player;

    void Start()
    {
        player = Player.instance;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += moveSpeed * moveDirection * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (eItem)
            {
                case EItem.Strong: // 레벨업 증가 아이템
                    if (GameManager.instance.currentLevel < 4)
                    {
                        GameManager.instance.currentLevel++;
                    }
                    else
                        GameManager.instance.currentScore += 300;
                    break;

                case EItem.Shild: // 방어 아이템
                    break;

                case EItem.Repair: // 내구도 수리 아이템
                    if (player.currentHp + 20 <= player.maxHp)
                        player.currentHp += 20;
                    else
                        player.currentHp = player.maxHp;
                    break;

                case EItem.Fuel: // 연료 증가 아이템
                    if (player.currentFuel + 30 <= player.maxFuel)
                        player.currentFuel += 30;
                    else
                        player.currentFuel = player.maxFuel;
                    break;

                case EItem.Score: // 점수 증가 아이템
                    GameManager.instance.currentScore += 500;
                    break;
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("DestroyWall"))
            Destroy(gameObject);
    }
}
