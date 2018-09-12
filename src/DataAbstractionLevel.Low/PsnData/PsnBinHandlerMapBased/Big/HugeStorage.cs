using System;
using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Big {
	class HugeStorage {
		private const int ListSize = 10000;
		private int _totalCount;
		private readonly List<List<IPsnCommandPartsPosition>> _items;
		public HugeStorage() {
			_totalCount = 0;
			_items = new List<List<IPsnCommandPartsPosition>>();
		}

		public void Add(IPsnCommandPartsPosition cmdPartsPosition) {
			int listIndex = (_totalCount + 1)/ ListSize;
			if (listIndex >= _items.Count) {
				_items.Add(new List<IPsnCommandPartsPosition>(ListSize));
			}
			_totalCount++;
		}

		public IPsnCommandPartsPosition GetAt(int index) {
			throw new NotImplementedException("TODO");
		}

		public void RemoveAt(int index) {
			throw new NotImplementedException("TODO");
		}

		public int Length => _totalCount;
		public int Count => _totalCount;
	}
}
