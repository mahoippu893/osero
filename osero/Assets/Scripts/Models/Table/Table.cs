using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

#nullable enable

/// <summary>
///     �Q�[���ՃN���X
/// </summary>
public class Table : BaseModel {

    protected const int TABLE_CELL_SIZE = 30;

    // ==================================================
    // �v���p�e�B
    // ==================================================

    /// <summary>
    ///     �Q�[���Ղɐݒ肳����̃��X�g
    /// </summary>
    public List<Column> Columns { get; set; } = new List<Column>();

    /// <summary>
    ///     �Q�[���Ղɐݒ肳���s�̃��X�g
    /// </summary>
    public List<Row> Rows { get; set; } = new List<Row>();


    // ==================================================
    // Public���\�b�h
    // ==================================================

    /// <summary>
    ///     �Q�[���Ղ�����������
    /// </summary>
    /// <param name="indexCount"></param>
    public void InitializeTable(int indexCount) {


        // ���̏�Ԃ̔j��
        foreach (var row in Rows) {

            foreach (var cell in row.Cells) {

                if (cell.Stone is not null) {
                    cell.Stone.DisposeUnityInstance();
                }

                cell.DisposeUnityInstance();
            }

            row.Cells.Clear();
        }
        Rows.Clear();
        Columns.Clear();


        // ------------------------


        // ��̐���
        for (int i = 0; i < indexCount; i++) {

            var column = new Column(this);
            Columns.Add(column);
        }

        // ��̐���
        for (int i = 0; i < indexCount; i++) {

            var row = new   Row(this);
            Rows.Add(row);

            // �Z���̐���
            foreach (var column in Columns) {

                var cell = new Cell(column, row);
                row.Cells.Add(cell);

                // Unity�C���X�^���X�𐶐�����
                cell.CreateUnityInstance(
                    new Vector3(TABLE_CELL_SIZE, TABLE_CELL_SIZE, 0),
                    new Vector3(((column.Index is null) ? 0 : (int)column.Index) * TABLE_CELL_SIZE, -i * TABLE_CELL_SIZE, 0),
                    new Quaternion(0, 0, 0, 0));
            }
        }
    }


    /// <summary>
    ///     �΂�u��
    /// </summary>
    public void PutStone(int columnIndex, int rowIndex, StoneColors color) {

        // ���͒l�̃G���[����
        if (columnIndex < 0) {
            return;
        }
        if (rowIndex < 0) {
            return;
        }

        if (Columns.Count <= columnIndex) {
            return;
        }
        if (Rows.Count <= rowIndex) {
            return;
        }


        // �ΏۃZ���ɐ΂�u��
        var targetCell = Rows[rowIndex].Cells[columnIndex];
        targetCell.PutStone(color);


        // ------------------------
        // �Ђ�����Ԃ�����
        // ------------------------

        // �����
        ReverseTop(columnIndex, rowIndex, color);

        // ������
        ReverseBottom(columnIndex, rowIndex, color);

        // �E����
        ReverseRight(columnIndex, rowIndex, color);

        // ������
        ReverseLeft(columnIndex, rowIndex, color);

        // �������
        ReverseTopLeft(columnIndex, rowIndex, color);

        // �E�����
        ReverseTopRight(columnIndex, rowIndex, color);

        // ��������
        ReverseBottomLeft(columnIndex, rowIndex, color);

        // �E������
        ReverseBottomRight(columnIndex, rowIndex, color);
    }


    /// <summary>
    ///     �w��̃Z���ɐ΂����łɒu����Ă��邩�𔻒肷��
    /// </summary>
    /// <returns></returns>
    public bool IsSetStoneCell(int columnIndex, int rowIndex) {

        var targetRow = Rows.FirstOrDefault(x => x.Index == rowIndex);
        if (targetRow is null) {
            return true;
        }

        var targetCell = targetRow.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
        if (targetCell is null) {
            return true;
        }

        return (targetCell.Stone is not null);
    }

    /// <summary>
    ///     �܂��X�y�[�X�����邩�ǂ����𔻒肷��
    /// </summary>
    /// <returns></returns>
    public bool HasSpace() {

        return (Rows.Sum(x => x.Cells.Count(x => x.Stone is null)) > 0);
    }

    /// <summary>
    ///     �w��Z���ׂ̗ɐ΂��u����Ă��邩�𔻒肷��
    /// </summary>
    /// <returns></returns>
    public bool IsSetNextStoneCell(int columnIndex, int rowIndex) {

        var targetRow = Rows.FirstOrDefault(x => x.Index == rowIndex);
        if (targetRow is null) {
            return false;
        }

        // ��
        if (targetRow.Cells.Count(x => x.OwningColumn.Index == columnIndex - 1 && x.Stone is not null) > 0) {
            return true;
        }

        // �E
        if (targetRow.Cells.Count(x => x.OwningColumn.Index == columnIndex + 1 && x.Stone is not null) > 0) {
            return true;
        }

        // ---------

        var topRow = Rows.FirstOrDefault(x => x.Index == rowIndex - 1);
        if (topRow is not null) {

            // ��
            if (topRow.Cells.Count(x => x.OwningColumn.Index == columnIndex && x.Stone is not null) > 0) {
                return true;
            }

            // �E��
            if (topRow.Cells.Count(x => x.OwningColumn.Index == columnIndex + 1 && x.Stone is not null) > 0) {
                return true;
            }

            // ����
            if (topRow.Cells.Count(x => x.OwningColumn.Index == columnIndex - 1 && x.Stone is not null) > 0) {
                return true;
            }
        }

        // ---------

        var bottomRow = Rows.FirstOrDefault(x => x.Index == rowIndex + 1);
        if (bottomRow is not null) {

            // ��
            if (bottomRow.Cells.Count(x => x.OwningColumn.Index == columnIndex && x.Stone is not null) > 0) {
                return true;
            }

            // �E��
            if (bottomRow.Cells.Count(x => x.OwningColumn.Index == columnIndex + 1 && x.Stone is not null) > 0) {
                return true;
            }

            // ����
            if (bottomRow.Cells.Count(x => x.OwningColumn.Index == columnIndex - 1 && x.Stone is not null) > 0) {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     ���̐΂�u�����Ƃň�ł��Ђ�����Ԃ邩�𔻒肷��
    /// </summary>
    /// <returns></returns>
    public bool IsReverseIfSetStone(int columnIndex, int rowIndex, StoneColors color) {

        // �����
        if (CheckReverseTop(columnIndex, rowIndex, color)) {
            return true;
        }

        // ������
        if (CheckReverseBottom(columnIndex, rowIndex, color)) {
            return true;
        }

        // �E����
        if (CheckReverseRight(columnIndex, rowIndex, color)) {
            return true;
        }

        // ������
        if (CheckReverseLeft(columnIndex, rowIndex, color)) {
            return true;
        }

        // �������
        if (CheckReverseTopLeft(columnIndex, rowIndex, color)) {
            return true;
        }

        // �E�����
        if (CheckReverseTopRight(columnIndex, rowIndex, color)) {
            return true;
        }

        // ��������
        if (CheckReverseBottomLeft(columnIndex, rowIndex, color)) {
            return true;
        }

        // �E������
        if (CheckReverseBottomRight(columnIndex, rowIndex, color)) {
            return true;
        }

        return false;
    }

    /// <summary>
    ///     �w��F��z�u�\���𔻒肷��
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public bool IsSetableColor(StoneColors color) {

        // �S�Z����T������
        foreach (var row in Rows) {
            foreach (var cell in row.Cells.Where(x => x.Stone is null)) {

                if (cell.OwningColumn.Index is null) {
                    continue;
                }
                if (cell.OwningRow.Index is null) {
                    continue;
                }

                // �ׂɐ΂����邩
                if (IsSetNextStoneCell((int)cell.OwningColumn.Index, (int)cell.OwningRow.Index) == false) {
                    continue;
                }

                // �΂�u�����甽�]���邩
                if (IsReverseIfSetStone((int)cell.OwningColumn.Index, (int)cell.OwningRow.Index, color)) {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    ///     �Ώۂ̐F�̐΂̔z�u�����v�Z����
    /// </summary>
    /// <returns></returns>
    public int CountStoneColor(StoneColors color) {

        return (Rows.Sum(x => x.Cells.Count(x => x.Stone is not null && x.Stone.Color == color)));
    }


    // ==================================================
    // Private���\�b�h
    // ==================================================

    /// <summary>
    ///     �Ώۂ̏�������Ђ�����Ԃ��邩�𔻒肷��
    /// </summary>
    private bool CheckReverseTop(int columnIndex, int rowIndex, StoneColors color) {

        int otherColorCount = 0;

        // �Ώۂ̏㑤�ɕʐF�ɋ��܂ꂽ���F�����邩�H
        foreach (var row in Rows.Where(x => x.Index < rowIndex).OrderByDescending(x => x.Index)) {

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return false;
            }
            if (cell.Stone is null) {
                return false;
            }


            // �F�������ꍇ�A�����܂łɂ݂����ʐF�̐��Ŗ߂�l�����߂�
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // �T�������ɓ��F���Ȃ��ꍇ
        return false;
    }

    /// <summary>
    ///     �Ώۂ̏�����̐΂��Ђ�����Ԃ�
    /// </summary>
    private void ReverseTop(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseTop(columnIndex, rowIndex, color) == false) {
            return;
        }

        // �s���Aindex��DESC�Ń\�[�g�����[�v���s���A�Ώۂ��������ɏ��Ԃɕ]��
        foreach (var row in Rows.Where(x => x.Index < rowIndex).OrderByDescending(x => x.Index)) {

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return;
            }

            // �΂��u����Ă��Ȃ�������I��
            if (cell.Stone is null) {
                return;
            }

            // ���F������������I��
            if (cell.Stone.Color == color) {
                return;
            }

            // �Ђ�����Ԃ�
            cell.Stone.Reverse();
        }
    }

    /// <summary>
    ///     �Ώۂ̉��������Ђ�����Ԃ��邩�𔻒肷��
    /// </summary>
    private bool CheckReverseBottom(int columnIndex, int rowIndex, StoneColors color) {

        int otherColorCount = 0;

        // �Ώۂ̉����ɕʐF�ɋ��܂ꂽ���F�����邩�H
        foreach (var row in Rows.Where(x => x.Index > rowIndex).OrderBy(x => x.Index)) {

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return false;
            }
            if (cell.Stone is null) {
                return false;
            }


            // �F�������ꍇ�A�����܂łɂ݂����ʐF�̐��Ŗ߂�l�����߂�
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // �T�������ɓ��F���Ȃ��ꍇ
        return false;
    }

    /// <summary>
    ///     �Ώۂ̉������̐΂��Ђ�����Ԃ�
    /// </summary>
    private void ReverseBottom(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseBottom(columnIndex, rowIndex, color) == false) {
            return;
        }

        // �s���Aindex��ASC�Ń\�[�g�����[�v���s���A�Ώۂ��牺�����ɏ��Ԃɕ]��
        foreach (var row in Rows.Where(x => x.Index > rowIndex).OrderBy(x => x.Index)) {

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return;
            }

            // �΂��u����Ă��Ȃ�������I��
            if (cell.Stone is null) {
                return;
            }

            // ���F������������I��
            if (cell.Stone.Color == color) {
                return;
            }

            // �Ђ�����Ԃ�
            cell.Stone.Reverse();
        }
    }

    /// <summary>
    ///     �Ώۂ̉E�����̐΂��Ђ�����Ԃ��邩�𔻒肷��
    /// </summary>
    private bool CheckReverseRight(int columnIndex, int rowIndex, StoneColors color) {

        var targetRow = Rows.FirstOrDefault(x => x.Index == rowIndex);
        if (targetRow is null) {
            return false;
        }

        int otherColorCount = 0;

        // �Ώۂ̉E���ɕʐF�ɋ��܂ꂽ���F�����邩�H
        foreach (var cell in targetRow.Cells.Where(x => x.OwningColumn.Index > columnIndex).OrderBy(x => x.OwningColumn.Index)) {

            if (cell.Stone is null) {
                return false;
            }


            // �F�������ꍇ�A�����܂łɂ݂����ʐF�̐��Ŗ߂�l�����߂�
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // �T�������ɓ��F���Ȃ��ꍇ
        return false;
    }

    /// <summary>
    ///     �Ώۂ̉E�����̐΂��Ђ�����Ԃ�
    /// </summary>
    private void ReverseRight(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseRight(columnIndex, rowIndex, color) == false) {
            return;
        }

        var targetRow = Rows.FirstOrDefault(x => x.Index == rowIndex);
        if (targetRow is null) {
            return;
        }

        // ����Aindex��ASC�Ń\�[�g�����[�v���s���A�Ώۂ���E�����ɏ��Ԃɕ]��
        foreach (var cell in targetRow.Cells.Where(x => x.OwningColumn.Index > columnIndex).OrderBy(x => x.OwningColumn.Index)) {

            // �΂��u����Ă��Ȃ�������I��
            if (cell.Stone is null) {
                return;
            }

            // ���F������������I��
            if (cell.Stone.Color == color) {
                return;
            }

            // �Ђ�����Ԃ�
            cell.Stone.Reverse();
        }
    }

    /// <summary>
    ///     �Ώۂ̍������ɐ΂��Ђ�����Ԃ��邩�𔻒肷��
    /// </summary>
    private bool CheckReverseLeft(int columnIndex, int rowIndex, StoneColors color) {

        var targetRow = Rows.FirstOrDefault(x => x.Index == rowIndex);
        if (targetRow is null) {
            return false;
        }

        int otherColorCount = 0;

        // �Ώۂ̍����ɕʐF�ɋ��܂ꂽ���F�����邩�H
        foreach (var cell in targetRow.Cells.Where(x => x.OwningColumn.Index < columnIndex).OrderByDescending(x => x.OwningColumn.Index)) {

            if (cell.Stone is null) {
                return false;
            }


            // �F�������ꍇ�A�����܂łɂ݂����ʐF�̐��Ŗ߂�l�����߂�
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // �T�������ɓ��F���Ȃ��ꍇ
        return false;
    }

    /// <summary>
    ///     �Ώۂ̍������̐΂��Ђ�����Ԃ�
    /// </summary>
    private void ReverseLeft(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseLeft(columnIndex, rowIndex, color) == false) {
            return;
        }

        var targetRow = Rows.FirstOrDefault(x => x.Index == rowIndex);
        if (targetRow is null) {
            return;
        }

        // ����Aindex��DESC�Ń\�[�g�����[�v���s���A�Ώۂ��獶�����ɏ��Ԃɕ]��
        foreach (var cell in targetRow.Cells.Where(x => x.OwningColumn.Index < columnIndex).OrderByDescending(x => x.OwningColumn.Index)) {

            // �΂��u����Ă��Ȃ�������I��
            if (cell.Stone is null) {
                return;
            }

            // ���F������������I��
            if (cell.Stone.Color == color) {
                return;
            }

            // �Ђ�����Ԃ�
            cell.Stone.Reverse();
        }
    }

    /// <summary>
    ///     �Ώۂ̉E������ɐ΂��Ђ�����Ԃ��邩�𔻒肷��
    /// </summary>
    private bool CheckReverseTopRight(int columnIndex, int rowIndex, StoneColors color) {

        int otherColorCount = 0;

        // �N�_�ɋ߂��Ƃ��납�珇�ԂɁA�E������Ƀ`�F�b�N
        foreach (var row in Rows.Where(x => x.Index < rowIndex).OrderByDescending(x => x.Index)) {

            // �P���[�v���Ƃɗ�̃C���f�b�N�X�����炷
            ++columnIndex;
            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return false;
            }
            if (cell.Stone is null) {
                return false;
            }


            // �F�������ꍇ�A�����܂łɂ݂����ʐF�̐��Ŗ߂�l�����߂�
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // �T�������ɓ��F���Ȃ��ꍇ
        return false;
    }

    /// <summary>
    ///     �Ώۂ̉E������̐΂��Ђ�����Ԃ�
    /// </summary>
    private void ReverseTopRight(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseTopRight(columnIndex, rowIndex, color) == false) {
            return;
        }


        // �s���Aindex��DESC�Ń\�[�g�����[�v���s���A�Ώۂ��������ɏ��Ԃɕ]��
        // �P�s�]�����邲�Ƃɗ���W������炷
        foreach (var row in Rows.Where(x => x.Index < rowIndex).OrderByDescending(x => x.Index)) {

            ++columnIndex;

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return;
            }

            // �΂��u����Ă��Ȃ�������I��
            if (cell.Stone is null) {
                return;
            }

            // ���F������������I��
            if (cell.Stone.Color == color) {
                return;
            }

            // �Ђ�����Ԃ�
            cell.Stone.Reverse();
        }
    }

    /// <summary>
    ///     �Ώۂ̍�������ɐ΂��Ђ�����Ԃ��邩�𔻒肷��
    /// </summary>
    private bool CheckReverseTopLeft(int columnIndex, int rowIndex, StoneColors color) {

        int otherColorCount = 0;

        // �N�_�ɋ߂��Ƃ��납�珇�ԂɁA��������Ƀ`�F�b�N
        foreach (var row in Rows.Where(x => x.Index < rowIndex).OrderByDescending(x => x.Index)) {

            // �P���[�v���Ƃɗ�̃C���f�b�N�X�����炷
            --columnIndex;
            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return false;
            }
            if (cell.Stone is null) {
                return false;
            }


            // �F�������ꍇ�A�����܂łɂ݂����ʐF�̐��Ŗ߂�l�����߂�
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // �T�������ɓ��F���Ȃ��ꍇ
        return false;
    }

    /// <summary>
    ///     �Ώۂ̍�������̐΂��Ђ�����Ԃ�
    /// </summary>
    private void ReverseTopLeft(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseTopLeft(columnIndex, rowIndex, color) == false) {
            return;
        }


        // �s���Aindex��DESC�Ń\�[�g�����[�v���s���A�Ώۂ��������ɏ��Ԃɕ]��
        // �P�s�]�����邲�Ƃɗ���W������炷
        foreach (var row in Rows.Where(x => x.Index < rowIndex).OrderByDescending(x => x.Index)) {

            --columnIndex;

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return;
            }

            // �΂��u����Ă��Ȃ�������I��
            if (cell.Stone is null) {
                return;
            }

            // ���F������������I��
            if (cell.Stone.Color == color) {
                return;
            }

            // �Ђ�����Ԃ�
            cell.Stone.Reverse();
        }
    }

    /// <summary>
    ///     �Ώۂ̉E�������ɐ΂��Ђ�����Ԃ��邩�𔻒肷��
    /// </summary>
    private bool CheckReverseBottomRight(int columnIndex, int rowIndex, StoneColors color) {

        int otherColorCount = 0;

        // �N�_�ɋ߂��Ƃ��납�珇�ԂɁA��������Ƀ`�F�b�N
        foreach (var row in Rows.Where(x => x.Index > rowIndex).OrderBy(x => x.Index)) {

            // �P���[�v���Ƃɗ�̃C���f�b�N�X�����炷
            ++columnIndex;
            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return false;
            }
            if (cell.Stone is null) {
                return false;
            }


            // �F�������ꍇ�A�����܂łɂ݂����ʐF�̐��Ŗ߂�l�����߂�
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // �T�������ɓ��F���Ȃ��ꍇ
        return false;
    }

    /// <summary>
    ///     �Ώۂ̉E�������̐΂��Ђ�����Ԃ�
    /// </summary>
    private void ReverseBottomRight(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseBottomRight(columnIndex, rowIndex, color) == false) {
            return;
        }


        // �s���Aindex��ASC�Ń\�[�g�����[�v���s���A�Ώۂ��牺�����ɏ��Ԃɕ]��
        // �P�s�]�����邲�Ƃɗ���W������炷
        foreach (var row in Rows.Where(x => x.Index > rowIndex).OrderBy(x => x.Index)) {

            ++columnIndex;

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return;
            }

            // �΂��u����Ă��Ȃ�������I��
            if (cell.Stone is null) {
                return;
            }

            // ���F������������I��
            if (cell.Stone.Color == color) {
                return;
            }

            // �Ђ�����Ԃ�
            cell.Stone.Reverse();
        }
    }

    /// <summary>
    ///     �Ώۂ̍��������ɐ΂��Ђ�����Ԃ��邩�𔻒肷��
    /// </summary>
    private bool CheckReverseBottomLeft(int columnIndex, int rowIndex, StoneColors color) {

        int otherColorCount = 0;

        // �N�_�ɋ߂��Ƃ��납�珇�ԂɁA��������Ƀ`�F�b�N
        foreach (var row in Rows.Where(x => x.Index > rowIndex).OrderByDescending(x => x.Index)) {

            // �P���[�v���Ƃɗ�̃C���f�b�N�X�����炷
            --columnIndex;
            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return false;
            }
            if (cell.Stone is null) {
                return false;
            }


            // �F�������ꍇ�A�����܂łɂ݂����ʐF�̐��Ŗ߂�l�����߂�
            if (color == cell.Stone.Color) {
                return (otherColorCount > 0);
            }

            otherColorCount++;
        }

        // �T�������ɓ��F���Ȃ��ꍇ
        return false;
    }

    /// <summary>
    ///     �Ώۂ̍��������̐΂��Ђ�����Ԃ�
    /// </summary>
    private void ReverseBottomLeft(int columnIndex, int rowIndex, StoneColors color) {

        if (CheckReverseBottomLeft(columnIndex, rowIndex, color) == false) {
            return;
        }


        // �s���Aindex��ASC�Ń\�[�g�����[�v���s���A�Ώۂ��牺�����ɏ��Ԃɕ]��
        // �P�s�]�����邲�Ƃɗ���W������炷
        foreach (var row in Rows.Where(x => x.Index > rowIndex).OrderByDescending(x => x.Index)) {

            --columnIndex;

            var cell = row.Cells.FirstOrDefault(x => x.OwningColumn.Index == columnIndex);
            if (cell is null) {
                return;
            }

            // �΂��u����Ă��Ȃ�������I��
            if (cell.Stone is null) {
                return;
            }

            // ���F������������I��
            if (cell.Stone.Color == color) {
                return;
            }

            // �Ђ�����Ԃ�
            cell.Stone.Reverse();
        }
    }

}
