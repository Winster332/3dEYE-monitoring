using System;

namespace RadiusVision.Mjpeg
{
	public class FrameReadyEventArgs : EventArgs
	{
		public byte[] FrameBuffer { get; }

		public FrameReadyEventArgs(byte[] buffer)
		{
			FrameBuffer = buffer;
		}
	}
}
