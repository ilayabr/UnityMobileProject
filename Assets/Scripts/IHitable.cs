
/// <summary>
/// An interface to let object get hit by the player
/// </summary>
public interface IHitable
{
    bool OnHit(ShellTypes sheelUSed, float jammerVal);
}
