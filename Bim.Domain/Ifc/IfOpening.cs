﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bim.Common.Measures;
using Bim.Domain;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.ProductExtension;

namespace Bim.Domain.Ifc
{

   
   
    public class IfOpening : IfElement
    {

        public IfWall IfWall { get; set; }
        public OpeningType OpeningType { get; set; }
        public Direction Direction { get; set; }

        public IIfcRelVoidsElement IfcOpening { get; set; }
        public IIfcLocalPlacement LocalPlacement { get; set; }
        public IfOpening():base(null)
        {

        }
        public IfOpening(IfWall ifWall, IIfcRelVoidsElement ifcOpening):base(ifWall.IfModel)
        {
            IfWall = ifWall;
            IfcOpening = ifcOpening;
        }
        public IfOpening( IfOpening opening):base(opening.IfModel)
        {
            IfWall = opening.IfWall;
            OpeningType = opening.OpeningType;
            Direction = opening.Direction;
            IfcOpening = opening.IfcOpening;
            LocalPlacement = opening.LocalPlacement;
            IfLocation = new IfLocation( opening.IfLocation);
            IfDimension = new IfDimension( opening.IfDimension);
        }

        public void Flip(Axis axis)
        {
            switch (axis)
            {
                case Axis.xAxis:
                    IfLocation.X = IfLocation.X - IfDimension.XDim;
                    break;
                case Axis.yAxis:
                    IfLocation.Y = IfLocation.Y - IfDimension.YDim;
                    break;
                case Axis.zAxis:
                    IfLocation.Z = IfLocation.Z - IfDimension.ZDim;
                    break;
                default:
                    IfLocation.X = IfLocation.X - IfDimension.XDim;
                    IfLocation.Y = IfLocation.Y - IfDimension.YDim;
                    IfLocation.Z = IfLocation.Z - IfDimension.ZDim;
                    break;
            }
        }


        public static List<IfOpening> GetOpenings(IfWall ifWall)
        {
            var openings = new List<IfOpening>();
            IfOpening ifopening;
            foreach (var opening in ifWall.IfcWall.HasOpenings)
            {

                ifopening = new IfOpening(ifWall, opening);

                var opnng = (IIfcAxis2Placement3D)((IIfcLocalPlacement)opening
                    .RelatedOpeningElement.ObjectPlacement).RelativePlacement;

                var oLocation = opnng.Location;//get opening location point

                var recProfile = opening.RelatedOpeningElement.Representation.Representations.SelectMany(a => a.Items)
                    .OfType<IIfcExtrudedAreaSolid>().Select(a => a.SweptArea)
                    .OfType<IIfcRectangleProfileDef>().FirstOrDefault(); //get rec profile

                var recDepth = opening.RelatedOpeningElement.Representation.
                    Representations.SelectMany(a => a.Items).
                    OfType<IIfcExtrudedAreaSolid>().Select(a => a.Depth).FirstOrDefault();
                //get filling elment doors or windows
                var voids = ((IfcOpeningElement)opening.RelatedOpeningElement)
                    .HasFillings.FirstOrDefault();

                var voidsPlacement = (IfcLocalPlacement)voids.RelatedBuildingElement.ObjectPlacement;
                ifopening.LocalPlacement = voidsPlacement;

                var dir = ((IIfcAxis2Placement3D)voidsPlacement.RelativePlacement).RefDirection;

                string filling = " ";
                if (voids != null)
                {
                    filling = voids.RelatedBuildingElement.GetType().Name;
                }
                else
                {
                    filling = "not Defined";
                }


                ifopening.IfLocation = new IfLocation(Length.FromFeet(oLocation.X).Inches, Length.FromFeet(oLocation.Y).Inches, Length.FromFeet(oLocation.Z).Inches);

                ifopening.IfDimension = new IfDimension(Length.FromFeet(recProfile.YDim).Inches, Length.FromFeet(recDepth).Inches, Length.FromFeet(recProfile.XDim).Inches);

                switch (filling)
                {
                    case "IfcDoor":
                        ifopening.OpeningType = OpeningType.Door;
                        ifopening.IfDimension = new IfDimension(Length.FromFeet(recProfile.XDim).Inches, Length.FromFeet(recDepth).Inches, Length.FromFeet(recProfile.YDim).Inches);

                        break;
                    case "IfcWindow":
                        ifopening.OpeningType = OpeningType.Window;
                        ifopening.IfDimension = new IfDimension(Length.FromFeet(recProfile.YDim).Inches, Length.FromFeet(recDepth).Inches, Length.FromFeet(recProfile.XDim).Inches);
                        break;
                    default:
                        ifopening.OpeningType = OpeningType.Window;
                        break;

                }

                if (dir!=null &&dir.X < 0)
                {
                    ifopening.Direction = Direction.Negative;
                    //ifopening.Flip(Axis.xAxis);
                }
                else
                {
                    ifopening.Direction = Direction.Positive;
                }

                openings.Add(ifopening);
            }

            return openings;
        }
    }
}
