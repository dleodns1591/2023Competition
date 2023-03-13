using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShildRot : MonoBehaviour
{
    [SerializeField] int speed = 0;

    void Start()
    {
        
    }

    void Update()
    {
        gameObject.transform.Rotate(0, Time.deltaTime * speed, 0);
    }
}
