namespace Player
{
    public enum PlayerCommands
    {
        MoveLeft,
        MoveRight,
        Jump,
        Use
    }

    public struct PlayerCommand
    {
        public int Tick;
        public PlayerCommands Type;
    }
}