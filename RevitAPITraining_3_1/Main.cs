using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITraining_3_1
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> reference = uidoc.Selection.PickObjects(ObjectType.Face, "Выберите элементы по грани");
            List<ElementId> walls = new List<ElementId>();

            double Sum = 0;

            foreach (var item in reference)
            {
                Element element = doc.GetElement(item);
                if (element is Wall)
                {
                    Wall wall = element as Wall;
                    if (!walls.Contains(wall.Id))
                    {
                        walls.Add(wall.Id);
                        double Sum1 = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
                        double Sum2 = UnitUtils.ConvertFromInternalUnits(Sum1, UnitTypeId.CubicMeters);

                        Sum += Sum2;
                    }
                }
            }
            TaskDialog.Show("Объем стен", $"{Sum.ToString()} m3");
            return Result.Succeeded;
        }
    }
}
