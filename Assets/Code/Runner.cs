using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour
{
	public Worm worm;
		
	public float maxSpeed = 10.0f;
	public float jumpSpeed = 80.0f;
	public float gravity = 9.8f;
	public float bounce = 0.0f;
	public float minimumX = 162.0f;
	public float maximumX = 210.0f;
	public float xMatchSpeed = 0.0f;
	
	private float speed = 0.0f;
	private float ySpeed = 1.0f;
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
		
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			speed -= 15.0f * Time.deltaTime;
			speed = speed < 0 ? 0 : speed;
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			speed += 15.0f * Time.deltaTime;
			speed = speed > worm.GetRotationSpeed() + Globals.MAX_SPEED_DIFFERENCE ? worm.GetRotationSpeed() + Globals.MAX_SPEED_DIFFERENCE : speed;
		}
		else if (Input.GetKeyUp(KeyCode.Space))
		{
			speed = 0.0f;
		}
	}
		
	private void DoGravity()
	{
		ySpeed -= gravity * Time.deltaTime;
		transform.up = transform.position - worm.transform.position;
	}
	
	private void UpdatePosition()
	{
		//gravity
		transform.position += transform.up * ySpeed * Time.deltaTime;
		
		//lateral speed
		float speedRatio = (speed - worm.GetRotationSpeed()) / Globals.MAX_SPEED_DIFFERENCE;
		speedRatio = Mathf.Clamp(speedRatio, -1.0f, 1.0f);
		speedRatio = (speedRatio + 1.0f) / 2.0f;
		float desiredX = (maximumX - minimumX) * speedRatio + minimumX;
		//TODO maybe don't use a lerp, instead do a constant speed change
		transform.position = Vector3.Lerp(transform.position, new Vector3(desiredX, transform.position.y, transform.position.z), Time.deltaTime * xMatchSpeed);
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