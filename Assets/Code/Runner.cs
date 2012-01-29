using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour
{
	public Worm worm;
		
	public float maxSpeed = 10.0f;
	public float jumpSpeed = 80.0f;
	public float gravity = 9.8f;
	public float bounce = 0.0f;
	
	private float speed = 0.0f;
	private float ySpeed = 1.0f;
	private bool onGround = false;

	private float height;
	private float collisionHeight;
	private Animation anim;
	private string currentAnimationName;
	
	// Use this for initialization
	public void Start ()
	{
		height = (transform.collider as CapsuleCollider).height * transform.localScale.y;
		collisionHeight = height / 2;
		speed = worm.GetCircumferenceSpeed();
		anim = GetComponentInChildren(typeof(Animation)) as Animation;
		currentAnimationName = "run";
		maxSpeed += worm.GetCircumferenceSpeed();
	}
	
	// Update is called once per frame
	public void Update ()
	{
		DoGravity();
		ProcessInput();
		UpdatePosition();
		ProcessCollisions();
		
		PlayAnimation("run");
		anim["run"].speed = (speed / maxSpeed) * 2.5f;
	}
	
	public void OnTriggerEnter(Collider c)
	{
		Obstacle o = (c.GetComponent(typeof(Obstacle)) as Obstacle);
		if (o != null)
		{
			HitObstacle(o);
		}
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
			speed = speed > maxSpeed ? maxSpeed : speed;
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
		transform.localPosition += transform.up * ySpeed * Time.deltaTime;
		
		if (Physics.Raycast(transform.localPosition, -transform.up, collisionHeight*1.15f, 1 << Globals.LAYER_WORM))
		{
			//lateral speed
			float overallSpeed = speed - worm.GetCircumferenceSpeed();
			transform.localPosition += transform.right * overallSpeed * Time.deltaTime;
		}
	}
	
	private void ProcessCollisions()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.localPosition, -transform.up, out hit, collisionHeight, 1 << Globals.LAYER_WORM))
		{
			transform.localPosition = hit.point + transform.up * collisionHeight;
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
		if (Physics.Raycast(transform.localPosition, -transform.up, out hit, collisionHeight, 1 << Globals.LAYER_WORM))
		{
			transform.localPosition = hit.point + transform.up * (collisionHeight+0.01f);
		}
		//play anim
	}
	
	private void Land()
	{
		onGround = true;
		ySpeed = Mathf.Abs(ySpeed) * bounce;
		//play anim
	}
	
	private void HitObstacle(Obstacle o)
	{
		speed -= o.GetCollisionSpeedLoss();
		speed = speed < 0 ? 0 : speed;
		//play anim
	}
	
	private void PlayAnimation(string name)
	{
		if (!anim)
		{
			return;
		}
		
		if (ShouldPlayAnimation(name))
		{
			anim.CrossFade(name);
			currentAnimationName = name;
		}
	}
	
	private bool ShouldPlayAnimation(string name)
	{
		switch(name)
		{
			case "run":
				if (onGround && (anim.IsPlaying("jump") || anim.IsPlaying("jump_land") || anim.IsPlaying("jump_takeoff")))
				{
					return true;
				}
				return false;
			case "jump_takeoff":
			case "jump":
				if (!onGround && anim.IsPlaying("run"))
				{
					return true;
				}
				return false;
			case "jump_land":
				return true;
			case "hit":
				return true;
		}
		return false;
	}
}