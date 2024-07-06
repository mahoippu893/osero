using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

/// <summary>
///     セルのプレハブにアタッチするクラス
/// </summary>
public class CellView : MonoBehaviour {

    // ==================================================
    // Publicメソッド
    // ==================================================

    /// <summary>
    ///     列のインデックス
    /// </summary>
    [SerializeField]
    public int? ColumnIndex { get; set; } = 0;

    /// <summary>
    ///     行のインデックス
    /// </summary>
    [SerializeField]
    public int? RowIndex { get; set; } = 0;

    // ==================================================
    // Eventハンドラ
    // ==================================================

    /// <summary>
    ///     クリックされたときの処理
    /// </summary>
    public void OnClick() {

        if (ColumnIndex is null) {
            return;
        }
        if (RowIndex is null) {
            return;
        }

        OseroController.GetInstance().PutStone((int)ColumnIndex, (int)RowIndex);
    }
}
