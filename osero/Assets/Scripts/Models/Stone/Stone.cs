using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

#nullable enable

/// <summary>
///     石の色
/// </summary>
public enum StoneColors {

    Black,
    White
}

/// <summary>
///     オセロの石クラス
/// </summary>
public class Stone : BaseModel, ICreatableUnityInstance {

    // Prefabのファイルパス
    protected const string PREFAB_PATH = "Prefabs/Stone";

    private GameObject? _targetInstance;

    // ==================================================
    // プロパティ
    // ==================================================

    /// <summary>
    ///     現在の色
    /// </summary>
    public StoneColors Color { get; set; }


    // ==================================================
    // コンストラクタ
    // ==================================================

    /// <summary>
    ///     コンストラクタ
    /// </summary>
    public Stone(StoneColors color) {

        Color = color;
    }


    // ==================================================
    // Publicメソッド
    // ==================================================

    /// <summary>
    ///     ひっくり返る
    /// </summary>
    public void Reverse() {

        switch (Color) {
            case StoneColors.Black:
                Color = StoneColors.White; 
                break;
            case StoneColors.White:
                Color = StoneColors.Black;
                break;
        }

        // 表示色を変える
        if (_targetInstance is null) {
            return;
        }
        var view = _targetInstance.GetComponent<StoneView>();
        view.SetColor(Color);
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

        var view = _targetInstance.GetComponent<StoneView>();
        view.SetColor(Color);
    }

    /// <summary>
    ///     Unityのインスタンスを破壊する
    /// </summary>
    public void DisposeUnityInstance() {

        GameObject.Destroy(_targetInstance);
        _targetInstance = null;
    }
}
