using System;
using System.IO;
using AlienJust.Support.IO;
using RPD.DAL;

namespace RPD.DalRelease.Configuration.System {
	/// <summary>
	/// ���������� ������� ����� ��� ������
	/// </summary>
	internal class RpdDumpRule //binary "serialization" length = 7
	{
		#region props
		/// <summary>
		/// ����� ������� �������� � �����������
		/// </summary>
		public byte Type { get; set; }

		/// <summary>
		/// �������� ���������� ������� �������� (���� ����, 400)
		/// </summary>
		public short ControlValue { get; set; }

		/// <summary>
		/// ������� ������
		/// </summary>
		public short MaxValue { get; set; }

		/// <summary>
		/// ������ ������
		/// </summary>
		public short MinValue { get; set; }

		/// <summary>
		/// �������������� ������ �������� ��� �������
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
		/// � ���, ��� ���� ��������� ����������� � ����� ������ ���������!
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
			bool result = true;//���������� ����� ������� ��� ������� ���������
			
			//���� ���� ���� �������� �� ��������� - ������� ��� ������ �� ������� (�������� Value �� ��������������):
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
			//condtitionToModify.ConnectionPointIndex = connectionPointIndex;//connectionPointIndex ������������ ��� �������� RPD ������ �� ������ ������� �����������
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