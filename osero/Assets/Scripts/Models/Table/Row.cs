using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

/// <summary>
///     ゲーム盤の行クラス
/// </summary>
public class Row : BaseModel {

    // ==================================================
    // プロパティ
    // ==================================================

    /// <summary>
    ///     この列が所属するゲーム盤
    /// </summary>
    public Table OwningTable { get; set; }

    /// <summary>
    ///     この列のゲーム盤上の位置
    /// </summary>
    public int? Index {
        get {

            if (OwningTable.Rows.Contains(this) == false) {
                return null;
            }

            return OwningTable.Rows.IndexOf(this);
        }
    }

    /// <summary>
    ///     行に設定されるセルのリスト
    /// </summary>
    public List<Cell> Cells = new List<Cell>();


    // ==================================================
    // コンストラクタ
    // ==================================================

    /// <summary>
    ///     コンストラクタ
    /// </summary>
    /// <param name="table"></param>
    public Row(Table table) {

        OwningTable = table;
    }
}
