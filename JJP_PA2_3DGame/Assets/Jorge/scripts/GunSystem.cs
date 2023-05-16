using UnityEngine;
using TMPro;
//using UnityEditor.PackageManager;

public class GunSystem : MonoBehaviour
{
    public int damage;
    public float fireRate, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap, bulletsPerShot;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    public GameObject muzzleFlash,muzzleFlash2, bulletHoleGraphic;
    public TextMeshProUGUI text;

    public Animator animator; public string shootAnimationName, ReloadAnimationName;
    public AudioManager audioManager;
    
    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        ShootInput();

        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    private void ShootInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if((bulletsLeft <= 0 || Input.GetKeyDown(KeyCode.R)) && bulletsLeft < magazineSize && !reloading ) Reload();
      
  

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0) {
            bulletsShot = bulletsPerTap;
            animator.Play(shootAnimationName);  
            audioManager.Play(shootAnimationName);
            Shoot();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;
        
        for (int i = 0; i < bulletsPerShot; i++)
        {
            float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);

        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, z);

        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            //Debug.Log(rayHit.collider.name);



            //Gets the gameobject with an Damage Script with raycast hitinfo
            Hittable damageAux = rayHit.collider.GetComponent<Hittable>();
            if (damageAux != null)
                damageAux.BaseHit(damage);
            
        Debug.LogError(rayHit.normal);
        Transform t =  Instantiate(muzzleFlash, rayHit.point, Quaternion.identity).transform;
        t.transform.up = rayHit.normal;
         if (damageAux != null) t.parent = damageAux.transform;
  
            Instantiate(muzzleFlash2, attackPoint.position, Quaternion.identity);

           

        }

        }
        
        
        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", fireRate);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
       // animator.Play(ReloadAnimationName);  
       // audioManager.Play(ReloadAnimationName);
        
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
