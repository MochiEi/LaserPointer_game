using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SampleScore : MonoBehaviour
{
    int stageNum = 1;        // �X�e�[�W�ԍ�
    float timeFloat = 0.0f;  // �o�ߎ��ԁi���������_���j
    float timeSecond = 0.0f; // �o�ߎ��ԁi�b���j
    float setTime = 0.0f;    // ���v����
    string timeString;       // ���Ԃ𕶎���ŕ\��������
    string filePath = @"C:\Users\G2324\Documents\GitHub\TGS\TGS\Assets\TextScore\scores.csv"; // CSV�t�@�C���̃p�X
    string userName = "Player"; // ���[�U�[���i�����l��"Player"�j

    public Text time1;       // �X�e�[�W1�̎��Ԃ�\������Text�I�u�W�F�N�g
    public Text time2;       // �X�e�[�W2�̎��Ԃ�\������Text�I�u�W�F�N�g
    public Text time3;       // �X�e�[�W3�̎��Ԃ�\������Text�I�u�W�F�N�g
    public Text time4;       // ���v���Ԃ�\������Text�I�u�W�F�N�g

    void Start()
    {
        // CSV�t�@�C�������݂��Ȃ��ꍇ�A�V�K�ɍ쐬����
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            Debug.Log($"CSV file created at: {filePath}");
        }
        else
        {
            Debug.Log($"CSV file already exists at: {filePath}");
        }
    }

    void Update()
    {
        timeFloat += Time.deltaTime;

        // �X�y�[�X�L�[�������ꂽ��X�R�A�v�����������s����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TimeScoreMane();
        }
    }

    public void TimeScoreMane()
    {
        if (stageNum <= 4)
        {
            timeString = timeFloat.ToString("N2");
            timeSecond = float.Parse(timeString);

            // �e�X�e�[�W���ƂɎ��Ԃ�\�����A���v���Ԃɉ��Z����
            if (stageNum == 1)
            {
                time1.text = $"1st {timeSecond.ToString("N2")}�b";
                setTime += timeSecond;
            }
            else if (stageNum == 2)
            {
                time2.text = $"2nd {timeSecond.ToString("N2")}�b";
                setTime += timeSecond;
            }
            else if (stageNum == 3)
            {
                time3.text = $"3rd {timeSecond.ToString("N2")}�b";
                setTime += timeSecond;
            }
            else if (stageNum == 4)
            {
                time4.text = $"TotalTime {setTime.ToString("N2")}�b";

                // RankingMane �N���X�̃C���X�^���X���擾���A�����L���O���X�V����
                RankingMane rankingMane = FindObjectOfType<RankingMane>();
                rankingMane.WriteToCSV(userName, setTime);
                rankingMane.ShowRanking();
            }

            stageNum++;
            timeFloat = 0.0f; // �o�ߎ��Ԃ����Z�b�g
        }
    }

    // ���[�U�[����ݒ肷�邽�߂̃��\�b�h
    public void SetUserName(string newName)
    {
        userName = newName;
    }
}
