using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;
using System.Windows;

namespace WpfApp3
{
	public class RadiusVisionParser
	{
		public IList<IpCamera> Cameras { get; set; }
		public event EventHandler<IList<IpCamera>> CamerasComplated;
		public RadiusVisionParser()
		{
			Cameras = new List<IpCamera>();
		}
		public void GetCamerasList()
		{
			Task.Run(() =>
			{
				Uri uri = new Uri("https://radiusvision.com/live-cameras-demo-clips-mobotix-axis/");
				Regex reHref = new Regex(@"(<table[^>]*>(?:.|\n)*?<\/table>)");
				string html = new WebClient().DownloadString(uri);
				int index = 1;
				foreach (Match match in reHref.Matches(html))
				{
					var htmlTable = match.Groups[1].ToString();
					var reItems = new Regex(@"(<strong[^>]*>(?:.|\n)*?<\/strong>)");

					var fields = new List<string>();
					foreach (Match item in reItems.Matches(htmlTable))
					{
						var str = item.Groups[1].ToString();


						var indexRun = str.IndexOf("<strong>") + 8;
						var indexEnd = str.IndexOf("</strong>") - 8;

						var field = str.Substring(indexRun, indexEnd);

						if (field == $"<i>{index} </i>")
						{
							index++;

							if (fields.Count != 0)
							{
								var camera = new IpCamera();
								camera.Id = Guid.NewGuid();
								camera.Name = fields[0];
								camera.Host = fields[1];
								camera.Locations = new List<string>();
								for (var i = 2; i < fields.Count; i++)
								{
									camera.Locations.Add(fields[i]);
								}
								Cameras.Add(camera);
							}
							fields.Clear();
						}
						else
						{
							fields.Add(field);
						}
					}
				}
				Application.Current.Dispatcher.Invoke(new Action(() =>
				{
					CamerasComplated(null, Cameras);
				}));
			});
		}
	}

	public class IpCamera
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Host { get; set; }
		public IList<string> Locations { get; set; }
	}
}
