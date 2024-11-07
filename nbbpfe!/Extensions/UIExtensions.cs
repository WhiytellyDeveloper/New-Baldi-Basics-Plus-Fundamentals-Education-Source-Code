using UnityEngine.UI;
using UnityEngine;
using MTM101BaldAPI.Reflection;

namespace nbppfe.Extensions
{
    public static class UIExtensions
    {
        public static Canvas CreateCanvas(bool setGlobalCam, float planeDistance = 0.31f)
        {
            GameObject gameObject = new GameObject("Canvas");
            gameObject.layer = 5;
            Canvas canvas = gameObject.AddComponent<Canvas>();
            CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
            gameObject.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.additionalShaderChannels = (AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent);
            canvasScaler.referencePixelsPerUnit = 1f;
            canvasScaler.referenceResolution = new Vector2(480f, 360f);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.scaleFactor = 1f;
            gameObject.AddComponent<PlaneDistance>().ReflectionSetVariable("planeDistance", planeDistance);
            if (setGlobalCam)            
                canvas.worldCamera = Singleton<GlobalCam>.Instance.Cam;
            
            return canvas;
        }
    }
}
