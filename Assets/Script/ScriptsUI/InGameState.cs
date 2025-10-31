using UnityEngine;

public class InGameState : UIState
{
    public InGameState(UIManager uiManager) : base(uiManager) { }

    public override void Enter()
    {
        Debug.Log("Entrando al estado de En Juego");

        if (m_uiManager.inGameHudPanel != null)
            m_uiManager.inGameHudPanel.SetActive(true);

        if (m_uiManager.mainMenuPanel != null)
            m_uiManager.mainMenuPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void Exit()
    {
        Debug.Log("Saliendo del estado de En Juego");

        if (m_uiManager.inGameHudPanel != null)
            m_uiManager.inGameHudPanel.SetActive(false);
    }
}
