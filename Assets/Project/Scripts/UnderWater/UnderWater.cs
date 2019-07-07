using UnityEngine;
using System.Collections;

//This script makes possible underwater effects attach to WaterPlane with a trigger.

public class UnderWater : MonoBehaviour {

	//Define variables
	public Color cFogColor = new Color (0f, 0.4f, 0.7f, 1f);
	public float nFogDensity = 0.02f;
	
	//The scene's default fog settings
	private bool defaultFog;
	private Color defaultFogColor;
	private float defaultFogDensity;
	
	// The Underwater plane to make the distorsion effect.
	public GameObject UnderWaterPlane = null;

	void Start () 
	{
		defaultFog = RenderSettings.fog;
		defaultFogColor = RenderSettings.fogColor;
		defaultFogDensity = RenderSettings.fogDensity;
	}
	
	void OnTriggerEnter (Collider other) 
	{
		if(other.name.Contains("First Person"))
		{
			RenderSettings.fog = true;
			RenderSettings.fogColor = cFogColor;
			RenderSettings.fogDensity = nFogDensity;
			if(UnderWaterPlane != null)
				UnderWaterPlane.SetActive(true);
		}
	}
	
	void OnTriggerExit (Collider other) 
	{
		if(other.name.Contains("First Person"))
		{
			RenderSettings.fog = defaultFog;
			RenderSettings.fogColor = defaultFogColor;
			RenderSettings.fogDensity = defaultFogDensity;
			if(UnderWaterPlane != null)
				UnderWaterPlane.SetActive(false);
		}
	}
}
