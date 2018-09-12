using System;
using System.IO;

namespace DataAbstractionLevel.Low.InternalKitchen.Streamable
{
	static class StreamExtensions
	{
		public static void CopyToWithProgress(this Stream source, Stream destination, Action<double> progressChangeAction) {
			var totalLength = source.Length - source.Position;
			int readedInIteration;
			int totalReaded = 0;
			var buffer = new byte[2048]; // TODO: allow to setup buffer length
			while ((readedInIteration = source.Read(buffer, 0, buffer.Length)) > 0)
			{
				destination.Write(buffer, 0, readedInIteration);
				totalReaded += readedInIteration;
				progressChangeAction(totalReaded * 100.0 / totalLength);
			}
		}
	}
}
