using UnityEngine;
using System.Collections;

public class SinusCurveModifier : MonoBehaviour {
	public float scale = 10.0f;
	public float speed = 1.0f;
	public bool HasMeshCollider = false;
	private Vector3[] baseHeight;
	
	
	// Update is called once per frame
	void Update () {
	   MeshFilter meshF = GetComponent(typeof(MeshFilter)) as MeshFilter;
	   Mesh mesh = meshF.mesh;
	
	   if(baseHeight == null)
	      baseHeight = mesh.vertices;

	   Vector3[] vertices = new Vector3[baseHeight.Length];
	   for (int i=0;i<vertices.Length;i++){
	      Vector3 vertex = baseHeight[i];
	      //vertex.y += Mathf.Sin(Time.time * speed+ baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale;
	      vertex.y += Mathf.Sin(Time.time * speed + baseHeight[i].y + baseHeight[i].z) * (scale/2)
	                + Mathf.Sin(Time.time * speed + baseHeight[i].y + baseHeight[i].x) * (scale/2);
	      vertices[i] = vertex;
	   }
	   mesh.vertices = vertices;
	   mesh.RecalculateNormals();

	   if(HasMeshCollider){
	   		MeshCollider MeshC = GetComponent(typeof(MeshCollider)) as MeshCollider;
	   		MeshC.sharedMesh = mesh;
	   }
	}
}
