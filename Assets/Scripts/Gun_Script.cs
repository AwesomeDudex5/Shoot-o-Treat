using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public enum gunType { pistol, smg, shotgun }

public class Gun_Script : MonoBehaviour
{
    //stats
    public float dmg;
    public float range = 100f;
    public float fireRate;

    //type of gun
    public gunType type;

    //ammo for guns
    public int pistolCurrentAmmo;
    int pistolAmmoLimit; //so it doesn't go beyond magazine

    public int smgCurrentAmmo;
    int smgAmmoLimit;
    public int smgAmmoInStock;

    public int shotgunCurrentAmmo;
    int shotgunAmmonLimit;
    public int shotgunAmmoInStock;

    //reload stats
    public bool isReloading;
    private float reloadTime = 1f;


    //camera with gun in view
    public Camera fpsCamera;

    //special effects
    public GameObject impacteEffect;
    public ParticleSystem muzzlelash;

    //firing stats
    //delays for pistol and shotgun are flags because they can only fire 1 at a time
    //smg gets a float firerate that becuase it is firing while pressed down/
    private float fireDelay = 0f;
    private float delayInSeconds;
    private bool canShoot;
    public bool isFiring;

    GameObject gameController;

    void Start()
    {
        gameController = GameObject.Find("Game Controller");

        isReloading = false;
        isFiring = false;
        canShoot = true;

        //set ammo for starting
        setAmmo();

        switch(type)
        {
            case gunType.pistol:
                dmg = 20f;
                pistolAmmoLimit = 15;
                delayInSeconds = 0.2f;
                break;
            case gunType.smg:
                dmg = 10f;
                fireRate = 15;
                smgAmmoLimit = 30;
                break;
            case gunType.shotgun:
                dmg = 40f;
                shotgunAmmonLimit = 6;
                delayInSeconds = 0.5f;
                break;
            default:
                Debug.Log("No Gun Type");
                break;
        }
    }

    //reset flag when switching weapons
    void OnEnable()
    {
        isReloading = false;
        isFiring = false;
        canShoot = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (isReloading)
            return;

        if(Input.GetKeyDown("r"))
        {
            if(pistolCurrentAmmo < pistolAmmoLimit || smgCurrentAmmo < smgAmmoLimit || shotgunCurrentAmmo < shotgunAmmonLimit)
            {
                StartCoroutine(Reload());
            }
        }


        if(type == gunType.pistol)
        {
            if(pistolCurrentAmmo <= 0)
            {
                StartCoroutine(Reload());
            }

            //if current is not 0, proceed to shoot
            //then subtract 1 from current ammo
            if(Input.GetButtonDown("Fire1") && pistolCurrentAmmo > 0)
            {
                if (canShoot)
                {
                    Shoot();
                   // Debug.Log("Gun Type: Pistol Current Ammo: " + pistolCurrentAmmo);
                    canShoot = false;
                    isFiring = false;
                    StartCoroutine(ShootDelay());
                }
            }
        }

        if(type == gunType.shotgun)
        {
            if(shotgunCurrentAmmo <= 0 && shotgunAmmoInStock > 0)
            {
                StartCoroutine(Reload());
            }

            if(Input.GetButtonDown("Fire1") && shotgunCurrentAmmo > 0)
            {
                if (canShoot)
                {
                    Shoot();
                   // Debug.Log("Gun Type: ShotGun Current Ammo: " + shotgunCurrentAmmo + " Ammo in Stock: " + shotgunAmmoInStock);
                    canShoot = false;
                    isFiring = false;
                    StartCoroutine(ShootDelay());
                }
            }
        }

        if (type == gunType.smg)
        {
            if(smgCurrentAmmo <=  0 && smgAmmoInStock > 0)
            {
                StartCoroutine(Reload());
            }

            if (Input.GetButton("Fire1") && Time.time >= fireDelay && smgCurrentAmmo > 0)
            {
                fireDelay = Time.time + 1f / fireRate;
                Shoot();
                //Debug.Log("Gun Type: SMG Current Ammo: " + smgCurrentAmmo + " Ammo in Stock: " + smgAmmoInStock);
            }
        }

        //if restarting, restock ammo
        if(gameController.GetComponent<Game_Controller>().restarting)
        {
            setAmmo();
        }
    }

    public void Shoot()
    {
        muzzlelash.Play();
        isFiring = true;
        Debug.Log("isFiring: " + isFiring);
        RaycastHit hit;

        switch (type)
        {
            case gunType.pistol:
                pistolCurrentAmmo--;
                FindObjectOfType<Audio_Manager>().playSound("Pistol Fire");
                break;
            case gunType.smg:
                smgCurrentAmmo--;
                FindObjectOfType<Audio_Manager>().playSound("SMG Fire");
                break;
            case gunType.shotgun:
                shotgunCurrentAmmo--;
                FindObjectOfType<Audio_Manager>().playSound("Shotgun Fire");
                break;
            default:
                Debug.Log("No Gun Type");
                break;
        }

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            //shoot and give enemy damage
            EnemyInfo enemey = hit.transform.GetComponent<EnemyInfo>();

        if( enemey != null)
            {
                enemey.TakeDamage(dmg);
            }
            GameObject impactGO = Instantiate(impacteEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 0.5f);
        }
        isFiring = false;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        int ammoNeeded = 0;
        //Debug.Log("Reloading, please wait...");
        FindObjectOfType<Audio_Manager>().playSound("Reload");
        yield return new WaitForSeconds(reloadTime);

        //since pistol has unlimited ammon is stock
        //reset clip to magazine limit
        if(type == gunType.pistol)
        {
            pistolCurrentAmmo = pistolAmmoLimit;
            //Debug.Log("Reloaded! Current Ammo: " + pistolCurrentAmmo);
        }

        //since smgs and shotguns need ammo in stock
        //reset clip to limit, subtract it from ammo in stock
        //however if the limit exceeds ammo in stock, only use the remainder of ammo in stock
        //if there is still ammo in the clip, put whatever is needed into clip
        if (type == gunType.smg)
        {
            ammoNeeded = smgAmmoLimit - smgCurrentAmmo;
            if(ammoNeeded > smgAmmoInStock)
            {
                smgCurrentAmmo += smgAmmoInStock;
                smgAmmoInStock -= smgAmmoInStock;
            }
            if(ammoNeeded <= smgAmmoInStock)
            {
                smgAmmoInStock -= ammoNeeded;
                smgCurrentAmmo = smgAmmoLimit;
            }
            //Debug.Log("Reloaded! Current Ammo: " + smgCurrentAmmo + "Ammo In Stock: " + smgAmmoInStock);
        }

        if(type == gunType.shotgun)
        {
            ammoNeeded = shotgunAmmonLimit - shotgunCurrentAmmo;
            if(ammoNeeded > shotgunAmmoInStock)
            {
                shotgunCurrentAmmo += shotgunAmmoInStock;
                shotgunAmmoInStock -= shotgunAmmoInStock;
            }
            if(ammoNeeded <= shotgunAmmoInStock)
            {
                shotgunAmmoInStock -= ammoNeeded;
                shotgunCurrentAmmo = shotgunAmmonLimit;
            }
            //Debug.Log("Reloaded! Current Ammo: " + shotgunCurrentAmmo + "Ammo In Stock: " + shotgunAmmoInStock);
        }

        //reset reloading flag
        isReloading = false;
       
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        canShoot = true;
    }

    public void setAmmo()
    {
        //set ammo for starting
        pistolCurrentAmmo = 15;

        smgCurrentAmmo = 30;
        smgAmmoInStock = 30;

        shotgunCurrentAmmo = 6;
        shotgunAmmoInStock = 6;
    }

}
