using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This script controls the UI for the gun (ammo)
 * Also handles weapon switching
 */
public class Gun_Controller : MonoBehaviour
{

    public int selectWeaponIndex = 0;

    public Text currentGunAmmo;
    public Text currentGunType;

    //animator applies same animation to all guns
    private Animator anim;


    //ammo holders
    gunType type;
    int smgAmmoToAdd;
    int shotgunAmmoToAdd;


    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("swaying", false);
        anim.SetBool("firing", false);
        SelectWeapon();

        smgAmmoToAdd = 0;
        shotgunAmmoToAdd = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //update ammo text
        //ammo received here will be applied to the gun when it is active
        switch (type)
        {
            case gunType.pistol:
                currentGunAmmo.text = gameObject.transform.GetChild(selectWeaponIndex).gameObject.GetComponent<Gun_Script>().pistolCurrentAmmo + "/--";
                break;
            case gunType.smg:
                gameObject.transform.GetChild(selectWeaponIndex).gameObject.GetComponent<Gun_Script>().smgAmmoInStock += smgAmmoToAdd;
                smgAmmoToAdd = 0;
                currentGunAmmo.text = gameObject.transform.GetChild(selectWeaponIndex).gameObject.GetComponent<Gun_Script>().smgCurrentAmmo + "/" + gameObject.transform.GetChild(selectWeaponIndex).gameObject.GetComponent<Gun_Script>().smgAmmoInStock;
                break;
            case gunType.shotgun:
                gameObject.transform.GetChild(selectWeaponIndex).gameObject.GetComponent<Gun_Script>().shotgunAmmoInStock += shotgunAmmoToAdd;
                shotgunAmmoToAdd = 0;
                currentGunAmmo.text = gameObject.transform.GetChild(selectWeaponIndex).gameObject.GetComponent<Gun_Script>().shotgunCurrentAmmo + "/" + gameObject.transform.GetChild(selectWeaponIndex).gameObject.GetComponent<Gun_Script>().shotgunAmmoInStock;
                break;
            default:
                Debug.Log("No Gun Type");
                break;
        }

        //if moving, apply gun sway
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            //anim.Play("Gun_Sway");
            applySway(true);
        }
        else
        {
            applySway(false);
        }

        
        int previousSelectedWeapon = selectWeaponIndex;
        /*
         * this snippet allows weapon switching via scolling
         * allows looping back around by resseting when it goees out of bounds
         * */
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectWeaponIndex >= transform.childCount - 1)
                selectWeaponIndex = 0;
            else
                selectWeaponIndex++;
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectWeaponIndex <= 0)
                selectWeaponIndex = transform.childCount - 1;
            else
                selectWeaponIndex--;
        }

        if(previousSelectedWeapon != selectWeaponIndex)
        {
            SelectWeapon();
        }

        //Debug.Log(gameObject.transform.GetChild(selectWeaponIndex).gameObject.GetComponent<Gun_Script>().isFiring);

        if (Input.GetButtonDown("Fire1"))
        {
            applyFiringAnimation();
        }

        //get flag for current weapon is firing
        /*if (gameObject.transform.GetChild(selectWeaponIndex).gameObject.GetComponent<Gun_Script>().isFiring)
        {
            Debug.Log("isFiring flag triggered");
            applyFiringAnimation();
        }*/

            //if reload flag is triggered
            //apply reload animation and stats
            if (gameObject.transform.GetChild(selectWeaponIndex).gameObject.GetComponent<Gun_Script>().isReloading)
        {
            //Debug.Log("isReloading triggered");
            applyReloadingAnim();
        }
        
    }

    public void resetAmmo()
    {
        foreach(Transform weapon in transform)
        {
            weapon.gameObject.SetActive(true);
            weapon.gameObject.GetComponent<Gun_Script>().setAmmo();
            weapon.gameObject.SetActive(false);
        }
        selectWeaponIndex = 0;
        gameObject.transform.GetChild(selectWeaponIndex).gameObject.SetActive(true);

    }

    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectWeaponIndex)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
        type = gameObject.transform.GetChild(selectWeaponIndex).gameObject.GetComponent<Gun_Script>().type;
        currentGunType.text = "Gun Type: " + type;
    }

    public void ApplyAmmo(int amount, gunType ammoType)
    {
        
        switch(ammoType)
        {
            case gunType.shotgun:
                shotgunAmmoToAdd += amount;
                break;
            case gunType.smg:
                smgAmmoToAdd += amount;
                break;
            default:
                Debug.Log("Error, can't apply ammo");
                break;
        }
        
    }

    void applyFiringAnimation()
    {
        //anim.SetBool("firing", true);
        anim.Play("fire");
    }

    void applySway(bool enable)
    {
        anim.SetBool("reloading", false);
        anim.SetBool("swaying", enable);
    }

    void applyReloadingAnim()
    {
        //anim.SetBool("swaying", false);
        anim.SetBool("reloading", true);
    }

}
