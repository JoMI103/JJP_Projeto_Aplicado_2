using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolSystem : MonoBehaviour
{
    Animator animator;
    public bool isInAction;
    
    
    [SerializeField] float coolDown;
    
    private void Awake() {
        isInAction = false;
    }
    
    public void toolAction(){
        animator.Play("action");
        isInAction = true; Invoke("endAction", coolDown);
    }
    
    
    private void endAction(){
        isInAction = false;
    }
}
