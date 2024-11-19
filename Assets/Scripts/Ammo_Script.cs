using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Script : MonoBehaviour
{

    //ammo pick up stats
    public int shotgunAmmo = 3;
    public int smgAmmo = 10;
    public int ammountToGive;

    GameObject Gun;
    public gunType type;

    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Gun = GameObject.Find("Gun");
        Player = GameObject.Find("Player");

        switch(type)
        {
            case gunType.smg:
                ammountToGive = smgAmmo;
                break;
            case gunType.shotgun:
                ammountToGive = shotgunAmmo;
                break;
            default:
                Debug.Log("No Type Given, No Ammo To Give");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, Player.transform.position) < 4)
        {
            Gun.GetComponent<Gun_Controller>().ApplyAmmo(ammountToGive, type);
            Destroy(gameObject);
        }
    }

    /*void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Colliding With: " + collision.gameObject.name);

        if (collision.gameObject.name == "Player")
        {
            Debug.Log("player Detected");
            Gun.GetComponent<Gun_Controller>().ApplyAmmo(ammountToGive, type);
            Destroy(gameObject);
        }
    }
    */

}
