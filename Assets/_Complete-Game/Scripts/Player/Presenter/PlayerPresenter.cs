
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
        }

        public void Present()
        {
            _playerView.UpdateFoodText(_player.Food.ToString());
        }
    }
}