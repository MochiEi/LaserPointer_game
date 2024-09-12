using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //プログラム内変数
    int stageNum = 1;//1st,2nd,3rd,Totalのステージ指定
    float timeFloat = 0.0f;//一時的なタイマー、ここの変数を統合すれば機能可能、true、falseによるタイマー管理はゲーム側のプログラムで必要。
    float timeSecond = 0.0f;//小数点第二指定した時間を1st,2nd,3rd、で出力、描画
    float setTime;//最終の時間を描画＆保持する変数
    string timeString;//Text変換用（小数点第二指定）

    //アタッチ用変数
    public Text time1;
    public Text time2;
    public Text time3;
    public Text time4;

    //後に実装予定（時間足りん）
    StreamWriter rankUserScore;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeFloat += Time.deltaTime;

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

            if (stageNum == 1)
            {
                time1.text = "1st " + timeSecond + "秒";
                setTime = setTime + timeSecond;
                Debug.Log(setTime+"1");
                timeFloat = 0.0f;
            }
            if (stageNum == 2)
            {

                time2.text = "2nd " + timeSecond + "秒";
                setTime = setTime + timeSecond;
                Debug.Log(setTime+"2");
                timeFloat = 0.0f;
            }
            if (stageNum == 3)
            {

                time3.text = "3rd " + timeSecond + "秒";
                setTime = setTime + timeSecond;
                timeFloat = 0.0f;
                Debug.Log(setTime+"3");
            }
            if (stageNum == 4)
            {

                time4.text = "TotalTime " + setTime + "秒";
            }
            stageNum++;
        }
    }
}


