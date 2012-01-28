using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour
{
	public float maxSpeed = 10.0f;
	public float jumpSpeed = 80.0f;
	public float gravity = 9.8f;
	
	private float speed = 0.0f;
	private float ySpeed = 0.0f;
	private bool jumping = false;
	private bool jumpingThisFrame = false;
	private float height;
	private float collisionHeight;
	
	// Use this for initialization
	void Start ()
	{
		height = (transform.collider as CapsuleCollider).height;
		collisionHeight = height / 2;
		Debug.Log("boop" + speed);
	}
	
	// Update is called once per frame
	void Update ()
	{
		ProcessInput();
		DoGravity();
		ProcessCollisions();
		
		transform.position = new Vector3(transform.position.x, transform.position.y + ySpeed * Time.deltaTime, transform.position.z);
		jumpingThisFrame = false;
	}
	
	private void ProcessInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
				
			//Only raycast against runners.
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << Globals.LAYER_RUNNER))
			{
				Jump();
			}
		}
	}
	
	private void DoGravity()
	{
		ySpeed -= gravity * Time.deltaTime;
	}
	
	private void ProcessCollisions()
	{
		if (!jumpingThisFrame)
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, -transform.up, out hit, collisionHeight, 1 << Globals.LAYER_SNAKE))
			{
				transform.position = new Vector3(transform.position.x, hit.point.y-collisionHeight, transform.position.y);
				Land();
			}
		}
	}
	
	private void Jump()
	{
		if (jumping)
		{
			return;
		}
		
		jumping = true;
		jumpingThisFrame = true;
		ySpeed += jumpSpeed;
		//play anim
	}
	
	private void Land()
	{
		if (!jumping)
		{
			return;
		}
		
		jumping = false;
		//play anim
	}
}