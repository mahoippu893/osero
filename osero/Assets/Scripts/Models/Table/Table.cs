using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

#nullable enable

/// <summary>
///     ゲーム盤クラス
/// </summary>
public class Table : BaseModel {

    protected const int TABLE_CELL_SIZE = 30;

    // ==================================================
    // プロパティ
    // ==================================================

    /// <summary>
    ///     ゲーム盤に設定される列のリスト
    /// </summary>
    public List<Column> Columns { get; set; } = new List<Column>();

    /// <summary>
    ///     ゲーム盤に設定される行のリスト
    /// </summary>
    public List<Row> Rows { get; set; } = new List<Row>();


    // ==================================================
    // Publicメソッド
    // ==================================================

    /// <summary>
    ///     ゲーム盤を初期化する
    /// </summary>
    /// <param name="indexCount"></param>
    public void InitializeTable(int indexCount) {


        // 今の状態の破棄
        foreach (var row in Rows) {

            foreach (var cell in row.Cells) {

                if (cell.Stone is not null) {
                    cell.Stone.DisposeUnityInstance();
                }

                cell.DisposeUnityInstance();
            }

            row.Cells.Clear();
        }
        Rows.Clear();
        Columns.Clear();


        // ------------------------


        // 列の生成
        for (int i = 0; i < indexCount; i++) {

            var column = new Column(this);
            Columns.Add(column);
        }

        // 列の生成
        for (int i = 0; i < indexCount; i++) {

            var row = new   Row(this);
            Rows.Add(row);

            // セルの生成
            foreach (var column in Columns) {

                var cell = new Cell(column, row);
                row.Cells.Add(cell);

                // Unityインスタンスを生成する
                cell.CreateUnityInstance(
                    new Vector3(TABLE_CELL_SIZE, TABLE_CELL_SIZE, 0),
                    new Vector3(((column.Index is null) ? 0 : (int)column.Index) * TABLE_CELL_SIZE, -i * TABLE_CELL_SIZE, 0),
                    new Quaternion(0, 0, 0, 0));
            }
        }
    }


    /// <summary>
    ///     石を置く
    /// </summary>
    public void PutStone(int columnIndex, int rowIndex, StoneColors color) {

        // 入力値のエラー制御
        if (columnIndex < 0) {
            return;
        }
        if (rowIndex < 0) {
            return;
        }

        if (Columns.Count <= columnIndex) {
            return;
        }
        if (Rows.Count <= rowIndex) {
            return;
        }


        // 対象セルに石を置く
        var targetCell = Rows[rowIndex].Cells[columnIndex];
        targetCell.PutStone(color);


        // ------------------------
        // ひっくり返す処理
        // ------------------------

        // 上方向
        ReverseTop(columnIndex, rowIndex, color);

        // 下方向
        ReverseBottom(columnIndex, rowIndex, color);

        // 右方向
        ReverseRight(columnIndex, rowIndex, color);

        // 左方向
        ReverseLeft(columnIndex, rowIndex, color);

        // 左上方向
        ReverseTopLeft(columnIndex, rowIndex, color);

        // 右上方向
        ReverseTopRight(columnIndex, rowIndex, color);

        // 左下方向
        ReverseBottomLeft(columnIndex, rowIndex, color);

        // 右下方向
        ReverseBottomRight(columnIndex, rowIndex, color);
    }


    /// <summary>
    ///     指定のセルに石がすでに置かれているかを判定する
    /// </summary>
    /// <returns></returns>
    public bool IsSetStoneCell(int columnIndex, int rowIndex) {

        var targetRow = Rows.FirstOrDefault(x => x.Index == rowIndex);
        if (targetRow is null) {
            return true;
        }

        var targetCell = targetRow.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
        if (targetCell is null) {
            return true;
        }

        return (targetCell.Stone is not null);
    }

    /// <summary>
    ///     まだスペースがあるかどうかを判定する
    /// </summary>
    /// <returns></returns>
    public bool HasSpace() {

        return (Rows.Sum(x => x.Cells.Count(x => x.Stone is null)) > 0);
    }

    /// <summary>
    ///     指定セルの隣に石が置かれているかを判定する
    /// </summary>
    /// <returns></returns>
    public bool IsSetNextStoneCell(int columnIndex, int rowIndex) {

        var targetRow = Rows.FirstOrDefault(x => x.Index == rowIndex);
        if (targetRow is null) {
            return false;
        }

        // 左
        if (targetRow.Cells.Count(x => x.OwningColumn.Index == columnIndex - 1 && x.Stone is not null) > 0) {
            return true;
        }

        // 右
        if (targetRow.Cells.Count(x => x.OwningColumn.Index == columnIndex + 1 && x.Stone is not null) > 0) {
            return true;
        }

        // ---------

        var topRow = Rows.FirstOrDefault(x => x.Index == rowIndex - 1);
        if (topRow is not null) {

            // 上
            if (topRow.Cells.Count(x => x.OwningColumn.Index == columnIndex && x.Stone is not null) > 0) {
                return true;
            }

            // 右上
            if (topRow.Cells.Count(x => x.OwningColumn.Index == columnIndex + 1 && x.Stone is not null) > 0) {
                return true;
            }

            // 左上
            if (topRow.Cells.Count(x => x.OwningColumn.Index == columnIndex - 1 && x.Stone is not null) > 0) {
                return true;
            }
        }

        // ---------

        var bottomRow = Rows.FirstOrDefault(x => x.Index == rowIndex + 1);
        if (bottomRow is not null) {

            // 下
            if (bottomRow.Cells.Count(x => x.OwningColumn.Index == columnIndex && x.Stone is not null) > 0) {
                return true;
            }

            // 右下
            if (bottomRow.Cells.Count(x => x.OwningColumn.Index == columnIndex + 1 && x.Stone is not null) > 0) {
                return true;
            }

            // 左下
            if (bottomRow.Cells.Count(x => x.OwningColumn.Index == columnIndex - 1 && x.Stone is not null) > 0) {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     その石を置くことで一つでもひっくり返るかを判定する
    /// </summary>
    /// <returns></returns>
    public bool IsReverseIfSetStone(int columnIndex, int rowIndex, StoneColors color) {

        // 上方向
        if (CheckReverseTop(columnIndex, rowIndex, color)) {
            return true;
        }

        // 下方向
        if (CheckReverseBottom(columnIndex, rowIndex, color)) {
            return true;
        }

        // 右方向
        if (CheckReverseRight(columnIndex, rowIndex, color)) {
            return true;
        }

        // 左方向
        if (CheckReverseLeft(columnIndex, rowIndex, color)) {
            return true;
        }

        // 左上方向
        if (CheckReverseTopLeft(columnIndex, rowIndex, color)) {
            return true;
        }

        // 右上方向
        if (CheckReverseTopRight(columnIndex, rowIndex, color)) {
            return true;
        }

        // 左下方向
        if (CheckReverseBottomLeft(columnIndex, rowIndex, color)) {
            return true;
        }

        // 右下方向
        if (CheckReverseBottomRight(columnIndex, rowIndex, color)) {
            return true;
        }

        return false;
    }

    /// <summary>
    ///     指定色を配置可能かを判定する
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public bool IsSetableColor(StoneColors color) {

        // 全セルを探索する
        foreach (var row in Rows) {
            foreach (var cell in row.Cells.Where(x => x.Stone is null)) {

                if (cell.OwningColumn.Index is null) {
                    continue;
                }
                if (cell.OwningRow.Index is null) {
                    continue;
                }

                // 隣に石があるか
                if (IsSetNextStoneCell((int)cell.OwningColumn.Index, (int)cell.OwningRow.Index) == false) {
                    continue;
                }

                // 石を置いたら反転するか
                if (IsReverseIfSetStone((int)cell.OwningColumn.Index, (int)cell.OwningRow.Index, color)) {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    ///     対象の色の石の配置数を計算する
    /// </summary>
    /// <returns></returns>
    public int CountStoneColor(StoneColors color) {

        return (Rows.Sum(x => x.Cells.Count(x => x.Stone is not null && x.Stone.Color == color)));
    }


    // ==================================================
    // Privateメソッド
    // ==================================================

    /// <summary>
    ///     対象の上方向をひっくり返せるかを判定する
    /// </summary>
    private bool CheckReverseTop(int columnIndex, int rowIndex, StoneColors color) {

        int otherColorCount = 0;

        // 対象の上側に別色に挟まれた同色があるか？
        foreach (var row in Rows.Where(x => x.Index < rowIndex).OrderByDescending(x => x.Index)) {

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return false;
            }
            if (cell.Stone is null) {
                return false;
            }


            // 色が同じ場合、そこまでにみつけた別色の数で戻り値を決める
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // 探索方向に同色がない場合
        return false;
    }

    /// <summary>
    ///     対象の上方向の石をひっくり返す
    /// </summary>
    private void ReverseTop(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseTop(columnIndex, rowIndex, color) == false) {
            return;
        }

        // 行を、indexをDESCでソート→ループを行い、対象から上方向に順番に評価
        foreach (var row in Rows.Where(x => x.Index < rowIndex).OrderByDescending(x => x.Index)) {

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return;
            }

            // 石が置かれていなかったら終了
            if (cell.Stone is null) {
                return;
            }

            // 同色が見つかったら終了
            if (cell.Stone.Color == color) {
                return;
            }

            // ひっくり返す
            cell.Stone.Reverse();
        }
    }

    /// <summary>
    ///     対象の下方向をひっくり返せるかを判定する
    /// </summary>
    private bool CheckReverseBottom(int columnIndex, int rowIndex, StoneColors color) {

        int otherColorCount = 0;

        // 対象の下側に別色に挟まれた同色があるか？
        foreach (var row in Rows.Where(x => x.Index > rowIndex).OrderBy(x => x.Index)) {

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return false;
            }
            if (cell.Stone is null) {
                return false;
            }


            // 色が同じ場合、そこまでにみつけた別色の数で戻り値を決める
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // 探索方向に同色がない場合
        return false;
    }

    /// <summary>
    ///     対象の下方向の石をひっくり返す
    /// </summary>
    private void ReverseBottom(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseBottom(columnIndex, rowIndex, color) == false) {
            return;
        }

        // 行を、indexをASCでソート→ループを行い、対象から下方向に順番に評価
        foreach (var row in Rows.Where(x => x.Index > rowIndex).OrderBy(x => x.Index)) {

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return;
            }

            // 石が置かれていなかったら終了
            if (cell.Stone is null) {
                return;
            }

            // 同色が見つかったら終了
            if (cell.Stone.Color == color) {
                return;
            }

            // ひっくり返す
            cell.Stone.Reverse();
        }
    }

    /// <summary>
    ///     対象の右方向の石をひっくり返せるかを判定する
    /// </summary>
    private bool CheckReverseRight(int columnIndex, int rowIndex, StoneColors color) {

        var targetRow = Rows.FirstOrDefault(x => x.Index == rowIndex);
        if (targetRow is null) {
            return false;
        }

        int otherColorCount = 0;

        // 対象の右側に別色に挟まれた同色があるか？
        foreach (var cell in targetRow.Cells.Where(x => x.OwningColumn.Index > columnIndex).OrderBy(x => x.OwningColumn.Index)) {

            if (cell.Stone is null) {
                return false;
            }


            // 色が同じ場合、そこまでにみつけた別色の数で戻り値を決める
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // 探索方向に同色がない場合
        return false;
    }

    /// <summary>
    ///     対象の右方向の石をひっくり返す
    /// </summary>
    private void ReverseRight(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseRight(columnIndex, rowIndex, color) == false) {
            return;
        }

        var targetRow = Rows.FirstOrDefault(x => x.Index == rowIndex);
        if (targetRow is null) {
            return;
        }

        // 列を、indexをASCでソート→ループを行い、対象から右方向に順番に評価
        foreach (var cell in targetRow.Cells.Where(x => x.OwningColumn.Index > columnIndex).OrderBy(x => x.OwningColumn.Index)) {

            // 石が置かれていなかったら終了
            if (cell.Stone is null) {
                return;
            }

            // 同色が見つかったら終了
            if (cell.Stone.Color == color) {
                return;
            }

            // ひっくり返す
            cell.Stone.Reverse();
        }
    }

    /// <summary>
    ///     対象の左方向に石をひっくり返せるかを判定する
    /// </summary>
    private bool CheckReverseLeft(int columnIndex, int rowIndex, StoneColors color) {

        var targetRow = Rows.FirstOrDefault(x => x.Index == rowIndex);
        if (targetRow is null) {
            return false;
        }

        int otherColorCount = 0;

        // 対象の左側に別色に挟まれた同色があるか？
        foreach (var cell in targetRow.Cells.Where(x => x.OwningColumn.Index < columnIndex).OrderByDescending(x => x.OwningColumn.Index)) {

            if (cell.Stone is null) {
                return false;
            }


            // 色が同じ場合、そこまでにみつけた別色の数で戻り値を決める
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // 探索方向に同色がない場合
        return false;
    }

    /// <summary>
    ///     対象の左方向の石をひっくり返す
    /// </summary>
    private void ReverseLeft(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseLeft(columnIndex, rowIndex, color) == false) {
            return;
        }

        var targetRow = Rows.FirstOrDefault(x => x.Index == rowIndex);
        if (targetRow is null) {
            return;
        }

        // 列を、indexをDESCでソート→ループを行い、対象から左方向に順番に評価
        foreach (var cell in targetRow.Cells.Where(x => x.OwningColumn.Index < columnIndex).OrderByDescending(x => x.OwningColumn.Index)) {

            // 石が置かれていなかったら終了
            if (cell.Stone is null) {
                return;
            }

            // 同色が見つかったら終了
            if (cell.Stone.Color == color) {
                return;
            }

            // ひっくり返す
            cell.Stone.Reverse();
        }
    }

    /// <summary>
    ///     対象の右上方向に石をひっくり返せるかを判定する
    /// </summary>
    private bool CheckReverseTopRight(int columnIndex, int rowIndex, StoneColors color) {

        int otherColorCount = 0;

        // 起点に近いところから順番に、右上方向にチェック
        foreach (var row in Rows.Where(x => x.Index < rowIndex).OrderByDescending(x => x.Index)) {

            // １ループごとに列のインデックスをずらす
            ++columnIndex;
            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return false;
            }
            if (cell.Stone is null) {
                return false;
            }


            // 色が同じ場合、そこまでにみつけた別色の数で戻り値を決める
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // 探索方向に同色がない場合
        return false;
    }

    /// <summary>
    ///     対象の右上方向の石をひっくり返す
    /// </summary>
    private void ReverseTopRight(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseTopRight(columnIndex, rowIndex, color) == false) {
            return;
        }


        // 行を、indexをDESCでソート→ループを行い、対象から上方向に順番に評価
        // １行評価するごとに列座標を一つずらす
        foreach (var row in Rows.Where(x => x.Index < rowIndex).OrderByDescending(x => x.Index)) {

            ++columnIndex;

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return;
            }

            // 石が置かれていなかったら終了
            if (cell.Stone is null) {
                return;
            }

            // 同色が見つかったら終了
            if (cell.Stone.Color == color) {
                return;
            }

            // ひっくり返す
            cell.Stone.Reverse();
        }
    }

    /// <summary>
    ///     対象の左上方向に石をひっくり返せるかを判定する
    /// </summary>
    private bool CheckReverseTopLeft(int columnIndex, int rowIndex, StoneColors color) {

        int otherColorCount = 0;

        // 起点に近いところから順番に、左上方向にチェック
        foreach (var row in Rows.Where(x => x.Index < rowIndex).OrderByDescending(x => x.Index)) {

            // １ループごとに列のインデックスをずらす
            --columnIndex;
            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return false;
            }
            if (cell.Stone is null) {
                return false;
            }


            // 色が同じ場合、そこまでにみつけた別色の数で戻り値を決める
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // 探索方向に同色がない場合
        return false;
    }

    /// <summary>
    ///     対象の左上方向の石をひっくり返す
    /// </summary>
    private void ReverseTopLeft(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseTopLeft(columnIndex, rowIndex, color) == false) {
            return;
        }


        // 行を、indexをDESCでソート→ループを行い、対象から上方向に順番に評価
        // １行評価するごとに列座標を一つずらす
        foreach (var row in Rows.Where(x => x.Index < rowIndex).OrderByDescending(x => x.Index)) {

            --columnIndex;

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return;
            }

            // 石が置かれていなかったら終了
            if (cell.Stone is null) {
                return;
            }

            // 同色が見つかったら終了
            if (cell.Stone.Color == color) {
                return;
            }

            // ひっくり返す
            cell.Stone.Reverse();
        }
    }

    /// <summary>
    ///     対象の右下方向に石をひっくり返せるかを判定する
    /// </summary>
    private bool CheckReverseBottomRight(int columnIndex, int rowIndex, StoneColors color) {

        int otherColorCount = 0;

        // 起点に近いところから順番に、左上方向にチェック
        foreach (var row in Rows.Where(x => x.Index > rowIndex).OrderBy(x => x.Index)) {

            // １ループごとに列のインデックスをずらす
            ++columnIndex;
            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return false;
            }
            if (cell.Stone is null) {
                return false;
            }


            // 色が同じ場合、そこまでにみつけた別色の数で戻り値を決める
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // 探索方向に同色がない場合
        return false;
    }

    /// <summary>
    ///     対象の右下方向の石をひっくり返す
    /// </summary>
    private void ReverseBottomRight(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseBottomRight(columnIndex, rowIndex, color) == false) {
            return;
        }


        // 行を、indexをASCでソート→ループを行い、対象から下方向に順番に評価
        // １行評価するごとに列座標を一つずらす
        foreach (var row in Rows.Where(x => x.Index > rowIndex).OrderBy(x => x.Index)) {

            ++columnIndex;

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return;
            }

            // 石が置かれていなかったら終了
            if (cell.Stone is null) {
                return;
            }

            // 同色が見つかったら終了
            if (cell.Stone.Color == color) {
                return;
            }

            // ひっくり返す
            cell.Stone.Reverse();
        }
    }

    /// <summary>
    ///     対象の左下方向に石をひっくり返せるかを判定する
    /// </summary>
    private bool CheckReverseBottomLeft(int columnIndex, int rowIndex, StoneColors color) {

        int otherColorCount = 0;

        // 起点に近いところから順番に、左上方向にチェック
        foreach (var row in Rows.Where(x => x.Index > rowIndex).OrderByDescending(x => x.Index)) {

            // １ループごとに列のインデックスをずらす
            --columnIndex;
            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return false;
            }
            if (cell.Stone is null) {
                return false;
            }


            // 色が同じ場合、そこまでにみつけた別色の数で戻り値を決める
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // 探索方向に同色がない場合
        return false;
    }

    /// <summary>
    ///     対象の左下方向の石をひっくり返す
    /// </summary>
    private void ReverseBottomLeft(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseBottomLeft(columnIndex, rowIndex, color) == false) {
            return;
        }


        // 行を、indexをASCでソート→ループを行い、対象から下方向に順番に評価
        // １行評価するごとに列座標を一つずらす
        foreach (var row in Rows.Where(x => x.Index > rowIndex).OrderByDescending(x => x.Index)) {

            --columnIndex;

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return;
            }

            // 石が置かれていなかったら終了
            if (cell.Stone is null) {
                return;
            }

            // 同色が見つかったら終了
            if (cell.Stone.Color == color) {
                return;
            }

            // ひっくり返す
            cell.Stone.Reverse();
        }
    }

}
