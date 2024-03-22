using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace RAA_GetThumbnails
{
	internal class ImageView
	{
		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);
		public static ImageEntity GetImageEntityFromType(ElementType type)
		{
			Size imgSize = new Size(200, 200);

			Bitmap image = type.GetPreviewImage(imgSize);

			try
			{
				if(image != null)
				{
					// return image data
					ImageEntity ie = new ImageEntity();
					ie.FileName = type.Name;
					ie.ImageBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
						image.GetHbitmap(),
						IntPtr.Zero,
						System.Windows.Int32Rect.Empty,
						System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
					return ie;
				}
				else
				{
					return null;
				}
				
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		public static List<ImageEntity> GetAllImagesData(List<string> listFamilyFiles)
		{
			try
			{
				List<ImageEntity> list = new List<ImageEntity>();
				foreach (string familyFile in listFamilyFiles)
				{
					//ImageEntity ie = GetImageEntity(familyFile);

					//list.Add(ie);
				}
				return list;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
	public class ImageEntity
	{
		public string FileName { get; set; }
		public System.Windows.Media.ImageSource ImageBitmap { get; set; }
	}
}
