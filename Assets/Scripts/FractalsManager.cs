using System;
using System.IO;
using UnityEngine;

public class FractalsManager : MonoBehaviour
{
    [SerializeField]
    private ComputeShader fractals;
    public RenderTexture fractalTexture;

    [SerializeField]
    [Range(0, 1000)]
    private float iterations;
    [SerializeField]
    private float cap;

    Vector2 position = new Vector2(-0.5f, 0f);
    Vector2 juliaPos = Vector2.zero;
    [SerializeField]
    float zoom = 1;

    [SerializeField]
    Vector3Int rgb;
    [SerializeField]
    Vector3Int rgbOffset;
    [SerializeField]
    [Range(0, 1)]
    float percentage;
    [SerializeField]
    bool useZoomIterations;
    [SerializeField]
    float zoomMultiplier = 1;
    readonly float speed = 1;

    [SerializeField]
    private bool takeScreenshot = false;

    // Start is called before the first frame update
    void Start()
    {
        Directory.CreateDirectory($"{Application.persistentDataPath}\\Screenshots");

        fractalTexture = new RenderTexture(2560, 1440, 1, RenderTextureFormat.ARGBFloat);
        fractalTexture.enableRandomWrite = true;
        fractalTexture.Create();

        fractals.SetTexture(0, "_texture", fractalTexture);
        fractals.SetTexture(1, "_texture", fractalTexture);
        fractals.SetInt("_width", fractalTexture.width);
        fractals.SetInt("_height", fractalTexture.height);

        transform.GetComponentInChildren<MeshRenderer>().material.mainTexture = fractalTexture;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();
        UpdateFractalsShader();

        if (takeScreenshot)
        {
            takeScreenshot = false;

            string path = $"{Application.persistentDataPath}\\Screenshots\\{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.png";

            ScreenCapture.CaptureScreenshot(path, 2);
        }
    }

    void UpdateInputs()
    {
        if (Input.GetKey(KeyCode.W))
        {
            position += Time.deltaTime * speed / zoom * Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            position += Time.deltaTime * speed / zoom * Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            position += Time.deltaTime * speed / zoom * Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            position += Time.deltaTime * speed / zoom * Vector2.right;
        }

        if (Input.GetKey(KeyCode.I))
        {
            juliaPos += Time.deltaTime * speed / zoom * Vector2.up;
        }
        if (Input.GetKey(KeyCode.J))
        {
            juliaPos += Time.deltaTime * speed / zoom * Vector2.left;
        }
        if (Input.GetKey(KeyCode.K))
        {
            juliaPos += Time.deltaTime * speed / zoom * Vector2.down;
        }
        if (Input.GetKey(KeyCode.L))
        {
            juliaPos += Time.deltaTime * speed / zoom * Vector2.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            zoom *= 1 - (Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.E))
        {
            zoom *= 1 + (Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.R))
        {
            iterations++;
        }
        if (Input.GetKey(KeyCode.T))
        {
            iterations--;
            if (iterations < 0) iterations = 0;
        }
        if (Input.GetKey(KeyCode.F))
        {
            iterations += Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.G))
        {
            iterations -= Time.deltaTime * 10;
            if (iterations < 0) iterations = 0;
        }

        if (Input.GetKey(KeyCode.X))
        {
            percentage += Time.deltaTime;
            if (percentage > 1) percentage = 1;
        }
        if (Input.GetKey(KeyCode.C))
        {
            percentage -= Time.deltaTime;
            if (percentage < 0) percentage = 0;
        }
    }

    void UpdateFractalsShader()
    {
        fractals.SetInt("_iterations", (int)iterations);
        fractals.SetFloat("_cap", cap);
        fractals.SetFloats("_juliaPos", juliaPos.x, juliaPos.y);
        fractals.SetFloats("_position", position.x, position.y);
        fractals.SetFloat("_zoom", zoom);
        fractals.SetInts("_rgb", rgb.x, rgb.y, rgb.z);
        fractals.SetInts("_rgbOffset", rgbOffset.x, rgbOffset.y, rgbOffset.z);
        fractals.SetFloat("_test", zoomMultiplier);
        fractals.SetFloat("_percentage", percentage);
        fractals.SetBool("_useZoomIterations", useZoomIterations);
        fractals.SetFloat("_zoomMultiplier", zoomMultiplier);

        int threadGroupsX = (fractalTexture.width / 32) + 1;
        int threadGroupsY = (fractalTexture.height / 32) + 1;

        fractals.Dispatch(1, threadGroupsX, threadGroupsY, 1);
        fractals.Dispatch(0, threadGroupsX, threadGroupsY, 1);
    }
}

