using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

/// <summary>
///     �Z���̃v���n�u�ɃA�^�b�`����N���X
/// </summary>
public class CellView : MonoBehaviour {

    // ==================================================
    // Public���\�b�h
    // ==================================================

    /// <summary>
    ///     ��̃C���f�b�N�X
    /// </summary>
    [SerializeField]
    public int? ColumnIndex { get; set; } = 0;

    /// <summary>
    ///     �s�̃C���f�b�N�X
    /// </summary>
    [SerializeField]
    public int? RowIndex { get; set; } = 0;

    // ==================================================
    // Event�n���h��
    // ==================================================

    /// <summary>
    ///     �N���b�N���ꂽ�Ƃ��̏���
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
