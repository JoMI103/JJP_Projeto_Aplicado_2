using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepExplode : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    
    private void Start() {
        StartCoroutine("explode");
    }
    [SerializeField] float velocity;
    
    private IEnumerator explode(){
        float time = 0;
        while(time < 5f){
            meshRenderer.sharedMaterial.SetFloat("_ExpandSlider",time );
            time += Time.deltaTime * velocity;
            yield return null;
        }
        
        Destroy(this.gameObject);
    }
}
