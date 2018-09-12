using System.Collections.Generic;
using System.Linq;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.InternalKitchen.Contracts;

namespace DataAbstractionLevel.Low.InternalKitchen.Id {
	public class IdentifierHashBased : IIdentifier
	{
		private readonly byte[] _hash;

		public IdentifierHashBased(IEnumerable<byte> hash) {
			_hash = hash.ToArray();
		}

		public override string ToString() {
			return _hash.ToHumanString(string.Empty).ToLower();
		}

		public string IdentyString
		{
			get { return ToString(); }
		}
	}
}