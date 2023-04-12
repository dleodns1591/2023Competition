using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public static TitleManager instance;
    void Awake() => instance = this;

    [Header("지구")]
    [SerializeField] GameObject earth;
    [SerializeField] int earthRotSpeed = 0;

    [Header("시작 연출")]
    [SerializeField] int startMeteorRotSpeed = 0;
    [SerializeField] Image fade;
    [SerializeField] GameObject startMeteor;
    [SerializeField] GameObject earthParticle;
    bool isFade = false;

    [Header("시작 전 연출")]
    [SerializeField] GameObject startBeforeMeteor;

    [Header("UI")]
    [SerializeField] GameObject AllUI;
    [SerializeField] GameObject rankingWindow;
    [SerializeField] GameObject settingWindow;
    [SerializeField] GameObject howToPlayWindow;
    [SerializeField] Button startBtn;
    [SerializeField] Button howToPlayBtn;
    [SerializeField] Button rankingBtn;
    [SerializeField] Button settingBtn;
    [SerializeField] Button quitBtn;
    [SerializeField] Button bgmBtn;
    [SerializeField] Button effectBtn;
    bool isRanking = false;
    bool ishowToPlay = false;
    bool isSetting = false;
    bool isStart = false;
    bool isStartFade = false;

    void Start()
    {
        Time.timeScale = 1;

        Btns();
        GameManager.instance.GameReset();
        StartCoroutine(StartBefore());
    }

    void Update()
    {
        Earth();
        RankingWindow();
        HowToPlayWindow();
        SettingWindow();
        SoundSettingWindow();
        StartCoroutine(StartFade());
    }

    void FixedUpdate()
    {
        StartDirector();
    }

    void Earth()
    {
        earth.transform.Rotate(new Vector3(earthRotSpeed, earthRotSpeed, -earthRotSpeed) * earthRotSpeed * Time.deltaTime);
    }

    IEnumerator StartBefore()
    {
        while (true)
        {
            int randomPosX = Random.Range(10, 43);
            int randomSummon = Random.Range(2, 4);
            Vector3 summonPos = new Vector3(randomPosX, 50, 35);
            Instantiate(startBeforeMeteor, summonPos, startBeforeMeteor.transform.rotation);

            yield return new WaitForSeconds(randomSummon);
        }
    }

    void HowToPlayWindow()
    {
        if (ishowToPlay)
            howToPlayWindow.transform.localPosition = Vector3.Lerp(howToPlayWindow.transform.localPosition, new Vector3(-100, 0, 0), 0.1f);
        else
            howToPlayWindow.transform.localPosition = Vector3.Lerp(howToPlayWindow.transform.localPosition, new Vector3(-100, -740, 0), 0.1f);

    }

    void RankingWindow()
    {
        if (isRanking)
            rankingWindow.transform.localPosition = Vector3.Lerp(rankingWindow.transform.localPosition, Vector3.zero, 0.1f);
        else
            rankingWindow.transform.localPosition = Vector3.Lerp(rankingWindow.transform.localPosition, new Vector3(760, 0, 0), 0.1f);
    }

    void SettingWindow()
    {
        if (isSetting)
            settingWindow.transform.localPosition = Vector3.Lerp(settingWindow.transform.localPosition, Vector3.zero, 0.1f);
        else
            settingWindow.transform.localPosition = Vector3.Lerp(settingWindow.transform.localPosition, new Vector3(760, 0, 0), 0.1f);
    }

    void StartDirector()
    {
        if (isStart)
        {
            isStartFade = true;
            earthRotSpeed = 0;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(0, 7, 0), 0.05f);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(0, 40, 0), 0.05f);

            startMeteor.transform.position = Vector3.Lerp(startMeteor.transform.position, earth.transform.position, 0.03f);
            startMeteor.transform.GetChild(0).Rotate(new Vector3(startMeteorRotSpeed, startMeteorRotSpeed, -startMeteorRotSpeed) * startMeteorRotSpeed * Time.deltaTime);
        }
    }

    IEnumerator StartFade()
    {
        if (isStartFade && !isFade)
        {
            isFade = true;
            yield return new WaitForSeconds(2);
            earthParticle.SetActive(true);
            StartCoroutine(ShakeCamer(2, 2));

            yield return new WaitForSeconds(0.5f);
            float fadeCount = 0;
            fade.color = new Color(255, 255, 255, fadeCount);

            while (fadeCount < 1)
            {
                fadeCount += 0.01f;
                fade.color = new Color(255, 255, 255, fadeCount);
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(1);

            SceneManager.LoadScene("Tutorial");
        }
    }

    IEnumerator ShakeCamer(float shakeTime, float shakeDamage)
    {
        Vector3 cameraPos = Camera.main.transform.position;

        while (shakeTime >= 0)
        {
            shakeTime -= 0.01f;
            Camera.main.transform.position = cameraPos + Random.insideUnitSphere * shakeDamage;
            yield return new WaitForSeconds(0.01f);
        }

        Camera.main.transform.position = cameraPos;
    }

    void SoundSettingWindow()
    {
        if (!GameManager.instance.isBGM)
            bgmBtn.GetComponent<Image>().color = Color.white;
        else
            bgmBtn.GetComponent<Image>().color = Color.yellow;

        if (!GameManager.instance.isEffect)
            effectBtn.GetComponent<Image>().color = Color.white;
        else
            effectBtn.GetComponent<Image>().color = Color.yellow;
    }

    void Btns()
    {
        startBtn.onClick.AddListener(() =>
        {
            AllUI.SetActive(false);
            isStart = true;

        });

        howToPlayBtn.onClick.AddListener(() =>
        {
            if (!ishowToPlay)
                ishowToPlay = true;
            else
                ishowToPlay = false;
        });

        rankingBtn.onClick.AddListener(() =>
        {
            if (!isRanking)
                isRanking = true;
            else
                isRanking = false;
        });

        settingBtn.onClick.AddListener(() =>
        {
            if (!isSetting)
                isSetting = true;
            else
                isSetting = false;
        });

        bgmBtn.onClick.AddListener(() =>
        {
            if (!GameManager.instance.isBGM)
            {
                GameManager.instance.isBGM = true;
                bgmBtn.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                GameManager.instance.isBGM = false;
                bgmBtn.GetComponent<Image>().color = Color.white;
            }
        });

        effectBtn.onClick.AddListener(() =>
        {
            if (!GameManager.instance.isEffect)
            {
                GameManager.instance.isEffect = true;
                effectBtn.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                GameManager.instance.isEffect = false;
                effectBtn.GetComponent<Image>().color = Color.white;
            }
        });

        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
