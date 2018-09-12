namespace DataAbstractionLevel.Low.PsnData.Contracts {
	public enum PsnCommandPartConfirmation
	{
		WrongAddress,
		WrongCommandCode,
		WrongFirstCrcByte,
		WrongSecondCrcByte,
		WrongSomeDefParam,
		EverythyngIsOk
	}
}