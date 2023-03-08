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

    [Header("¿Ãµø")]
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (eItem)
            {
                case EItem.Strong:
                    if (player.level < 4)
                        player.level++;
                    else
                        GameManager.instance.currentScore += 300;
                    break;

                case EItem.Shild:
                    break;

                case EItem.Repair:
                    if (player.currentHp + 20 <= player.maxHp)
                        player.currentHp += 20;
                    else
                        player.currentHp = player.maxHp;
                    break;

                case EItem.Fuel:
                    if (player.currentFuel + 30 <= player.maxFuel)
                        player.currentFuel += 30;
                    else
                        player.currentFuel = player.maxFuel;
                    break;

                case EItem.Score:
                    GameManager.instance.currentScore += 500;
                    break;
            }

            Destroy(gameObject);
        }
    }
}
