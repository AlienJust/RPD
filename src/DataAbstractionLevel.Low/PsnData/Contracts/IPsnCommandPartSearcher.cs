using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnData.Contracts {
	public interface IPsnCommandPartSearcher {
		/// <summary>
		/// ѕровер€ет команду на совпадение предопределенных значений команды с данными входного массива
		/// </summary>
		/// <param name="cmdPartData">—писок байтов, с которыми будут сравниватьс€ нужна€ часть команды</param>
		/// <param name="startByte">Start byte</param>
		/// <param name="cmdPartInfo"> усок команды (запрос или ответ)</param>
		/// <returns>‘лаг "команда найдена"</returns>
		PsnCommandPartConfirmation IsHereCmdPart(byte[] cmdPartData, int startByte, IPsnProtocolCommandPartConfiguration cmdPartInfo);
	}
}