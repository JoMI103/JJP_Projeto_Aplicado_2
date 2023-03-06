using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void ShootInput()
    {
        if(allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading) Reload();

        if (readyToShoot && !reloading && bulletsLeft > 0) Shoot();
        
    }

    private void Shoot()
    {
        readyToShoot = false;

        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, range, isEnemy))
        {
            Debug.Log(rayHit.collider.name);
            if (rayHit.collider.CompareTag("Enemy"))
                rayHit.collider.GetComponent<SheepBehaviour>().TakeDamage(damage);
        }
        
        bulletsLeft--;
        Invoke("ResetShot", fireRate);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {

    }
}
