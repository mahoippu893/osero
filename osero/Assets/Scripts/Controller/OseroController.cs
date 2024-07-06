using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

#nullable enable

public enum OseroStatus {
    NotStart,
    Playing,
    Finish
}

/// <summary>
///     オセロのゲーム全体を管理するコントローラー
/// </summary>
public class OseroController {

    // ゲーム盤を何x何とするか
    protected int GAME_TABLE_SIZE = 10;

    // どちらの色が先行の色か
    protected StoneColors START_STONE_COLOR = StoneColors.White;

    // Canvasのファイルパス
    protected const string CANVAS_PREFAB_PATH = "Prefabs/Canvas";

    private static OseroController? _instance = null;

    private Table? _table;
    private Canvas? _canvas;
    private StoneColors _currentColor;

    // ==================================================
    // プロパティ
    // ==================================================

    /// <summary>
    ///     現在の色
    /// </summary>
    public StoneColors CurrentColor {
        get { return _currentColor; }
        set {

            if (_currentColor == value) {
                return;
            }

            _currentColor = value;

            if (_canvas is not null) {

                var view = _canvas.GetComponent<CanvasView>();
                view.SetChangeColorLabel(_currentColor);
            }
        }
    }

    public OseroStatus Status { get; set; }

    // ==================================================
    // コンストラクタ
    // ==================================================

    /// <summary>
    ///     コンストラクタ
    ///     シングルトン設計のためprivateにして非公開
    /// </summary>
    private OseroController() {

        _currentColor = START_STONE_COLOR;
        Status = OseroStatus.NotStart;
        _canvas = GameObject.Instantiate(Resources.Load<Canvas>(CANVAS_PREFAB_PATH));

        var view = _canvas.GetComponent<CanvasView>();
        view.SetChangeColorLabel(_currentColor);
    }


    // ==================================================
    // Publicメソッド
    // ==================================================

    /// <summary>
    ///     インスタンスを取得する
    /// </summary>
    public static OseroController GetInstance() {

        if (_instance is null) {
            _instance = new OseroController();
        }

        return _instance;
    }

    /// <summary>
    ///     ゲームを開始する
    /// </summary>
    public void StartGame() {

        // 対局中か？
        switch (Status) {
            case OseroStatus.Playing:
                return;
        }


        if (_table is null) {
            _table = new Table();
        }

        // ゲーム盤を初期化する
        _table.InitializeTable(GAME_TABLE_SIZE);


        // 最初の石を置く
        _table.PutStone((GAME_TABLE_SIZE / 2) - 1, (GAME_TABLE_SIZE / 2) - 1, StoneColors.White);
        _table.PutStone((GAME_TABLE_SIZE / 2) - 1, (GAME_TABLE_SIZE / 2), StoneColors.Black);
        _table.PutStone((GAME_TABLE_SIZE / 2), (GAME_TABLE_SIZE / 2) - 1, StoneColors.Black);
        _table.PutStone((GAME_TABLE_SIZE / 2), (GAME_TABLE_SIZE / 2), StoneColors.White);


        // Controllerで管理する状態の変更
        CurrentColor = START_STONE_COLOR;
        Status = OseroStatus.Playing;



        if (_canvas is null) {
            return;
        }

        var view = _canvas.GetComponent<CanvasView>();
        view.SetEnableRematchButton(false);
    }

    /// <summary>
    ///     盤面に石を配置する
    /// </summary>
    public void PutStone(int columnIndex, int rowIndex) {

        if (_table is null) {
            return;
        }

        // ----------------------------------------
        // 石を配置できるかのチェック
        // ----------------------------------------

        // 対局中か？
        switch (Status) {
            case OseroStatus.NotStart:
            case OseroStatus.Finish:
                return;
        }

        // すでに石が置かれているか？
        if (_table.IsSetStoneCell(columnIndex, rowIndex)) {
            return;
        }

        // 配置先にとなりあう石があるか？
        if (_table.IsSetNextStoneCell(columnIndex, rowIndex) == false) {
            return;
        }

        // その石を置くことで一つでもひっくり返るか？
        if (_table.IsReverseIfSetStone(columnIndex, rowIndex, CurrentColor) == false) {
            return;
        }

        // ----------------------------------------
        // 石を配置
        // ----------------------------------------

        _table.PutStone(columnIndex, rowIndex, CurrentColor);

        // 色を反転
        switch (CurrentColor) {
            case StoneColors.White:
                CurrentColor = StoneColors.Black;
                break;
            case StoneColors.Black:
                CurrentColor = StoneColors.White;
                break;
        }


        // ----------------------------------------
        // ゲーム終了かを判定
        // ----------------------------------------

        if (_table.HasSpace() == false) {

            // 完了にする

            Status = OseroStatus.Finish;


            // 勝った方を計算

            int whiteCount = _table.CountStoneColor(StoneColors.White);
            int blackCount = _table.CountStoneColor(StoneColors.Black);

            if (_canvas is null) {
                return;
            }

            var view = _canvas.GetComponent<CanvasView>();
            if (whiteCount > blackCount) {
                view.SetWinnerLabel(StoneColors.White);
            } else if (blackCount > whiteCount) {
                view.SetWinnerLabel(StoneColors.Black);
            } else {
                view.SetDrawLabel();
            }

            view.SetEnableRematchButton(true);
            return;
        }


        // 次の色で色をおける箇所があるか？
        if (_table.IsSetableColor(CurrentColor) == false) {
        
            // パスとして、もう一度おけるようにする
            switch (CurrentColor) {
                case StoneColors.White:
                    CurrentColor = StoneColors.Black;
                    break;
                case StoneColors.Black:
                    CurrentColor = StoneColors.White;
                    break;
            }
            return;
        }
    }
}
