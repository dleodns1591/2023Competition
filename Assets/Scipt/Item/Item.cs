using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum EItem
    {
        None,
        Strong,
        Shild,
        HpHeal,
        FuelHeal,
        ScoreUp,
    }
    public EItem eItem;

    [Header("이동")]
    [SerializeField] int moveSpeed = 0;
    [SerializeField] Vector3 moveDirection = Vector3.zero;

    [Header("파티클")]
    [SerializeField] GameObject strongParticle;
    [SerializeField] GameObject hpParticle;
    [SerializeField] GameObject fuelParticle;


    void Start()
    {
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerGod")) 
        {
            switch(eItem)
            {
                case EItem.Strong:
                    if (GameManager.instance.currentLevel < 3)
                    {
                        GameManager.instance.currentLevel++;
                        Instantiate(strongParticle, Player.instance.transform.position, strongParticle.transform.rotation);
                    }
                    else
                        GameManager.instance.currentScore += 300;
                    break;

                case EItem.Shild:
                    Player.instance.ItemShild();
                    break;

                case EItem.HpHeal:
                    Instantiate(hpParticle, Player.instance.transform.position, hpParticle.transform.rotation);

                    if (Player.instance.currentHp + 20 <= Player.instance.maxHp)
                        Player.instance.currentHp += 20;
                    else
                        Player.instance.currentHp = Player.instance.maxHp;
                    break;

                case EItem.FuelHeal:
                    Instantiate(fuelParticle, Player.instance.transform.position, fuelParticle.transform.rotation);

                    if (Player.instance.currentFuel + 20 <= Player.instance.maxFuel)
                        Player.instance.currentFuel += 20;
                    else
                        Player.instance.currentFuel = Player.instance.maxFuel;
                    break;

                case EItem.ScoreUp:
                    GameManager.instance.currentScore += 400;
                    break;
            }

            Destroy(gameObject);
        }
    }
}
