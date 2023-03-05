using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyList = new List<GameObject>();
    [SerializeField] int spawnTime = 0;

    void Start()
    {

    }

    void Update()
    {
        
    }

    void Awake()
    {
        StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {
        while(true)
        {
            float posX = Random.Range(-17, 15);
            int enemyRandom = Random.Range(0, enemyList.Count);

            Instantiate(enemyList[enemyRandom], new Vector3(posX, Player.instance.transform.position.y, 17), enemyList[enemyRandom].transform.rotation);
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
