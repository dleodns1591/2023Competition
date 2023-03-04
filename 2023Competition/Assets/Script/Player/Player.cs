using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("¿Ãµø")]
    [SerializeField] int moveSpeed;
    [SerializeField] Vector3 moveDirection = Vector3.zero;

    void Start()
    {

    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(x, 0, y);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (x == 1)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, -20)), 0.1f);
        else if (x == -1)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 20)), 0.1f);
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), 0.1f);

    }
}
