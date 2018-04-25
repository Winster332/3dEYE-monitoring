using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

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
