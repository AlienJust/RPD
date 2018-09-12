namespace RPD.DAL {
	/// <summary>
	/// Условия, при которых РПД производит дампы аварий
	/// </summary>
	public interface IDumpCondition {
		/// <summary>
		/// № точки подключения (индекс в массиве правил (1-47)), если значение равно нулю - Condition не используется
		/// </summary>
		int ConnectionPointIndex { get; set; }

		/// <summary>
		/// Верхний предел, при превышении которого будет дамп
		/// </summary>
		int HighLimit { get; set; }

		/// <summary>
		/// Флаг, указывающий используется ли верхний предел в условии
		/// </summary>
		bool UseHighLimit { get; set; }

		/// <summary>
		/// Нижний предел, если значение уйдет ниже него - будет дамп
		/// </summary>
		int LowLimit { get; set; }

		/// <summary>
		/// Флаг, указывающий используется ли нижний предел в условии
		/// </summary>
		bool UseLowLimit { get; set; }

		/// <summary>
		/// Флаг использования значения по модулю при сравнении с условием
		/// </summary>
		bool UseValueAbs { get; set; }

		/// <summary>
		/// Исользовать логическое И (то есть фактически услови дампа сработает, если значение не выйдет из диапазона, а войдет в него)
		/// </summary>
		bool UseLogicalAnd { get; set; }

		/// <summary>
		/// Контролируемое значение
		/// </summary>
		int ControlValue { get; set; }

		/// <summary>
		/// Используется ли контролируемое значение для данного канала
		/// </summary>
		bool UseControlValue { get; set; }

		/// <summary>
		/// Используется ли условие
		/// </summary>
		bool IsUsed { get; set; }

		/// <summary>
		/// Копирует значения членов класса из другого объекта с интерфейсом IDumpCondition
		/// </summary>
		/// <param name="source">Условие-источник для копирования</param>
		void CopyFrom(IDumpCondition source);
	}
}
