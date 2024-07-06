using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

#nullable enable

public enum OseroStatus {
    NotStart,
    Playing,
    Finish
}

/// <summary>
///     �I�Z���̃Q�[���S�̂��Ǘ�����R���g���[���[
/// </summary>
public class OseroController {

    // �Q�[���Ղ���x���Ƃ��邩
    protected int GAME_TABLE_SIZE = 10;

    // �ǂ���̐F����s�̐F��
    protected StoneColors START_STONE_COLOR = StoneColors.White;

    // Canvas�̃t�@�C���p�X
    protected const string CANVAS_PREFAB_PATH = "Prefabs/Canvas";

    private static OseroController? _instance = null;

    private Table? _table;
    private Canvas? _canvas;
    private StoneColors _currentColor;

    // ==================================================
    // �v���p�e�B
    // ==================================================

    /// <summary>
    ///     ���݂̐F
    /// </summary>
    public StoneColors CurrentColor {
        get { return _currentColor; }
        set {

            if (_currentColor == value) {
                return;
            }

            _currentColor = value;

            if (_canvas is not null) {

                var view = _canvas.GetComponent<CanvasView>();
                view.SetChangeColorLabel(_currentColor);
            }
        }
    }

    public OseroStatus Status { get; set; }

    // ==================================================
    // �R���X�g���N�^
    // ==================================================

    /// <summary>
    ///     �R���X�g���N�^
    ///     �V���O���g���݌v�̂���private�ɂ��Ĕ���J
    /// </summary>
    private OseroController() {

        _currentColor = START_STONE_COLOR;
        Status = OseroStatus.NotStart;
        _canvas = GameObject.Instantiate(Resources.Load<Canvas>(CANVAS_PREFAB_PATH));

        var view = _canvas.GetComponent<CanvasView>();
        view.SetChangeColorLabel(_currentColor);
    }


    // ==================================================
    // Public���\�b�h
    // ==================================================

    /// <summary>
    ///     �C���X�^���X���擾����
    /// </summary>
    public static OseroController GetInstance() {

        if (_instance is null) {
            _instance = new OseroController();
        }

        return _instance;
    }

    /// <summary>
    ///     �Q�[�����J�n����
    /// </summary>
    public void StartGame() {

        // �΋ǒ����H
        switch (Status) {
            case OseroStatus.Playing:
                return;
        }


        if (_table is null) {
            _table = new Table();
        }

        // �Q�[���Ղ�����������
        _table.InitializeTable(GAME_TABLE_SIZE);


        // �ŏ��̐΂�u��
        _table.PutStone((GAME_TABLE_SIZE / 2) - 1, (GAME_TABLE_SIZE / 2) - 1, StoneColors.White);
        _table.PutStone((GAME_TABLE_SIZE / 2) - 1, (GAME_TABLE_SIZE / 2), StoneColors.Black);
        _table.PutStone((GAME_TABLE_SIZE / 2), (GAME_TABLE_SIZE / 2) - 1, StoneColors.Black);
        _table.PutStone((GAME_TABLE_SIZE / 2), (GAME_TABLE_SIZE / 2), StoneColors.White);


        // Controller�ŊǗ������Ԃ̕ύX
        CurrentColor = START_STONE_COLOR;
        Status = OseroStatus.Playing;



        if (_canvas is null) {
            return;
        }

        var view = _canvas.GetComponent<CanvasView>();
        view.SetEnableRematchButton(false);
    }

    /// <summary>
    ///     �Ֆʂɐ΂�z�u����
    /// </summary>
    public void PutStone(int columnIndex, int rowIndex) {

        if (_table is null) {
            return;
        }

        // ----------------------------------------
        // �΂�z�u�ł��邩�̃`�F�b�N
        // ----------------------------------------

        // �΋ǒ����H
        switch (Status) {
            case OseroStatus.NotStart:
            case OseroStatus.Finish:
                return;
        }

        // ���łɐ΂��u����Ă��邩�H
        if (_table.IsSetStoneCell(columnIndex, rowIndex)) {
            return;
        }

        // �z�u��ɂƂȂ肠���΂����邩�H
        if (_table.IsSetNextStoneCell(columnIndex, rowIndex) == false) {
            return;
        }

        // ���̐΂�u�����Ƃň�ł��Ђ�����Ԃ邩�H
        if (_table.IsReverseIfSetStone(columnIndex, rowIndex, CurrentColor) == false) {
            return;
        }

        // ----------------------------------------
        // �΂�z�u
        // ----------------------------------------

        _table.PutStone(columnIndex, rowIndex, CurrentColor);

        // �F�𔽓]
        switch (CurrentColor) {
            case StoneColors.White:
                CurrentColor = StoneColors.Black;
                break;
            case StoneColors.Black:
                CurrentColor = StoneColors.White;
                break;
        }


        // ----------------------------------------
        // �Q�[���I�����𔻒�
        // ----------------------------------------

        if (_table.HasSpace() == false) {

            // �����ɂ���

            Status = OseroStatus.Finish;


            // �����������v�Z

            int whiteCount = _table.CountStoneColor(StoneColors.White);
            int blackCount = _table.CountStoneColor(StoneColors.Black);

            if (_canvas is null) {
                return;
            }

            var view = _canvas.GetComponent<CanvasView>();
            if (whiteCount > blackCount) {
                view.SetWinnerLabel(StoneColors.White);
            } else if (blackCount > whiteCount) {
                view.SetWinnerLabel(StoneColors.Black);
            } else {
                view.SetDrawLabel();
            }

            view.SetEnableRematchButton(true);
            return;
        }


        // ���̐F�ŐF��������ӏ������邩�H
        if (_table.IsSetableColor(CurrentColor) == false) {
        
            // �p�X�Ƃ��āA������x������悤�ɂ���
            switch (CurrentColor) {
                case StoneColors.White:
                    CurrentColor = StoneColors.Black;
                    break;
                case StoneColors.Black:
                    CurrentColor = StoneColors.White;
                    break;
            }
            return;
        }
    }
}
