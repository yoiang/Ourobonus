using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{
	public GameObject lightningObject;
	public int distanceFromCamera = 5;
	
	private GameObject instantiatedProjectile;
	
	// Use this for initialization
	void Start () 
	{
		instantiatedProjectile = null;
	}
	
	// Update is called once per frame
	void Update ()
	{
		InputUpdate();
	}
	
	private void InputUpdate ()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,
                                                                    Input.mousePosition.y,
                                                                    Camera.main.nearClipPlane + distanceFromCamera));
		
		// Spawn projectile.
		if ( Input.GetMouseButtonDown(0) )
		{
			instantiatedProjectile = (GameObject)Object.Instantiate (lightningObject, mousePosition, transform.rotation);
		}
		
		// Allow the user to move newly spawned object.
		if ( Input.GetMouseButton(0) )
		{
			//Debug.Log ("Mouse 1 is held down.");
			instantiatedProjectile.transform.position = mousePosition;
		}
		
		// Apply a force to the object and release it into
		// the world.
		if ( Input.GetMouseButtonUp(0) )
		{
			//Debug.Log ("Mouse 1 was released.");
		}
	}
	
}
