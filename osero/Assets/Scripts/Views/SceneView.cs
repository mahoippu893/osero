using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

/// <summary>
///     シーンにアタッチするクラス
/// </summary>
public class SeceneView : MonoBehaviour {

    // ==================================================
    // Eventハンドラ
    // ==================================================

    /// <summary>
    ///     読み込み時の処理
    /// </summary>
    private void Start() {

        OseroController.GetInstance().StartGame();
    }
}
