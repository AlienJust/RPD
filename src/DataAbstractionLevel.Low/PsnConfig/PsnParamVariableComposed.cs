using DataAbstractionLevel.Low.PsnConfig.Contracts;
using NCalc;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal sealed class PsnParamVariableComposed : PsnParamBase, IPsnProtocolParameterConfigurationVariable
	{
		private readonly Expression _expr;
		private byte[] _cmdPartContext;
		private int _startByte;

		public PsnParamVariableComposed(string id, string name, string expression) :
			base(id, name, false) {
			_expr = new Expression(expression);
			_expr.EvaluateParameter += EvaluateParameter;
		}


		public override double GetValue(byte[] cmdPartContext, int startByte)
		{
			_cmdPartContext = cmdPartContext;
			_startByte = startByte;
			return (double)_expr.Evaluate();
		}

		private void EvaluateParameter(string name, ParameterArgs args) {
			try
			{
				if (name.Contains(".")) {
					var parts = name.Split('.'); // 2.2 or 2.3.1
					if (parts.Length == 2) {
						args.Result = PsnParamValueExtractor.GetBitValueFromBytes(_cmdPartContext, _startByte, int.Parse(parts[0]), int.Parse(parts[1])) ? 1.0 : 0.0;
						args.HasResult = true;
					}
					else if (parts.Length == 3) {
						args.Result = PsnParamValueExtractor.GetMultibitUnsignedValueFromBytes(_cmdPartContext, _startByte, int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
						args.HasResult = true;
					}
					else {
						args.HasResult = false;
					}
				}
				else {
					// else we have "u1" or "s19" or "r20" - byte or sbyte value or random beetween 0 and 20;
					if (name.StartsWith("u")) {
						args.Result = (int)(PsnParamValueExtractor.GetByteValueFromBytes(_cmdPartContext, _startByte, int.Parse(name.Substring(1))));
						args.HasResult = true;
					}
					else if (name.StartsWith("s")) {
						args.Result = (int)(PsnParamValueExtractor.GetSByteValueFromBytes(_cmdPartContext, _startByte, int.Parse(name.Substring(1))));
						args.HasResult = true;
					}
					else if (name.StartsWith("r")) {
						args.Result = PsnParamValueExtractor.GetRandomIntValue(int.Parse(name.Substring(1)));
						args.HasResult = true;
					}
					else {
						args.HasResult = false;
					}
				}
			}
			catch {
				args.HasResult = false;
			}
		}
	}
}