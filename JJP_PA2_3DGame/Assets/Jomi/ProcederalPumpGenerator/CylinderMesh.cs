using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CylinderMesh: MonoBehaviour
{
     [SerializeField]Mesh mesh;
     
     [SerializeField] float Radius;
     int layers, resolution;
     
     Vector2[] xyPos;
     
     public void create(Mesh mesh,int layers ,int resolution){
        this.layers =layers;
        this.resolution = resolution;
        this.mesh = mesh;
        
        xyPos = new Vector2[resolution];
        
        for (int i = 0; i < resolution; i++)
        {
            var radians = 2 * MathF.PI / resolution * i;
            var vertical = MathF.Sin(radians);
            var horizontal = MathF.Cos(radians); 
            xyPos[i] =  new Vector2(horizontal,vertical);
        }
     }
     
     
    public void constructMesh()
    {
        Vector3[] vertices = new Vector3[layers * resolution];
        Vector2[] uv = new Vector2[layers * resolution];
        int[] triangles = new int[resolution * (layers-1)* 2 * 3];
        int triIndex = 0;
        float height = 0;
        
        
        
        for (int y = 0; y < layers; y++)
        {
            height =  y / (float)(layers - 1);
            
            for (int v = 0; v < resolution; v++)
            {
                int vertId = y * resolution + v;
                Vector3 pointOnUnitCube = new Vector3 (xyPos[v].x, height, xyPos[v].y);

                vertices[vertId] = pointOnUnitCube;
                
                uv[vertId] = new Vector2(0,height);
                
                if(y+1 != layers){
                    
                    if(v+1 == resolution){
               
                        triangles[triIndex] = vertId;
                        triangles[triIndex + 2] =  (y+1) * resolution;
                        triangles[triIndex + 1] = vertId + resolution;
                        triangles[triIndex + 3] = vertId;
                        triangles[triIndex + 5] =  y * resolution;
                        triangles[triIndex + 4] =  (y+1) * resolution;
                    }else{   
                        triangles[triIndex] = vertId;
                        triangles[triIndex + 2] = vertId + resolution + 1;
                        triangles[triIndex + 1] = vertId + resolution;
                        triangles[triIndex + 3] = vertId;
                        triangles[triIndex + 5] = vertId + 1;
                        triangles[triIndex + 4] = vertId + resolution + 1;
                    }

                    triIndex += 6;
                    
                }                
            }
            
            
        }
        
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
         mesh.RecalculateBounds();
        
    }
    
    
    public void buildMeshArroundSpline(catmullRomSpline cmrs){
        cmrs.calcSplinesLeghts();


        float height = 0;
        
        Vector3 currentPos = cmrs.GetCatmullRomPosition(0);
        Vector3 nextPos = Vector3.zero;
        Vector3 currentUp = Vector3.zero;
        
        Vector3[] vertices = new Vector3[layers * resolution];
        Vector3 up, forward, right; Matrix4x4 M;
        
        void calcMatrix(){
            up = (nextPos - currentPos).normalized;
     
                forward = Vector3.forward;
                right = Vector3.Cross(up, forward).normalized;
                forward = Vector3.Cross(right, up).normalized; 
          
            
             M = new Matrix4x4();
            M.SetColumn(0, right);
            M.SetColumn(1, up);
            M.SetColumn(2, forward);
        }
        
        void SetVertexPos(int nLayer){
            for(int v = 0; v < resolution; v++){
                Vector3 currentxyzPos = M * new Vector3(xyPos[v].x,0,xyPos[v].y) * Radius;
                
                int vertId = nLayer * resolution + v;
                Vector3 pointOnUnitCube = currentPos + currentxyzPos;

                vertices[vertId] = pointOnUnitCube;
            }
        }
        
        for(int l = 0; l < layers-1; l++){
            height = (l+1) / (float)(layers - 1);
            nextPos = cmrs.GetCatmullRomPosition(height);

            calcMatrix();
            SetVertexPos(l);

            if(l != layers-2) currentPos = nextPos;  //para calcular a normal dos ultimos vertices
        }
        
        currentPos = cmrs.GetCatmullRomPosition(0.9999f);
        nextPos = cmrs.GetCatmullRomPosition(1.0000f);
        calcMatrix(); SetVertexPos(layers-1);
        
        mesh.vertices = vertices; 
         mesh.RecalculateNormals();
         mesh.RecalculateBounds();
    }
    
    public void recalc(){
        
    }
}
