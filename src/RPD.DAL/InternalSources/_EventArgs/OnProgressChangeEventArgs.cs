using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPD.EventArgs
{
	/// <summary>
	/// Аргументы изменения прогресса операции
	/// </summary>
	public class OnProgressChangeEventArgs
	{
		private readonly int _progressPercent;

		/// <summary>
		/// Прогресс в процентах
		/// </summary>
		public int ProgressPercent
		{
			get
			{
				if (_progressPercent < 0) return 0;
				else if (_progressPercent > 100) return 100;
				else return _progressPercent;
			}
		}

		private readonly object _argument;

		/// <summary>
		/// Дополнительный аргумент
		/// </summary>
		public object Argument
		{
			get { return _argument; }
		}

		/// <summary>
		/// Иницализирует новый экземпляр OnProgressChangeEventArgs
		/// </summary>
		/// <param name="progressPercent">Прогресс в процентах</param>
		/// <param name="argument">Дополнительный аргумент</param>
		public OnProgressChangeEventArgs(int progressPercent, object argument)
		{
			_progressPercent = progressPercent;
			_argument = argument;
		}

		/// <summary>
		/// Иницализирует новый экземпляр OnProgressChangeEventArgs
		/// </summary>
		/// <param name="progressPercent">Прогресс в процентах</param>
		public OnProgressChangeEventArgs(int progressPercent)
		{
			_progressPercent = progressPercent;
			_argument = null;
		}
	}
}
