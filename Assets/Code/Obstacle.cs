using UnityEngine;
using System.Collections;

public class Obstacle : Entity
{
	public float collisionSpeedLoss;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnTriggerEnter(Collider c)
	{		
		Runner r = c.GetComponent(typeof(Runner)) as Runner;
		if (r != null)
		{
			//don't need to do anything actually, the Runner handles it
		}
	}
	
	public void OnTriggerExit(Collider c)
	{
		
	}
	
	public float GetCollisionSpeedLoss()
	{
		return collisionSpeedLoss;
	}
}
