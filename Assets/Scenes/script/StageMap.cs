using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMap : MonoBehaviour
{
	int[,] map;
	private List<int[,]> maps = new List<int[,]>();

	int selectedStage = 0;

	public FadeSceneLoader fadeSceneLoader;

	// Start is called before the first frame update
	void Start()
    {
		// 複数のマップを初期化
		maps.Add(new int[,] {
			{9,9,9,9,9,9,9},
			{9,1,0,0,0,0,9},
			{9,0,3,0,3,0,9},
			{9,0,0,2,0,0,9},
			{9,0,2,3,2,0,9},
			{9,0,0,0,0,0,9},
			{9,9,9,9,9,9,9},
		});

		maps.Add(new int[,] {
			{9,9,9,9,9,9,9},
			{9,0,1,0,0,0,9},
			{9,0,3,0,3,0,9},
			{9,0,0,2,0,0,9},
			{9,0,2,3,2,0,9},
			{9,0,0,0,0,0,9},
			{9,9,9,9,9,9,9},
		});

		maps.Add(new int[,] {
			{9,9,9,9,9,9,9},
			{9,0,0,1,0,0,9},
			{9,0,3,0,3,0,9},
			{9,0,0,2,0,0,9},
			{9,0,2,3,2,0,9},
			{9,0,0,0,0,0,9},
			{9,9,9,9,9,9,9},
		});

		maps.Add(new int[,] {
			{9,9,9,9,9,9,9},
			{9,0,0,0,4,0,9},
			{9,0,3,0,3,0,9},
			{9,0,0,2,0,0,9},
			{9,0,2,3,2,0,9},
			{9,0,0,0,0,0,9},
			{9,9,9,9,9,9,9},
		});

		 // 選択されたステージ（例: 0 番目のステージ）
		//SelectStage(selectedStage);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	// map を公開するプロパティ
	public int[,] Map {
		get { return map; }
	}

	public void SelectStage(int bossID) {
		if (bossID > 0 && bossID < maps.Count) {
			selectedStage = bossID - 1;
			map = maps[bossID];
			fadeSceneLoader.CallCoroutine();
		} else {
			Debug.LogError("ステージインデックスが範囲外です");
		}
	}
}
