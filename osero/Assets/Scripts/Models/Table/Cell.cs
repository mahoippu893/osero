using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

/// <summary>
///     ゲーム盤のセルクラス
/// </summary>
public class Cell : BaseModel, ICreatableUnityInstance {

    // Prefabのファイルパス
    protected const string PREFAB_PATH = "Prefabs/Cell";
 
    private GameObject? _targetInstance;

    // ==================================================
    // プロパティ
    // ==================================================

    /// <summary>
    ///     このセルが所属するゲーム盤の列
    /// </summary>
    public Column OwningColumn { get; set; }

    /// <summary>
    ///     このセルが所属するゲーム盤の行
    /// </summary>
    public Row OwningRow { get; set; }

    /// <summary>
    ///     自分の上に配置されている石のインスタンス
    /// </summary>
    public Stone? Stone { get; set; }


    // ==================================================
    // コンストラクタ
    // ==================================================

    /// <summary>
    ///     コンストラクタ
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    public Cell(Column column, Row row) { 
    
        OwningColumn = column;
        OwningRow = row;
    }


    // ==================================================
    // Publicメソッド
    // ==================================================

    /// <summary>
    ///     石を置く
    /// </summary>
    public void PutStone(StoneColors color) {

        if (_targetInstance is null) {
            return;
        }

        var stone = new Stone(color);
        Stone = stone;

        stone.CreateUnityInstance(_targetInstance.transform.localScale, _targetInstance.transform.position, new Quaternion(0, 0, 0, 0));
    }

    /// <summary>
    ///     Unityのインスタンスを生成する
    /// </summary>
    /// <param name="size"></param>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    public void CreateUnityInstance(Vector3 size, Vector3 pos, Quaternion rot) {

        if (_targetInstance is not null) {
            DisposeUnityInstance();
        }

        _targetInstance = GameObject.Instantiate(Resources.Load<GameObject>(PREFAB_PATH), pos, rot);
        _targetInstance.transform.localScale = size;

        var view = _targetInstance.GetComponent<CellView>();
        view.RowIndex = OwningRow.Index;
        view.ColumnIndex = OwningColumn.Index;
    }

    /// <summary>
    ///     Unityのインスタンスを破壊する
    /// </summary>
    public void DisposeUnityInstance() {

        GameObject.Destroy(_targetInstance);
        _targetInstance = null;
    }
}
