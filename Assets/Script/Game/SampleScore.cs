using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SampleScore : MonoBehaviour
{
    int stageNum = 1;        // ステージ番号
    float timeFloat = 0.0f;  // 経過時間（浮動小数点数）
    float timeSecond = 0.0f; // 経過時間（秒数）
    float setTime = 0.0f;    // 合計時間
    string timeString;       // 時間を文字列で表したもの
    string filePath = @"C:\Users\G2324\Documents\GitHub\TGS\TGS\Assets\TextScore\scores.csv"; // CSVファイルのパス
    string userName = "Player"; // ユーザー名（初期値は"Player"）

    public Text time1;       // ステージ1の時間を表示するTextオブジェクト
    public Text time2;       // ステージ2の時間を表示するTextオブジェクト
    public Text time3;       // ステージ3の時間を表示するTextオブジェクト
    public Text time4;       // 合計時間を表示するTextオブジェクト

    void Start()
    {
        // CSVファイルが存在しない場合、新規に作成する
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

        // スペースキーが押されたらスコア計測処理を実行する
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

            // 各ステージごとに時間を表示し、合計時間に加算する
            if (stageNum == 1)
            {
                time1.text = $"1st {timeSecond.ToString("N2")}秒";
                setTime += timeSecond;
            }
            else if (stageNum == 2)
            {
                time2.text = $"2nd {timeSecond.ToString("N2")}秒";
                setTime += timeSecond;
            }
            else if (stageNum == 3)
            {
                time3.text = $"3rd {timeSecond.ToString("N2")}秒";
                setTime += timeSecond;
            }
            else if (stageNum == 4)
            {
                time4.text = $"TotalTime {setTime.ToString("N2")}秒";

                // RankingMane クラスのインスタンスを取得し、ランキングを更新する
                RankingMane rankingMane = FindObjectOfType<RankingMane>();
                rankingMane.WriteToCSV(userName, setTime);
                rankingMane.ShowRanking();
            }

            stageNum++;
            timeFloat = 0.0f; // 経過時間をリセット
        }
    }

    // ユーザー名を設定するためのメソッド
    public void SetUserName(string newName)
    {
        userName = newName;
    }
}
