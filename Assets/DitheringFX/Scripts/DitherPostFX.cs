using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
[PostProcess(typeof(DitherPostFXRenderer), PostProcessEvent.AfterStack, "Custom/DitherPostFX")]
public sealed class DitherPostFX : PostProcessEffectSettings
{
    public enum DitherKernelMode { DITHER2x2, DITHER3x3, DITHER4x4, DITHER8x8 };
    [System.Serializable]
    public sealed class DitherKernelParameter : ParameterOverride<DitherKernelMode> { }

    public FloatParameter resolutionScale = new FloatParameter { value = 1.0f };
    public IntParameter colorSteps = new IntParameter { value = 256 };
    public FloatParameter centerShift = new FloatParameter { value = 0.0f };

    public DitherKernelParameter ditherKernelMode = new DitherKernelParameter { value=DitherKernelMode.DITHER2x2 };
}

public sealed class DitherPostFXRenderer : PostProcessEffectRenderer<DitherPostFX> {
    public override void Render(PostProcessRenderContext context) {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/DitherPostFX"));
        sheet.DisableKeyword("DITHER2X2");
        sheet.DisableKeyword("DITHER3X3");
        sheet.DisableKeyword("DITHER4X4");
        sheet.DisableKeyword("DITHER8X8");
        switch (settings.ditherKernelMode.value) {
            case DitherPostFX.DitherKernelMode.DITHER2x2: sheet.EnableKeyword("DITHER2X2"); break;
            case DitherPostFX.DitherKernelMode.DITHER3x3: sheet.EnableKeyword("DITHER3X3"); break;
            case DitherPostFX.DitherKernelMode.DITHER4x4: sheet.EnableKeyword("DITHER4X4"); break;
            case DitherPostFX.DitherKernelMode.DITHER8x8: sheet.EnableKeyword("DITHER8X8"); break;
        }
        sheet.properties.SetFloat("_CenterShift", settings.centerShift.value);
        sheet.properties.SetVector("_Resolution", new Vector2( context.width * settings.resolutionScale, context.height * settings.resolutionScale ));
        sheet.properties.SetInt("_ColorSteps", settings.colorSteps);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
