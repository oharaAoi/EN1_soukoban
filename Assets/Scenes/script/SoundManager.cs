using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	public AudioClip soundClip; // �Đ������������t�@�C��
	public AudioSource audioSource; // UI Text�ɃA�^�b�`���ꂽAudio�R���|�[�l���g

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
		// �X�y�[�X�L�[�������ꂽ�特���Đ�����
		if (Input.GetKeyDown(KeyCode.Space)) {
			PlaySound();
		}
	}

    public void PlaySound() {
        audioSource.PlayOneShot(soundClip);
	}
}
