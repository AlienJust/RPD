using System.Collections.Generic;

namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	class PsnVirtualParameterSimple : IPsnVirtualParameter {
		private readonly IUid _id;
		private readonly string _name;
		private readonly string _expression;
		private readonly IEnumerable<IPsnVirtualParameterPart> _parts;
		public PsnVirtualParameterSimple(IUid id, string name, string expression, IEnumerable<IPsnVirtualParameterPart> parts) {
			_id = id;
			_name = name;
			_expression = expression;
			_parts = parts;
		}

		public IUid Id {
			get { return _id; }
		}

		public string Name {
			get { return _name; }
		}

		public string Expression {
			get { return _expression; }
		}

		public IEnumerable<IPsnVirtualParameterPart> Parts {
			get { return _parts; }
		}
	}
}