using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
	public Camera MainCamera = null;
	// One to render to texture, and another to render normally to switch between (preview)
	public Camera[] PortalCameras = null;
	public Transform Source = null;
	public Transform Destination = null;
	
	// Helpers
	public static Quaternion QuaternionFromMatrix(Matrix4x4 m) { return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1)); }
	public static Vector4 PosToV4(Vector3 v) { return new Vector4( v.x, v.y, v.z, 1.0f ); }
	public static Vector3 ToV3(Vector4 v) { return new Vector3( v.x, v.y, v.z ); }

	public static Vector3 ZeroV3 = new Vector3( 0.0f, 0.0f, 0.0f );
	public static Vector3 OneV3 = new Vector3( 1.0f, 1.0f, 1.0f );
	
	private void LateUpdate() {
		foreach (Camera portalCamera in PortalCameras) {
			Vector3 cameraInSourceSpace = Source.InverseTransformPoint (MainCamera.transform.position);
			Quaternion cameraInSourceSpaceRot = Quaternion.Inverse (Source.rotation) * MainCamera.transform.rotation;

			// Note: Valid if PortalCamera and MainCamera are each in space of/children
			// of Source and Destination transforms respectively
			// portalCamera.transform.localPosition = cameraInSourceSpace;
			// portalCamera.transform.localRotation = cameraInSourceSpaceRot;

			// Transform Portal Camera to World Space relative to Destination transform,
			// matching the Main Camera position/orientation
			portalCamera.transform.position = Destination.TransformPoint (cameraInSourceSpace);
			portalCamera.transform.rotation = Destination.rotation * cameraInSourceSpaceRot;

			portalCamera.fieldOfView = MainCamera.fieldOfView;
			portalCamera.aspect = MainCamera.aspect;
		}
	}
}