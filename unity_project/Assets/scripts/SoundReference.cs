using UnityEngine;
using System.Collections;



public class SoundReference : MonoBehaviour {

	public AudioClip Done;

	public static SoundReference instance;
	void Awake(){
		instance = this;
		gameObject.AddComponent<AudioSource>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlaySound(AudioClip clip){
		audio.PlayOneShot(clip);
	}
}
