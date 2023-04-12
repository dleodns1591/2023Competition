using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] Material material;
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
        Vector2 offset = material.mainTextureOffset;
        offset.x += rotSpeed * Time.deltaTime;
        offset.y += rotSpeed * Time.deltaTime;
        material.mainTextureOffset = offset;
    }
}
