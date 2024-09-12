using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPinter : MonoBehaviour
{
    TargetCreate targetCreate;
    int targetNum;

    [SerializeField] TargetController targetController;
    int No;

    // Start is called before the first frame update
    void Start()
    {
        targetNum = 1;
    }

    // Update is called once per frame
    void Update()
    {
        No = targetController.No;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            targetCreate = collision.GetComponent<TargetCreate>();

            int num = targetCreate.num;

            if (No == 10)
            {
                Debug.Log(targetNum + "Å@ÅFÅ@" + num);

                if (targetNum == num)
                {
                    targetNum++;
                    Destroy(collision.gameObject);
                }

            }
        }
    }
}
