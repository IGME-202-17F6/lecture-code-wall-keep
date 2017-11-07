using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour {

	CharacterController characterController;
	Vector3 acceleration;
	protected Vector3 velocity;

	public float mass = 1f;
	public float maxSpeed = 0.5f;
	public float maxTurn = 0.25f;
	public float radius = 0.5f;
	public GameObject futurePosIndicator;

	protected virtual void Start () {
		this.characterController = GetComponent<CharacterController> ();
		// get our position from the game object

		// no forces to start off
		velocity = Vector3.zero;
		acceleration = Vector3.zero;
	}

	// Will be overridden by derived classes.
	// No direct implementation here!
	protected abstract void CalcSteering ();

	// returns a steering vector towards the given targetPos
	protected Vector3 Seek(Vector3 targetPos) {
		Vector3 toTarget = targetPos - this.transform.position;
		Vector3 desiredVelocity = toTarget.normalized * maxSpeed;
		Vector3 steeringForce = desiredVelocity - velocity;

		return VectorHelper.Clamp (steeringForce, maxTurn);
	}

	// should return a steering vector away from the given targetPos
	protected Vector3 Flee(Vector3 targetPos) {
		return Vector3.zero;
	}

	// returns a steering vector that decreases within threshold distance, stopping at radii
	protected Vector3 Arrive(Vector3 targetPos, float threshold, float radii) {
		Vector3 toTarget = targetPos - this.transform.position;
		Vector3 desiredVelocity;

		if (toTarget.magnitude - radii < threshold) {
			// slow down
			float percentFromCenter = (toTarget.magnitude - radii) / threshold;
			float fractionOfMaxSpeed = percentFromCenter * maxSpeed;

			desiredVelocity = toTarget.normalized * fractionOfMaxSpeed;
		} else {
			// max speed!
			desiredVelocity = toTarget.normalized * maxSpeed;
		}
			
		Vector3 steeringForce = desiredVelocity - velocity;

		return VectorHelper.Clamp (steeringForce, maxTurn);
	}

	public void ApplyForce(Vector3 force) {
		acceleration += force / mass;
	}

	public Vector3 FuturePosition(float seconds) {
		return this.transform.position + velocity * seconds;
	}

	public Vector3 AvoidWall() {
		return Seek (Vector3.zero);
	}
		
	void LateUpdate () {

		CalcSteering ();

		// update velocity, position
		velocity += acceleration;
		velocity = VectorHelper.Clamp (velocity, maxSpeed);
		this.characterController.Move (velocity * Time.deltaTime);

		this.futurePosIndicator.transform.position = FuturePosition (3);

		acceleration = Vector3.zero;
	}

	protected virtual void OnRenderObject() {
		ColorHelper.black.SetPass (0);

		GL.Begin (GL.LINES);
		GL.Vertex (transform.position);
		GL.Vertex (transform.position + velocity.normalized);
		GL.End ();
	}
}
