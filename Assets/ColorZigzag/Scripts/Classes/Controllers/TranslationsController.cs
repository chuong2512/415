using UnityEngine;
using System.Collections.Generic;

public class TranslationsController {

	public static TranslationsController instance;
    
    private static Dictionary <string, string> textTranslations;

    public static string GetText (string key, string language) {

        if (textTranslations == null) {

            Debug.LogError ("No translations");
            return "null";
        }

        if (textTranslations.ContainsKey (key + "_" +language)) {

            return textTranslations [key + "_" +language];
        }

        if (key == "null") {

            Debug.LogError ("No null translations");
            return "null";
        }

        Debug.LogWarning ("Haven't translated: " + key);
        return GetText ("null", language);
    }

    public TranslationsController () {

        if (instance != null) {

            return;
        }

        instance = this;
        textTranslations = new Dictionary<string, string> ();

        var text = (ResourcesController.LoadOnce ("Translations") as TextAsset).text;

        var splitted = text.Split ('☻');

        var languages = splitted [0].Split ('☺');

        string [] currentList;

        for (int i = 1; i < splitted.Length; i++) {

            currentList = splitted [i].Split ('☺');

            if (currentList.Length < languages.Length) {

                continue;
            }

            for (int q = 0; q < languages.Length; q++) {

                textTranslations.Add (currentList [0] + "_" + languages [q], currentList [q + 1]);
            }
        }

    }
}
