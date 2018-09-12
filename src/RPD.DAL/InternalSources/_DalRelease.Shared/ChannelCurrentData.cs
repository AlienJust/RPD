using RPD.DAL;

namespace RPD.DalRelease.Shared
{
	/// <summary>
	/// Текущие данные канала РПД
	/// </summary>
	class RpdChannelCurrentData
	{
		private readonly RpdChannel _channel;

		/// <summary>
		/// Канал
		/// </summary>
		public RpdChannel Channel { get { return _channel; } }

		/// <summary>
		/// Данные канала
		/// </summary>
		public double Data { get; private set; }
		
		/// <summary>
		/// Создаёт новый экземпляр класса
		/// </summary>
		/// <param name="channel">Ассоциируемый канал</param>
		/// <param name="data">Данные канала</param>
		public RpdChannelCurrentData(RpdChannel channel, double data)
		{
			_channel = channel;
			Data = data;
		}
	}

	/// <summary>
	/// Текущие данные канала ПСН
	/// </summary>
	class PsnChannelCurrentData
	{
		private readonly IPsnChannel _channel;

		/// <summary>
		/// Канал
		/// </summary>
		public IPsnChannel Channel { get { return _channel; } }

		/// <summary>
		/// Данные канала
		/// </summary>
		public double Data { get; private set; }
		
		/// <summary>
		/// Создаёт новый экземпляр класса
		/// </summary>
		/// <param name="channel">Ассоциируемый канал</param>
		/// <param name="data">Данные канала</param>
		public PsnChannelCurrentData(IPsnChannel channel, double data)
		{
			_channel = channel;
			Data = data;
		}
	}
}
