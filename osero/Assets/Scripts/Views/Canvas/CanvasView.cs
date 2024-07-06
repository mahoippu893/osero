using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Canvasにアタッチするクラス
/// </summary>
public class CanvasView : MonoBehaviour {

    // ==================================================
    // プロパティ
    // ==================================================

    /// <summary>
    ///     現在どっちのターンかのラベル
    /// </summary>
    [SerializeField]
    public TextMeshProUGUI CurrentColorLabel;

    /// <summary>
    ///     再試合ボタン
    /// </summary>
    [SerializeField]
    public Button RematchButton;


    // ==================================================
    // Publicメソッド
    // ==================================================

    /// <summary>
    ///     現在どっちのターンかのラベルのテキストを変更する
    /// </summary>
    /// <param name="color"></param>
    public void SetChangeColorLabel(StoneColors color) {

        switch (color) {
            case StoneColors.Black:

                CurrentColorLabel.text = "黒 の番です。";
                break;
            case StoneColors.White:

                CurrentColorLabel.text = "白 の番です。";
                break;
            default:

                CurrentColorLabel.text = "";
                break;
        }
    }

    /// <summary>
    ///     勝者のラベルを表示する
    /// </summary>
    /// <param name="color"></param>
    public void SetWinnerLabel(StoneColors color) {

        switch (color) {
            case StoneColors.Black:

                CurrentColorLabel.text = "試合終了！\r\n結果は 黒 の勝利です！";
                break;
            case StoneColors.White:

                CurrentColorLabel.text = "試合終了！\r\n結果は 白 の勝利です！";
                break;
        }
    }

    /// <summary>
    ///     勝者のラベルを表示する
    /// </summary>
    /// <param name="color"></param>
    public void SetDrawLabel() {

        CurrentColorLabel.text = "試合終了！\r\n結果は同数で 引き分け です！";
    }

    /// <summary>
    ///     再挑戦ボタンの表示を切り替える
    /// </summary>
    public void SetEnableRematchButton(bool enable) {

        RematchButton.gameObject.SetActive(enable);
    }

    // ==================================================
    // Eventハンドラ
    // ==================================================

    /// <summary>
    ///     開始時の処理
    /// </summary>
    private void Start() {

        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    /// <summary>
    ///     再試合ボタンクリック時の処理
    /// </summary>
    public void OnClickRematchButton() {

        OseroController.GetInstance().StartGame();
    }
}
