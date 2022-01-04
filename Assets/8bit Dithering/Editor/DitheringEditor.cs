// Author: O.V.Pavlov http://ovpavlov.blogspot.ru/ 
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dithering))]
public class DitheringEditor: Editor
{
    Dithering _dithering;
    public override void OnInspectorGUI()
    {
        if (_dithering == null) _dithering = (Dithering)target;
        //_dithering.GrayScaleShader = (Shader)EditorGUILayout.ObjectField("GrayScaleShader", _dithering.GrayScaleShader, typeof(Shader));
        //_dithering.ColoredShader = (Shader)EditorGUILayout.ObjectField("ColoredShader", _dithering.ColoredShader, typeof(Shader));
        //_dithering.ColloredHsvShader = (Shader)EditorGUILayout.ObjectField("ColloredHsvShader", _dithering.ColloredHsvShader, typeof(Shader));


        _dithering.DitheringTexture = (Texture) EditorGUILayout.ObjectField("dithering Tex", _dithering.DitheringTexture, typeof(Texture), true);
        _dithering.ShaderType = (Dithering.Type)EditorGUILayout.EnumPopup("Type", _dithering.ShaderType);
        switch (_dithering.ShaderType)
        {
            case Dithering.Type.Collored:
                ComponentField("Red Steps", ref _dithering.RedSteps);
                ComponentField("Green Steps", ref _dithering.GreenSteps);
                ComponentField("Blue Steps", ref _dithering.BlueSteps);
                DrawBitInfo('R', 'G', 'B', _dithering.RedSteps, _dithering.GreenSteps, _dithering.BlueSteps);
                break;
            case Dithering.Type.ColloredHsv:
                ComponentField("Hue Steps", ref _dithering.HueSteps);
                ComponentField("Saturation Steps", ref _dithering.SaturationSteps);
                ComponentField("Value Steps", ref _dithering.ValueSteps);
                DrawBitInfo('H', 'S', 'V', _dithering.HueSteps, _dithering.SaturationSteps, _dithering.ValueSteps);
                break;
            case Dithering.Type.Grayscale:
                ComponentField("Brightness Steps", ref _dithering.BrightnessSteps);
                break;
        }

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Open Dithering Texture Creator")) CreateDitheringTexWizard.CreateWizard();
        GUILayout.EndHorizontal();

        if (GUI.changed)
        {
            _dithering.Repaint();
        }
    }
    static void ComponentField(string text, ref int value)
    {
        EditorGUILayout.BeginHorizontal();
        value = EditorGUILayout.IntField(text, value);
        value = Mathf.Max(value, 2);
        GUILayout.Label(PowerOfTwo(value) + " Bit", GUILayout.ExpandWidth(false), GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();
    }
    static int PowerOfTwo(int val)
    {
        if (val <= 2) return 1;
        --val;
        for (int i = 0; i < 32; i++)
        {
            int n = 1 << i;
            if (val < n) return i;
        }
        return 32;
    }
    static string GetBitInfo(char c0, char c1, char c2, int v0, int v1, int v2)
    {
        int pot0 = PowerOfTwo(v0);
        int pot1 = PowerOfTwo(v1);
        int pot2 = PowerOfTwo(v2);

        string s0 = new string(c0, pot0);
        string s1 = new string(c1, pot1);
        string s2 = new string(c2, pot2);
        return string.Concat(s0, s1, s2, " ", (pot0 + pot1 + pot2), " Bit");
    }
     static void DrawBitInfo(char c0, char c1, char c2, int v0, int v1, int v2)
     {
         GUILayout.BeginHorizontal();
         GUILayout.FlexibleSpace();
         GUILayout.Label(GetBitInfo(c0, c1, c2, v0, v1, v2));
         GUILayout.Space(30);
         GUILayout.EndHorizontal();
     }

}
