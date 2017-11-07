using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Look at how clean and simple this guy is!
// All the complexity is handled by Mover
// And we can focus on just what's different about our Arriver
public class Rover : Mover {

	public GameObject target;
	public float threshold = 2f;

	protected override void Start () {
		base.Start ();
		velocity = new Vector3 (Random.Range (0, maxSpeed) - maxSpeed / 2f, 0, Random.Range (0, maxSpeed) - maxSpeed / 2f);
	}

	protected override void CalcSteering() {
		Vector3 fp = FuturePosition (1f);
		float xWeight = Mathf.Abs (fp.x) > 20f ? 1f + Mathf.Abs (fp.x) - 20f : 0f;
		float zWeight = Mathf.Abs (fp.z) > 20f ? 1f + Mathf.Abs (fp.z) - 20f : 0f;

		if (xWeight + zWeight > 0) {
			Vector3 wallForce = AvoidWall ();
			ApplyForce ((xWeight + zWeight) * wallForce);
		} 
		if (target) {
			ApplyForce (Seek (target.transform.position));
		}
	}

	protected override void OnRenderObject() {
		base.OnRenderObject ();

		if (target) {
			ColorHelper.green.SetPass (0);

			GL.Begin (GL.LINES);
			GL.Vertex (transform.position);
			GL.Vertex (target.transform.position);
			GL.End ();
		}
	}
}
