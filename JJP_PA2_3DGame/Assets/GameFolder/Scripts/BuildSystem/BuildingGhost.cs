using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] private float speed = 15f;

    private Transform visual;
    private BuildingTypeSO buildingTypeSO;


    private void Start()
    {
        RefreshVisual();
        GridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
    }

    private void Instance_OnSelectedChanged(object sender, System.EventArgs e) 
    {
        RefreshVisual();
    }


    private void LateUpdate()
    {
        if(GridBuildingSystem.Instance.GetMouseWorldSnappedPosition(out Vector3 targetPosition))
            visual?.gameObject.SetActive(true); else visual?.gameObject.SetActive(false);
        
        targetPosition.y = 0.1f;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
        transform.rotation = Quaternion.Lerp(transform.rotation, 
            GridBuildingSystem.Instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);
    }


    private void RefreshVisual()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        BuildingTypeSO buildingTypeSO = GridBuildingSystem.Instance.GetPlacedObjectTypeSO();

        if (buildingTypeSO != null)
        {
            visual = Instantiate(buildingTypeSO.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
            SetLayerRecursive(visual.gameObject, 11);
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

