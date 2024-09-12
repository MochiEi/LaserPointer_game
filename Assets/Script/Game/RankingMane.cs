using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class RankingMane : MonoBehaviour
{
    public GameObject scoreCanvas;

    // CSV�t�@�C���̃p�X
    private string filePath;
    public Text rankTextPrefab; // �e�L�X�g�v���n�u
    private List<Text> instantiatedTexts = new List<Text>(); // ���������e�L�X�g���Ǘ����郊�X�g

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "scores.csv");

        // �e�X�g�p�̃f�[�^���������݁i�K�v�ɉ����č폜�j
        WriteToCSV("NO NAME", 25.0f);
        WriteToCSV("NO NAME", 20.5f);
        WriteToCSV("NO NAME", 30.2f);

        // �����L���O��\������
        ShowRanking();
    }

    void Update()
    {
        // Delete �L�[�������ꂽ��f�[�^���폜����
        if (Input.GetKeyDown(KeyCode.P))
        {
            DeleteAllDataFromCSV(); // CSV�t�@�C�����炷�ׂẴf�[�^���폜����4
           
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
           scoreCanvas.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            scoreCanvas.gameObject.SetActive(true);
        }

    }

    // �e�L�X�g�𓮓I�ɐ������郁�\�b�h
    public void TextCreater(Transform parentTransform, string textContent, float width, float height)
    {
        // �����L���O�e�L�X�g�̃C���X�^���X��
        Text newTextComponent = Instantiate(rankTextPrefab, parentTransform);
        newTextComponent.text = textContent;
        newTextComponent.rectTransform.localScale = Vector3.one; // �X�P�[����1�ɐݒ�
        newTextComponent.rectTransform.sizeDelta = new Vector2(width+100, height+10); // ���ƍ�����ݒ�
        newTextComponent.font = Font.CreateDynamicFontFromOSFont("Arial Unicode MS", 40); // �t�H���g��ݒ�
        newTextComponent.color = Color.black;
        newTextComponent.alignment = TextAnchor.MiddleCenter;

        // ���������e�L�X�g�����X�g�ɒǉ�
        instantiatedTexts.Add(newTextComponent);

        // �d�Ȃ�Ȃ��悤��Y���W�𒲐�����
        float yOffset = instantiatedTexts.Count * height * 1.2f; // 1.2f�̓e�L�X�g�̊Ԋu�����ɓK�����l
        newTextComponent.rectTransform.localPosition = new Vector3(0, -yOffset+100, 0);
        newTextComponent.alignment = TextAnchor.MiddleLeft; // �������ɐݒ�
    }

    // CSV�t�@�C���Ƀf�[�^���������ރ��\�b�h
    public void WriteToCSV(string userName, float setTime)
    {
        try
        {
            // �����L���O�\�����N���A
            ClearTexts();

            // �����̃����L���O�f�[�^��ǂݍ���
            List<RankingData> rankingDataList = LoadRankingData();

            // �V�����f�[�^��ǉ�
            rankingDataList.Add(new RankingData(userName, setTime));

            // �����L���O�f�[�^��setTime�̏����Ƀ\�[�g����
            rankingDataList.Sort((x, y) => x.setTime.CompareTo(y.setTime));

            // ���ʂƏ��Ԃ��X�V
            UpdateRankAndOrder(rankingDataList);

            // �����L���O�f�[�^��CSV�t�@�C���ɏ�������
            SaveRankingData(rankingDataList);

            // �����L���O���ĕ\������
            ShowRanking();
        }
        catch (IOException e)
        {
            Debug.LogError($"Error writing to CSV file: {e}");
        }
    }

    // CSV�t�@�C�����烉���L���O��ǂݍ���ŕԂ����\�b�h
    private List<RankingData> LoadRankingData()
    {
        List<RankingData> rankingDataList = new List<RankingData>();

        try
        {
            // CSV�t�@�C����ǂݍ���ŁA�f�[�^���i�[���郊�X�g���쐬����
            if (File.Exists(filePath))
            {
                List<string> lines = File.ReadAllLines(filePath).ToList();

                foreach (string line in lines)
                {
                    string[] entries = line.Split(',');
                    if (entries.Length >= 2)
                    {
                        string userName = entries[0];
                        float setTime = float.Parse(entries[1].Replace("�b", ""));
                        rankingDataList.Add(new RankingData(userName, setTime));
                    }
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Error reading CSV file: {e}");
        }

        return rankingDataList;
    }

    // ���ʂƏ��Ԃ��X�V���郁�\�b�h
    private void UpdateRankAndOrder(List<RankingData> rankingDataList)
    {
        int rank = 1;
        foreach (RankingData data in rankingDataList)
        {
            data.rank = rank;
            rank++;
        }
    }

    // �����L���O�f�[�^��CSV�t�@�C���ɏ������ރ��\�b�h
    private void SaveRankingData(List<RankingData> rankingDataList)
    {
        try
        {
            // CSV�t�@�C���Ƀ����L���O�f�[�^����������
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (RankingData data in rankingDataList)
                {
                    writer.WriteLine($"{data.userName},{data.setTime.ToString("N2")}�b");
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Error writing to CSV file: {e}");
        }
    }

    // CSV�t�@�C���������̃��[�U�[�̃f�[�^���폜���郁�\�b�h
    public void DeleteUserFromCSV(string userName)
    {
        try
        {
            // �����̃����L���O�f�[�^��ǂݍ���
            List<RankingData> rankingDataList = LoadRankingData();

            // �Ώۂ̃��[�U�[���폜
            rankingDataList.RemoveAll(data => data.userName == userName);

            // ���ʂƏ��Ԃ��X�V
            UpdateRankAndOrder(rankingDataList);

            // �����L���O�f�[�^��CSV�t�@�C���ɏ�������
            SaveRankingData(rankingDataList);

            // �����L���O���ĕ\������
            ShowRanking();
        }
        catch (IOException e)
        {
            Debug.LogError($"Error deleting user '{userName}' from CSV file: {e}");
        }
    }

    // �����L���O�f�[�^���N���A���郁�\�b�h
    private void ClearTexts()
    {
        foreach (var text in instantiatedTexts)
        {
            Destroy(text.gameObject);
        }
        instantiatedTexts.Clear();
    }

    // CSV�t�@�C�����烉���L���O��ǂݍ���ŕ\�����郁�\�b�h
    public void ShowRanking()
    {
        try
        {
            // �����L���O�\�����N���A
            ClearTexts();

            // CSV�t�@�C����ǂݍ���ŁA�f�[�^���i�[���郊�X�g���쐬����
            List<RankingData> rankingDataList = LoadRankingData();

            // �����L���O�f�[�^��setTime�̏����Ƀ\�[�g����
            rankingDataList.Sort((x, y) => x.setTime.CompareTo(y.setTime));

            // ���5�ʂ܂ł̃����L���O�̕\��
            int rank = 1;
            foreach (RankingData data in rankingDataList.Take(5)) // ���5�ʂ܂ł�\��
            {
                TextCreater(rankTextPrefab.transform.parent, $"{rank}��: {data.userName} - {data.setTime.ToString("N2")}�b", 400, 40);
                rank++;
            }

            // �c��̃����L���O�� "noname" �Ŗ��߂�
            int remainingSlots = 5 - rankingDataList.Count;
            for (int i = 1; i <= remainingSlots; i++)
            {
                TextCreater(rankTextPrefab.transform.parent, $"{rank}��: NO NAME{i} - --.-�b", 400, 40); // �_�~�[�f�[�^�Ƃ��ĕ\��
                rank++;
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Error reading CSV file: {e}");
        }
    }
    // ���[�U�[���Ɛݒu���Ԃ��������L���O�f�[�^�̃N���X
    private class RankingData
    {
        public string userName;
        public float setTime;
        public int rank; // ���ʂ�ێ�����t�B�[���h

        public RankingData(string name, float time)
        {
            userName = name;
            setTime = time;
            rank = 0; // �����l��0�ŁA��Ōv�Z�ŏ㏑������
        }
    }

    public void DeleteAllDataFromCSV()
    {
        try
        {
            // ���ׂẴ����L���O�f�[�^���N���A����
            List<RankingData> rankingDataList = new List<RankingData>();

            // ���ʂ��X�V���܂��i�f�[�^���Ȃ��̂ŁA���ׂ�0�ɂȂ�܂��j
            UpdateRankAndOrder(rankingDataList);

            // ��̃����L���O�f�[�^���X�g��CSV�t�@�C���ɕۑ����܂�
            SaveRankingData(rankingDataList);

            // �X�V���ꂽ�����L���O��\�����܂��i��ɂȂ�܂��j
            ShowRanking();

            // �S�Ẵ����L���O�f�[�^�� "noname" �̎��ɂ���f�[�^�̐ݒu���Ԃ� 0�b �ɂ��܂�
            List<RankingData> updatedRankingDataList = LoadRankingData();
            foreach (RankingData data in updatedRankingDataList)
            {
                if (data.userName.StartsWith("NO NAME"))
                {
                    data.setTime = 0f;
                }
            }

            // �X�V���������L���O�f�[�^��CSV�t�@�C���ɏ������݂܂�
            SaveRankingData(updatedRankingDataList);

            // �����L���O���ĕ\�����܂�
            ShowRanking();
        }
        catch (IOException e)
        {
            Debug.LogError($"CSV�t�@�C�����炷�ׂẴf�[�^���폜����ۂɃG���[���������܂���: {e}");
        }
    }
}