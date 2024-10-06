using UnityEngine;
using System.Collections;

public class ScenePassageController : MonoBehaviour {

	public static ScenePassageController instance = null;

	private static bool isAlphaIncreasing;
	private string sceneToLoad = "";
	private static float alphaSpeed = 1f;
	private static float maxAlpha = 1f;

    private static Actions.VoidVoid onEnd;

    private static bool isJustSceneLoaded = false;

	public static void OnSceneLoaded () {

        alphaSpeed = 1f;
        isJustSceneLoaded = true;
		isAlphaIncreasing = false;
        
        instance.gameObject.GetComponent <BoxCollider> ().enabled = false;
        onEnd = () => {

        };
	}

	public void LoadScene < SceneController > (Actions.VoidAction beforeEnd = null) where SceneController : new () {

        if (sceneToLoad != "") {

            return;
        }

        gameObject.GetComponent <BoxCollider> ().enabled = true;

        alphaSpeed = 1f;
        maxAlpha = 1f;

        gameObject.GetComponent <Renderer> ().material.color = new Color (0,0,0,0);

        
		gameObject.transform.position = new Vector3 (Camera.main.transform.position.x,GUIController.layer + 0.5f,Camera.main.transform.position.z);
		sceneToLoad = "+";
		isAlphaIncreasing = true;
        
        if (beforeEnd == null) {

            beforeEnd = (a) => { 
                a ();
            };
        }

        onEnd = () => {
            

            beforeEnd ( () => {
                
                AudioController.instance.ClearSounds ();
			    new SceneController ();
                UpdateController.OnSceneLoaded ();
			    sceneToLoad = "";
		        
                OnSceneLoaded ();
            });
        };

	}
	void Start () {

		if (instance != null) {

			Destroy(gameObject);
			return;
		}

		instance = this;
		DontDestroyOnLoad (gameObject);
		isAlphaIncreasing = false;
		gameObject.transform.position = new Vector3 (0,GUIController.layer + 0.5f,0);
        onEnd = () => { };
	}
	
	void Update () {
	
        if (isJustSceneLoaded) {

            isJustSceneLoaded = false;
            return;
        }

		if (gameObject.GetComponent <Renderer> ().material.color.a >= maxAlpha && (sceneToLoad != "")) {

            gameObject.GetComponent <Renderer> ().material.color = new Color (0, 0, 0, maxAlpha);
            onEnd ();
		}
		
		if (isAlphaIncreasing && gameObject.GetComponent <Renderer> ().material.color.a < maxAlpha) {
            
            gameObject.transform.position = new Vector3 (Camera.main.transform.position.x,GUIController.layer + 0.5f,Camera.main.transform.position.z);
			gameObject.GetComponent <Renderer> ().material.color += new Color (0,0,0,Time.deltaTime*alphaSpeed);

		}
		
		if (!isAlphaIncreasing && gameObject.GetComponent <Renderer> ().material.color.a > 0) {
			
			gameObject.GetComponent <Renderer> ().material.color -= new Color (0,0,0,Time.deltaTime*alphaSpeed);
            gameObject.transform.position = new Vector3 (Camera.main.transform.position.x,GUIController.layer + 0.5f,Camera.main.transform.position.z);

			if (gameObject.GetComponent <Renderer> ().material.color.a <= 0) {

                onEnd ();
				gameObject.transform.position = new Vector3 (0,-10,0);
			}
		}

		                                                                                   
			
	}
}
