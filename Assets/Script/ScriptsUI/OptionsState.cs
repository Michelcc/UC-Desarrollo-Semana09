using UnityEngine;

public class OptionsState : UIState
{
    private UIState _previousState;

    public OptionsState(UIManager uiManager, UIState previousState) : base(uiManager)
    {
        _previousState = previousState;
    }

    public override void Enter()
    {
        m_uiManager.OptionsPanel.SetActive(true);
    }

    public override void Exit()
    {
        m_uiManager.OptionsPanel.SetActive(false);
    }

    public void OnBackButtonClicked()
    {
        m_uiManager.ChangeState(_previousState);
    }
}


