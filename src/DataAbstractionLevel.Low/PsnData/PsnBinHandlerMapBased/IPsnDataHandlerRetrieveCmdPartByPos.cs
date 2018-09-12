namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased
{
    /// <summary>
    /// Provides "back-search"
    /// </summary> 
    public interface IPsnDataHandlerRetrieveCmdPartByPos
    {
        byte[] GetCommandPartBytes(int positionInData);
    }
}