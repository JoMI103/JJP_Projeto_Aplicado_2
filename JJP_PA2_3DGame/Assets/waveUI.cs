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
    
    
    
    float waveTime = 0;
    
    IEnumerator timer()
    {
        
        
        while(true){
            
            if(waveSystem.waveTime <= 0){
                 timeQuantityText.text =  "Next Wave(" +waveSystem.nSheeps.ToString() + ")";
            }else{
        
                timeQuantityText.text = ((int)waveSystem.waveTime).ToString();
            }
            
            yield return null;
        }
    }
    
    
}
