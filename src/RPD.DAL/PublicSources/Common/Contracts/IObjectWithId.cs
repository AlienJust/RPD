namespace RPD.DAL {
	/// <summary>
	/// ������ � ������������ ��������
	/// </summary>
	public interface IObjectWithId
	{
		/// <summary>
		/// ������������� �������
		/// </summary>
		IUid Id { get; }
	}
}