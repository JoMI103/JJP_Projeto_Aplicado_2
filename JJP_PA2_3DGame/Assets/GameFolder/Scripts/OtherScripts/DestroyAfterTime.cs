using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float time;
    AudioManager audioManager;
    [SerializeField] string audioString;

    void Start() { 
        if(audioString != "") {
            audioManager = GetComponent<AudioManager>();
        audioManager.Play(audioString); 
        }
        Invoke("destroy", time); }

    private void destroy(){ Destroy(gameObject); }
}
