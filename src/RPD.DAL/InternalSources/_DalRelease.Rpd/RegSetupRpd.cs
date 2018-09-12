using System;
using System.IO;

namespace RPD.DAL {
	internal class RegSetupRpd
	{
		public const int SizeInBytes = 608;
		private const float DefaultTimeValue = 20000.0f;
		/// <summary>
		/// // ��������� ������ ������� �� �������� �����
		/// </summary>
		/// <param name="raw">����� �����</param>
		public void FillSetupStructFromRaw(byte[] raw)
		{
			//�������� ����� �������
			if (raw.Length == SizeInBytes)
			{
				var ms = new MemoryStream(raw);
				var br = new BinaryReader(ms);
				Channels = br.ReadUInt16();
				Reserv1 = br.ReadUInt16();
				Time = br.ReadSingle();
				if (Math.Abs(Time - 0) < 0.01) {
					//Log.Global.Info("Warning, Time = " + Time.ToString("f2") + " so far using default value " + DefaultTimeValue.ToString("f1"));
					Time = DefaultTimeValue;
				}
				NDiap = br.ReadUInt32();
				NDiapDop = br.ReadUInt16();
				Reserv2 = br.ReadUInt16();
				A = br.ReadUInt32();
				B = br.ReadUInt32();
				TimeDumpCom = br.ReadSingle();
				Reserv3 = br.ReadUInt16();
				Reserv4 = br.ReadUInt16();

				var porogs = new AllThresholds(br);
				AllThresholds = porogs; //new TPorogAll(br);
				var tarirs = new Calibrations(br);
				Calibrations = tarirs;

				br.Close();
			}
		}

		public RegSetupRpd()
		{
			Channels = 0;
			Reserv1 = 0;
			Time = DefaultTimeValue;
			NDiap = 0;
			NDiapDop = 0;
			Reserv2 = 0;
			A = 0;
			B = 0;
			TimeDumpCom = 0.0f;
			Reserv3 = 0;
			Reserv4 = 0;

			AllThresholds = new AllThresholds();
			Calibrations = new Calibrations();
		}

		/// <summary>
		/// ����� ������� (�������� = 15 � ����� ����� = 0000 0000 0000 1111 => 4 ������
		/// </summary>
		public ushort Channels { get; set; }

// ����� ��������� �������
		public ushort Reserv1 { get; set; }

		/// <summary>
		/// �� ����� ���� ��� ������� (��) => dticks = 10 000 000 / Time
		/// </summary>
		public float Time { get; set; }

//������������� ��� ������ �� ����
		public uint NDiap { get; set; } //��������� 16 ������� �� 2 ����, ������ 0-2
		public ushort NDiapDop { get; set; } //������������ �� �������������� ����� - ���/�� - 0/1 
		public ushort Reserv2 { get; set; }
		public uint A { get; set; } //���������� ������� �� ������
		public uint B { get; set; } //���������� ������� ����� ������
		public float TimeDumpCom { get; set; } //��� ����� ����� ��������� ����� 1,2 � 3     
		public ushort Reserv3 { get; set; }
		public ushort Reserv4 { get; set; }

		//private TPorogAll porogAll = new TPorogAll();
		//public TPorogAll AllThresholds { get { return this.porogAll; } set { this.porogAll = value; } }// { get; set; } //10*16 word //��� ������
		public AllThresholds AllThresholds { get; set; }

		//private TTarir tarir = new TTarir();//{ get; set; }//8*16 word //��� ������
		//public TTarir Calibrations { get { return tarir; } set { tarir = value; } }
		public Calibrations Calibrations { get; set; }

		public override string ToString()
		{
			string result = "";
			result += "Channels = " + Channels + Environment.NewLine;
			result += "Reserv1 = " + Reserv1 + Environment.NewLine;
			result += "Time = " + Time + Environment.NewLine;
			result += "NDiap = " + NDiap + Environment.NewLine;
			result += "NDiapDop = " + NDiapDop + Environment.NewLine;
			result += "Reserv2 = " + Reserv2 + Environment.NewLine;
			result += "A = " + A + Environment.NewLine;
			result += "B = " + B + Environment.NewLine;
			result += "TimeDumpCom = " + TimeDumpCom + Environment.NewLine;
			result += "Reserv3 = " + Reserv3 + Environment.NewLine;
			result += "Reserv4 = " + Reserv4 + Environment.NewLine;
			result += Environment.NewLine + "AllThresholds:" + Environment.NewLine;
			result += AllThresholds.ToString();
			result += Environment.NewLine + "Tarirs:" + Environment.NewLine;
			result += Calibrations.ToString();


			return result;
		}

		public string ToString(bool showPorogs, bool showTarirs)
		{
			string result = "";
			result += "Channels = " + Channels + Environment.NewLine;
			result += "Reserv1 = " + Reserv1 + Environment.NewLine;
			result += "Time = " + Time + Environment.NewLine;
			result += "NDiap = " + NDiap + Environment.NewLine;
			result += "NDiapDop = " + NDiapDop + Environment.NewLine;
			result += "Reserv2 = " + Reserv2 + Environment.NewLine;
			result += "A = " + A + Environment.NewLine;
			result += "B = " + B + Environment.NewLine;
			result += "TimeDumpCom = " + TimeDumpCom + Environment.NewLine;
			result += "Reserv3 = " + Reserv3 + Environment.NewLine;
			result += "Reserv4 = " + Reserv4 + Environment.NewLine;
			if (showPorogs)
			{
				result += Environment.NewLine + "AllThresholds:" + Environment.NewLine;
				result += AllThresholds.ToString();
			}
			if (showTarirs)
			{
				result += Environment.NewLine + "Tarirs:" + Environment.NewLine;
				result += Calibrations.ToString();
			}


			return result;
		}
	}
}