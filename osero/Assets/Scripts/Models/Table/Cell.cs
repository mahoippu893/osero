using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

/// <summary>
///     �Q�[���Ղ̃Z���N���X
/// </summary>
public class Cell : BaseModel, ICreatableUnityInstance {

    // Prefab�̃t�@�C���p�X
    protected const string PREFAB_PATH = "Prefabs/Cell";
 
    private GameObject? _targetInstance;

    // ==================================================
    // �v���p�e�B
    // ==================================================

    /// <summary>
    ///     ���̃Z������������Q�[���Ղ̗�
    /// </summary>
    public Column OwningColumn { get; set; }

    /// <summary>
    ///     ���̃Z������������Q�[���Ղ̍s
    /// </summary>
    public Row OwningRow { get; set; }

    /// <summary>
    ///     �����̏�ɔz�u����Ă���΂̃C���X�^���X
    /// </summary>
    public Stone? Stone { get; set; }


    // ==================================================
    // �R���X�g���N�^
    // ==================================================

    /// <summary>
    ///     �R���X�g���N�^
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    public Cell(Column column, Row row) { 
    
        OwningColumn = column;
        OwningRow = row;
    }


    // ==================================================
    // Public���\�b�h
    // ==================================================

    /// <summary>
    ///     �΂�u��
    /// </summary>
    public void PutStone(StoneColors color) {

        if (_targetInstance is null) {
            return;
        }

        var stone = new Stone(color);
        Stone = stone;

        stone.CreateUnityInstance(_targetInstance.transform.localScale, _targetInstance.transform.position, new Quaternion(0, 0, 0, 0));
    }

    /// <summary>
    ///     Unity�̃C���X�^���X�𐶐�����
    /// </summary>
    /// <param name="size"></param>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    public void CreateUnityInstance(Vector3 size, Vector3 pos, Quaternion rot) {

        if (_targetInstance is not null) {
            DisposeUnityInstance();
        }

        _targetInstance = GameObject.Instantiate(Resources.Load<GameObject>(PREFAB_PATH), pos, rot);
        _targetInstance.transform.localScale = size;

        var view = _targetInstance.GetComponent<CellView>();
        view.RowIndex = OwningRow.Index;
        view.ColumnIndex = OwningColumn.Index;
    }

    /// <summary>
    ///     Unity�̃C���X�^���X��j�󂷂�
    /// </summary>
    public void DisposeUnityInstance() {

        GameObject.Destroy(_targetInstance);
        _targetInstance = null;
    }
}
