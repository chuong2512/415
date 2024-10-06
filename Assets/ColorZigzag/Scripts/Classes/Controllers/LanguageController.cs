using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanguageController {

    public static LanguageController instance;
    
	public static List <string> languages;
	public static List <string> languagesSelfNames;
    
    private static List < List <char> > languageLetters;
    private static List < List <int> > languageLetterChances;
    private static List < List <string> > languageGameLetters;
    private static List <string> languageFontLetters;

    public static string NextLanguage (string language) {

        return languages [(languages.FindIndex ((a) => a == language) + 1) % languages.Count];
    }

    public static string PreviousLanguage (string language) {

        return languages [(languages.FindIndex ((a) => a == language) + languages.Count - 1) % languages.Count];
    }

    public static List <char> LanguageLetters (string language) {

        return languageLetters [languages.FindIndex ((a) => a == language)];
    }

    public static List <int> LanguageLetterChances (string language) {

        return languageLetterChances [languages.FindIndex ((a) => a == language)];
    }
    
    public static List <string> LanguageGameLetters (string language) {

        return languageGameLetters [languages.FindIndex ((a) => a == language)];
    }
    
    public static string LanguageFontLetters (string language) {

        return languageFontLetters [languages.FindIndex ((a) => a == language)];
    }

    public LanguageController () {
        
        if (instance != null) {
        
            return;
        }
        
        instance = this;

        languages = new List<string> ();
        languagesSelfNames = new List<string> ();
        languageLetters = new List<List<char>> ();
        languageLetterChances = new List<List<int>> ();
        languageGameLetters = new List<List<string>> ();
        languageFontLetters = new List<string> ();

        var init = (ResourcesController.LoadOnce ("Languages") as TextAsset).text.Split ('\n');

        for (int i = 0; i + 4 < init.Length; i += 5) {

            var languageNames = init [i + 0].Split (' ');

            languages.Add (languageNames [0]);
            languagesSelfNames.Add (languageNames [0]);

            var letters = init [i + 1].Split (' ');
            List <char> resultLetters = new List <char> ();

            for (int q = 0; q < letters.Length; q++) {

                resultLetters.Add (letters [q] [0]);
            }

            languageLetters.Add (resultLetters);

            var chances = init [i + 2].Split (' ');
            List <int> resultChances = new List <int> ();

            for (int q = 0; q < chances.Length; q++) {

                resultChances.Add (int.Parse (chances [q]));
            }

            languageLetterChances.Add (resultChances);

            var gameLetters = init [i + 3].Split (' ');

            Debug.Log (gameLetters [gameLetters.Length - 1]);

            languageGameLetters.Add (new List<string> (gameLetters));

            languageFontLetters.Add (init [i + 4]);

        }

        if (languages.Count == 0) {

            Debug.LogError ("No languages");
        }
    }

    public static bool IsKnownLanguage (string language) {

        return languages.Contains (language);
    }
}
