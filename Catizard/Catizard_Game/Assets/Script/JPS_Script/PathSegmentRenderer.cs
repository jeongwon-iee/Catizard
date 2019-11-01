using UnityEngine;

// Draws a segment of a line
public class PathSegmentRenderer : MonoBehaviour 
{
	[SerializeField] private GameObject _arrowHead = null;

	[SerializeField] float arrowHeadOffset = 0.0f;

#region Pub Methods

	public void DrawSegment( Vector3 start_pos, Vector3 end_pos )
	{
		// find relative vecotr from start pos to end pos
		Vector2 euler_vector = end_pos - start_pos;

		// Shift the line endings so that the line begins at the edge of the block and stops right before the arrowhead sprite
		Vector3 local_end_pos = new Vector3( euler_vector.magnitude, 0, 0 );
		Vector3 arrow_head_pos = local_end_pos;

		// Move the line renderer down a bit to compensate for the arrow head sprite
		local_end_pos.x -= arrowHeadOffset;

		// Perform the Math to find the rotation of the arrow head
		float rotation = Mathf.Rad2Deg * Mathf.Atan2( euler_vector.y, euler_vector.x );

		_arrowHead.transform.localPosition = arrow_head_pos;   // set world position, not local
		this.transform.position = start_pos;
		this.transform.rotation = Quaternion.Euler( 0, 0, rotation );
	}

#endregion

}
