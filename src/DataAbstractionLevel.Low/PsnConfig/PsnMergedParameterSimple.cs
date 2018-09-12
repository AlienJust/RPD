using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	class PsnMergedParameterSimple : IPsnMergedParameter {
		private readonly IIdentifier _id;
		private readonly string _name;
		private readonly string _expression;
		private readonly bool _isMsIntegrated;
		private readonly IEnumerable<IPsnMergedParameterPart> _parts;
		

		public PsnMergedParameterSimple(IIdentifier id, string name, string expression, bool isMsIntegrated, IEnumerable<IPsnMergedParameterPart> parts) {
			_id = id;
			_name = name;
			_expression = expression;
			_isMsIntegrated = isMsIntegrated;
			_parts = parts;
			
		}

		public IIdentifier Id {
			get { return _id; }
		}

		public string Name {
			get { return _name; }
		}

		public string Expression {
			get { return _expression; }
		}

		public bool IsMsIntegrated
		{
			get { return _isMsIntegrated; }
		}

		public IEnumerable<IPsnMergedParameterPart> Parts {
			get { return _parts; }
		}
	}
}