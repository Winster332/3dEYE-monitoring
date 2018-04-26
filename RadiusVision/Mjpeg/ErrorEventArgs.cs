using System;

namespace RadiusVision.Mjpeg
{
	public class ErrorEventArgs : EventArgs
	{
		public string Message { get; }

		public ErrorEventArgs(string message)
		{
			Message = message;
		}
	}
}
