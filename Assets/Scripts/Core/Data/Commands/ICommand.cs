public interface ICommand {
    public bool Condition();
    public void Execute();
    public void Undo();
}