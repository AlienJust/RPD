namespace RPD.DAL.Common.Contracts {
	internal interface IBuilder<out T> {
		T Build();
	}
}