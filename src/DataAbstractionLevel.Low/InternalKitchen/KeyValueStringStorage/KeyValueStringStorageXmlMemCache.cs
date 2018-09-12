using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DataAbstractionLevel.Low.InternalKitchen.KeyValueStringStorage.Contracts;

namespace DataAbstractionLevel.Low.InternalKitchen.KeyValueStringStorage
{
	//TODO: check thread safity needs
	public class KeyValueStringStorageXmlMemCache : IKeyValueStringStorage {
		private readonly Dictionary<string, string> _entriesDictionary;
		private const string RootNodeName = "Entries";
		private readonly string _pathToExistingXmlFile;
		private readonly XDocument _doc;


		public KeyValueStringStorageXmlMemCache(string pathToExistingXmlFile)
		{
			_pathToExistingXmlFile = pathToExistingXmlFile;
			_entriesDictionary = new Dictionary<string, string>();
			_doc = RecreateStorageIfNeeded();
			FillDictionary();
		}

		private XDocument RecreateStorageIfNeeded() {
			XDocument doc;
			if (!File.Exists(_pathToExistingXmlFile))
			{
				doc = new XDocument();
			}
			else {
				try {
					doc = XDocument.Load(_pathToExistingXmlFile);
				}
				catch {
					// invalid XML document
					File.Delete(_pathToExistingXmlFile);
					doc = new XDocument();
				}
			}
			if (doc.Element(RootNodeName) == null)
			{
				doc.Add(new XElement(RootNodeName));
				doc.Save(_pathToExistingXmlFile);
			}
			// else root element exists and have nothing to do with file
			return doc;
		}

		private void FillDictionary()
		{
			var rootElement = _doc.Element(RootNodeName);
			if (rootElement != null)
			{
				foreach (var xelement in rootElement.Elements("Entry"))
				{
					var keyAttribute = xelement.Attribute("Key");
					if (keyAttribute != null) {
						_entriesDictionary.Add(keyAttribute.Value, xelement.Value);
					}
				}
			}
			else throw new Exception("Storage is broken, cannot get root element");
		}

		public string GetValue(string key) {
			try {
				return _entriesDictionary[key];
			}
			catch (Exception ex) {
				throw new Exception("Key not found exception", ex); // Exception strategy, can be nullable stratey
			}
			
		}

		public void SetValue(string key, string value) {
			var rootElement = _doc.Element(RootNodeName);
			if (rootElement != null) {
				if (!_entriesDictionary.ContainsKey(key)) {
					_entriesDictionary.Add(key, value);
					rootElement.Add(new XElement("Entry", new XAttribute("Key", key), value));
				}
				else {
					_entriesDictionary[key] = value;
					foreach (var xelement in rootElement.Elements("Entry")) {
						var keyAttribute = xelement.Attribute("Key");
						if (key == keyAttribute.Value) {
							xelement.Value = value;
							_doc.Save(_pathToExistingXmlFile);
						}
					}
				}
				_doc.Save(_pathToExistingXmlFile);
			}
			else throw new Exception("Storage is broken, cannot get root element");
		}

		public void Remove(string key) {
			if (!_entriesDictionary.ContainsKey(key)) return;
			
			_entriesDictionary.Remove(key);
			var rootElement = _doc.Element(RootNodeName);
			if (rootElement != null) {
				rootElement.Elements("Entry").Where(x => {
					var atr = x.Attribute("Key");
					if (atr != null) {
						if (atr.Value == key)
							return true;
					}
					return false;
				}).Remove();
				_doc.Save(_pathToExistingXmlFile);
			}
			else throw new Exception("Storage is broken, cannot get root element");
		}
	}
}
