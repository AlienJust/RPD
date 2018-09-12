using System;
using System.IO;
using System.Net;
using System.Net.FtpClient;

namespace DataAbstractionLevel.Low.InternalKitchen.Streamable {
	class FtpFileStream : Stream
	{
		private readonly string _ftpUrl;
		private readonly int _ftpPort;
		private readonly string _username;
		private readonly string _password;
		private readonly string _filename;

		private readonly MemoryStream _ms;

		public FtpFileStream(string ftpUrl, int ftpPort, string username, string password, string filename, bool doInitialRead)
		{
			_ftpUrl = ftpUrl;
			_ftpPort = ftpPort;
			_username = username;
			_password = password;
			_filename = filename;
			_ms = new MemoryStream();
			if (doInitialRead)
			{
				try
				{
					//Console.WriteLine(_filename + " > Connecting");
					using (var conn = new FtpClient())
					{
						conn.Host = _ftpUrl;
						conn.Port = _ftpPort;
						conn.Credentials = new NetworkCredential(_username, _password);

						using (Stream istream = conn.OpenRead(filename))
						{
							try
							{
								istream.CopyTo(_ms);
							}
							finally
							{
								istream.Close();
							}
						}
					}
					//Console.WriteLine(_filename + " > Data taken");
				}
				catch /*(Exception ex)*/ {
					// if no file entry in zip, memory stream just created
					//Console.WriteLine(ex);
					// continue on even error (after getting empty memory stream)
				}
			}
			_ms.Seek(0, SeekOrigin.Begin);
		}

		public override void Flush()
		{
			//_ms.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return _ms.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			_ms.SetLength(value);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			//Console.WriteLine(_streamName + " > " + _filename + "@" + _zipFile.Name + " > " + "Stream Read() called");
			return _ms.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			//Console.WriteLine(_streamName + " > " + _filename + "@" + _zipFile.Name + " > " + "Stream Write() called, bytes to write=" + count);
			try
			{
				_ms.Write(buffer, offset, count);
				if (count > 0)
				{
					var pos = _ms.Position;

					// Если попытаться записать методом UpdateEntry только что записанный файл, то возникает ошибка состояния zip архива
					// Поэтому решено сперва удалять Entry из архива - это помогает
					using (var conn = new FtpClient())
					{
						conn.Host = _ftpUrl;
						conn.Port = _ftpPort;
						conn.Credentials = new NetworkCredential(_username, _password);

						using (Stream ostream = conn.OpenWrite(_filename))
						{
							try
							{
								_ms.Seek(0, SeekOrigin.Begin);
								_ms.CopyTo(ostream);
								// istream.Position is incremented accordingly to the writes you perform
								_ms.Position = pos;
							}
							finally
							{
								ostream.Close();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Не удалось произвести запись в файл, убедитесь что имя файла указано верно и у вас есть доступ к нему", ex);
			}
		}

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override long Length
		{
			get { return _ms.Length; }
		}

		public override long Position
		{
			get { return _ms.Position; }
			set { _ms.Position = value; }
		}

		protected override void Dispose(bool disposing)
		{
			_ms.Dispose();
			base.Dispose(disposing);
		}
	}
}