using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(CellShadingRenderer), PostProcessEvent.BeforeStack, "Shader Graphs/PosterizeGraph")]
public sealed class  CellShadingScript : PostProcessEffectSettings
{
    [Tooltip("posterize band amount")]
    public IntParameter posterize = new IntParameter ();
}

public sealed class CellShadingRenderer : PostProcessEffectRenderer<CellShadingScript>
{
    public override void Render(PostProcessRenderContext context)
    {
        //context;
        var sheet = context.propertySheets.Get(Shader.Find("Shader Graphs/posterizeGraph"));
        sheet.properties.SetFloat("Steps", settings.posterize);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet,1 );
    }
}