using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour {

	// GemeObjectの追加
	public GameObject playerPrefab;
	public GameObject boxPrefab;
	public GameObject particlePrefab;
	public GameObject wallPrefab;

	// prefabではない
	public GameObject clearText;
	public GameObject goalsObj;
	public FadeSceneLoader fadeSceneLoader;

	//private StageMap stageMap;

	//private Stack<MoveState> moveStack = new Stack<MoveState>();
	// 配列の宣言
	int[,] map;
	GameObject[,] field;// ゲーム管理用の配列

	private Stack<GameObject[,]> fieldStack = new Stack<GameObject[,]>();

	// Start is called before the first frame update
	void Start() {

		// フルスクリーンにする
		Screen.SetResolution(1280, 720, true);

		// StageMap コンポーネントへの参照を取得
		StageMap stageMap = FindObjectOfType<StageMap>();

		map = stageMap.Map;

		// 複数のマップを初期化
		map = (new int[,] {
			{9,9,9,9,9,9,9},
			{9,1,0,0,0,0,9},
			{9,0,3,0,3,0,9},
			{9,0,0,2,0,0,9},
			{9,0,2,3,2,0,9},
			{9,0,0,0,0,0,9},
			{9,9,9,9,9,9,9},
		});

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
						new Vector3(col - map.GetLength(1) / 2, map.GetLength(0) / 2 - row, 0),     // pos(カメラの中心に来るような処理をしている)
																									//new Vector3(col, map.GetLength(0) -row , 0),     // pos
						Quaternion.LookRotation(new Vector3(0.0f, -1.0f, 0.0f), Vector3.back)      // rotate
					);
				} else if (map[row, col] == 2) {
					field[row, col] = Instantiate(
						boxPrefab,            // object
						new Vector3(col - map.GetLength(1) / 2, map.GetLength(0) / 2 - row, 0),     // pos
																									//new Vector3(col, map.GetLength(0) -row , 0),     // pos
						Quaternion.identity      // rotate
					);
				} else if (map[row, col] == 3) {
					GameObject instantiate = Instantiate(
						goalsObj,
						new Vector3(col - map.GetLength(1) / 2, map.GetLength(0) / 2 - row, 0.01f),
						Quaternion.identity
					);
				} else if (map[row, col] == 9) {
					field[row, col] = Instantiate(
						wallPrefab,
						new Vector3(col - map.GetLength(1) / 2, map.GetLength(0) / 2 - row, 0),
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
			Vector2Int playerIndexPlus = new Vector2Int(playerIndex.x + 1, playerIndex.y);
			// スタックに移動前の情報を格納しておく
			GameObject[,] fieldCopy = (GameObject[,])field.Clone();
			fieldStack.Push(fieldCopy);

			// 移動処理
			MoveNumber("Player", playerIndex, playerIndexPlus);

			// クリア判定
			if (IsCleared()) {
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
			// スタックに移動前の情報を格納しておく
			fieldStack.Push(field);
			// 移動処理
			MoveNumber("Player", playerIndex, playerIndexPlus);

			// クリア判定
			if (IsCleared()) {
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
			// スタックに移動前の情報を格納しておく
			fieldStack.Push(field);
			// 移動処理
			MoveNumber("Player", playerIndex, playerIndexPlus);

			if (IsCleared()) {
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
			// スタックに移動前の情報を格納しておく
			fieldStack.Push(field);
			// 移動処理
			MoveNumber("Player", playerIndex, playerIndexPlus);

			// クリア判定
			if (IsCleared()) {
				clearText.SetActive(true);
			}
		}

		if (IsCleared()) {
			fadeSceneLoader.isClear = true;
			fadeSceneLoader.CallCoroutine();
		}

		// ---------------------------------------------------------------
		// ↓　Undo
		// ---------------------------------------------------------------
		if (Input.GetKeyDown(KeyCode.Z)) {
			if (fieldStack.Count > 0) {
				if (field != null) {
					// field の要素を順番に破棄する
					foreach (GameObject obj in field) {
						if (obj != null) {
							// GameObject を破棄する
							Destroy(obj);
						}
					}

					// field を null に設定する
					field = null;
				}

				field = fieldStack.Pop();

				for (int row = 0; row < field.GetLength(0); row++) {
					for (int col = 0; col < field.GetLength(1); col++) {
						// fieldの要素に応じてオブジェクトを生成する
						if (field[row, col] == null) continue; // もしも要素がnullならば、何もしない

						if (field[row, col].tag == "Player") {
							field[row, col] = Instantiate(
								playerPrefab,
								new Vector3(col - field.GetLength(1) / 2, field.GetLength(0) / 2 - row, 0),
								Quaternion.LookRotation(new Vector3(0.0f, -1.0f, 0.0f), Vector3.back)
							);
						} else if (field[row, col].tag == "Box") {
							field[row, col] = Instantiate(
								boxPrefab,
								new Vector3(col - field.GetLength(1) / 2, field.GetLength(0) / 2 - row, 0),
								Quaternion.identity
							);
						} else if (field[row, col].tag == "Goal") {
							Instantiate(
								goalsObj,
								new Vector3(col - field.GetLength(1) / 2, field.GetLength(0) / 2 - row, 0.01f),
								Quaternion.identity
							);
						} else if (field[row, col].tag == "Wall") {
							field[row, col] = Instantiate(
								wallPrefab,
								new Vector3(col - field.GetLength(1) / 2, field.GetLength(0) / 2 - row, 0),
								Quaternion.identity
							);
						}
					}
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

			// 動く先が壁だったら
			if (field[moveTo.y, moveTo.x] != null) {
				if (field[moveTo.y, moveTo.x].tag == "Wall") {
					return false;
				}
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
			//field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x - map.GetLength(1) / 2, map.GetLength(0) - moveTo.y , 0);
			//field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x - map.GetLength(1) / 2, map.GetLength(0) / 2 - moveTo.y, 0);

			Vector3 moveFromPosition = new Vector3(moveFrom.x - map.GetLength(1) / 2, -moveFrom.y + map.GetLength(0) / 2, 0);
			Vector3 moveToPosition = new Vector3(moveTo.x - map.GetLength(1) / 2, -moveTo.y + map.GetLength(0) / 2, 0);
			field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);
			// particleの生成.
			CreateParticle(moveToPosition);

			// 変わらない処理(移動して箱があったら再帰内で移動)
			field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
			field[moveTo.y, moveTo.x].transform.rotation = Quaternion.LookRotation(moveToPosition - moveFromPosition, Vector3.back);
			field[moveFrom.y, moveFrom.x] = null;

			return true;
		}

		//=================================================================================================================
		//	↓　ゴール判定
		//=================================================================================================================
		bool IsCleared() {
			List<Vector2Int> goals = new List<Vector2Int>();

			// mapから3(ゴールの位置)を抽出する
			for (int row = 0; row < map.GetLength(0); row++) {
				for (int col = 0; col < map.GetLength(1); col++) {
					if (map[row, col] == 3) {
						goals.Add(new Vector2Int(col, row));
					}
				}
			}

			// 抽出した位置をfield配列に代入し、そこに箱があるかを判定
			for (int index = 0; index < goals.Count; index++) {
				GameObject clear = field[goals[index].y, goals[index].x];
				if (clear == null || clear.tag != "Box") {
					// 1つでもなかったらアウト
					return false;
				}
			}

			// 
			return true;
		}

		//=================================================================================================================
		//	↓　particleの生成
		//=================================================================================================================
		void CreateParticle(Vector3 position) {
			for (int oi = 0; oi < 7; oi++) {
				GameObject instance = Instantiate(
					particlePrefab,            // object
					position,                  // pos
					Quaternion.identity        // rotate
				);
			}
		}

		//=================================================================================================================
		//	↓　Fieldをコピーする
		//=================================================================================================================
		GameObject[,] CopyField(GameObject[,] original) {
			int rows = original.GetLength(0);
			int cols = original.GetLength(1);
			GameObject[,] copy = new GameObject[rows, cols];

			for (int i = 0; i < rows; i++) {
				for (int j = 0; j < cols; j++) {
					if (original[i, j] != null) {
						GameObject instance = Instantiate(original[i, j]);
						instance.SetActive(false);  // 非アクティブにする
						copy[i, j] = instance;
					} else {
						copy[i, j] = null;
					}
				}
			}
			return copy;
		}

	}
}

//===================================================================================
// 位置の情報を更新するためのクラス
//===================================================================================
//public class MoveState {
//	public Vector2Int oldPlayerPos;
//	public Vector2Int newPlayerPos;
//	public List<Vector2Int> oldBoxPos;
//	public List<Vector2Int> newBoxPos;

//	public MoveState(Vector2Int oldPlayerPos, Vector2Int newPlayerPos, List<Vector2Int> oldBoxPos, List<Vector2Int> newBoxPos) {
//		this.oldPlayerPos = oldPlayerPos;
//		this.newPlayerPos = newPlayerPos;
//		this.oldBoxPos = oldBoxPos;
//		this.newBoxPos = newBoxPos;
//	}
//};
