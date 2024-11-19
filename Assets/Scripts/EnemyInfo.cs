using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public enum enemyType { macaron, donut, lolipop, candy }

public class EnemyInfo : MonoBehaviour
{

    public float health;
    public bool dead;
    public float range;
    private float roarDelay;
    private bool canRoar;

    //parameters for damage flash
    public Renderer meshRenderer;
    public Color NormalColour = Color.white;
    public Color FlashColour = Color.red;
    private float flashDamageDelay = 0.1f;


    
    //enemy types
    //each enemy has different attack animations
   // public enum enemyType { macaron, lollipop, donut};
    //public enemyType type;

    GameObject healthDrop;
    public GameObject[] ammodrop;

    float ammodropChance = 0.6f;

    private GameObject gameController;

    public enemyType type;

    private void Start()
    {
        setRandomDelay();
        canRoar = true;
        gameController = GameObject.Find("Game Controller");
        dead = false;
    }

    private void Update()
    {
        if (canRoar)
        {
            StartCoroutine(playNoise());
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        StartCoroutine(flashDamage());
        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        dead = true;
        gameController.GetComponent<Game_Controller>().enemiesAlive--;
        gameController.GetComponent<Game_Controller>().numberOfEnemies.text = "Enemies Left: " + gameController.GetComponent<Game_Controller>().enemiesAlive + "";
        dropAmmo();
        Destroy(gameObject);
    }

    void dropAmmo()
    {
        //get random cahnce to drop ammo
        float randomNumber = Random.Range(0f, 1.0f);
        int randomAmmoIndex = Random.Range(0, 1);
        Debug.Log("Random Number Index Drop: " + randomAmmoIndex);

        //spawn ammo drop where enemies were at
        if(randomNumber <= ammodropChance)
        {
            Instantiate(ammodrop[randomAmmoIndex], gameObject.transform.position, Quaternion.identity);
        }

    }

    void setRandomDelay()
    {
        roarDelay = Random.Range(0.1f, 9f);
    }

    IEnumerator playNoise()
    {
        canRoar = false;

        switch(type)
        {
            case enemyType.macaron:
                FindObjectOfType<Audio_Manager>().playSound("Monster1");
                break;
            case enemyType.donut:
                FindObjectOfType<Audio_Manager>().playSound("Monster2");
                break;
            case enemyType.lolipop:
                FindObjectOfType<Audio_Manager>().playSound("Monster3");
                break;
            case enemyType.candy:
                FindObjectOfType<Audio_Manager>().playSound("Monster4");
                break;
        }

        yield return new WaitForSeconds(roarDelay);
        canRoar = true;
        Debug.Log(type + "Rawr");
    }

    IEnumerator flashDamage()
    {
        meshRenderer.material.color = FlashColour;
        yield return new WaitForSeconds(flashDamageDelay);
        meshRenderer.material.color = NormalColour;
    }

}
