using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Webカメラ
public class WebCam : MonoBehaviour
{
    // UI
    RawImage rawImage;
    WebCamTexture webCamTexture;

    [SerializeField, Header("カメラ設定 (webカメラの性能に合わせて)")] 
    float camWidth = 1920.0f;
    [SerializeField] 
    float camHeight = 1080.0f;
    [SerializeField] 
    int FPS = 60;

    [SerializeField, Header("レティクル")]
    Image image;
    [SerializeField, Header("抽出色")]
    Color targetColor = Color.red;
    [SerializeField, Header("色しきい値"), Range(0, 1)]
    float colorThreshold = 1.0f;
    [SerializeField, Header("インカメ")]
    bool inCamera;

    void Start()
    {        
        // Webカメラを初期化
        webCamTexture = new WebCamTexture((int)camWidth , (int)camHeight , FPS);
        webCamTexture.Play();
        // Webカメラの開始
        this.rawImage = GetComponent<RawImage>();
        this.rawImage.texture = this.webCamTexture;
    }

    void Update()
    {
        if (webCamTexture.isPlaying)
        {
            DetectLaserPointer();
        }
    }

    void DetectLaserPointer()
    {
        Color32[] pixels = webCamTexture.GetPixels32();
        int width = webCamTexture.width;
        int height = webCamTexture.height;

        float rDiff = Mathf.Abs(pixels[0].r / 255.0f - targetColor.r);
        float gDiff = Mathf.Abs(pixels[0].g / 255.0f - targetColor.g);
        float bDiff = Mathf.Abs(pixels[0].b / 255.0f - targetColor.b);

        //Debug.Log(rDiff.ToString("n1"));
        //Debug.Log(gDiff.ToString("n1"));
        //Debug.Log(bDiff.ToString("n1"));


        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color32 pixel = pixels[y * width + x];

                if (IsLaserPointerColor(pixel))
                {
                    //Debug.Log($"Laser pointer detected at ({x}, {y})");

                    if (!inCamera)
                        image.transform.position = new Vector3(x, y, 0);
                    else
                        image.transform.position = new Vector3(camWidth - x, y, 0);
                    
                    return;
                }
            }
        }
    }

    bool IsLaserPointerColor(Color32 pixel)
    {
        float rDiff = Mathf.Abs(pixel.r / 255.0f - targetColor.r);
        float gDiff = Mathf.Abs(pixel.g / 255.0f - targetColor.g);
        float bDiff = Mathf.Abs(pixel.b / 255.0f - targetColor.b);

        if (rDiff < colorThreshold && gDiff < colorThreshold && bDiff < colorThreshold)
            Debug.Log("R:" + rDiff.ToString("n1") + "  G:" + gDiff.ToString("n1") + "  B:" + bDiff.ToString("n1"));
        
        return (rDiff < colorThreshold && gDiff < colorThreshold && bDiff < colorThreshold);
    }
}
