namespace RpdCommandSystem.Contracts {
	/// <summary>
	/// ��������� ������ ������ � ���
	/// </summary>
	public interface IRpdCommandWriter {
		void WriteCommandSync(IRpdControlCommand cmd, int pauseSeconds);
	}
}