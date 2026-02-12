using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UIBlurRendererFeature : ScriptableRendererFeature
{
  class BlurPass : ScriptableRenderPass
  {
    private Material blurMaterial;
    private RTHandle tempTexture;

    public BlurPass(Material material)
    {
      blurMaterial = material;
      renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
      ConfigureInput(ScriptableRenderPassInput.Color);

      var descriptor = renderingData.cameraData.cameraTargetDescriptor;
      descriptor.depthBufferBits = 0;

      RenderingUtils.ReAllocateIfNeeded(
          ref tempTexture,
          descriptor,
          FilterMode.Bilinear,
          TextureWrapMode.Clamp,
          name: "_UIBlurTemp"
      );
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
      if (blurMaterial == null)
        return;

      CommandBuffer cmd = CommandBufferPool.Get("UI Blur");

      var source = renderingData.cameraData.renderer.cameraColorTargetHandle;

      Blit(cmd, source, tempTexture, blurMaterial, 0);
      Blit(cmd, tempTexture, source);

      context.ExecuteCommandBuffer(cmd);
      CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
      if (tempTexture != null)
        tempTexture.Release();
    }
  }

  public Material blurMaterial;
  BlurPass blurPass;

  public override void Create()
  {
    blurPass = new BlurPass(blurMaterial);
  }

  public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
  {
    renderer.EnqueuePass(blurPass);
  }
}
