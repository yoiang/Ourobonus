using UnityEngine;
using System.Collections;

public class Worm : MonoBehaviour 
{
	public float radiusSize = 100.0f;
	public float rotationSpeed = 10f;
	
	protected Vector3 currentScale;
	
	public uint segmentCount = 50;
	public GameObject segmentPrefab;
	protected ArrayList segments;
	
	// HACK: editor
	protected static float _radiusSize;
	protected static uint _segmentCount;
	protected static GameObject _segmentPrefab;

	void Start () 
	{
		currentScale = transform.localScale;		
		// HACK:
		_radiusSize = radiusSize;
		_segmentCount = segmentCount;
		_segmentPrefab = segmentPrefab;
		
		CreateSegments();
	}
	
	void CreateSegments()
	{
		segments = new ArrayList( ( int )segmentCount );
				
		for( uint travSegments = 0; travSegments < segmentCount; travSegments ++ )
		{
			GameObject segment = Worm.CreateSegment( travSegments );
			segments.Add( segment );
			segment.transform.parent = transform;	
		}
	}
	
	protected static GameObject CreateSegment( uint segmentNumber )
	{
		GameObject segment = GameObject.Instantiate( _segmentPrefab, Worm.SegmentLocation( segmentNumber ), Worm.SegmentRotation( segmentNumber ) ) as GameObject;
		
		float scale = SegmentScale();
		segment.transform.localScale = new Vector3( scale * 0.5f, scale * 0.6f, scale * 0.5f );		
		
		return segment;
	}
	
	protected static Vector3 SegmentLocation( uint segmentNumber )
	{
		Quaternion segmentRotation = Quaternion.AngleAxis( Worm.SegmentAngle( segmentNumber ), new Vector3( 0, 0, 1 ) );		
		return segmentRotation * new Vector3( 0, _radiusSize, 0 );
	}
	
	protected static Quaternion SegmentRotation( uint segmentNumber )
	{
		Quaternion assetRotation = Quaternion.AngleAxis( 90, new Vector3( 0, 0, 1 ) );
		Quaternion segmentRotation = Quaternion.AngleAxis( Worm.SegmentAngle( segmentNumber ), new Vector3( 0, 0, 1 ) );		
		return assetRotation * segmentRotation;
	}
	
	protected static float SegmentScale()
	{
		Vector3 firstLocation = new Vector3( 0, _radiusSize, 0 );		
		Quaternion locationRotation = Quaternion.AngleAxis( Worm.SegmentAngle( 1 ), new Vector3( 0, 0, 1 ) );
		return ( firstLocation - locationRotation * firstLocation ).magnitude;
	}
	
	protected static float SegmentAngle( uint segmentNumber = 0 )
	{
		return 360.0f / _segmentCount * segmentNumber;
	}
	
	void Update () 
	{
		transform.Rotate( new Vector3( 0f, 0f, 1f ), rotationSpeed * Time.deltaTime );
	}
	
	public float GetRotationSpeed()
	{
		return rotationSpeed;
	}
}
