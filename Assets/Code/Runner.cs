using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour
{
	public Worm worm;
	
	public float maxSpeed = 10.0f;
	public float jumpSpeed = 80.0f;
	public float gravity = 9.8f;
	public float bounce = 0.0f;
	
	//private float speed = 0.0f;
	private float ySpeed = 0.0f;
	private bool onGround = false;
	private float height;
	private float collisionHeight;
	
	// Use this for initialization
	void Start ()
	{
		height = (transform.collider as CapsuleCollider).height * transform.localScale.y;
		collisionHeight = height / 2;
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
		transform.up = transform.position - worm.transform.position;
	}
	
	private void UpdatePosition()
	{
		transform.position += transform.up * ySpeed * Time.deltaTime;
	}
	
	private void ProcessCollisions()
	{
		RaycastHit hit;
		//Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - collisionHeight, transform.position.z));
		if (Physics.Raycast(transform.position, -transform.up, out hit, collisionHeight, 1 << Globals.LAYER_WORM))
		{
			transform.position = hit.point + transform.up * collisionHeight;
			Land();
		}
		else
		{
			onGround = false;
		}
	}
	
	private void Jump()
	{
		if (!onGround)
		{
			return;
		}
		
		ySpeed = jumpSpeed;
		
		//hackaroony, we gotta clear the floor
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -transform.up, out hit, collisionHeight, 1 << Globals.LAYER_WORM))
		{
			transform.position = hit.point + transform.up * (collisionHeight+0.01f);
		}
		//play anim
	}
	
	private void Land()
	{
		onGround = true;
		ySpeed = Mathf.Abs(ySpeed) * bounce;
		//play anim
	}
}