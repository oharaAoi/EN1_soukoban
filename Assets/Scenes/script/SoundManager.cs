using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	public AudioClip soundClip; // 再生したい音声ファイル
	public AudioSource audioSource; // UI TextにアタッチされたAudioコンポーネント

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
		// スペースキーが押されたら音を再生する
		if (Input.GetKeyDown(KeyCode.Space)) {
			PlaySound();
		}
	}

    public void PlaySound() {
        audioSource.PlayOneShot(soundClip);
	}
}
