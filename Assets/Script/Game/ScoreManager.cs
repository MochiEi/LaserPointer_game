using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //�v���O�������ϐ�
    int stageNum = 1;//1st,2nd,3rd,Total�̃X�e�[�W�w��
    float timeFloat = 0.0f;//�ꎞ�I�ȃ^�C�}�[�A�����̕ϐ��𓝍�����΋@�\�\�Atrue�Afalse�ɂ��^�C�}�[�Ǘ��̓Q�[�����̃v���O�����ŕK�v�B
    float timeSecond = 0.0f;//�����_���w�肵�����Ԃ�1st,2nd,3rd�A�ŏo�́A�`��
    float setTime;//�ŏI�̎��Ԃ�`�恕�ێ�����ϐ�
    string timeString;//Text�ϊ��p�i�����_���w��j

    //�A�^�b�`�p�ϐ�
    public Text time1;
    public Text time2;
    public Text time3;
    public Text time4;

    //��Ɏ����\��i���ԑ����j
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
                time1.text = "1st " + timeSecond + "�b";
                setTime = setTime + timeSecond;
                Debug.Log(setTime+"1");
                timeFloat = 0.0f;
            }
            if (stageNum == 2)
            {

                time2.text = "2nd " + timeSecond + "�b";
                setTime = setTime + timeSecond;
                Debug.Log(setTime+"2");
                timeFloat = 0.0f;
            }
            if (stageNum == 3)
            {

                time3.text = "3rd " + timeSecond + "�b";
                setTime = setTime + timeSecond;
                timeFloat = 0.0f;
                Debug.Log(setTime+"3");
            }
            if (stageNum == 4)
            {

                time4.text = "TotalTime " + setTime + "�b";
            }
            stageNum++;
        }
    }
}


