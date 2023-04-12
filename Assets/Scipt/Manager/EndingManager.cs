using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [Header("제 2의 지구")]
    [SerializeField] GameObject earth02;
    [SerializeField] int earth02RotSpeed = 0;

    [Header("스테이지 창")]
    [SerializeField] CanvasGroup stageWindow;

    [Header("플레이어")]
    [SerializeField] GameObject player;

    [Header("연출")]
    [SerializeField] GameObject cam01;

    [Header("UI")]
    [SerializeField] GameObject clearWindow;
    [SerializeField] GameObject directionText;
    [SerializeField] Text currentTime;
    [SerializeField] Text currentScore;
    [SerializeField] Text totalTime;
    [SerializeField] Text totalScore;
    [SerializeField] Button nextBtn;
    [SerializeField] Button titleBtn;
    bool isClear = false;
    bool isTotal = false;

    void Start()
    {
        Time.timeScale = 1;

        Btns();
        StartCoroutine(CamDirector());
        StartCoroutine(StageWindow());
        StartCoroutine(TextDirector());
    }

    void Update()
    {
        Earth02Rot();
    }

    void FixedUpdate()
    {
        ClearWindow();
        EndingDirector();
    }

    void Earth02Rot()
    {
        earth02.transform.Rotate(new Vector3(0, earth02RotSpeed, 0) * earth02RotSpeed * Time.deltaTime);
    }

    IEnumerator StageWindow()
    {
        float fadeCount = 1;
        stageWindow.alpha = fadeCount;
        while (fadeCount >= 0)
        {
            fadeCount -= 0.01f;
            stageWindow.alpha = fadeCount;
            yield return new WaitForSeconds(0.01f);
        }
    }

    void ClearWindow()
    {
        if (isClear)
        {
            if (!isTotal)
            {
                isTotal = true;
                GameManager.instance.totalScore = GameManager.instance.currentScore;
            }

            clearWindow.transform.localPosition = Vector3.Lerp(clearWindow.transform.localPosition, Vector3.zero, 0.5f);
            GameManager.instance.totalScore = GameManager.instance.currentScore;

            currentTime.text = "Time : " + (int)GameManager.instance.currentTime;
            currentScore.text = "Score : " + GameManager.instance.currentScore;

            totalTime.text = "Total Time : " + (int)GameManager.instance.currentTime;
            totalScore.text = "Total Score : " + GameManager.instance.totalScore;
        }
    }

    IEnumerator CamDirector()
    {
        while (true)
        {
            cam01.SetActive(false);
            yield return new WaitForSeconds(1);
            cam01.SetActive(true);
            yield return new WaitForSeconds(1);
        }
    }

    void EndingDirector()
    {
        player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(0, 0, 8), 0.03f);
    }

    IEnumerator TextDirector()
    {
        yield return new WaitForSeconds(4);
        directionText.SetActive(true);
    }

    void Btns()
    {
        nextBtn.onClick.AddListener(() =>
        {
            isClear = true;
        });

        titleBtn.onClick.AddListener(() =>
        {
            Ranking.instance.AddRanking();
            SceneManager.LoadScene(0);
        });
    }

}
