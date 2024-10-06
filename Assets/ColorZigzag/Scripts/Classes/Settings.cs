using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Settings
{
    public static string language
    {
        get
        {
            if (!PlayerPrefs.HasKey("Language"))
            {
                if (LanguageController.IsKnownLanguage(Application.systemLanguage.ToString()))
                {
                    PlayerPrefs.SetString("Language", Application.systemLanguage.ToString());
                }
                else
                {
                    if (LanguageController.IsKnownLanguage("English"))
                    {
                        PlayerPrefs.SetString("Language", "English");
                    }
                    else
                    {
                        PlayerPrefs.SetString("Language", LanguageController.languages[0]);
                    }
                }
            }

            return "English"; //PlayerPrefs.GetString ("Language"); 
        }

        set
        {
            PlayerPrefs.SetString("Language", value);
            GUIController.OnLanguageChange();
        }
    }

    public enum Scene
    {
        Menu,
        Game
    };

    public static Scene scene;

    public static bool isTutorial
    {
        get
        {
            if (!PlayerPrefs.HasKey("isTutorial"))
                return true;

            return (PlayerPrefs.GetInt("isTutorial") == 1);
        }
        set { PlayerPrefs.SetInt("isTutorial", value ? 1 : 0); }
    }

    public const string adsIdAndroid = "ca-app-pub-4782252445867445/3279633612";
    public const string bannerIdAndroid = "ca-app-pub-4782252445867445/9557347216";

    public static bool music
    {
        get
        {
            if (!PlayerPrefs.HasKey("music"))
                return true;

            return (PlayerPrefs.GetInt("music") == 1);
        }
        set
        {
            if (value)
                AudioController.instance.UnMuteMusic();
            else
                AudioController.instance.MuteMusic();

            PlayerPrefs.SetInt("music", value ? 1 : 0);
        }
    }

    public static bool sounds
    {
        get
        {
            if (!PlayerPrefs.HasKey("sounds"))
                return true;

            return (PlayerPrefs.GetInt("sounds") == 1);
        }
        set
        {
            if (value)
                AudioController.instance.UnMuteSounds();
            else
                AudioController.instance.MuteSounds();

            PlayerPrefs.SetInt("sounds", value ? 1 : 0);
        }
    }

    public static bool vibration
    {
        get
        {
            if (!PlayerPrefs.HasKey("vibration"))
                return false;

            return (PlayerPrefs.GetInt("vibration") == 1);
        }
        set { PlayerPrefs.SetInt("vibration", value ? 1 : 0); }
    }

    public static int money
    {
        get
        {
            if (!PlayerPrefs.HasKey("money"))
            {
                PlayerPrefs.SetInt("money", 600);
            }

            return PlayerPrefs.GetInt("money");
        }

        set
        {
            PlayerPrefs.SetInt("money", value);
            AGameManager.OnChangeCoin?.Invoke(0);
        }
    }

    public static bool isAds
    {
        get
        {
            if (!PlayerPrefs.HasKey("isAds"))
            {
                PlayerPrefs.SetInt("isAds", 1);
            }

            return PlayerPrefs.GetInt("isAds") == 1;
        }

        set { PlayerPrefs.SetInt("isAds", value ? 1 : 0); }
    }

    public static int bestScore
    {
        get
        {
            if (!PlayerPrefs.HasKey("bestScore"))
                return 0;

            return PlayerPrefs.GetInt("bestScore");
        }
        set { PlayerPrefs.SetInt("bestScore", Math.Max(value, bestScore)); }
    }

    public static string currentPrefabName
    {
        get
        {
            if (!PlayerPrefs.HasKey("currentPrefabName"))
                return "Sphere";

            return PlayerPrefs.GetString("currentPrefabName");
        }
        set { PlayerPrefs.SetString("currentPrefabName", value); }
    }

    public static int currentPrefab
    {
        get
        {
            if (!PlayerPrefs.HasKey("currentPrefab"))
                return 1;

            return PlayerPrefs.GetInt("currentPrefab");
        }
        set { PlayerPrefs.SetInt("currentPrefab", value); }
    }

    public static Vector3 Rotation(int prefabNumber)
    {
        switch (prefabNumber)
        {
            case 1:
                return new Vector3(0, 0, 0);
            case 2:
                return new Vector3(0, 0, 0);
            case 3:
                return new Vector3(0, 0, 0);
            case 4:
                return new Vector3(-90, 0, 0);
        }

        return new Vector3(0, 0, 0);
    }

    public static string likeURL = "https://www.facebook.com/";
    public static string rateURL = "https://www.play.google.com/";

    public static int level = 0;

    public static Actions.VoidVoid replayLevel;
    public static Actions.VoidVoid nextLevel;

    public static string GetText(string key)
    {
        return TranslationsController.GetText(key, language);
    }

    public Settings()
    {
    }

    public static bool IsOpenedTexture(string name)
    {
        if (!PlayerPrefs.HasKey("openedTexture_" + name))
            return false;

        return PlayerPrefs.GetInt("openedTexture_" + name) == 1;
    }

    public static void OpenTexture(string name)
    {
        PlayerPrefs.SetInt("openedTexture_" + name, 1);
    }

    public static bool AddSkillCount(string skill, int toAdd)
    {
        if (PlayerPrefs.HasKey(skill + "Count"))
        {
            if (PlayerPrefs.GetInt(skill + "Count") + toAdd >= 0)
            {
                PlayerPrefs.SetInt(skill + "Count", PlayerPrefs.GetInt(skill + "Count") + toAdd);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (toAdd < 0)
            {
                return false;
            }
            else
            {
                PlayerPrefs.SetInt(skill + "Count", toAdd);
                return true;
            }
        }
    }

    public static int GetSkillCount(string skill)
    {
        if (PlayerPrefs.HasKey(skill + "Count"))
        {
            return PlayerPrefs.GetInt(skill + "Count");
        }

        return 0;
    }

    public static void SetRecordWords(int words)
    {
        if (!PlayerPrefs.HasKey(level + "_words"))
        {
            PlayerPrefs.SetInt(level + "_words", words);
            return;
        }

        PlayerPrefs.SetInt(level + "_words", PlayerPrefs.GetInt(level + "_words") == 0
            ? words
            : Mathf.Min(words, PlayerPrefs.GetInt(level + "_words")));
    }

    public static void SetRecordPoints(int points)
    {
        if (!PlayerPrefs.HasKey(level + "_points"))
        {
            PlayerPrefs.SetInt(level + "_points", points);
            return;
        }

        PlayerPrefs.SetInt(level + "_points", Mathf.Max(points, PlayerPrefs.GetInt(level + "_points")));
    }

    private static string baseFontLetters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz[{\\|]}^~_#&</>";

    private static char GetRightChar(char c, ref string fontLetters)
    {
        for (int i = 0; i < fontLetters.Length; i++)
        {
            if (fontLetters[i] == c)
                return baseFontLetters[i];
        }

        return c;
    }

    public static string TranslateText(string text, bool isCustomLanguage = false, string customLanguage = "English")
    {
        string toUseLanguage = isCustomLanguage ? customLanguage : language;

        var fontLetters = LanguageController.LanguageFontLetters(toUseLanguage);
        string res = "";

        for (int i = 0; i < text.Length; i++)
        {
            res += GetRightChar(text[i], ref fontLetters);
        }

        return res;
    }
}