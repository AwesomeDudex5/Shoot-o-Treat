using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{

    public int health = 100;
    private float dmgBarTaken = 0.1f;
    public bool isHit;
    public bool isDead;

    public GameObject healthBar;

    public Image damageSprite;
    private float opacity = 0.2f ;
    private float flashDamageDelay = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        isHit = false;
        healthBar.transform.localScale = new Vector3(1f, 1f, 1f);

        //set opactiy for damage flash, then disbale
        // damageSprite.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, opacity);
        var tempColor = damageSprite.color;
        tempColor.a = 0.1f;
        damageSprite.color = tempColor;
        damageSprite.gameObject.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
     
        if(isHit)
        {
            StartCoroutine(damageFlash());
            health -= 10;
            healthBar.transform.localScale -= new Vector3(dmgBarTaken, 0, 0);
            isHit = false;
        }

        if(health <= 0)
        {
            isDead = true;
        }
    }

    public void resetHealth()
    {
        isDead = false;
        health = 100;
        healthBar.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    IEnumerator damageFlash()
    {
        damageSprite.gameObject.SetActive(true);
        yield return new WaitForSeconds(flashDamageDelay);
        damageSprite.gameObject.SetActive(false);
    }
}
