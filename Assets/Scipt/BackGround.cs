using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [Header("¿Ãµø")]
    [SerializeField] int moveSpeed = 0;
    [SerializeField] float scrollRange = 0;
    [SerializeField] GameObject target;
    [SerializeField] Vector3 moveDirection = Vector3.zero;

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

        if (transform.position.z <= -scrollRange)
            transform.position = target.transform.position + Vector3.forward * scrollRange;
    }
}
