using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioController : MonoBehaviour {

	public static AudioController instance = null;

	private List <GameObject> sounds;
	private List <GameObject> musics;

	public GameObject CreateAudio (string name, bool isLoop = false, bool isMusic = false) {

		var audio = GamePullController.CreateAudio ();

        audio.GetComponent <AudioSource> ().clip = ResourcesController.Load ("Audio/" + name) as AudioClip;
		audio.GetComponent <AudioSource> ().loop = isLoop;
		audio.GetComponent <AudioSource> ().Play ();

		(isMusic?musics:sounds).Add (audio);

        audio.GetComponent <AudioSource> ().mute = (isMusic && !Settings.music) || (!isMusic && !Settings.sounds);

        if (isMusic) {

            audio.GetComponent <AudioSource> ().volume = 0;
        }

		return audio;
	}
    
	public void RemoveAudio (GameObject audio) {

		musics.Remove (audio);
		sounds.Remove (audio);
		GamePullController.DestroyAudio (audio);
	}
    
	public void PauseAudio (GameObject audio) {

		audio.GetComponent <AudioSource> ().Pause ();
	}

	public void UnPauseAudio (GameObject audio) {

		audio.GetComponent <AudioSource> ().UnPause ();
	}
    
    public void MuteSounds () {

        foreach (var s in sounds) {

            s.GetComponent <AudioSource> ().mute = true;
        }
    }

    public void MuteMusic () {

        foreach (var s in musics) {

            s.GetComponent <AudioSource> ().mute = true;
        }
    }
    
    public void UnMuteSounds () {

        foreach (var s in sounds) {

            s.GetComponent <AudioSource> ().mute = false;
        }
    }
    
    public void UnMuteMusic () {

        foreach (var s in musics) {

            s.GetComponent <AudioSource> ().mute = false;
        }
    }

    
    public void ClearSounds () {

        foreach (var s in sounds) {

            GamePullController.DestroyAudio (s);
        }

        sounds.Clear ();
    }

	void Start () {
		
		if (instance != null) {
			
			Destroy(gameObject);
			return;
		}
		sounds = new List<GameObject> ();
		musics = new List<GameObject> ();
		instance = this;
	}

    void Update () {

        for (int i = musics.Count - 1; i >= 0; i--) {

            if (musics [i].GetComponent <AudioSource> ().volume < 1) {

                musics [i].GetComponent <AudioSource> ().volume += Mathf.Min (Time.deltaTime, 0.2f) / 4f;
            }

            if ((musics [i].GetComponent <AudioSource> ().time == musics [i].GetComponent <AudioSource> ().clip.length) 
                && !musics [i].GetComponent <AudioSource> ().isPlaying) {

                GamePullController.DestroyAudio (musics[i]);
                musics.RemoveAt (i);
            }
        }

        for (int i = sounds.Count - 1; i >= 0; i--) {

            if ((sounds [i].GetComponent <AudioSource> ().time == sounds [i].GetComponent <AudioSource> ().clip.length) 
                && !sounds [i].GetComponent <AudioSource> ().isPlaying) {

                GamePullController.DestroyAudio (sounds[i]);
                sounds.RemoveAt (i);
            }
        }

    }

}
