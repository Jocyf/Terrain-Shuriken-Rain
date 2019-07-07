// collision event script attached to a ParticleSystem
// applies a force to rigid bodies that are hit by particles

using UnityEngine;
using System.Collections;

public class ShurikenCollisions : MonoBehaviour 
{
	private ParticleCollisionEvent[] collisionEvents= new ParticleCollisionEvent[16];
	public Transform explosionObject;

	void OnParticleCollision (GameObject other)
	{
		// adjust array length
		int safeLength = GetComponent<ParticleSystem>().GetSafeCollisionEventSize();
		if (collisionEvents.Length < safeLength)
		{
			collisionEvents = new ParticleCollisionEvent[safeLength];
		}

		// get collision events for the gameObject that the script is attached to
		int numCollisionEvents = GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);

		// apply some force to RigidBody components
		for (int i = 0; i < numCollisionEvents; i++) 
		{
			if (explosionObject != null) 
			{
				Vector3 pos = collisionEvents[i].intersection;
				Transform.Instantiate(explosionObject, pos,  Quaternion.identity );
			}
		}
	}

}