using UnityEngine;
using System.Collections;

public class Projectile : Entity 
{
	public bool isBillboarded;
	
	// Use this for initialization
	public override void Start () 
	{
		//--------------------------------------
		// Place wherever the mouse-pointer is.
		//--------------------------------------
		foreach (Transform child in transform)
		{
			child.localPosition = new Vector3(0,0,0);
			//child.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		}
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		//------------------------
		// Existing in the world.
		//------------------------
		if ( isBillboarded )
			transform.LookAt (transform.position + Camera.main.transform.rotation * Vector3.back,
							  Camera.main.transform.rotation * Vector3.up);
	}
}
