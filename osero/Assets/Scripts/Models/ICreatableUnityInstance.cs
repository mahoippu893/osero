using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Unityのインスタンスを生成可能なクラスに実装するインターフェース
/// </summary>
public interface ICreatableUnityInstance {

    // ==================================================
    // プロパティ
    // ==================================================

    /// <summary>
    ///     Unityのインスタンスを生成する
    /// </summary>
    public void CreateUnityInstance(Vector3 size, Vector3 pos, Quaternion rot);

    /// <summary>
    ///     Unityのインスタンスを破壊する
    /// </summary>
    public void DisposeUnityInstance();
}
