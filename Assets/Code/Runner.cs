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
	private bool onGround = false;
	private float height;
	private float collisionHeight;
	
	// Use this for initialization
	void Start ()
	{
		height = (transform.collider as CapsuleCollider).height * transform.localScale.y;
		collisionHeight = height / 2;
		Debug.Log("boop" + speed);
	}
	
	// Update is called once per frame
	void Update ()
	{
		DoGravity();
		ProcessInput();
		UpdatePosition();
		ProcessCollisions();
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
	
	private void UpdatePosition()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y + ySpeed * Time.deltaTime, transform.position.z);
	}
	
	private void ProcessCollisions()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -transform.up, out hit, collisionHeight, 1 << Globals.LAYER_SNAKE))
		{
			transform.position = new Vector3(transform.position.x, hit.point.y+collisionHeight, transform.position.z);
			Land();
		}
	}
	
	private void Jump()
	{	
		if (jumping || !onGround)
		{
			return;
		}
		
		jumping = true;
		ySpeed += jumpSpeed;
		
		//hackaroony, we gotta clear the floor
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -transform.up, out hit, collisionHeight, 1 << Globals.LAYER_SNAKE))
		{
			transform.position = new Vector3(transform.position.x, hit.point.y+collisionHeight+0.01f, transform.position.z);
		}
		//play anim
	}
	
	private void Land()
	{
		onGround = true;
		
		if (!jumping)
		{
			return;
		}
		jumping = false;
		//play anim
	}
}