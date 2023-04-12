using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    void Awake() => instance = this;

    [SerializeField] int tutorialNum;
    [SerializeField] int playerNameNum;
    [SerializeField] GameObject tutorialObj;
    [SerializeField] Text playerNameTyping;
    [SerializeField] Button nextBtn;
    public bool isTutorial = false;
    bool isLast = false;

    void Start()
    {
        Time.timeScale = 0;
        Next();
        tutorialObj.transform.GetChild(0).gameObject.SetActive(true);
    }

    void Update()
    {
        Tutorial();


    }

    void Tutorial()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (tutorialNum < tutorialObj.transform.childCount && tutorialNum != playerNameNum)
            {
                tutorialNum++;

                tutorialObj.transform.GetChild(tutorialNum - 1).gameObject.SetActive(false);

                if (tutorialNum != tutorialObj.transform.childCount)
                    tutorialObj.transform.GetChild(tutorialNum).gameObject.SetActive(true);
            }
        }

        if (tutorialNum == tutorialObj.transform.childCount)
        {
            if (!isLast)
            {
                isTutorial = true;
                isLast = true;
                Time.timeScale = 1;
            }
        }
    }

    void Next()
    {
        nextBtn.onClick.AddListener(() =>
        {
            GameManager.instance.PlayerName = playerNameTyping.text;
            tutorialNum++;

            tutorialObj.transform.GetChild(tutorialNum - 1).gameObject.SetActive(false);
            tutorialObj.transform.GetChild(tutorialNum).gameObject.SetActive(true);
        });
    }
}
