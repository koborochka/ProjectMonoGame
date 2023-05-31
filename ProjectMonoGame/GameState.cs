namespace ProjectMonoGame;

public enum State : byte
{
    Game, 
    Menu
}

public class GameState
{
    public State State { get; set; }
    public GameState()
    {
        State = State.Menu;
    }
}