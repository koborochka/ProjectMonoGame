namespace ProjectMonoGame;

public enum State : byte
{
    Game, 
    Menu,
    DeathScreen
}

public class GameState
{
    public State State { get; set; }
    public GameState()
    {
        State = State.Menu;
    }
}