using UnityEngine;
using System.Collections;

public class OceanTexMovement : MonoBehaviour {

	public float scrollSpeed = 0.01f;
	public float dir = 0.0f;
	
	// Update is called once per frame
	void Update () {
		float offset = Time.time * scrollSpeed;
	    //renderer.material.SetTextureOffset ("_BumpMap", new Vector2(offset/-7.0f, offset));
		//renderer.material.SetTextureOffset ("_ColorControl", new Vector2(offset/-7.0f, offset));
		//renderer.material.SetTextureOffset ("_MainTex", new Vector2(-offset, 0.0f));
		GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", new Vector2(-offset, dir));
	}
}
