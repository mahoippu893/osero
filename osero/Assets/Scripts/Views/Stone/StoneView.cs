using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

/// <summary>
///     �΂̃v���n�u�ɃA�^�b�`����N���X
/// </summary>
public class StoneView : MonoBehaviour {

    // ==================================================
    // Public���\�b�h
    // ==================================================

    /// <summary>
    ///     �F��ݒ肷��
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
