using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

/// <summary>
///     ゲーム盤の列クラス
/// </summary>
public class Column : BaseModel {

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

            if (OwningTable.Columns.Contains(this) == false) {
                return null;
            }

            return OwningTable.Columns.IndexOf(this);
        }
    }


    // ==================================================
    // コンストラクタ
    // ==================================================

    /// <summary>
    ///     コンストラクタ
    /// </summary>
    /// <param name="table"></param>
    public Column(Table table) {

        OwningTable = table;
    }
}
