using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickSound : MonoBehaviour {

    public AudioClip sound;

    protected AudioSource source;
    protected Button button { get { return GetComponent<Button>(); } }

	public void Start () {
        source = Camera.main.GetComponent<AudioSource>();

        button.onClick.AddListener(() => PlaySound());
	}
	
	void PlaySound() {
        source.PlayOneShot(sound, 1f);
    }
}
