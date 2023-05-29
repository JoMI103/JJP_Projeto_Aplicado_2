using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class waveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveNumberText, timeQuantityText;
    [SerializeField] private TextMeshProUGUI finalCounter;
    [SerializeField] private WaveSystem waveSystem;
    [SerializeField] private AudioManager audioManager;
    
    Vector3 startScale;
    
    private void Start() {
        StartCoroutine(timer());
        startScale = finalCounter.transform.localScale;
        finalCounter.text = "";
    }


    void Update()
    {
        waveNumberText.text = "Wave: " + waveSystem.currentWave.ToString();

    }
    
    
    
    float waveTime = 0;
    
    IEnumerator timer()
    {
        float currentTime = waveSystem.waveTime +1;
        
        while(true){
            
            if(waveSystem.waveTime <= 0){
                 timeQuantityText.text =  waveSystem.nSheeps.ToString();
            }else{
                if(waveSystem.waveTime > 3){
                    
                    if(currentTime != waveSystem.waveTime) audioManager.Play("tic");
                    timeQuantityText.text = ((int)waveSystem.waveTime).ToString(); 
                }else{
                    yield return StartCoroutine(finalTimer());
                }
            }
            
            yield return null;
        }
    }
    
    [SerializeField] float scale, velocity;
    
    IEnumerator finalTimer(){
        float currentTime = waveSystem.waveTime +1;
        timeQuantityText.text = " "; 
        
        while( waveSystem.waveTime > 0){
            if(currentTime != waveSystem.waveTime){
                audioManager.Play("pan"); Debug.Log(currentTime);
                finalCounter.text = waveSystem.waveTime.ToString();
                finalCounter.transform.localScale = startScale;
                LeanTween.scale(finalCounter.gameObject, startScale * scale,velocity);
            }
            currentTime = waveSystem.waveTime;
            
            yield return null;
        }
    
        
        StartCoroutine(go());
    }
    
    [SerializeField] string goMessage;
    
    IEnumerator go(){

        audioManager.Play("pom");
        finalCounter.text = goMessage;
        finalCounter.transform.localScale = startScale;
        LeanTween.scale(finalCounter.gameObject, startScale * scale,velocity);
        yield return new WaitForSeconds(1);
        finalCounter.text = "";
    }
}
