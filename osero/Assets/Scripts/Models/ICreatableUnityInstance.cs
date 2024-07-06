using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Unity�̃C���X�^���X�𐶐��\�ȃN���X�Ɏ�������C���^�[�t�F�[�X
/// </summary>
public interface ICreatableUnityInstance {

    // ==================================================
    // �v���p�e�B
    // ==================================================

    /// <summary>
    ///     Unity�̃C���X�^���X�𐶐�����
    /// </summary>
    public void CreateUnityInstance(Vector3 size, Vector3 pos, Quaternion rot);

    /// <summary>
    ///     Unity�̃C���X�^���X��j�󂷂�
    /// </summary>
    public void DisposeUnityInstance();
}
