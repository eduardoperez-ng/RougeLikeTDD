using Completed.Interfaces;
using Completed.View;

namespace Completed.Presenter
{
    public class GhostManagerPresenter
    {
        private GhostManager _ghostManager;
        private GhostManagerView _view;
        private ILevelManager _levelManager;
        
        public GhostManagerPresenter(GhostManager ghostManager, GhostManagerView view, ILevelManager levelManager)
        {
            _ghostManager = ghostManager;
            _view = view;
            _view.Init(this);
            _levelManager = levelManager;
            
            _ghostManager.CommandExecutedEvent.AddListener(UpdateExecutedCommandList);
        }

        public void OnMoveGhostButtonClicked()
        {
            if (_ghostManager.TryMoveGhostForDay(_levelManager.CurrentDay))
            {
                _view.UpdateGhostStatus("Active");
            }
        }
        
        public void OnStopGhostButtonClicked()
        {
            _ghostManager.StopGhost();
            _view.UpdateGhostStatus("Stopped");
        }

        private void UpdateExecutedCommandList(string command)
        {
            _view.UpdateExecutedCommandList(command);
        }
    }
}