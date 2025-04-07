using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PortraitAspectRatio : MonoBehaviour
{
    [SerializeField] Vector2 targetAspectRatio = new Vector2(9, 16);

    private Camera cam;
    private float lastWidth;
    private float lastHeight;

    void Start()
    {
        cam = GetComponent<Camera>();
        UpdateViewport();
    }

    void Update()
    {
        // Detecta si cambió el tamaño de la pantalla
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            UpdateViewport();
        }
    }

    void UpdateViewport()
    {
        float targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Rect rect = cam.rect;

        if (scaleHeight < 1.0f)
        {
            // Añade letterbox (barras negras arriba y abajo)
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            // Añade pillarbox (barras negras a los costados)
            float scaleWidth = 1.0f / scaleHeight;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        cam.rect = rect;

        lastWidth = Screen.width;
        lastHeight = Screen.height;
    }
}
