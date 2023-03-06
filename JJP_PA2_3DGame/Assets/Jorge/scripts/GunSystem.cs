using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    public int damage;
    public float fireRate, spread, range, reloadTime, timeBetweenShots;
    public int magSize, bulletsPerClick;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask isEnemy;

    public GameObject muzzleFlash, bulletHole;
    public TextMeshProUGUI text;

    public void Awake()
    {
        bulletsLeft = magSize;
        readyToShoot = true;
    }

    private void Update()
    {
        ShootInput();

        text.SetText(bulletsLeft + " / " + magSize);
    }

    private void ShootInput()
    {
        if(allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading) Reload();

        if (readyToShoot && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerClick;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0); 

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, range, isEnemy))
        {
            Debug.Log(rayHit.collider.name);
            if (rayHit.collider.CompareTag("Enemy"))
                rayHit.collider.GetComponent<SheepBehaviour>().TakeDamage(damage);
        }

        Instantiate(bulletHole, rayHit.point, Quaternion.Euler(0, 180, 0));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        
        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", fireRate);

        if(bulletsShot > 0 && bulletsLeft > 0)
        Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magSize;
        reloading= false;
    }
}
