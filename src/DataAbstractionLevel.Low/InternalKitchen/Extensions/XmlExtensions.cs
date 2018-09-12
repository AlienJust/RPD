using System.Xml;

namespace DataAbstractionLevel.Low.InternalKitchen.Extensions
{

	internal static class XmlExtensions
	{
		/// <summary>
		/// Получает атрибут узла по имени
		/// </summary>
		/// <param name="node">Узел</param>
		/// <param name="attributeName">Название атрибута</param>
		/// <returns>Найденный атрибут, либо null</returns>
		public static XmlAttribute GetAttribute(this XmlNode node, string attributeName)
		{
			XmlAttribute result = null;
			if (node.Attributes != null)
			{
				foreach (XmlAttribute attr in node.Attributes)
				{
					if (attr.Name == attributeName)
					{
						result = attr;
						break;
					}
				}
			}
			return result;
		}
	}
}
