using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Canvas�ɃA�^�b�`����N���X
/// </summary>
public class CanvasView : MonoBehaviour {

    // ==================================================
    // �v���p�e�B
    // ==================================================

    /// <summary>
    ///     ���݂ǂ����̃^�[�����̃��x��
    /// </summary>
    [SerializeField]
    public TextMeshProUGUI CurrentColorLabel;

    /// <summary>
    ///     �Ď����{�^��
    /// </summary>
    [SerializeField]
    public Button RematchButton;


    // ==================================================
    // Public���\�b�h
    // ==================================================

    /// <summary>
    ///     ���݂ǂ����̃^�[�����̃��x���̃e�L�X�g��ύX����
    /// </summary>
    /// <param name="color"></param>
    public void SetChangeColorLabel(StoneColors color) {

        switch (color) {
            case StoneColors.Black:

                CurrentColorLabel.text = "�� �̔Ԃł��B";
                break;
            case StoneColors.White:

                CurrentColorLabel.text = "�� �̔Ԃł��B";
                break;
            default:

                CurrentColorLabel.text = "";
                break;
        }
    }

    /// <summary>
    ///     ���҂̃��x����\������
    /// </summary>
    /// <param name="color"></param>
    public void SetWinnerLabel(StoneColors color) {

        switch (color) {
            case StoneColors.Black:

                CurrentColorLabel.text = "�����I���I\r\n���ʂ� �� �̏����ł��I";
                break;
            case StoneColors.White:

                CurrentColorLabel.text = "�����I���I\r\n���ʂ� �� �̏����ł��I";
                break;
        }
    }

    /// <summary>
    ///     ���҂̃��x����\������
    /// </summary>
    /// <param name="color"></param>
    public void SetDrawLabel() {

        CurrentColorLabel.text = "�����I���I\r\n���ʂ͓����� �������� �ł��I";
    }

    /// <summary>
    ///     �Ē���{�^���̕\����؂�ւ���
    /// </summary>
    public void SetEnableRematchButton(bool enable) {

        RematchButton.gameObject.SetActive(enable);
    }

    // ==================================================
    // Event�n���h��
    // ==================================================

    /// <summary>
    ///     �J�n���̏���
    /// </summary>
    private void Start() {

        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    /// <summary>
    ///     �Ď����{�^���N���b�N���̏���
    /// </summary>
    public void OnClickRematchButton() {

        OseroController.GetInstance().StartGame();
    }
}
