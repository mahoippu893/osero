using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

/// <summary>
///     �Q�[���Ղ̍s�N���X
/// </summary>
public class Row : BaseModel {

    // ==================================================
    // �v���p�e�B
    // ==================================================

    /// <summary>
    ///     ���̗񂪏�������Q�[����
    /// </summary>
    public Table OwningTable { get; set; }

    /// <summary>
    ///     ���̗�̃Q�[���Տ�̈ʒu
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
    ///     �s�ɐݒ肳���Z���̃��X�g
    /// </summary>
    public List<Cell> Cells = new List<Cell>();


    // ==================================================
    // �R���X�g���N�^
    // ==================================================

    /// <summary>
    ///     �R���X�g���N�^
    /// </summary>
    /// <param name="table"></param>
    public Row(Table table) {

        OwningTable = table;
    }
}
