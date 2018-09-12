using System;
using System.IO;
using AlienJust.Support.IO;
using RPD.DAL;

namespace RPD.DalRelease.Configuration.System {
	/// <summary>
	/// Репрезентс правило дампа при аварии
	/// </summary>
	internal class RpdDumpRule //binary "serialization" length = 7
	{
		#region props
		/// <summary>
		/// Маска условий контроля и регистрации
		/// </summary>
		public byte Type { get; set; }

		/// <summary>
		/// Параметр выполнения условий контроля (пока один, 400)
		/// </summary>
		public short ControlValue { get; set; }

		/// <summary>
		/// Верхний предел
		/// </summary>
		public short MaxValue { get; set; }

		/// <summary>
		/// Нижний предел
		/// </summary>
		public short MinValue { get; set; }

		/// <summary>
		/// Инициализирует пустую болванку для правила
		/// </summary>
		public RpdDumpRule()
		{
			Type = 0;
			ControlValue = 0;
			MaxValue = 0;
			MinValue = 0;
		}
		#endregion
		/// <summary>
		/// О нет, мне лень описывать конструктор с таким числом парамеров!
		/// </summary>
		/// <param name="type"></param>
		/// <param name="controlValue"></param>
		/// <param name="maxValue"></param>
		/// <param name="minValue"></param>
		public RpdDumpRule(byte type, short controlValue, short maxValue, short minValue)
		{
			Type = type;
			ControlValue = controlValue;
			MaxValue = maxValue;
			MinValue = minValue;
		}
		public RpdDumpRule(IDumpCondition condition)
		{
			Type = 0;
			if (condition.UseValueAbs) Type += 0x01;
			if (condition.UseHighLimit) Type += 0x02;
			if (condition.UseLowLimit) Type += 0x04;
			if (condition.UseControlValue) Type += 0x08;

			MaxValue = (short)condition.HighLimit;
			MinValue = (short)condition.LowLimit;
			ControlValue = (short)condition.ControlValue;
		}
		public bool IsEqual(RpdDumpRule anotherRule)
		{
			bool result = true;//изначально будем считать что правила совпадают
			
			//если хоть один параметр не совпадает - считаем что правла не совпали (параметр Value не контролируется):
			if (Type != anotherRule.Type) result = false;
			else if (MaxValue != anotherRule.MaxValue) result = false;
			else if (MinValue != anotherRule.MinValue) result = false;
			else if (ControlValue != anotherRule.ControlValue) result = false;

			return result;
		}
		public void ReadFromStream(Stream stream)
		{
			var br = new AdvancedBinaryReader(stream, false);
			Type = br.ReadByte();
			ControlValue = br.ReadInt16();
			MaxValue = br.ReadInt16();
			MinValue = br.ReadInt16();
		}
		public void WriteToStream(Stream stream)
		{
			var bw = new AdvancedBinaryWriter(stream, false);
			bw.Write(Type);
			bw.Write(ControlValue);
			bw.Write(MaxValue);
			bw.Write(MinValue);
		}
		public void UpdateChannelCondition(IDumpCondition condtitionToUpdate)
		{
			//condtitionToModify.ConnectionPointIndex = connectionPointIndex;//connectionPointIndex определяется при создании RPD канала из записи таблицы измерителей
			condtitionToUpdate.IsUsed = condtitionToUpdate.ConnectionPointIndex > 0;

			condtitionToUpdate.ControlValue = ControlValue;
			condtitionToUpdate.HighLimit = MaxValue;
			condtitionToUpdate.LowLimit = MinValue;

			condtitionToUpdate.UseValueAbs = (Type & 0x01) == 0x01;
			condtitionToUpdate.UseHighLimit = (Type & 0x02) == 0x02;
			condtitionToUpdate.UseLowLimit = (Type & 0x04) == 0x04;
			condtitionToUpdate.UseControlValue = (Type & 0x08) == 0x08;

			condtitionToUpdate.UseLogicalAnd = false;
		}
		public override string ToString()
		{
			string result = "";
			try
			{
				result =
					"Type=" + Type.ToString() + Environment.NewLine +
					"ControlValue=" + ControlValue.ToString() + Environment.NewLine +
					"MaxValue=" + MaxValue.ToString() + Environment.NewLine +
					"MinValue=" + MinValue.ToString();
			}
			catch (Exception ex)
			{
				result = ex.ToString();
			}
			return result;
		}
	}
}