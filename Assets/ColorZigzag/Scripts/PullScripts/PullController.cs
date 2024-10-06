using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PullController {

	private static Vector3 deadPosition = new Vector3 (0,-10,0);
	private static Vector3 deadRotation = new Vector3 (90,0,0);

	private static Dictionary < string, List <GameObject> > pull = new Dictionary<string, List<GameObject>> ();
	private static List <GameObject> tempGameObjectList;
	private static GameObject tempGameObject;

    private static List <GameObject> registry = new List<GameObject> ();

	public static void Create () {

		var newPull = new Dictionary<string, List<GameObject>> ();
        registry = new List<GameObject> ();

        foreach (var list in pull) {

            if (list.Key.Contains ("Undestroyable_asdasd")) {

                newPull.Add(list.Key, list.Value);

                foreach (var go in list.Value) {
                    
                    registry.Add (go);
                }
            }
        }

        pull = newPull;
	}

	public static GameObject GetObject(string type) {

		if (pull.TryGetValue(type, out tempGameObjectList)) {

			if (tempGameObjectList.Count > 0) {

				tempGameObject = tempGameObjectList[tempGameObjectList.Count-1]; 
				tempGameObjectList.Remove(tempGameObject);
				tempGameObject.SetActive(true);

                registry.Remove (tempGameObject);

				return tempGameObject;
			}
		}

		return null;
	}

	public static void AddObject(string type, GameObject gameObject) {

        if (registry.Contains (gameObject)) {

            Debug.LogWarning ("Pull already has '" + gameObject.name + "' inside");
            return;
        } else {

            registry.Add (gameObject);
        }

		gameObject.transform.position = deadPosition;
		gameObject.transform.rotation = Quaternion.Euler (deadRotation);
		gameObject.SetActive(false);

		if (pull.TryGetValue(type, out tempGameObjectList)) {

            if (!tempGameObjectList.Contains (gameObject)) {

                tempGameObjectList.Add(gameObject);
            }
		} else {
			pull[type] = new List<GameObject> ();
			pull[type].Add(gameObject);
		}
	}

}
