using UnityEngine;
using System.Collections;

public class CopyTransformv2 : MonoBehaviour {

	public Transform from;
	private Transform to;
	public Vector3 offset = Vector3.zero;

	private Vector3 Pos;
	//private Quaternion Rot;


	void Start(){
		//GameObject obj = GameObject.FindGameObjectWithTag("Player");
		//if(obj != null)
			//from = obj.transform;
		to = this.transform;
	}

	void LateUpdate () {

		Pos = from.position + offset;
		if(Pos != to.position)
			to.position = Pos;
	}

}
