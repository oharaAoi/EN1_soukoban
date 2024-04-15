using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// 配列の宣言
	int[,] map;

	// GemeObjectの追加
	public GameObject playerPrefab;


	// Start is called before the first frame update
	void Start() {

		//GameObject instance = Instantiate(
		//	playerPrefab,			 // object
		//	new Vector3(0, 0, 0),	 // pos
		//	Quaternion.identity		 // rotate
		//);

		// 配列の実体と作成を初期化
		map = new int[,] {
			{1,0,0,0,0},
			{0,0,0,0,0},
			{0,0,0,0,0}
		};

		string debugText = "";

		for(int row = 0; row < map.GetLength(0); row++) {
			for(int col = 0; col < map.GetLength(1); col++) {
				debugText += map[row, col].ToString() + ",";

				// mapの1(Player)を確認
				if (map[row, col] == 1) {
					GameObject instance = Instantiate(
						playerPrefab,			 // object
						new Vector3(col, row, 0),	 // pos
						Quaternion.identity		 // rotate
					);
				}
			}
			// 改行
			debugText += "\n";
		}

		Debug.Log(debugText);

		//PrintArray();

	}

	// Update is called once per frame
	//void Update() {
	//	// ---------------------------------------------------------------
	//	// ↓　右
	//	// ---------------------------------------------------------------
	//	if (Input.GetKeyDown(KeyCode.RightArrow)) {
	//		// playerの位置を取得
	//		int playerIndex = GetPlayerIndex();
	//		// 移動処理
	//		MoveNumber(1, playerIndex, playerIndex + 1);
	//		// 表示する処理
	//		PrintArray();
	//	}

	//	// ---------------------------------------------------------------
	//	// ↓ 左
	//	// ---------------------------------------------------------------
	//	if (Input.GetKeyDown(KeyCode.LeftArrow)) {
	//		// playerの位置を取得
	//		int playerIndex = GetPlayerIndex();
	//		// 移動処理
	//		MoveNumber(1, playerIndex, playerIndex - 1);
	//		// 表示する処理
	//		PrintArray();
	//	}
	//}

	////=================================================================================================================
	////	↓　配列を表示する関数
	////=================================================================================================================
	//void PrintArray() {
	//	// 文字列の宣言と初期化
	//	string debugText = "";

	//	for (int oi = 0; oi < map.Length; oi++) {
	//		// 要素を出力
	//		debugText += map[oi].ToString() + ",";
	//	}

	//	// 結合した文字列を出力
	//	Debug.Log(debugText);
	//}

	////=================================================================================================================
	////	↓　1が入っている要素を返す関数
	////=================================================================================================================
	//int GetPlayerIndex() {
	//	for (int oi = 0; oi < map.Length; oi++) {
	//		if (map[oi] == 1) {
	//			return oi;
	//		}
	//	}

	//	return -1;
	//}

	////=================================================================================================================
	////	↓　限界値かどうかを返す関数
	////=================================================================================================================
	//bool MoveNumber(int number, int moveFrom, int moveTo) {
	//	// 移動先が範囲外なら移送不可
	//	if (moveTo < 0 || moveTo >= map.Length) {
	//		return false; // 動けない
	//	}

	//	// 移動先に2(箱)があったら
	//	if (map[moveTo] == 2) {
	//		// 移動方向を計算(どこからどこまで)
	//		int velocity = moveTo - moveFrom;
	//		// 箱の移動処理さらに先へ箱を進ませる
	//		// 移動先に箱があった時は箱も移動分進ませる
	//		bool success = MoveNumber(2, moveTo, moveTo + velocity);
	//		// もし箱の移動が出来なかったら,プレイヤーも移動できない
	//		if (!success) {
	//			return false;
	//		}
	//	}

	//	// 変わらない処理(移動して箱があったら再帰内で移動)
	//	map[moveTo] = number;
	//	map[moveFrom] = 0;
	//	return true;
	//}
}