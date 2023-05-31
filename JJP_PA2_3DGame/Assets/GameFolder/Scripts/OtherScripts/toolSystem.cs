using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolSystem : MonoBehaviour
{
    Animator animator;
    AudioManager audioManager;
    
    public bool isInAction; public bool canInteract;
    
    
    
    [SerializeField] float coolDown;
    
    private void Awake() {
        animator = GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>();
        
        isInAction = false;
    }
    
    public void toolAction(){
        animator.Play("action");
        audioManager.Play("action");
        canInteract = true;
        isInAction = true; 
       
    }
    
    
    private void endAction(){
        isInAction = false;
    }
}
