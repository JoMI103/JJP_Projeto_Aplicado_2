using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class waveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveNumberText, timeQuantityText;
    
    [SerializeField] private WaveSystem waveSystem;
    
    private void Start() {
        StartCoroutine(timer());
    }


    void Update()
    {
        waveNumberText.text = "Wave: " + waveSystem.currentWave.ToString();

    }
    
    
    public void StartCount(int timecurrent){
        waveTime = timecurrent;
    }
    
    float waveTime = 0;
    
    IEnumerator timer()
    {
        
        
        while(true){
            
            if(waveTime < 0){
                 timeQuantityText.text =  "Next Wave(" +waveSystem.nSheeps.ToString() + ")";
            }else{
                waveTime-= Time.deltaTime;
                timeQuantityText.text = ((int)waveTime).ToString();
            }
            
            yield return null;
        }
    }
    
    
}
