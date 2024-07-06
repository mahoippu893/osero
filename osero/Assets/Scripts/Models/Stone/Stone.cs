using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

#nullable enable

/// <summary>
///     �΂̐F
/// </summary>
public enum StoneColors {

    Black,
    White
}

/// <summary>
///     �I�Z���̐΃N���X
/// </summary>
public class Stone : BaseModel, ICreatableUnityInstance {

    // Prefab�̃t�@�C���p�X
    protected const string PREFAB_PATH = "Prefabs/Stone";

    private GameObject? _targetInstance;

    // ==================================================
    // �v���p�e�B
    // ==================================================

    /// <summary>
    ///     ���݂̐F
    /// </summary>
    public StoneColors Color { get; set; }


    // ==================================================
    // �R���X�g���N�^
    // ==================================================

    /// <summary>
    ///     �R���X�g���N�^
    /// </summary>
    public Stone(StoneColors color) {

        Color = color;
    }


    // ==================================================
    // Public���\�b�h
    // ==================================================

    /// <summary>
    ///     �Ђ�����Ԃ�
    /// </summary>
    public void Reverse() {

        switch (Color) {
            case StoneColors.Black:
                Color = StoneColors.White; 
                break;
            case StoneColors.White:
                Color = StoneColors.Black;
                break;
        }

        // �\���F��ς���
        if (_targetInstance is null) {
            return;
        }
        var view = _targetInstance.GetComponent<StoneView>();
        view.SetColor(Color);
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

        var view = _targetInstance.GetComponent<StoneView>();
        view.SetColor(Color);
    }

    /// <summary>
    ///     Unity�̃C���X�^���X��j�󂷂�
    /// </summary>
    public void DisposeUnityInstance() {

        GameObject.Destroy(_targetInstance);
        _targetInstance = null;
    }
}
