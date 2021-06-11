namespace Player
{
    public enum PlayerCommands
    {
        MoveLeft,
        MoveRight,
        Jump
    }

    public struct PlayerCommand
    {
        public int Tick;
        public PlayerCommands Type;
    }
}