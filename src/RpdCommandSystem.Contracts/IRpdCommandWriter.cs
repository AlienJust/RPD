namespace RpdCommandSystem.Contracts {
	/// <summary>
	/// Интерфейс записи команд в РПД
	/// </summary>
	public interface IRpdCommandWriter {
		void WriteCommandSync(IRpdControlCommand cmd, int pauseSeconds);
	}
}