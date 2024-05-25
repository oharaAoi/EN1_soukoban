using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSceneLoader : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 1.0f;
    public string nextSceneName;
    public bool isClear = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void CallCoroutine() {
		StartCoroutine(FadeOutAndLoadScene());
	}

	public IEnumerator FadeOutAndLoadScene() {
        fadePanel.enabled = true;   // �p�l����L����
        float elapsedTime = 0.0f;   // �o�ߎ��Ԃ�������
        Color startColor = fadePanel.color;     // �t�F�[�h�p�l���̊J�n�F���擾
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);

        // �A�j���[�V���������s
        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
			fadePanel.color = Color.Lerp(startColor, endColor, t);
			yield return null;
		}

		fadePanel.color = endColor;  // �t�F�[�h������������ŏI�F�ɐݒ�
		SceneManager.LoadScene(nextSceneName); // �V�[�������[�h���ă��j���[�V�[���ɑJ��
	}
}

