using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject shild;
    [SerializeField] int shildSpeed = 0;

    [Header("체력")]
    [SerializeField] float currentHp;
    [SerializeField] int maxHp;
    [SerializeField] int hpDownSpeed = 0;
    [SerializeField] Slider bossSlider;

    [Header("죽음")]
    [SerializeField] GameObject dieParticle;
    bool isDie = false;
    bool isDieBossMove = false;

    [Header("공격")]
    bool isAttack = false;

    void Start()
    {
        currentHp = maxHp;
    }

    void Update()
    {
        shild.transform.Rotate(new Vector3(0, shildSpeed * Time.deltaTime, 0));

        BossHP();
        StartCoroutine(Die());
    }

    void BossHP()
    {
        bossSlider.value = Mathf.Lerp(bossSlider.value, currentHp / maxHp, Time.deltaTime * hpDownSpeed);
    }

    void Attack()
    {

    }

    IEnumerator Die()
    {
        Vector3 dieParticlePos = new Vector3(transform.position.x, -3, transform.position.z);
        Vector3 dieBossPos = new Vector3(transform.position.x, -25, transform.position.z);

        if (isDieBossMove)
            transform.position = Vector3.Lerp(transform.position, dieBossPos, 0.01f);

        if (currentHp < 0 && !isDie)
        {
            isDie = true;
            GameObject particle = Instantiate(dieParticle, dieParticlePos, Quaternion.identity);
            isDieBossMove = true;

            yield return new WaitForSeconds(3f);

            Time.timeScale = 0;

            Destroy(particle);
            UIManager.instance.clearWindow.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            currentHp -= Bullet.instance.attack;
            Destroy(other.gameObject);
        }
    }
}
