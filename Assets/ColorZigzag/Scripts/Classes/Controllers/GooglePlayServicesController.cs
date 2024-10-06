using UnityEngine;
using System.Collections.Generic;

public class GooglePlayServicesController {

    public static GooglePlayServicesController instance;

    private static Dictionary <string, string> achievements;

    public static void Authenticate (Actions.VoidVoid onSuccess = null, Actions.VoidVoid onFail = null) {

        Social.localUser.Authenticate ((bool success) => {

            Debug.Log ("Authenticate :" + success);

            if (success) {

                if (onSuccess != null) {

                    onSuccess ();
                }
            } else {

                if (onFail != null) {

                    onFail ();
                }
            }

        });
    }

    private static void CheckInstance () {

        if (instance == null) {

            new GooglePlayServicesController (true);
        }
    }

    private static bool CheckAuthentification () {

        CheckInstance ();

        if (!Social.localUser.authenticated) {

            Authenticate ();
        }

        return true;
    }

    public static void LoadAchievements (Actions.VoidVoid onEnd) {
        
        if (!CheckAuthentification ()) {

            return;
        }

        Social.LoadAchievements ((f) => {

            Debug.Log (f);

            foreach (var a in f) {

                Debug.Log ("LoadAchievements: " + a.id);
            }
        });
    }

    public static void LoadAchievementDescriptions (Actions.VoidVoid onEnd) {
        
        CheckAuthentification ();

        Social.LoadAchievementDescriptions ((g) => {

            foreach (var a in g) {

                Debug.Log ("LoadAchievementDescriptions: " + a.id + " " + a.title);
                
                if (!achievements.ContainsValue (a.id)) {

                    achievements.Add (a.title, a.id);
                }
            }
        });
    }

    /// <summary>
    /// Show achievements UI
    /// </summary>
    public static void Showachievements () {

        if (!CheckAuthentification ()) {

            return;
        }

        Social.ShowAchievementsUI ();
    }

    /// <summary>
    /// Report progress for an achievement
    /// </summary>
    /// <param name="achievement">achievement name in the dictionary</param>
    /// <param name="toAdd">0 .. 100</param>
    /// <param name="onSuccess"></param>
    /// <param name="onFail"></param>
    public static void ReportProgress (string achievement, float toAdd, Actions.VoidVoid onSuccess = null, Actions.VoidVoid onFail = null) {
        
        if (!CheckAuthentification ()) {

            return;
        }

        if (!achievements.ContainsKey (achievement)) {

            Debug.Log ("Unknown achievement");
            onFail ();
            return;
        }

        Social.ReportProgress (achievements [achievement], toAdd, (h) => {

            if (h) {

                if (onSuccess != null) {

                    onSuccess ();
                }
            } else {

                if (onFail != null) {

                    onFail ();
                }
            }
        });
    }

    /// <param name="isMinimal">Authentificate and load data if false</param>
    /// <param name="_achievements">Dictionary name - id. Use name to call RportProgress</param>
    /// <param name="onLoad"></param>
    public GooglePlayServicesController (bool isMinimal = true, Dictionary <string, string> _achievements = null, Actions.VoidVoid onLoad = null) {

        instance = this;

        #if UNITY_ANDROID
		   //GooglePlayGames.PlayGamesPlatform.Activate();     
        #endif

        if (!isMinimal) {

            if (_achievements == null) {

                achievements = new Dictionary<string, string> ();
            } else {

                achievements = _achievements;
            }

            Authenticate (() => {

                LoadAchievements (() => {

                    LoadAchievementDescriptions (() => {

                        if (onLoad != null) {

                            onLoad ();
                        }
                    });
                });
            });
        }
    }

}
