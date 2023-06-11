using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : ToolInteractable
{
    [SerializeField] int maxHp = 1000; private int hpPoints;
    private const float starHealing = 60;
    float regTimer;     bool amazoniaMode = false;
    [SerializeField] Transform burn;
    
    
    
    [SerializeField] int hitTimes = 10; int hitNow;
    [SerializeField] int quantity;
    [SerializeField] private float respawnTime = 120.0f; 


    private Collider treeCollider;

    private void Start()
    {
        hpPoints = maxHp;
        treeCollider = GetComponent<Collider>();
    }
    
    private void Update() {
         if(hpPoints < maxHp){
            if(regTimer > starHealing){
                hpPoints += 50;
                updateTreeShader();
                regTimer-=0.5f;
                amazoniaMode = false;
            }
            regTimer += Time.deltaTime;
        }
        
        if(amazoniaMode){
            burn.gameObject.SetActive(true);
        }else{
            burn.gameObject.SetActive(false);
        }
        
        
        if(hpPoints < 1){
            
            
            Destroy(this.gameObject);
        }
    }
    
    [SerializeField] MeshRenderer m1,m2;
    private void updateTreeShader(){
        Debug.Log((float)hpPoints / (float)maxHp);
        m1.sharedMaterial.SetFloat("_Value",1- (float) hpPoints / (float)maxHp);   
        m2.sharedMaterial.SetFloat("_Value",1-(float) hpPoints / (float)maxHp);   
    }
    
    /*
    
    
        
    private IEnumerator destroyAnimation(){
        float time = 5;
        
        while(time > 0){
            time -= Time.deltaTime * animationVelocity;
            yield return null;
        }
    }
        */
    private PlayerHand playerHand;

    protected override void ToolInteract()
    {
        if(playerHand == null){
            playerHand = PlayerGO.GetComponent<PlayerHand>();
        }

        if(!treeCollider.enabled) return;

        if(playerHand.activeItem.handId == 5 && playerHand.toolsystem.canInteract)
        {
            playerHand.toolsystem.canInteract = false;
            hitNow++;
            if(hitNow >= hitTimes){
                hitNow = 0;
                PlayerStats p = PlayerGO.GetComponent<PlayerStats>();
                p.woodQuantity = p.woodQuantity + quantity;
                treeCollider.enabled = false;
                Transform childTransform = transform.GetChild(0); 
                childTransform.gameObject.SetActive(false);
                Invoke(nameof(Respawn), respawnTime);
            }
        }
    }

    private void Respawn()
    { 
        gameObject.SetActive(true);
        Transform childTransform = transform.GetChild(0);
        childTransform.gameObject.SetActive(true);
        treeCollider.enabled = true;
        
    }
    
    
    public void giveDmg(int dmg, bool fire){
        regTimer = 0;
        if(fire){
            amazoniaMode = true;
        }
         updateTreeShader();
        hpPoints -=dmg;
        
    }
    
    
}