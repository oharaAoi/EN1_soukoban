using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// GemeObjectの追加
	public GameObject playerPrefab;
	public GameObject boxPrefab;


	// prefubではない
	public GameObject clearText;
	public GameObject goalsObj;

	// 配列の宣言
	int[,] map;
	GameObject[,] field;// ゲーム管理用の配列


	// Start is called before the first frame update
	void Start() {

		//GameObject instance = Instantiate(
		//	playerPrefab,			 // object
		//	new Vector3(0, 0, 0),	 // pos
		//	Quaternion.identity		 // rotate
		//);

		// 配列の実体と作成を初期化
		map = new int[,] {
			{0,0,0,0,0},
			{0,3,1,3,0},
			{0,0,2,0,0},
			{0,2,3,2,0},
			{0,0,0,0,0},
		};

		field = new GameObject[
			map.GetLength(0),
			map.GetLength(1)
		];

		string debugText = "";

		for (int row = 0; row < map.GetLength(0); row++) {
			for (int col = 0; col < map.GetLength(1); col++) {
				debugText += map[row, col].ToString() + ",";

				// mapの1(Player)を確認
				if (map[row, col] == 1) {
					field[row, col] = Instantiate(
						playerPrefab,            // object
						new Vector3(col - map.GetLength(1) / 2, map.GetLength(0) / 2 - row , 0),     // pos(カメラの中心に来るような処理をしている)
						//new Vector3(col, map.GetLength(0) -row , 0),     // pos
						Quaternion.identity      // rotate
					);
				}else if(map[row, col] == 2) {
					field[row, col] = Instantiate(
						boxPrefab,            // object
						new Vector3(col - map.GetLength(1) / 2, map.GetLength(0) / 2 - row, 0),     // pos
						//new Vector3(col, map.GetLength(0) -row , 0),     // pos
						Quaternion.identity      // rotate
					);
				}else if(map[row, col] == 3) {
					GameObject instantiate = Instantiate(
						goalsObj,
						new Vector3(col - map.GetLength(1) / 2, map.GetLength(0) / 2 - row, 0.01f),
						Quaternion.identity
					);
				}
			}
			// 改行
			debugText += "\n";
		}

		Debug.Log(debugText);

	}

	// Update is called once per frame
	void Update() {
		// ---------------------------------------------------------------
		// ↓　右
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			// playerの位置を取得
			Vector2Int playerIndex = GetPlayerIndex();
			Vector2Int playerIndexPlus =new Vector2Int ( playerIndex.x + 1, playerIndex.y );
			// 移動処理
			MoveNumber("Player", playerIndex, playerIndexPlus);

			// クリア判定
			if (IsCleard()) {
				clearText.SetActive(true);
			}
		}

		// ---------------------------------------------------------------
		// ↓ 左
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			// playerの位置を取得
			Vector2Int playerIndex = GetPlayerIndex();
			Vector2Int playerIndexPlus = new Vector2Int(playerIndex.x - 1, playerIndex.y);
			// 移動処理
			MoveNumber("Player", playerIndex, playerIndexPlus);

			// クリア判定
			if (IsCleard()) {
				clearText.SetActive(true);
			}
		}

		// ---------------------------------------------------------------
		// ↓　上
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			// playerの位置を取得
			Vector2Int playerIndex = GetPlayerIndex();
			Vector2Int playerIndexPlus = new Vector2Int(playerIndex.x, playerIndex.y - 1);
			// 移動処理
			MoveNumber("Player", playerIndex, playerIndexPlus);

			if (IsCleard()) {
				clearText.SetActive(true);
			}
		}

		// ---------------------------------------------------------------
		// ↓　下
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			// playerの位置を取得
			Vector2Int playerIndex = GetPlayerIndex();
			Vector2Int playerIndexPlus = new Vector2Int(playerIndex.x, playerIndex.y + 1);
			// 移動処理
			MoveNumber("Player", playerIndex, playerIndexPlus);

			// クリア判定
			if (IsCleard()) {
				clearText.SetActive(true);
			}
		}
	}

	//=================================================================================================================
	//	↓　1が入っている要素を返す関数
	//=================================================================================================================
	Vector2Int GetPlayerIndex() {
		for (int row = 0; row < map.GetLength(0); row++) {
			for (int col = 0; col < map.GetLength(1); col++) {
				// まずはnullチェック
				if (field[row, col] == null) {
					continue;
				}
				// nullでないのでタグチェック
				if (field[row, col].tag == "Player") {
					return new Vector2Int(col, row);
				}
			}
		}
		// 見つからなかったので-1を返す
		return new Vector2Int(-1, -1);
	}

	//=================================================================================================================
	//	↓　限界値かどうかを返す関数
	//=================================================================================================================
	bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo) {
		// 移動先が範囲外なら移送不可
		if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) {
			return false; // 動けない
		}

		if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) {
			return false; // 動けない
		}

		// 移動先に2(箱)があったら
		if (field[moveTo.y, moveTo.x] != null) {
			if (field[moveTo.y, moveTo.x].tag == "Box") {
				// 移動方向を計算(どこからどこまで)
				Vector2Int velocity = moveTo - moveFrom;
				// 箱の移動処理さらに先へ箱を進ませる
				// 移動先に箱があった時は箱も移動分進ませる
				bool success = MoveNumber(tag, moveTo, moveTo + velocity);
				// もし箱の移動が出来なかったら,プレイヤーも移動できない
				if (!success) {
					return false; 
				}
			}
		}

		// moveFromにあるゲームオブジェクトに座標を操作する
		field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x - map.GetLength(1) / 2, map.GetLength(0) - moveTo.y , 0);

		// 変わらない処理(移動して箱があったら再帰内で移動)
		field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
		field[moveFrom.y, moveFrom.x] = null;

		return true;
	}

	//=================================================================================================================
	//	↓　ゴール判定
	//=================================================================================================================
	bool IsCleard() {
		List<Vector2Int> goals = new List<Vector2Int>();

		// mapから3(ゴールの位置)を抽出する
		for(int row = 0; row < map.GetLength(0); row++) {
			for(int col = 0; col < map.GetLength(1); col++) {
				if (map[row, col] == 3) {
					goals.Add(new Vector2Int(col, row));
				}
			}
		}

		// 抽出した位置をfield配列に代入し、そこに箱があるかを判定
		for(int index = 0; index < goals.Count; index++) {
			GameObject clear = field[goals[index].y, goals[index].x];
			if(clear == null || clear.tag != "Box") {
				// 1つでもなかったらアウト
				return false;
			}
		}

		// 
		return true;
	}

}
