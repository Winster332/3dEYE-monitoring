using System.Windows.Controls;
using RadiusVision.RVParser;

namespace WpfApp3.UIComponents
{
	/// <summary>
	/// Interaction logic for VisionCamera.xaml
	/// </summary>
	public partial class VisionCamera : UserControl
	{
		public VisionCamera()
		{
			InitializeComponent();
		}

		public void SetBuffer(byte[] frame)
		{

		}
		public void AddCamera(IpCamera camera)
		{
			var item = new TreeCameraItem();
			item.Width = 290;
			item.Height = 280;
			item.SetCamera(camera);

			listBox.Items.Add(item);
		}
	}
}
