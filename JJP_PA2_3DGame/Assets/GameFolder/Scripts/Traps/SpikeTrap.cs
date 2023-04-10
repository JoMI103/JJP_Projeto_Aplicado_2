using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private AudioManager au;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private PlacedBuilding placedBuilding;

    private int currentDmg;
    [SerializeField] private LayerMask giveDmg;
    public bool canattack;

    private void Awake()
    {   
        placedBuilding = GetComponent<PlacedBuilding>();
        au = GetComponent<AudioManager>();
    }

    private void Start()
    {
        currentDmg = placedBuilding.buildingTypeSO.damage;
    }

    public void attack()
    {
        au.Play("SpikeTrap");

        RaycastHit[] raycastHit = Physics.BoxCastAll(
            boxCollider.center + transform.position, 
            boxCollider.size, Vector3.up, Quaternion.identity, 1, giveDmg);

        foreach (RaycastHit hit in raycastHit)
        {
            Hittable damageAux = hit.collider.GetComponent<Hittable>();
            if (damageAux != null) damageAux.BaseHit(currentDmg);
        }
    }
}
