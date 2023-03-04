using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float scrollRange = 0;
    [SerializeField] int moveSpeed = 0;
    [SerializeField] Vector3 moveDirection = Vector3.back;

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
            transform.position = target.position + Vector3.forward * scrollRange;
    }
}
