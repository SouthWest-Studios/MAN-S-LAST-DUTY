using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurRenderFeature : ScriptableRendererFeature
{
    class BlurRenderPass : ScriptableRenderPass
    {
        private Material blurMaterial;
        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;

        public BlurRenderPass(Material material)
        {
            blurMaterial = material;
            tempTexture.Init("_TempBlurTexture");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            source = renderingData.cameraData.renderer.cameraColorTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("BlurEffect");

            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            cmd.GetTemporaryRT(tempTexture.id, descriptor);

            // Primer paso: Renderizar con el material de desenfoque
            Blit(cmd, source, tempTexture.Identifier(), blurMaterial);
            Blit(cmd, tempTexture.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    [System.Serializable]
    public class BlurSettings
    {
        public Material blurMaterial;
    }

    public BlurSettings settings = new BlurSettings();
    BlurRenderPass blurPass;

    public override void Create()
    {
        blurPass = new BlurRenderPass(settings.blurMaterial)
        {
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(blurPass);
    }
}
