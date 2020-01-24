using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(CellShadingRenderer), PostProcessEvent.AfterStack, "Custom/CellShading")]
public sealed class  CellShadingScript : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Grayscale effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };
}

public sealed class CellShadingRenderer : PostProcessEffectRenderer<CellShadingScript>
{
    public override void Render(PostProcessRenderContext context)
    {
        //context;
        var sheet = context.propertySheets.Get(Shader.Find("Custom/CellShading"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}