using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class shows and hides doors (aka portals) when you walk into them. It listens for all OnPortalTransition events
// and manages the active portal.
public class DoorManager : MonoBehaviour {

	public delegate void DoorAction(Transform door);
	public static event DoorAction OnDoorOpen;

	public GameObject doorToVirtual;
	public GameObject doorToReality;

	public Camera mainCamera;

	private GameObject currDoor;

	private bool isCurrDoorOpen = false;
	private bool isNextDoorVirtual = true;

	void Start(){
		PortalTransition.OnPortalTransition += OnDoorEntrance;
	}

	// This method is called from the Spawn Portal button in the UI. It spawns a portal in front of you.
	public void OpenDoorInFront(){
		if (!isCurrDoorOpen) {
			if (isNextDoorVirtual)
				currDoor = doorToVirtual;
			else
				currDoor = doorToReality;
			

			currDoor.SetActive (true);

			currDoor.transform.position = (Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up)).normalized
				+ mainCamera.transform.position;

			currDoor.transform.rotation = Quaternion.LookRotation (
				Vector3.ProjectOnPlane(currDoor.transform.position - mainCamera.transform.position, Vector3.up));

			currDoor.GetComponentInParent<Portal>().Source.transform.localPosition = currDoor.transform.position;

			isCurrDoorOpen = true;

			if (OnDoorOpen != null) {
				OnDoorOpen (currDoor.transform);
			}
		}
	}

	// Respond to the player walking into the doorway. Since there are only two portals, we don't need to pass which
	// portal was entered.
	private void OnDoorEntrance() {
		currDoor.SetActive(false);
		isCurrDoorOpen = false;
		isNextDoorVirtual = !isNextDoorVirtual;
	}
}
