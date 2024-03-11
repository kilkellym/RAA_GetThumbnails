using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace RAA_GetThumbnails
{
	/// <summary>
	/// Interaction logic for ImageWindow.xaml
	/// </summary>
	public partial class ImageWindow : Window
	{
		public ObservableCollection<ImageEntity> Images { get; set; }
		public ImageWindow(List<ImageEntity> imageList)
		{
			InitializeComponent();
			Images = new ObservableCollection<ImageEntity>();
			this.DataContext = this;
			LoadImages(imageList);
		}

		// Method to add images, call this method to populate your grid
		public void LoadImages(IEnumerable<ImageEntity> imageEntities)
		{
			foreach (var imageEntity in imageEntities)
			{
				Images.Add(imageEntity);
			}

			ImageGrid.ItemsSource = Images;
		}

		// Optionally, you can load images from paths directly (example method)
		public void AddImageFromPath(string path, string fileName)
		{
			var imageEntity = new ImageEntity
			{
				ImagePath = path,
				FileName = fileName,
				ImageBitmap = new BitmapImage(new Uri(path))
			};

			Images.Add(imageEntity);
		}

		private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			TaskDialog td = new TaskDialog("Image Clicked");
			
			TaskDialog.Show("Image Clicked", "Insert family into Revit");
		}
	}
}
