using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RawImage))]
public class UIMesh : MonoBehaviour
{
    public RenderTexture texture;
    public Mesh mesh;
    public Material material;

    private RawImage _rawImage;
    public RawImage rawImage
    {
        get
        {
            if (!_rawImage)
            {
                _rawImage = GetComponent<RawImage>();
            }
            return _rawImage;
        }
    }

    public Vector3 position = Vector3.zero;
    public Vector3 rotation = Vector3.zero;
    public Vector3 scale = Vector3.one;
    public float uniformScale = 1.0f;

    public float rotationSpeed = 10.0f;
    float rot;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            rot += rotationSpeed * Time.deltaTime;
            StartCoroutine(OnPostRender());
        }
        else
        {
            Render();
        }
    }

    IEnumerator OnPostRender()
    {
        yield return new WaitForEndOfFrame();

        Render();
    }

    void Init()
    {
        texture = new RenderTexture(256, 256, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        rawImage.texture = texture;
    }

    void Render()
    {
        RenderTexture currentActiveRT = RenderTexture.active;
        Graphics.SetRenderTarget(texture);
        GL.Clear(true, true, Color.clear);

        GL.PushMatrix();
        GL.LoadOrtho();

        if (material.SetPass(0))
        {
            Graphics.DrawMeshNow(mesh,
                Matrix4x4.Translate(position) * Matrix4x4.Translate(new Vector3(0.5f, 0.5f)) *
                Matrix4x4.Rotate(Quaternion.Euler(0, rot, 0)) * Matrix4x4.Rotate(Quaternion.Euler(rotation)) *
                Matrix4x4.Scale(scale) * Matrix4x4.Scale(Vector3.one * uniformScale)
            );
        }

        GL.PopMatrix();
        if (currentActiveRT != null) RenderTexture.active = currentActiveRT;
    }
}