using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainSound : MonoBehaviour
{
	// サウンドを止めるシーン名
	//public string sceneToStopSound;

	private AudioSource audioSource;

	// Start is called before the first frame update
	void Start()
    {
		DontDestroyOnLoad(this);
		audioSource = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update()
    {
       
    }

	public void StopSound() {
		if(audioSource != null && audioSource.isPlaying) {
			audioSource.Stop();
		}
	}

	public void PlaySound() {
		if (audioSource != null && !audioSource.isPlaying) {
			audioSource.Play();
		}
	}
}
