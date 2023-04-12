using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] GameObject bgm;
    [SerializeField] GameObject effect;

    void Start()
    {

    }

    void Update()
    {
        SoundSetting();
    }

    void SoundSetting()
    {
        if (GameManager.instance.isBGM)
            bgm.SetActive(true);
        else
            bgm.SetActive(false);


        if (GameManager.instance.isEffect)
            effect.SetActive(true);
        else
            effect.SetActive(false);
    }
}
