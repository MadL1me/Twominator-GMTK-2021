namespace Player
{
    public abstract class PlayerCommand
    {
        protected PlayerController _playerController;
        
        public PlayerCommand(PlayerController playerController)
        {
            _playerController = playerController;
        }
        
        public virtual void Execute() { }
        public virtual void Undo() { }
    }
    
    public class PlayerMoveLeftPlayerCommand : PlayerCommand
    {
        private float _deltaTime;
        
        public PlayerMoveLeftPlayerCommand(PlayerController playerController, float deltaTime) : base(playerController)
        {
            _deltaTime = deltaTime;
        }
        
        public override void Execute()
        {
            _playerController.MoveLeft(_deltaTime);
        }
    }
    
    public class PlayerMoveRightPlayerCommand : PlayerCommand
    {
        private float _deltaTime;

        public PlayerMoveRightPlayerCommand(PlayerController playerController, float deltaTime) : base(playerController)
        {
            _deltaTime = deltaTime;
        }

        public override void Execute()
        {
            _playerController.MoveRight(_deltaTime);
        }
    }
    
    public class PlayerJumpCommand : PlayerCommand
    {
        public PlayerJumpCommand(PlayerController playerController) : base(playerController) { }

        public override void Execute()
        {
            _playerController.Jump();
        }
    }
}