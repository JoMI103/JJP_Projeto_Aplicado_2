using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Trap : DefenseBuilding
{
    [Space(10)]
    [Header("TrapAtributes")]
    [Space(10)]

    [SerializeField] private Transform centerTrapCollider;
    [SerializeField] private Vector3 boxCollSize;

    protected override void GetSurroundSheeps()
    {
        sheeps.Clear();

        var outSurroundingObjects = Physics.OverlapBox(centerTrapCollider.position, boxCollSize / 2, transform.rotation, sheepLayer);

        foreach (var surroundingObject in outSurroundingObjects)
        {
            EnemySheep enemySheep = surroundingObject.GetComponent<EnemySheep>();
            if (enemySheep != null) { sheeps.Add(enemySheep); }
        }
    }


    #region Gizmos

    public bool debugGizmos;

    private void OnDrawGizmos()
    {
        if (!debugGizmos) { return; }
        Gizmos.color = Color.red;
        Gizmos.matrix = centerTrapCollider.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, boxCollSize);
    }

    #endregion
}
