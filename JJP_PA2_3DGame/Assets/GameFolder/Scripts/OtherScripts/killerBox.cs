using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class killerBox : MonoBehaviour
{
    [SerializeField] private int hp = 20;

    [SerializeField] GameObject g;
    [SerializeField] private TextMeshProUGUI hpCount;

    [SerializeField] private int currentDmg;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private LayerMask giveDmg;
    // Start is called before the first frame update
    void Start()
    {
        hpCount.text = " HP:" + hp.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] raycastHit = Physics.BoxCastAll(boxCollider.center + transform.position - new Vector3(0,transform.localScale.y/2,0),transform.localScale / 2, Vector3.up, Quaternion.identity, 0, giveDmg);

        if (Input.GetKeyDown(KeyCode.B)) { restart(); }

        foreach (RaycastHit hit in raycastHit)
        {
            EnemySheep sheep = hit.collider.GetComponent<EnemySheep>();
            if (sheep != null)
            {

                if (hp > 1)
                {

                    sheep.death();
                    hp--;
                    hpCount.text = " HP:" + hp.ToString();
                }
                else
                {
                    
                    g.SetActive(true);

                }
            }
        }
        
      
    }

    void OnDrawGizmos()
    {

        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(1,0.2f,0.2f,0.3f);

        Gizmos.DrawCube(boxCollider.center + transform.position, transform.localScale);
    }

    public void restart()
    {
        SceneManager.LoadScene( "Menu");
        

    }

}
