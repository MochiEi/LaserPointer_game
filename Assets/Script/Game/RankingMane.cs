using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class RankingMane : MonoBehaviour
{
    public GameObject scoreCanvas;

    // CSVファイルのパス
    private string filePath;
    public Text rankTextPrefab; // テキストプレハブ
    private List<Text> instantiatedTexts = new List<Text>(); // 生成したテキストを管理するリスト

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "scores.csv");

        // テスト用のデータを書き込み（必要に応じて削除）
        WriteToCSV("NO NAME", 25.0f);
        WriteToCSV("NO NAME", 20.5f);
        WriteToCSV("NO NAME", 30.2f);

        // ランキングを表示する
        ShowRanking();
    }

    void Update()
    {
        // Delete キーが押されたらデータを削除する
        if (Input.GetKeyDown(KeyCode.P))
        {
            DeleteAllDataFromCSV(); // CSVファイルからすべてのデータを削除する4
           
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

    // テキストを動的に生成するメソッド
    public void TextCreater(Transform parentTransform, string textContent, float width, float height)
    {
        // ランキングテキストのインスタンス化
        Text newTextComponent = Instantiate(rankTextPrefab, parentTransform);
        newTextComponent.text = textContent;
        newTextComponent.rectTransform.localScale = Vector3.one; // スケールを1に設定
        newTextComponent.rectTransform.sizeDelta = new Vector2(width+100, height+10); // 幅と高さを設定
        newTextComponent.font = Font.CreateDynamicFontFromOSFont("Arial Unicode MS", 40); // フォントを設定
        newTextComponent.color = Color.black;
        newTextComponent.alignment = TextAnchor.MiddleCenter;

        // 生成したテキストをリストに追加
        instantiatedTexts.Add(newTextComponent);

        // 重ならないようにY座標を調整する
        float yOffset = instantiatedTexts.Count * height * 1.2f; // 1.2fはテキストの間隔調整に適した値
        newTextComponent.rectTransform.localPosition = new Vector3(0, -yOffset+100, 0);
        newTextComponent.alignment = TextAnchor.MiddleLeft; // 左揃えに設定
    }

    // CSVファイルにデータを書き込むメソッド
    public void WriteToCSV(string userName, float setTime)
    {
        try
        {
            // ランキング表示をクリア
            ClearTexts();

            // 既存のランキングデータを読み込む
            List<RankingData> rankingDataList = LoadRankingData();

            // 新しいデータを追加
            rankingDataList.Add(new RankingData(userName, setTime));

            // ランキングデータをsetTimeの昇順にソートする
            rankingDataList.Sort((x, y) => x.setTime.CompareTo(y.setTime));

            // 順位と順番を更新
            UpdateRankAndOrder(rankingDataList);

            // ランキングデータをCSVファイルに書き込む
            SaveRankingData(rankingDataList);

            // ランキングを再表示する
            ShowRanking();
        }
        catch (IOException e)
        {
            Debug.LogError($"Error writing to CSV file: {e}");
        }
    }

    // CSVファイルからランキングを読み込んで返すメソッド
    private List<RankingData> LoadRankingData()
    {
        List<RankingData> rankingDataList = new List<RankingData>();

        try
        {
            // CSVファイルを読み込んで、データを格納するリストを作成する
            if (File.Exists(filePath))
            {
                List<string> lines = File.ReadAllLines(filePath).ToList();

                foreach (string line in lines)
                {
                    string[] entries = line.Split(',');
                    if (entries.Length >= 2)
                    {
                        string userName = entries[0];
                        float setTime = float.Parse(entries[1].Replace("秒", ""));
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

    // 順位と順番を更新するメソッド
    private void UpdateRankAndOrder(List<RankingData> rankingDataList)
    {
        int rank = 1;
        foreach (RankingData data in rankingDataList)
        {
            data.rank = rank;
            rank++;
        }
    }

    // ランキングデータをCSVファイルに書き込むメソッド
    private void SaveRankingData(List<RankingData> rankingDataList)
    {
        try
        {
            // CSVファイルにランキングデータを書き込む
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (RankingData data in rankingDataList)
                {
                    writer.WriteLine($"{data.userName},{data.setTime.ToString("N2")}秒");
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Error writing to CSV file: {e}");
        }
    }

    // CSVファイルから特定のユーザーのデータを削除するメソッド
    public void DeleteUserFromCSV(string userName)
    {
        try
        {
            // 既存のランキングデータを読み込む
            List<RankingData> rankingDataList = LoadRankingData();

            // 対象のユーザーを削除
            rankingDataList.RemoveAll(data => data.userName == userName);

            // 順位と順番を更新
            UpdateRankAndOrder(rankingDataList);

            // ランキングデータをCSVファイルに書き込む
            SaveRankingData(rankingDataList);

            // ランキングを再表示する
            ShowRanking();
        }
        catch (IOException e)
        {
            Debug.LogError($"Error deleting user '{userName}' from CSV file: {e}");
        }
    }

    // ランキングデータをクリアするメソッド
    private void ClearTexts()
    {
        foreach (var text in instantiatedTexts)
        {
            Destroy(text.gameObject);
        }
        instantiatedTexts.Clear();
    }

    // CSVファイルからランキングを読み込んで表示するメソッド
    public void ShowRanking()
    {
        try
        {
            // ランキング表示をクリア
            ClearTexts();

            // CSVファイルを読み込んで、データを格納するリストを作成する
            List<RankingData> rankingDataList = LoadRankingData();

            // ランキングデータをsetTimeの昇順にソートする
            rankingDataList.Sort((x, y) => x.setTime.CompareTo(y.setTime));

            // 上位5位までのランキングの表示
            int rank = 1;
            foreach (RankingData data in rankingDataList.Take(5)) // 上位5位までを表示
            {
                TextCreater(rankTextPrefab.transform.parent, $"{rank}位: {data.userName} - {data.setTime.ToString("N2")}秒", 400, 40);
                rank++;
            }

            // 残りのランキングを "noname" で埋める
            int remainingSlots = 5 - rankingDataList.Count;
            for (int i = 1; i <= remainingSlots; i++)
            {
                TextCreater(rankTextPrefab.transform.parent, $"{rank}位: NO NAME{i} - --.-秒", 400, 40); // ダミーデータとして表示
                rank++;
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Error reading CSV file: {e}");
        }
    }
    // ユーザー名と設置時間を持つランキングデータのクラス
    private class RankingData
    {
        public string userName;
        public float setTime;
        public int rank; // 順位を保持するフィールド

        public RankingData(string name, float time)
        {
            userName = name;
            setTime = time;
            rank = 0; // 初期値は0で、後で計算で上書きする
        }
    }

    public void DeleteAllDataFromCSV()
    {
        try
        {
            // すべてのランキングデータをクリアする
            List<RankingData> rankingDataList = new List<RankingData>();

            // 順位を更新します（データがないので、すべて0になります）
            UpdateRankAndOrder(rankingDataList);

            // 空のランキングデータリストをCSVファイルに保存します
            SaveRankingData(rankingDataList);

            // 更新されたランキングを表示します（空になります）
            ShowRanking();

            // 全てのランキングデータを "noname" の次にあるデータの設置時間を 0秒 にします
            List<RankingData> updatedRankingDataList = LoadRankingData();
            foreach (RankingData data in updatedRankingDataList)
            {
                if (data.userName.StartsWith("NO NAME"))
                {
                    data.setTime = 0f;
                }
            }

            // 更新したランキングデータをCSVファイルに書き込みます
            SaveRankingData(updatedRankingDataList);

            // ランキングを再表示します
            ShowRanking();
        }
        catch (IOException e)
        {
            Debug.LogError($"CSVファイルからすべてのデータを削除する際にエラーが発生しました: {e}");
        }
    }
}