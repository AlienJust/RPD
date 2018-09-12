namespace DataAbstractionLevel.Low.Storage.Contracts
{
    public interface IBackSearchData
    {
        /// <summary>
        /// Получает байты данных куска команды, расположенного в определённой позиции данных
        /// </summary>
        /// <param name="dataPosition"></param>
        /// <returns></returns>
        byte[] GetCommandPartBytes(int dataPosition);
    }
}