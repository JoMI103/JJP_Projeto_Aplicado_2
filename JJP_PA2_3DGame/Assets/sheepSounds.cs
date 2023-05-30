using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sheepSounds : MonoBehaviour
{
    AudioManager audioManager;
    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        Invoke("doMeeeee", Random.Range(0,26));
    }
    
    private void doMeeeee(){
        switch(Random.Range(0,5)){
            case 1: audioManager.Play("Meeeee1");break;
            case 2: audioManager.Play("Meeeee2");break;
            case 3: audioManager.Play("Meeeee3");break;
            case 4: audioManager.Play("Meeeee4");break;
            case 5: audioManager.Play("Meeeee5");break;
        }
        
        Invoke("doMeeeee", Random.Range(20,46));
    }


}
