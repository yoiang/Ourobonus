using UnityEngine;
using System.Collections;

public class Worm : MonoBehaviour 
{
	public float rotationSpeed = 10f;
	public float growthSpeed = 0.05f;
	
	public GameObject model;
	protected Vector3 currentScale;
	
	// Use this for initialization
	void Start () {
		currentScale = model.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		model.transform.Rotate( new Vector3( 0f, 1f, 0f ), rotationSpeed * Time.deltaTime );

		currentScale.x -= growthSpeed * Time.deltaTime;
		currentScale.z -= growthSpeed * Time.deltaTime;
		if ( currentScale.x < 0 )
		{
			currentScale.x = 0;
		}
		if ( currentScale.z < 0 )
		{
			currentScale.z = 0;
		}
		model.transform.localScale = currentScale;
	}
}
