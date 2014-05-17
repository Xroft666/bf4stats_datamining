using UnityEngine;
using System.Collections;



public class SoundReference : MonoBehaviour {

	public AudioClip Done;

	public AudioClip SmallProgress;
	public AudioClip BigProgress;

	public static SoundReference instance;
	void Awake(){
		instance = this;
		gameObject.AddComponent<AudioSource>();
	}

	public void PlaySound(AudioClip clip){
		audio.PlayOneShot(clip);
	}
}
