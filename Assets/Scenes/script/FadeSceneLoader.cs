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
        fadePanel.enabled = true;   // パネルを有効か
        float elapsedTime = 0.0f;   // 経過時間を初期化
        Color startColor = fadePanel.color;     // フェードパネルの開始色を取得
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);

        // アニメーションを実行
        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
			fadePanel.color = Color.Lerp(startColor, endColor, t);
			yield return null;
		}

		fadePanel.color = endColor;  // フェードが完了したら最終色に設定
		SceneManager.LoadScene(nextSceneName); // シーンをロードしてメニューシーンに遷移
	}
}

