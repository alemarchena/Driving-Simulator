using UnityEngine;

public class MenuScrollController : MonoBehaviour
{
    [SerializeField] RectTransform menuPanel;
    [SerializeField] float hiddenY = -1080f;   // posición oculta
    [SerializeField] float visibleY = 550f;     // posición visible
    [SerializeField] float speed = 5f;        // velocidad de movimiento

    bool isVisible = false;
    float targetY;

    void Start()
    {
        targetY = hiddenY;
        SetPanelY(hiddenY);
    }

    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll > 0f)
            ShowMenu();
        else if (scroll < 0f)
            HideMenu();

        // Movimiento suave
        Vector2 pos = menuPanel.anchoredPosition;
        pos.y = Mathf.Lerp(pos.y, targetY, Time.deltaTime * speed);
        menuPanel.anchoredPosition = pos;
    }

    void ShowMenu()
    {
        targetY = visibleY;
        isVisible = true;
    }

    void HideMenu()
    {
        targetY = hiddenY;
        isVisible = false;
    }

    void SetPanelY(float y)
    {
        Vector2 pos = menuPanel.anchoredPosition;
        pos.y = y;
        menuPanel.anchoredPosition = pos;
    }
}
