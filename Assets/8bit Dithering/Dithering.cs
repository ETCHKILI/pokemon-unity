// Author: O.V.Pavlov http://ovpavlov.blogspot.ru/ 
using UnityEngine;

[RequireComponent(typeof(Camera)), ExecuteInEditMode, AddComponentMenu("Image Effects/Color Adjustments/Dithering")]
public class Dithering : MonoBehaviour
{
    public Shader ColoredShader;
    public Shader ColloredHsvShader;
    public Shader GrayScaleShader;
    Shader DitheringShader
    { 
        get
        {
            switch (ShaderType)
            {
                case Type.Collored:
                    return ColoredShader;
                case Type.ColloredHsv:
                    return ColloredHsvShader;
                case Type.Grayscale:
                    return GrayScaleShader;
            }
            return null;
        }
    }
    public Type ShaderType;
    Material _material;
    public Texture DitheringTexture;

    public int RedSteps = 8;
    public int GreenSteps = 8;
    public int BlueSteps = 4;

    public int HueSteps = 8;
    public int SaturationSteps = 4;
    public int ValueSteps = 8;

    public int BrightnessSteps = 2;


    int _lastScreenX;
    int _lastScreenY;

    void Awake()
    {
        _lastScreenX = _lastScreenY = -1;
    }

    bool Support()
    {
        bool supported = SystemInfo.supportsImageEffects && DitheringShader && DitheringShader.isSupported;
        enabled = enabled && supported;
        return supported;
    }

    Material GetMaterial()
    {
        if (_material == null)
        {
            _material = new Material(DitheringShader);
            _material.hideFlags = HideFlags.HideAndDontSave;
            _material.SetTexture("_DitheringTex", DitheringTexture);
            SetSteps(_material);
        }
        return _material;
    }

    void SetSteps(Material material)
    {
        switch (ShaderType)
        {
            case Type.Grayscale:
                material.SetFloat("_ColorStepsA", (BrightnessSteps - 1) / 3f);
                material.SetFloat("_ColorStepsB", 1f / (BrightnessSteps - 1));
                break;
            case Type.ColloredHsv:
                material.SetVector("_ColorStepsA", new Vector4(HueSteps - 1, SaturationSteps - 1, ValueSteps - 1));
                material.SetVector("_ColorStepsB", new Vector4(1f / (HueSteps - 1), 1f / (SaturationSteps - 1), 1f / (ValueSteps - 1)));

                break;
            case Type.Collored:
                material.SetVector("_ColorStepsA", new Vector4(RedSteps - 1, GreenSteps - 1, BlueSteps - 1));
                material.SetVector("_ColorStepsB", new Vector4(1f / (RedSteps - 1), 1f / (GreenSteps - 1), 1f / (BlueSteps - 1)));
                break;
        }
    }

    public void Repaint()
    {
        _material = null;
        _lastScreenX = _lastScreenY = -1;
    }
    void OnEnable()
    {
        Repaint();
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!Support()) return;
        Material material = GetMaterial();

        if (_lastScreenX != source.width || _lastScreenY != source.height)
        {
            _lastScreenX = source.width;
            _lastScreenY = source.height;
            material.SetVector("_SceenAndTex", new Vector4(_lastScreenX / (float) DitheringTexture.width, _lastScreenY / (float) DitheringTexture.height, 0, 0));
        }
        Graphics.Blit(source, destination, material);
    }
    protected virtual void OnDisable()
    {
        if (_material) DestroyImmediate(_material);
    }

    public enum Type
    {
        Collored,
        ColloredHsv,
        Grayscale
    }
}