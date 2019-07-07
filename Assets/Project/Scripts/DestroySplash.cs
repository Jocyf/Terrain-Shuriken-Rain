using UnityEngine;

public class DestroySplash : MonoBehaviour 
{
	public float nTime = 0.3f;

	void Start () 
	{
		Destroy(this.gameObject, nTime);
	}
}
