using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shild : MonoBehaviour
{
    [Header("ȸ�� �ӵ�")]
    [SerializeField] int rotSpeed = 0;

    void Start()
    {
        
    }

    void Update()
    {
        Rot();
    }

    void Rot()
    {
        transform.Rotate(new Vector3(0, rotSpeed, 0), rotSpeed * Time.deltaTime);
    }
}
