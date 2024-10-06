using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdateController : MonoBehaviour {

    public static UpdateController instance;

    private const int version = 1;

    public static Actions.VoidFloat toUpdate;
    public static Actions.VoidFloat toFixedUpdate;
    
    public GameObject mainCamera;
    public GameObject gameCamera;
    public static GameObject theme;
    
    private static Dictionary <string, Actions.VoidFloat> updatables;
    private static Dictionary <string, Actions.VoidFloat> fixedUpdatables;

    private static List <string> updatablesToRemove;
    private static List <string> fixedUpdatablesToRemove;

    public static float timeSinceLevelLoad {

        get {return time - lastSceneLoadTime;}
    }
    
    public static float time {

        get {return Time.time;}
    }

    private static float lastSceneLoadTime = 0;

    public static void OnSceneLoaded () {

        lastSceneLoadTime = Time.time;
        CameraController.cameraPosition = new Vector2 (0, 0);
    }

    private IEnumerator IETimer (float time, Actions.VoidVoid func) {

        yield return new WaitForSeconds (time);

        func ();
    }

    private void _Timer (float time, Actions.VoidVoid func) {

        StartCoroutine (IETimer (time, func));
    }

    public static void Timer (float time, Actions.VoidVoid func) {

        instance._Timer (time, func);
    }

    public static void StopAllTimers () {

        instance.StopAllCoroutines ();
        fixedUpdatables = new Dictionary<string, Actions.VoidFloat> ();
        updatables = new Dictionary<string, Actions.VoidFloat> ();
    }

    private IEnumerator IELaunchIt (float count, float deltaTime, Actions.VoidInt func, Actions.VoidVoid onEnd) {

        for (int i = 0; i < count; i++) {

            func (i);
            yield return new WaitForSeconds (deltaTime);
        }

        onEnd ();
    }

    private void _LanunchIt (float count, float deltaTime, Actions.VoidInt func, Actions.VoidVoid onEnd) {

        StartCoroutine (IELaunchIt (count, deltaTime, func, onEnd));
    }

    public static void LaunchIt (float count, float deltaTime, Actions.VoidInt func, Actions.VoidVoid onEnd = null) {

        instance._LanunchIt (count, deltaTime, func, onEnd == null ? () => { } : onEnd);
    }

    public static void AddUpdatable (string key, Actions.VoidFloat value) {

        if (updatables.ContainsKey (key)) {

            Debug.LogWarning ("Reset Updatable");
            updatables [key] = value;
        }
        updatables.Add (key, value);
    }

    public static void AddFixedUpdatable (string key, Actions.VoidFloat value) {

        if (fixedUpdatables.ContainsKey (key)) {

            Debug.LogWarning ("Reset FixedUpdatable");
            fixedUpdatables [key] = value;
        } else {

            fixedUpdatables.Add (key, value);
        }
    }
    
    public static void RemoveUpdatable (string key) {

        updatablesToRemove.Add (key);
    }

    public static void RemoveFixedUpdatable (string key) {

        fixedUpdatablesToRemove.Add (key);
    }
    
    public static void SetMainCamera () {
        
        instance.mainCamera.GetComponent <Camera> ().enabled = true;
        instance.gameCamera.GetComponent <Camera> ().enabled = false;
    }

    public static void SetGameCamera () {
        
        instance.mainCamera.GetComponent <Camera> ().enabled = false;
        instance.gameCamera.GetComponent <Camera> ().enabled = true;
    }
    
    public static bool IsMainCamera () {

        return instance.mainCamera.GetComponent <Camera> ().enabled;
    }

    public static bool IsGameCamera () {

        return instance.mainCamera.GetComponent <Camera> ().enabled;
    }

	void Start () {

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
	    
		#if UNITY_IPHONE
		        GameController.platform = GameController.Platform.Phone;
        #endif

        #if UNITY_ANDROID
		        GameController.platform = GameController.Platform.Phone;
        #endif

        #if UNITY_STANDALONE_OSX
		        GameController.platform = GameController.Platform.PC;
        #endif

        #if UNITY_STANDALONE_WIN
		        GameController.platform = GameController.Platform.PC;
        #endif

        #if UNITY_EDITOR
		        GameController.platform = GameController.Platform.PC;
        #endif	

        instance = this;

        updatables = new Dictionary<string, Actions.VoidFloat> ();
        fixedUpdatables = new Dictionary<string, Actions.VoidFloat> ();
        
        updatablesToRemove = new List<string> ();
        fixedUpdatablesToRemove = new List<string> ();

		new GamePullController ();
        new LanguageController ();
        new Settings ();
        new TranslationsController ();
        new MenusController ();
        new ResourcesController ();
        new AdsController ();
        new IAPController ();
        new GameController ();

        new GooglePlayServicesController (false, new Dictionary<string, string> () {

            {"ReachScore10", "CgkI_P6_-d8UEAIQAA"}
            , {"ReachScore20", "CgkI_P6_-d8UEAIQAQ"}
            , {"OpenShop", "CgkI_P6_-d8UEAIQAg"}
            , {"PressPlayButton", "CgkI_P6_-d8UEAIQAw"}
            , {"MuteSounds", "CgkI_P6_-d8UEAIQBA"}


        }, () => {

        });
        
        Timer (2, () => {

            AdsController.instance.ShowBanner ();
        });
	}

    void FixedUpdate () {

        if (toFixedUpdate != null) {

            toFixedUpdate (Time.fixedDeltaTime);
        }

        foreach (var f in fixedUpdatables) {

            f.Value (Time.fixedDeltaTime);
        }

        foreach (var f in fixedUpdatablesToRemove) {

            fixedUpdatables.Remove (f);
        }

        fixedUpdatablesToRemove.Clear ();
    }

	void Update () {
	
        if (toUpdate != null) {

            toUpdate (Time.deltaTime);
        }

        foreach (var u in updatables) {

            u.Value (Time.deltaTime);
        }

        foreach (var u in updatablesToRemove) {

            updatables.Remove (u);
        }

        updatablesToRemove.Clear ();
        
	}
}
