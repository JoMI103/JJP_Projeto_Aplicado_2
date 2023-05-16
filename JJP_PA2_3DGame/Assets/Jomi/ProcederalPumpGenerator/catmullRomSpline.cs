using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class catmullRomSpline : MonoBehaviour
{
    [SerializeField] public Vector3[] HandlesPos;
    [SerializeField] MeshRenderer mf;
    [SerializeField] Cylinder cylinder;
    
    [SerializeField] Transform handlesParent;
    
    /*
    private void OnValidate() {
        calcPosLine();
    }
*/

    public void setCorners(Vector3[] corners){
        HandlesPos = corners;
        calcPosLine();
    }

/*
    void checkHandlesPos(){
        
        bool updatee = false;
        
        if(handlesParent.childCount != HandlesPos.Length){
            updatee=true;
            HandlesPos = new Vector3[handlesParent.childCount];
            int i = 0;
            foreach (Transform t in handlesParent)
            {
                
                HandlesPos[i] = t.position;
                i++;
            }
        }else{
            int i = 0;
            foreach (Transform t in handlesParent)
            {
                if(HandlesPos[i] != t.position){
                    updatee = true;
                    HandlesPos[i] = t.position;
                }
                i++;
            }
        }
        
        if(updatee) calcPosLine();
        
    }
    */
        
    public void calcPosLine(){
        Debug.Log("eeee");
        cylinder.cylinderMesh.buildMeshArroundSpline(this);
    }
    
    /*
    private void OnDrawGizmos() {
        checkHandlesPos();
        
        Gizmos.color = Color.red;
        foreach(Vector3 t in HandlesPos){
            Gizmos.DrawCube(t,Vector3.one * 0.25f);
        }
        Gizmos.color = Color.white;
    }
    */


    [SerializeField] int splineCalcDistPrecision = 2;
    int nSplines;
    float[] splinesPercent;
    
    public void calcSplinesLeghts(){
        int nSplines = HandlesPos.Length - 3;
        
        splinesPercent = new float[nSplines];
        splinesPercent[nSplines-1] = 1;
        
        if(nSplines == 1) return;
        
        Vector3 currentPos, nextPos;
        
        float totalSplines = 0;
        
        for(int s = 0; s < nSplines; s++){
            float total = 0;
            Vector3 p1 = HandlesPos[s], p2=HandlesPos[s+1] , p3 = HandlesPos[s+2],p4 = HandlesPos[s+3];
            
            currentPos  = GetCatmullRomPosition(0,p1,p2,p3,p4);
            
            for(int p = 1; p <= splineCalcDistPrecision; p++){
                float percent = p / splineCalcDistPrecision;
                
                nextPos = GetCatmullRomPosition(percent,p1,p2,p3,p4);
                total += Vector3.Distance(nextPos,currentPos);
                currentPos = nextPos;
            }
            splinesPercent[s] = total; totalSplines +=total;
        }
        
        float total2 = 0;
        for(int i = 0; i< splinesPercent.Length;i++){
            total2 += splinesPercent[i];
            splinesPercent[i] = total2/ totalSplines;
        }
        
        Debug.Log(123);
    }
    
    
    float InverseLerp(float a ,float b, float v){
        return (v-a)/(b-a);
    }
    
    public Vector3 GetCatmullRomPosition(float t){
        
        
        float aux1 = 0, aux2 = 1; int id =0;
        for (int i = 0; i < splinesPercent.Length; i++)
        {
            if(t <= splinesPercent[i]){id=i; aux2 = splinesPercent[i]; break; } else {aux1 = splinesPercent[i];}
        }
        
        return GetCatmullRomPosition(InverseLerp(aux1,aux2,t),HandlesPos[id],HandlesPos[id+1],HandlesPos[id+2],HandlesPos[id+3]);
    }
    
    
    public Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float alpha = 0.1f)
    {
        float dt0 = GetTime(p0, p1, alpha);
        float dt1 = GetTime(p1, p2, alpha);
        float dt2 = GetTime(p2, p3, alpha);

        Vector3 t1 = ((p1 - p0) / dt0) - ((p2 - p0) / (dt0 + dt1)) + ((p2 - p1) / dt1);
        Vector3 t2 = ((p2 - p1) / dt1) - ((p3 - p1) / (dt1 + dt2)) + ((p3 - p2) / dt2);

        t1 *= dt1;
        t2 *= dt1;

        Vector3 c0 = p1;
        Vector3 c1 = t1;
        Vector3 c2 = (3 * p2) - (3 * p1) - (2 * t1) - t2;
        Vector3 c3 = (2 * p1) - (2 * p2) + t1 + t2;
        Vector3 pos = CalculatePosition(t, c0, c1, c2, c3);

        return pos;
    }

    private float GetTime(Vector3 p0, Vector3 p1, float alpha)
    {
        if(p0 == p1)
            return 1;
        return Mathf.Pow((p1 - p0).sqrMagnitude, 0.5f * alpha);
    }

    private Vector3 CalculatePosition(float t, Vector3 c0, Vector3 c1, Vector3 c2, Vector3 c3)
    {
        float t2 = t * t;
        float t3 = t2 * t;
        return c0 + c1 * t + c2 * t2 + c3 * t3;
    }
}



