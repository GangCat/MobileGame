public interface IPlayerMoveSubject
{
    void RegisterObserver(IPlayerMoveObserver _observer);
    void UnregisterObserver(IPlayerMoveObserver _observer);
    void NotifyObservers(in EBlockType _blockType);
}
