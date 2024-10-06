using UnityEngine;
using System.Collections;

public class CameraController {

	public static float heightInMeters;
	public static float widthInMeters;
    
    public static float cameraMinSize = 0.5f;
    public static float cameraMaxSize = 25; 

    public static Vector2 sizeInMeters {

        get {

            return new Vector2 (widthInMeters, heightInMeters);
        }
    }

	public static float pixelSize;

	public static Vector2 cameraPosition {
		get { return new Vector2 (Camera.main.transform.position.x, Camera.main.transform.position.z);}
		set {
			Camera.main.transform.position = new Vector3 (value.x,Camera.main.transform.position.y,value.y);
			GUIController.OnCameraUpdate ();
		}
	}

	public static float cameraSize {

		get { return Camera.main.orthographicSize; }

		set {

			Camera.main.orthographicSize = Mathf.Clamp(value, cameraMinSize, cameraMaxSize);
			widthInMeters = Camera.main.orthographicSize/Screen.width*(2f*Screen.height);
			heightInMeters = GetHeightInMeters(widthInMeters);
			pixelSize = widthInMeters / Screen.height;
			GUIController.OnCameraUpdate ();
		}
	}


	public static float GetWidthInMeters (float _heightInMeters) {
		return _heightInMeters*Screen.height/(Screen.width);
	}

	public static float GetHeightInMeters (float _widthInMeters) {
		return _widthInMeters*Screen.width/(Screen.height);
	}
	
	public static void ResizeCamera (float _widthInMeters) {

		widthInMeters = _widthInMeters;
		Camera.main.orthographicSize = widthInMeters*Screen.width/(2f*Screen.height);	
		heightInMeters = GetHeightInMeters(widthInMeters);
		
		pixelSize = widthInMeters / Screen.height;

		GUIController.OnCameraUpdate ();

	}

    public CameraController () {

        cameraSize = cameraSize;
    }

}
