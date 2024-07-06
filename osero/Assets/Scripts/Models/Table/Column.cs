using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

/// <summary>
///     �Q�[���Ղ̗�N���X
/// </summary>
public class Column : BaseModel {

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

            if (OwningTable.Columns.Contains(this) == false) {
                return null;
            }

            return OwningTable.Columns.IndexOf(this);
        }
    }


    // ==================================================
    // �R���X�g���N�^
    // ==================================================

    /// <summary>
    ///     �R���X�g���N�^
    /// </summary>
    /// <param name="table"></param>
    public Column(Table table) {

        OwningTable = table;
    }
}
