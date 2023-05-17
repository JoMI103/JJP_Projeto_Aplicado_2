using UnityEngine;
using TMPro;
//using UnityEditor.PackageManager;

public class GunSystem : MonoBehaviour
{
    [Header("GunStats")]
    public int damage;
    public float fireRate, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap, bulletsPerShot;
    public bool allowButtonHold;
    
  
    [Header("Shot Start Position")]
    public Camera fpsCam; public Transform attackPoint;
    
    [Header("Hitable Objects")]
    public LayerMask whatIsEnemy;
    
    [Header("Effects/Audio/Animations")]
    
    public GameObject muzzleFlash; public GameObject muzzleFlash2;
    public TextMeshProUGUI AmmoDisplay;

    public Animator animator; 
    public AudioManager audioManager;
    public string shootAnimationName, ReloadAnimationName;
    
    [Header("InputManager")]
    [SerializeField] private InputManager inputManager;
    
    
    int bulletsLeft, bulletsShot; bool shooting, readyToShoot, reloading;
    private RaycastHit rayHit;
    
    private void Awake() {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        
    }
    
    private void Start() {
        inputManager.onFoot.PlaceShootAttack.performed += ctx => tryShoot();
        inputManager.onFoot.RotateReload.performed += ctx => tryReload();
    }
    
    private void Update() {
        if(bulletsLeft <= 0 && bulletsLeft < magazineSize && !reloading ) Reload();
    }
    
    private void tryShoot(){
        if(!this.gameObject.activeInHierarchy) return;
        
        if (readyToShoot && !reloading && bulletsLeft > 0) {
            Shoot();
        }
    }
    private void tryReload(){
         if(!this.gameObject.activeInHierarchy) return;
        if(bulletsLeft < magazineSize && bulletsLeft < magazineSize && !reloading ) Reload();
    }
    
    private void UpdateUI() {
        AmmoDisplay.SetText(bulletsLeft + " / " + magazineSize);
    }

    
    private void Shoot()
    {
       
        readyToShoot = false;
        
        bulletsShot = bulletsPerTap;
        animator.Play(shootAnimationName);  
        audioManager.Play(shootAnimationName);
        
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
   
                Transform t =  Instantiate(muzzleFlash, rayHit.point, Quaternion.identity).transform;
                t.transform.up = rayHit.normal;
                if (damageAux != null) t.parent = damageAux.transform;
    

            

            }

        }
        
        Instantiate(muzzleFlash2, attackPoint.position, Quaternion.identity);
        
        bulletsLeft--;
        bulletsShot--;
        
        UpdateUI();
        
        Invoke("ResetShot", fireRate);
        
        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
        
    }
  
   
    private void Reload()
    {
        animator.Play(ReloadAnimationName);  
      // audioManager.Play(ReloadAnimationName);
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    
    //Invoked Methods
    private void ResetShot()
    {
        readyToShoot = true;
    }
  
    
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        UpdateUI();
    }
}
