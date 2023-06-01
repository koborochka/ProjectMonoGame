namespace ProjectMonoGame;

public enum State : byte
{
    Game, 
    Menu
}

public class GameState
{
    
    public GameState()
    {
        State = State.Menu;
        CatsCollectedCount = 0;
    }
    public int CatsCollectedCount { get; set; } 
    public State State { get; set; }

}