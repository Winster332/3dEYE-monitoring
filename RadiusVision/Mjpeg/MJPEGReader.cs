using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace RadiusVision.Mjpeg
{
	public class MJPEGReader
	{
		private readonly byte[] JpegHeader = { 0xff, 0xd8 };
		private const int ChunkSize = 1024;
		private bool _streamActive;
		public byte[] CurrentFrame { get; private set; }
		private SynchronizationContext _context;
		public event EventHandler<FrameReadyEventArgs> FrameReady;
		public event EventHandler<ErrorEventArgs> Error;

		public MJPEGReader()
		{
			_context = SynchronizationContext.Current;
		}

		public void RunParse(Uri uri)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.BeginGetResponse(OnGetResponse, request);
		}
		public void ParseStream(Uri uri, string username, string password)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.Credentials = new NetworkCredential(username, password);
			request.BeginGetResponse(OnGetResponse, request);
		}

		public void StopStream()
		{
			_streamActive = false;
		}

		private void OnGetResponse(IAsyncResult asyncResult)
		{
			byte[] imageBuffer = new byte[1024 * 1024];
			var req = (HttpWebRequest)asyncResult.AsyncState;

			try
			{
				HttpWebResponse resp = (HttpWebResponse)req.EndGetResponse(asyncResult);
				string contentType = resp.Headers["Content-Type"];
				if (!string.IsNullOrEmpty(contentType) && !contentType.Contains("="))
					throw new Exception("Invalid content-type header.  The camera is likely not returning a proper MJPEG stream.");

				string boundary = resp.Headers["Content-Type"].Split('=')[1].Replace("\"", "");
				byte[] boundaryBytes = Encoding.UTF8.GetBytes(boundary.StartsWith("--") ? boundary : "--" + boundary);

				Stream s = resp.GetResponseStream();
				var br = new BinaryReader(s);

				_streamActive = true;

				byte[] buff = br.ReadBytes(ChunkSize);

				while (_streamActive)
				{
					int imageStart = FindInBytes(buff, JpegHeader);

					if (imageStart != -1)
					{
						int size = buff.Length - imageStart;
						Array.Copy(buff, imageStart, imageBuffer, 0, size);

						while (true)
						{
							buff = br.ReadBytes(ChunkSize);

							int imageEnd = FindInBytes(buff, boundaryBytes);
							if (imageEnd != -1)
							{
								Array.Copy(buff, 0, imageBuffer, size, imageEnd);
								size += imageEnd;

								byte[] frame = new byte[size];
								Array.Copy(imageBuffer, 0, frame, 0, size);

								ProcessFrame(frame);
								Array.Copy(buff, imageEnd, buff, 0, buff.Length - imageEnd);
								byte[] temp = br.ReadBytes(imageEnd);

								Array.Copy(temp, 0, buff, buff.Length - imageEnd, temp.Length);
								break;
							}

							Array.Copy(buff, 0, imageBuffer, size, buff.Length);
							size += buff.Length;
						}
					}
				}

			}
			catch (Exception ex)
			{
				if (Error != null)
					_context.Post(delegate { Error(this, new ErrorEventArgs(ex.Message)); }, null);
			}
		}

		private void ProcessFrame(byte[] frame)
		{
			CurrentFrame = frame;
			_context.Post(delegate
				{
					if (FrameReady != null)
						FrameReady(this, new FrameReadyEventArgs(CurrentFrame));
				}, null);
		}

		public int FindInBytes(byte[] buff, byte[] search)
		{
			for (int start = 0; start < buff.Length - search.Length; start++)
			{
				if (buff[start] == search[0])
				{
					int next;

					for (next = 1; next < search.Length; next++)
					{
						if (buff[start + next] != search[next])
							break;
					}

					if (next == search.Length)
						return start;
				}
			}
			return -1;
		}
	}
}
