
using Completed.Constants;

namespace Completed
{
    public class PlayerPresenter
    {
        private Player _player;
        private PlayerView _playerView;

        public PlayerPresenter(Player player, PlayerView playerView)
        {
            _player = player;
            _playerView = playerView;

            _player.PlayerMoveEvent.AddListener(Present);
            _player.PlayerCollisionEvent.AddListener(HandlePlayerCollisionEvent);
        }

        private void Present()
        {
            _playerView.UpdateFoodText(_player.Food.ToString());
        }

        private void HandlePlayerCollisionEvent(string colliderObjectName)
        {
            if (colliderObjectName == "Soda")
                _playerView.UpdateText($"+ {FoodConstants.PointsPerSoda} Food: {colliderObjectName}");
            
            if (colliderObjectName == "Food")
                _playerView.UpdateText($"+ {FoodConstants.PointsPerFood} Food: {colliderObjectName}");
            
        }
    }
}