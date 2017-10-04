using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This component lives on the camera parent object and triggers a transition when you walk through a portal 
[RequireComponent(typeof(Rigidbody))]
public class PortalTransition : MonoBehaviour {

	public delegate void PortalTransitionAction();
	public static event PortalTransitionAction OnPortalTransition;

	// The main camera is surrounded by a SphereCollider with IsTrigger set to On
	void OnTriggerEnter(Collider portal){
		Portal logic = portal.GetComponentInParent<Portal> ();
		transform.position = logic.PortalCameras[1].transform.position - GetComponentInChildren<Camera>().transform.localPosition;

		if (OnPortalTransition != null) {
			// Emit a static OnPortalTransition event every time the camera enters a portal. The DoorManager listens for this event.
			OnPortalTransition ();
		}
	}

}
