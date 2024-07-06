using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

/// <summary>
///     石のプレハブにアタッチするクラス
/// </summary>
public class StoneView : MonoBehaviour {

    // ==================================================
    // Publicメソッド
    // ==================================================

    /// <summary>
    ///     色を設定する
    /// </summary>
    public void SetColor(StoneColors color) {

        var spriteRender = gameObject.GetComponent<SpriteRenderer>();

        switch (color) {
            case StoneColors.Black:

                spriteRender.color = Color.black;
                break;
            case StoneColors.White:

                spriteRender.color = Color.white;
                break;
        }
    }
}
