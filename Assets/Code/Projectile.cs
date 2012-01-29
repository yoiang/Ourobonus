using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public Camera activeCamera;
	public bool isBillboarded;
	
	// Use this for initialization
	void Start () 
	{
		//------------------------
		// Launch into the world.
		//------------------------
	}
	
	// Update is called once per frame
	void Update () 
	{
		//------------------------
		// Existing in the world.
		//------------------------
		if ( isBillboarded )
			transform.LookAt (transform.position + activeCamera.transform.rotation * Vector3.back,
							  activeCamera.transform.rotation * Vector3.up);
	}
}
