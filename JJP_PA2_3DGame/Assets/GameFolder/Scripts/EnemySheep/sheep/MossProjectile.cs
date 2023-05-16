using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossProjectile : MonoBehaviour
{
    Vector3 targetPos; PlacedBuilding placedBuilding; int dmg; float velocity;
    PlayerStats playerStats;
    
    
    private IEnumerator player(){
        while( Vector3.Distance( transform.position , playerStats.transform.position) > 0.5f){
            transform.position =Vector3.MoveTowards(transform.position,playerStats.transform.position, Time.deltaTime * velocity);
            yield return new WaitForFixedUpdate();
        }
        
        if(playerStats!=null) playerStats.giveDmg(dmg);
           Destroy(this.gameObject);
    }
    
   private IEnumerator building(){
        while( Vector3.Distance( transform.position , targetPos) > 0.5f){
            transform.position = Vector3.MoveTowards(transform.position,targetPos, Time.deltaTime * velocity);
            yield return new WaitForFixedUpdate();
        }
        
        if(placedBuilding != null) placedBuilding.takeDamge(dmg);
         Destroy(this.gameObject);
    }
    
    public void shootBuilding(Vector3 target, PlacedBuilding pb,int dmg, float v){
        targetPos = target; placedBuilding = pb; this.dmg = dmg;   velocity = v;
        StartCoroutine(building());  
    }
    public void shootPlayer(PlayerStats ps, int dmg, float v){
        playerStats = ps; this.dmg = dmg; velocity = v;
        StartCoroutine(player());   
    }
}
