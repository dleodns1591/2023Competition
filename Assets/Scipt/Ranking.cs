using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Ranking : MonoBehaviour
{
    public static Ranking instance;
    void Awake() => instance = this;

    [SerializeField] GameObject rankingText;
    [SerializeField] string[] rankingList = new string[5];

    void Start()
    {
        RankingSystem();
    }

    void Update()
    {
        
    }

    void RankingSystem()
    {
        var rankingTxt = new StreamReader("Assets/Resources/Ranking.txt");

        for(int i = 0; i< rankingList.Length; i++)
        {
            string rankingLine = rankingTxt.ReadLine();
            rankingList[i] = (rankingLine == null || rankingLine == "") ? "-,0" : rankingLine;
        }

        if(rankingText != null)
        {
            for (int i = 0; i < rankingList.Length; i++)
            {
                GameObject ranking = rankingText.transform.GetChild(i).gameObject;
                ranking.GetComponent<Text>().text = $"{i + 1} Rank";
                ranking.transform.GetChild(0).GetComponent<Text>().text = $"{rankingList[i].Split(',')[0]}";
                ranking.transform.GetChild(1).GetComponent<Text>().text = $"{rankingList[i].Split(',')[1]}";
            }
        }
    }

    public void AddRanking()
    {
        StreamWriter sw = new StreamWriter("Assets/Resources/Ranking.txt");

        for (int i = 0; i < rankingList.Length; i++)
        {
            string[] txtScore = rankingList[i].Split(',');
            if(int.Parse(txtScore[1]) < GameManager.instance.totalScore)
            {
                for (int j = 3; j >= i; j--)
                    rankingList[j + 1] = rankingList[j];

                rankingList[i] = GameManager.instance.PlayerName + "," + GameManager.instance.totalScore;
                GameManager.instance.totalScore = 0;
            }
            sw.WriteLine(rankingList[i]);
        }
        sw.Close();
    }
}
