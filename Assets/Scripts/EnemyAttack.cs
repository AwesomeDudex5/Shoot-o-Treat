using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //attack attributes
    //delay before attack
    private float timer = 0f;
    public float attackDelayTime;
    public Animator anim;

    //player object to find
    GameObject player;
    public float range;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        anim.SetBool("attack", false);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        if(distance <= range)
        {
            if(timer >= attackDelayTime)
            {
                //if within range, attack then reset timer
                Attack();
                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            timer = 0f;
            anim.SetBool("attack", false);
        }
    }

    void Attack()
    {
        player.GetComponent<PlayerInfo>().isHit = true;
        anim.SetBool("attack", true);
    }
}
