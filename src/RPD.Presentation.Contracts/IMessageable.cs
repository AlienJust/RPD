using GalaSoft.MvvmLight.Messaging;

namespace RPD.Presentation.Contracts {
	/// <summary>
	/// 
	/// </summary>
	public interface IMessageable {
		void StartMessaging(IMessenger messenger);
	}
}
