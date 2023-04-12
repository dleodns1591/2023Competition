using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    Image fade;

    void Start()
    {
        fade = GetComponent<Image>();
        StartCoroutine(Fade());
    }

    void Update()
    {
    }

    IEnumerator Fade()
    {
        float fadeCount = 1;
        fade.color = new Color(255, 255, 255, fadeCount);

        while(fadeCount >= 0)
        {
            fadeCount -= 0.05f;
            fade.color = new Color(255, 255, 255, fadeCount);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}
