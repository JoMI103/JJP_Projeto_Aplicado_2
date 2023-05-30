using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletTrail : MonoBehaviour
{
    MeshFilter meshFilter; MeshRenderer meshRenderer;
    Vector3 pos1,pos2; float radius;

    
    [ContextMenu("SetUP")]
    public void setUp(Vector3 pos1, Vector3 pos2, float radius){
        meshFilter = GetComponent<MeshFilter>(); meshRenderer = GetComponent<MeshRenderer>();
        this.pos1 = pos1; this.pos2 = pos2; this.radius = radius;
        updateMesh();
        
        StartCoroutine(bulletTrail());

    }
    
    private void updateMesh(){
        Vector3 normal = (pos2-pos1).normalized;
        transform.up = normal; 
        transform.localScale = new Vector3(radius,Vector3.Distance(pos1,pos2) * 5, radius);
        
        /*
        
        
        Vector2[] uv =  meshFilter.mesh.uv;
        int[] triangles = meshFilter.mesh.triangles;
        
        int length = meshFilter.mesh.vertices.Length;
        int halfLenght = length / 2;
        Vector3[] vertices = new Vector3[length];
        
        Matrix4x4 M;
        
        void calcMatrix(){
            Vector3 normal = (pos2-pos1).normalized;
           
            Vector3 forward = Vector3.forward;
            Vector3 right = Vector3.Cross(normal, forward).normalized;
            forward = Vector3.Cross(right, normal).normalized; 
          
            
            M = new Matrix4x4();
            M.SetColumn(0, right);
            M.SetColumn(1, normal);
            M.SetColumn(2, forward);
        }

        Vector2[] xyPos = new Vector2[halfLenght];
        
        for (int i = 0; i < halfLenght ; i++)
        {
            var radians = 2 * MathF.PI / halfLenght * i;
            var vertical = MathF.Sin(radians);
            var horizontal = MathF.Cos(radians); 
            xyPos[i] =  new Vector2(horizontal,vertical);
        }
        
        
        calcMatrix();
        for(int i = 0; i < halfLenght; i++){
            vertices[i] =  M *  new Vector3(xyPos[i].x,0,xyPos[i].y) * radius;
            vertices[i] += pos1;
            vertices[i + halfLenght] = M * new Vector3(xyPos[i].x,0,xyPos[i].y) * radius;
            vertices[i + halfLenght] += pos2;
        }
        
        meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = vertices;  
        meshFilter.mesh.uv = uv;
        meshFilter.mesh.triangles = triangles;
        meshFilter.mesh.RecalculateNormals();
        */
    }
    
    private IEnumerator bulletTrail(){
        float timer = 0;
        while (timer < 0.2){
            meshRenderer.material.SetFloat("_Value",timer * 5);
            timer += Time.deltaTime;
            yield return null;
        }
        
        Destroy(this.gameObject);
    }
    

   
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + pos1, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + pos2, 0.1f);
    }
}
