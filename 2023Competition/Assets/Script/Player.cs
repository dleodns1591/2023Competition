using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player instance;
    void Awake() => instance = this;

    [Header("이동")]
    [SerializeField] int moveSpeed = 0;
    [SerializeField] Vector3 moveDirection = Vector3.zero;

    [Header("공격")]
    [SerializeField] GameObject bullet;
    [SerializeField] float attackRange = 0;

    [Header("내구도 / 연료")]
    public float currentHp = 0;
    public float currentFuel = 0;
    public int maxHp = 0;
    public int maxFuel = 0;

    [Header("스킬")]
    [SerializeField] Image hpSkill;
    [SerializeField] Image bombSkill;

    [SerializeField] float hpCoolTime = 0;
    [SerializeField] float hpCurrentCoolTime = 0;
    [SerializeField] float bombCoolTime = 0;
    [SerializeField] float bombCurrentCoolTime = 0;

    bool isHpSkillUse = false;
    bool isBombSkillUse = false;

    void Start()
    {
        currentHp = maxHp;
        currentFuel = maxFuel;
    }

    void Update()
    {
        Attack();
        GameOver();
        Skill();
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

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine("BulletSummon");
        else if (Input.GetKeyUp(KeyCode.Space))
            StopCoroutine("BulletSummon");
    }

    void Skill()
    {
        hpSkill.fillAmount = Mathf.Lerp(hpSkill.fillAmount, hpCurrentCoolTime / hpCoolTime, Time.deltaTime * 10);
        bombSkill.fillAmount = Mathf.Lerp(bombSkill.fillAmount, bombCurrentCoolTime / bombCoolTime, Time.deltaTime * 10);

        // 내구도 스킬
        if (Input.GetKeyDown(KeyCode.Z) && !isHpSkillUse)
        {
            isHpSkillUse = true;
            hpSkill.fillAmount = 1;
            hpCurrentCoolTime = hpCoolTime;
            StartCoroutine(HpCoolTime());

            if (currentHp + 10 <= maxHp)
                currentHp += 10;
            else
                currentHp = maxHp;
        }

        if (hpCurrentCoolTime == 0)
            isHpSkillUse = false;

        // 폭탄
        if (Input.GetKeyDown(KeyCode.X) && !isBombSkillUse)
        {
            isBombSkillUse = true;
            bombSkill.fillAmount = 1;
            bombCurrentCoolTime = bombCoolTime;
            StartCoroutine(BombCoolTime());
        }

        if (bombCurrentCoolTime == 0)
            isBombSkillUse = false;
    }

    IEnumerator HpCoolTime()
    {
        while (hpCurrentCoolTime > 0)
        {
            yield return new WaitForSeconds(1);
            hpCurrentCoolTime -= 1;
        }
        yield break;
    }

    IEnumerator BombCoolTime()
    {
        while (bombCurrentCoolTime > 0)
        {
            yield return new WaitForSeconds(1);
            bombCurrentCoolTime -= 1;
        }
        yield break;
    }

    void GameOver()
    {
        if (currentFuel <= 0 || currentHp <= 0)
        {
            Time.timeScale = 0;
            UIManager.instance.gameoverWindow.SetActive(true);
            UIManager.instance.overScore.text = "Score : " + GameManager.instance.currentScore;
        }
    }

    IEnumerator BulletSummon()
    {
        while (true)
        {
            Instantiate(bullet, new Vector3(transform.position.x + 1.1f, transform.position.y, transform.position.z + 3), Quaternion.Euler(new Vector3(90, 0, 0)));
            yield return new WaitForSeconds(attackRange);
        }
    }
}
