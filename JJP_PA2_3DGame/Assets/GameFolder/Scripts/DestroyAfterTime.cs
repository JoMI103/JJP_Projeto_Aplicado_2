using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float time;

    void Start() { Invoke("destroy", time); }

    private void destroy(){ Destroy(gameObject); }
}
