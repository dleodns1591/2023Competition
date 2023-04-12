using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shild : MonoBehaviour
{
    [Header("회전 속도")]
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
