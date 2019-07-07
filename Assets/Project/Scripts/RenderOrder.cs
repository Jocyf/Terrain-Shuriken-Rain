using UnityEngine;
using System.Collections;

// Used in waterfall
public class RenderOrder : MonoBehaviour 
{
	private Renderer myRenderer;

	void Start ()
	{
		myRenderer = GetComponent<Renderer> ();
		if (myRenderer != null) 
		{
			myRenderer.material.renderQueue = 2800;
		}
	}

}