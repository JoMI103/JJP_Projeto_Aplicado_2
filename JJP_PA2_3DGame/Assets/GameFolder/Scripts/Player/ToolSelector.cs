using UnityEngine;

public class ToolSelector : MonoBehaviour {

    [SerializeField] GameObject[] tools;

    private void Start() {
        selectTool(0);
    }

    public void selectTool(int id) {
        deSelectAll();
        if(id > 0 && id < tools.Length)
        tools?[id].SetActive(true);

    }

    private void deSelectAll() {
        foreach (GameObject tool in tools)
            if (tool != null) tool.SetActive(false);
    }
}
