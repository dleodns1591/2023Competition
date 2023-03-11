using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject shild;
    [SerializeField] int shildSpeed = 0;

    [Header("Ã¼·Â")]
    [SerializeField] float currentHp;
    [SerializeField] int maxHp;
    [SerializeField] int hpDownSpeed = 0;
    [SerializeField] Slider bossSlider;

    [Header("Á×À½")]
    [SerializeField] GameObject dieParticle;


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
        if (currentHp < 0)
        {
            Time.timeScale = 0;

            GameObject particle = Instantiate(dieParticle, transform.position, Quaternion.identity);

            yield return new WaitForSecondsRealtime(3);
            
            Destroy(particle);
            UIManager.instance.clearWindow.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
            currentHp -= Bullet.instance.attack;
    }
}
