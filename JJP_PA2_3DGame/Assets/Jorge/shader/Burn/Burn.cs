using UnityEngine;

public class Burn : MonoBehaviour
{
    public float burnSpeed = 1.0f; 
    private Material material;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        material = renderer.material;
    }

    private void Update()
    {
        
        material.SetFloat("_Time", Time.time * burnSpeed);
    }
}