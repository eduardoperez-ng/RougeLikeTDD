using System;
using Completed.Presenter;
using UnityEngine;
using UnityEngine.UI;

namespace Completed.View
{
    public class GhostManagerView : MonoBehaviour
    {
        public Button moveGhostButton;
        public Button stopGhostButton;
        public Text ghostStatusText;

        private GhostManagerPresenter _ghostManagerPresenter;
        
        public void Init(GhostManagerPresenter ghostManagerPresenter)
        {
            _ghostManagerPresenter = ghostManagerPresenter;
            moveGhostButton.onClick.AddListener(_ghostManagerPresenter.OnMoveGhostButtonClicked);
            stopGhostButton.onClick.AddListener(_ghostManagerPresenter.OnStopGhostButtonClicked);
        }

        public void UpdateGhostStatus(string status)
        {
            ghostStatusText.text = status;
            if (status == "Active")
            {
                ghostStatusText.color = Color.green;
                return;
            }

            if (status == "Stopped")
            {
                ghostStatusText.color = Color.red;
            }
        }

        private void OnDestroy()
        {
            if (moveGhostButton != null)
            {
                moveGhostButton.onClick.RemoveAllListeners();
            }
        }
    }
}