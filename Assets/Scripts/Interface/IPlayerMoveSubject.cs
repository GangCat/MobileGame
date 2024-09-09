public interface IPlayerMoveSubject
{
    void RegisterPlayerMoveObserver(IPlayerMoveObserver _observer);
    void UnregisterPlayerMoveObserver(IPlayerMoveObserver _observer);
    void NotifyPlayerMoveObservers(in EBlockType _blockType);
}
