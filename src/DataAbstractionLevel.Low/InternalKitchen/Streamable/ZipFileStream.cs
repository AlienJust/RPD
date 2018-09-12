using System;
using System.IO;
using Ionic.Zip;
using Ionic.Zlib;

namespace DataAbstractionLevel.Low.InternalKitchen.Streamable {
	class ZipFileStream : Stream {
		private readonly string _zipFilename;
		private readonly string _filename;
		private readonly string _streamName; // for debugging

		private readonly MemoryStream _ms;
		public ZipFileStream(string zipFilename, string filename, string streamName, bool doInitialRead) {
			_zipFilename = zipFilename;
			_filename = filename;
			_streamName = streamName;
			_ms = new MemoryStream();
			if (doInitialRead) {
				try {
					using (var zipFile = CreateZipFileInstance(_zipFilename)) {
						using (var stream = zipFile[_filename].OpenReader()) {
							stream.CopyTo(_ms);
						}
					}
				}
				catch /*(Exception ex)*/ {
					// if no file entry in zip, memory stream just created
					//Console.WriteLine(ex);
					// continue on even error (after getting empty memory stream)
				}
			}
			_ms.Seek(0, SeekOrigin.Begin);
		}

		public override void Flush() {
			//_ms.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin) {
			return _ms.Seek(offset, origin);
		}

		public override void SetLength(long value) {
			_ms.SetLength(value);
		}

		public override int Read(byte[] buffer, int offset, int count) {
			//Console.WriteLine(_streamName + " > " + _filename + "@" + _zipFile.Name + " > " + "Stream Read() called");
			return _ms.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count) {
			//Console.WriteLine(_streamName + " > " + _filename + "@" + _zipFile.Name + " > " + "Stream Write() called, bytes to write=" + count);
			try {


				_ms.Write(buffer, offset, count);
				if (count > 0) {
					var pos = _ms.Position;

					// Если попытаться записать методом UpdateEntry только что записанный файл, то возникает ошибка состояния zip архива
					// Поэтому решено сперва удалять Entry из архива - это помогает
					using (var zipFile = CreateZipFileInstance(_zipFilename))
					{
						if (zipFile.ContainsEntry(_filename)) {
							zipFile.RemoveEntry(_filename);
							zipFile.Save();
						}
						_ms.Seek(0, SeekOrigin.Begin);
						zipFile.UpdateEntry(_filename, _ms);
						zipFile.Save();
					}

					_ms.Position = pos;
				}
			}
			catch(Exception ex) {
				throw new Exception("Не удалось произвести запись в файл, убедитесь что имя файла указано верно и у вас есть доступ к нему", ex);
			}
		}

		public override bool CanRead {
			get { return true; }
		}

		public override bool CanSeek {
			get { return true; }
		}

		public override bool CanWrite {
			get { return true; }
		}

		public override long Length {
			get { return _ms.Length; }
		}

		public override long Position {
			get { return _ms.Position; }
			set { _ms.Position = value; }
		}

		protected override void Dispose(bool disposing)
		{
			_ms.Dispose();
			base.Dispose(disposing);
		}

		private ZipFile CreateZipFileInstance(string filename)
		{
			return new ZipFile(filename) { CompressionMethod = CompressionMethod.BZip2, CompressionLevel = CompressionLevel.BestCompression };
		}
	}
}