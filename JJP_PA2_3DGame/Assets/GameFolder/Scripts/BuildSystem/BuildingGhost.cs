using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] private float speed = 15f;

    private Transform visual;
    private BuildingTypeSO buildingTypeSO;
    [SerializeField] private PlayerBuild playerBuild;


    private void Start()
    {
        RefreshVisual();
        playerBuild.OnSelectedChanged += Instance_OnSelectedChanged;
    }

    private void Instance_OnSelectedChanged(object sender, System.EventArgs e) 
    {
        RefreshParent();
        RefreshVisual();
    }


    private void LateUpdate()
    {
        if (!playerBuild.hitting)
        {
            transform.position = playerBuild.mouseWorldPos;
            visual?.gameObject.SetActive(false);
            return;
        }
        playerBuild.GetMouseWorldSnappedPosition(out Vector3 targetPosition);
        visual?.gameObject.SetActive(true);

      
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * speed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation,
            playerBuild.GetPlacedObjectRotation(), Time.deltaTime * 15f);
        transform.localScale = new Vector3(1, 1, 1);

    }

    private void RefreshParent()
    {
        if (playerBuild.currentGrid == null) return;
        if (playerBuild.currentGrid.transform != transform.parent)
        {
            transform.parent = playerBuild.currentGrid.transform;
       
        }
    }


    private void RefreshVisual()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        BuildingTypeSO buildingTypeSO = playerBuild.GetPlacedObjectTypeSO();

        if (buildingTypeSO != null)
        {
            visual = Instantiate(buildingTypeSO.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
            visual.localScale = Vector3.one;
            //SetLayerRecursive(visual.gameObject, 11);
        }
    }


    private void SetLayerRecursive(GameObject targetGameObject, int layer)
    {
        targetGameObject.layer = layer;
        foreach (Transform child in targetGameObject.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }
}

