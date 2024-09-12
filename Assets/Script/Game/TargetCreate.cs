using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetCreate : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    private float randX;
    private float randY;
    private float randScale;

    private float delayTime;

    public bool isNext;
    // Start is called before the first frame update
    void Start()
    {
        isNext = false;
        delayTime = 0;
        canvas = transform.parent.parent.GetComponent<Canvas>();       
    }

    // Update is called once per frame
    void Update()
    {
        if (!isNext)
        {
            delayTime += Time.deltaTime;

            if (delayTime > 0.5f)
            {
                isNext = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("ScreenCollider") && !isNext)
        {
            Debug.Log(collision.gameObject.name);

            /// キャンバスの座標差
            float canvasX = canvas.transform.position.x / 2;
            float canvasY = canvas.transform.position.y / 2;

            randX = Random.Range(0, 960.0f + canvasX);
            randY = Random.Range(0, 540.0f + canvasY);
            randScale = Random.Range(1.0f, 5.0f);

            Vector3 pos = new Vector3(randX, randY, -10);
            float scale = randScale;

            transform.position = pos;
            transform.localScale = new Vector3(scale, scale, 1);

            delayTime = 0;
        }
    }
}
