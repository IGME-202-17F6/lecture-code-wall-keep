using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorHelper : MonoBehaviour {

	public static Vector3 Clamp(Vector3 v, float magLimit) {
		float mag = v.magnitude;
		if (mag > magLimit) {
			v *= magLimit / v.magnitude;
		}
		return v;
	}
}
