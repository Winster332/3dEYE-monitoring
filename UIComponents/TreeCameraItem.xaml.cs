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
	/// Interaction logic for TreeCameraItem.xaml
	/// </summary>
	public partial class TreeCameraItem : UserControl
	{
		MJPEGReader _mjpeg;
		public TreeCameraItem()
		{
			InitializeComponent();

			_mjpeg = new MJPEGReader();
			_mjpeg.FrameReady += mjpeg_FrameReady;

		}

		private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
		{
			using (var stream = new MemoryStream(e.FrameBuffer))
			{
				viewport.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None,
									  BitmapCacheOption.OnLoad);
			}
			textLoading.IsEnabled = false;
		}

		public void SetCamera(IpCamera camera)
		{
			this._name.Text = $"Name: {camera.Name}";

			this.Foreground = Brushes.White;
			_mjpeg.ParseStream(new Uri($"{camera.Host}/mjpg/video.mjpg"), "axisdemo", "19@Axis84");
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			_mjpeg.StopStream();
			((ListBox)this.Parent).Items.Remove(this);
		}
	}
}
