using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

/// <summary>
///     �V�[���ɃA�^�b�`����N���X
/// </summary>
public class SeceneView : MonoBehaviour {

    // ==================================================
    // Event�n���h��
    // ==================================================

    /// <summary>
    ///     �ǂݍ��ݎ��̏���
    /// </summary>
    private void Start() {

        OseroController.GetInstance().StartGame();
    }
}
