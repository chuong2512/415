using UnityEngine;
using System.Collections.Generic;

public class MenusController {

    public static bool RemoveMenus () {

        return false;
    }

    public static List <Actions.VoidVoid> queue;

    public static void CreateQueuedMenu (Actions.VoidVoid creator) {

        queue.Add (creator);
        CheckMenusQueue ();
    }

    public static void RemoveQueuedMenu () {

        queue.RemoveAt (0);
        CheckMenusQueue ();
    }

    private static void CheckMenusQueue () {

        if (queue.Count == 1) {

            UpdateController.Timer (0.5f, () => {

                queue [0] ();
            });
        }
    }

    public MenusController () {

        queue = new List<Actions.VoidVoid> ();
    }

}
