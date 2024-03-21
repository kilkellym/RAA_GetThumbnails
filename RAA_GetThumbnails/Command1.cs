#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Linq;
#endregion

namespace RAA_GetThumbnails
{
	[Transaction(TransactionMode.Manual)]
	public class Command1 : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			// this is a variable for the Revit application
			UIApplication uiapp = commandData.Application;

			// this is a variable for the current Revit model
			Document doc = uiapp.ActiveUIDocument.Document;
			string tempSavePath = Path.Combine(@"C:\temp", "FamilyThumbnails");

			// get families from the model
			FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(Family));

			int i = 0;
			int imageCounter = 0;
			int imageLimit = 35;

			List<ImageEntity> images = new List<ImageEntity>();
			do
			{
				Family family = collector.ElementAt(i) as Family;
				string path = ExportFamily(doc, family, tempSavePath);

				if (!string.IsNullOrEmpty(path))
				{
					images.Add(ImageView.GetImageEntity(path));
					imageCounter++;
				}
				i++;
			} while (imageCounter < imageLimit);

			List<ImageEntity> sortedImages = images.OrderBy(x => x.FileName).ToList();

			// Show the images in a window
			ImageWindow imageWindow = new ImageWindow(sortedImages);
			imageWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner; 
			imageWindow.ShowDialog();

			return Result.Succeeded;
		}
		public string ExportFamily(Document doc, Family family, string directoryPath)
		{
			string returnPath = "";

			// Ensure the directory exists
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			string savePath = Path.Combine(directoryPath, family.Name + ".rfa");

			// Open the family document
			if(family.IsEditable)
			{
				Document familyDoc = doc.EditFamily(family);
				if (familyDoc != null)
				{
					if(!File.Exists(savePath))
					{
						// Save the family document to the specified path
						SaveAsOptions options = new SaveAsOptions();
						options.OverwriteExistingFile = true;

						familyDoc.SaveAs(savePath, options);
					
						returnPath = familyDoc.PathName;

						// Optionally, you might want to close the family document if you're done with it
						familyDoc.Close(false); // 'false' to close without saving changes made during the session
					}
					else
						returnPath = savePath;
				}
			}
			
			return returnPath;

		}

		internal static PushButtonData GetButtonData()
		{
			// use this method to define the properties for this command in the Revit ribbon
			string buttonInternalName = "btnCommand1";
			string buttonTitle = "Button 1";

			ButtonDataClass myButtonData1 = new ButtonDataClass(
				buttonInternalName,
				buttonTitle,
				MethodBase.GetCurrentMethod().DeclaringType?.FullName,
				Properties.Resources.Blue_32,
				Properties.Resources.Blue_16,
				"This is a tooltip for Button 1");

			return myButtonData1.Data;
		}
	}
}
