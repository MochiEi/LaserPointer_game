using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject[] target;
    [SerializeField] GameObject targetObject;

    private Canvas canvas;

    private float randX;
    private float randY;
    private float randScale;

    private int No;
    private bool create;
    private bool isNext = true;

    private TargetCreate targetCreate;


    void Start()
    {
        No = 0;
        canvas = transform.parent.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!create)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        if (targetObject != null)
        {
            targetCreate = targetObject.GetComponent<TargetCreate>();
            isNext = targetCreate.isNext;
        }
            
        if (No < 10 && isNext)
        {
            /// キャンバスの座標差
            float canvasX = canvas.transform.position.x / 2;
            float canvasY = canvas.transform.position.y / 2;

            randX = Random.Range(0, 960.0f + canvasX);
            randY = Random.Range(0, 540.0f + canvasY);
            randScale = Random.Range(1.0f, 5.0f);

            Vector3 pos = new Vector3(randX, randY, 0);
            float scale = randScale;

            targetObject = Instantiate(target[No], transform.position, Quaternion.identity, transform);
            targetObject.transform.position = pos;
            targetObject.transform.localScale = new Vector3(scale, scale, 1);

            isNext = false;

            No++;
        }

        Debug.Log(targetObject.name + "        " + isNext);
    }
}
