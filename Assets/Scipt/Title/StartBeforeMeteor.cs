using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBeforeMeteor : MonoBehaviour
{

    [Header("¿Ãµø")]
    [SerializeField] int moveSpeed = 0;
    [SerializeField] int rotSpeed = 0;
    [SerializeField] Vector3 moveDirection = Vector3.zero;
    void Start()
    {
        Destroy(gameObject, 10);
    }

    void Update()
    {
        Rot();
        Move();
    }

    void Rot()
    {
        transform.GetChild(0).Rotate(new Vector3(rotSpeed, rotSpeed, -rotSpeed) * rotSpeed * Time.deltaTime);
    }

    void Move()
    {
        transform.position += moveSpeed * moveDirection * Time.deltaTime;
    }
}
