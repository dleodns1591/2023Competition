using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [Header("적 소환")]
    [SerializeField] List<GameObject> enemyList = new List<GameObject>();
    [SerializeField] int spawnTime = 0;

    [Header("아이템")]
    public List<GameObject> itemList = new List<GameObject>();

    void Start()
    {

    }

    void Update()
    {
        
    }

    void Awake()
    {
        instance = this;

        //StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {
        while(true)
        {
            float posX = Random.Range(-17, 15);
            int enemyRandom = Random.Range(0, enemyList.Count);

            Instantiate(enemyList[enemyRandom], new Vector3(posX, Player.instance.transform.position.y, 17), enemyList[enemyRandom].transform.rotation, gameObject.transform);
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
