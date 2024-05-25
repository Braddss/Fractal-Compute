using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MandelbrotSet : MonoBehaviour
{
    [SerializeField]
    private ComputeShader mandelBrot;
    public RenderTexture mandelTexture;

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
    int r = 1;
    [SerializeField]
    int g = 1;
    [SerializeField]
    int b = 1;
    [SerializeField]
    float test = 1;
    [SerializeField]
    Transform c;
    [SerializeField]
    [Range(0,1)]
    float percentage;
    [SerializeField]
    bool useZoomIterations;
    float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        mandelTexture = new RenderTexture(2560, 1440, 1, RenderTextureFormat.ARGBFloat);
        mandelTexture.enableRandomWrite = true;
        mandelTexture.Create();

        mandelBrot.SetTexture(0, "MandelTexture", mandelTexture);
        mandelBrot.SetTexture(1, "MandelTexture", mandelTexture);
        mandelBrot.SetTexture(2, "MandelTexture", mandelTexture);
        mandelBrot.SetInt("_width", mandelTexture.width);
        mandelBrot.SetInt("_height", mandelTexture.height);

        transform.GetComponentInChildren<MeshRenderer>().material.mainTexture = mandelTexture;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();
        UpdateShader();
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
            zoom *= 1 - Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            zoom *= 1 + Time.deltaTime * speed;
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

        if (Input.GetKey(KeyCode.Y))
        {
            percentage += Time.deltaTime;
            if (percentage > 1) percentage = 1;
        }
        if (Input.GetKey(KeyCode.X))
        {
            percentage -= Time.deltaTime;
            if (percentage < 0) percentage = 0;
        }
    }

    void UpdateShader()
    {
        mandelBrot.SetInt("_iterations", (int)iterations);
        mandelBrot.SetFloat("cap", cap);
        mandelBrot.SetFloats("cVal", juliaPos.x, juliaPos.y);
        mandelBrot.SetFloats("position", position.x, position.y);
        mandelBrot.SetFloat("zoom", zoom);
        mandelBrot.SetInt("r", r);
        mandelBrot.SetInt("g", g);
        mandelBrot.SetInt("b", b);
        mandelBrot.SetFloat("test", test);
        mandelBrot.SetFloat("percentage", percentage);
        mandelBrot.SetBool("useZoomIterations", useZoomIterations);

        int threadGroupsX = mandelTexture.width / 32 + 1;
        int threadGroupsY = mandelTexture.height / 32 + 1;

        mandelBrot.Dispatch(2, threadGroupsX, threadGroupsY, 1);
        if(percentage == 0)
        {
            mandelBrot.Dispatch(1, threadGroupsX, threadGroupsY, 1);
        }
        else if(percentage == 1)
        {
            mandelBrot.Dispatch(0, threadGroupsX, threadGroupsY, 1);
        }
        else
        {
            mandelBrot.Dispatch(0, threadGroupsX, threadGroupsY, 1);
            mandelBrot.Dispatch(1, threadGroupsX, threadGroupsY, 1);
        }
    }
}

