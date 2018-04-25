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

namespace WpfApp3.UIComponents
{
	/// <summary>
	/// Interaction logic for TreeCameras.xaml
	/// </summary>
	public partial class TreeCameras : UserControl
	{
		public event EventHandler<IpCamera> OpenCamera;
		public TreeCameras()
		{
			InitializeComponent();
		}

		private void ButtonClick_RefreshList(object sender, RoutedEventArgs e) => UpdateList();
		public void UpdateList()
		{
			RadiusVisionParser parser = new RadiusVisionParser();
			parser.CamerasComplated += Parser_CamerasComplated;
			parser.GetCamerasList();
		}

		private void Parser_CamerasComplated(object sender, IList<IpCamera> cameras)
		{
			tree.Items.Clear();
			var groups = new Dictionary<string, List<IpCamera>>();

			foreach (var camera in cameras)
			{
				foreach (var location in camera.Locations)
				{
					if (!groups.ContainsKey(location))
						groups.Add(location, new List<IpCamera>());

					groups[location].Add(camera);
				}
			}

			var color = new SolidColorBrush(Color.FromArgb(255, 0, 140, 210));
			var subColor = new SolidColorBrush(Color.FromArgb(255, 0, 210, 140));

			foreach (var group in groups)
			{
				var name = group.Key;
				var camerasFromGroup = group.Value;

				var item = new TreeViewItem();
				item.Foreground = color;
				item.Header = name;

				camerasFromGroup.ForEach(camera =>
				{
					var itemCamera = new TreeViewItem();
					itemCamera.Foreground = subColor;
					itemCamera.Header = camera.Name;
					itemCamera.MouseDoubleClick += (o, e) => OpenCamera(itemCamera, camera);
					item.Items.Add(itemCamera);
				});

				tree.Items.Add(item);
			}
		}
	}
}
