namespace AdSec_API_Utils
{
    using Oasys.AdSec;
    using Oasys.AdSec.Reinforcement;
    using Oasys.AdSec.Reinforcement.Groups;
    using Oasys.AdSec.Reinforcement.Layers;
    using Oasys.AdSec.Reinforcement.Preloads;
    using Oasys.Profiles;

    public static class Textify
    {
        public static string Text(ILoad load)
        {
            string text = string.Empty;
            text += Environment.NewLine;
            text += "Load: Axial : " + string.Format("{0:e2}", load.X.Newtons) +
                " Myy : " + string.Format("{0:e2}", load.YY.NewtonMeters) +
                " Mzz : " + string.Format("{0:e2}", load.ZZ.NewtonMeters);
            return text;
        }

        public static string Text(IDeformation deformaton)
        {
            string text = string.Empty;
            text += Environment.NewLine;
            text += "Deformation: ex : " + string.Format("{0:e2}", deformaton.X.Ratio) +
                " kyy : " + string.Format("{0:e2}", deformaton.YY.PerMeters) +
                " kzz : " + string.Format("{0:e2}", deformaton.ZZ.PerMeters);
            return text;
        }

        public static string Text(IMomentRange momentRange)
        {
            string text = string.Empty;
            text += Environment.NewLine;
            text += "Max : " + string.Format("{0:e2}", momentRange.Max.NewtonMeters) +
            " Min : " + string.Format("{0:e2}", momentRange.Min.NewtonMeters);
            return text;
        }

        public static string Text(IList<IMomentRange> momentRanges)
        {
            string text = string.Empty;
            foreach (IMomentRange range in momentRanges)
            {
                text += Text(range);
            }
            return text;
        }

        public static string Text(IStrengthResult ulsResult)
        {
            string text = string.Empty;
            text += Environment.NewLine;
            text += "ULS Result: " + Text(ulsResult.Load) + Text(ulsResult.Deformation) + Environment.NewLine +
                "Load Utilisation : " + string.Format("{0:e2}", ulsResult.LoadUtilisation.Value) + Environment.NewLine +
                "Deformation Utilisation : " + string.Format("{0:e2}", ulsResult.DeformationUtilisation.Value) + Environment.NewLine +
               "Moment ranges : " + Text(ulsResult.MomentRanges) + Environment.NewLine;
            return text;
        }

        public static string Text(ICrack crack)
        {
            string text = string.Empty;
            if (crack != null)
            {
                text += Environment.NewLine;
                text += "Crack: Width: " + crack.Width.Meters + Environment.NewLine;
                text += Text(crack.Position);
            }
            return text;
        }

        public static string Text(IServiceabilityResult slsResult)
        {
            string text = string.Empty;
            text += Environment.NewLine;
            text += "SLS Result: " + Text(slsResult.Load) + Text(slsResult.Deformation) + Environment.NewLine +
                "CrackingUtilisation : " + string.Format("{0:e2}", slsResult.CrackingUtilisation.Value) + Environment.NewLine +
                "MaximumWidthCrack : " + Text(slsResult.MaximumWidthCrack) + Environment.NewLine +
                "Uncracked moment ranges : " + Text(slsResult.UncrackedMomentRanges) + Environment.NewLine;
            return text;
        }

        public static string Text(ICover cover)
        {
            string text = string.Empty;
            text += "Cover: " + cover.UniformCover.Meters.ToString() + Environment.NewLine;
            return text;
        }

        public static string Text(IProfile profile)
        {
            string text = string.Empty;
            text += Environment.NewLine;
            text += "Profile: " + profile.Description() + Environment.NewLine;
            return text;
        }

        public static string Text(IList<IGroup> reinfocementGroups)
        {
            string text = string.Empty;
            foreach (IGroup? reinfocementGroup in reinfocementGroups)
            {
                text += Text(reinfocementGroup);
            }
            return text;
        }

        public static string Text(IBarBundle barBundle)
        {
            string text = string.Empty;
            text += Environment.NewLine;
            text += "Bar bundle: " + Environment.NewLine;
            text += "Material: " + barBundle.Material.GetType() + Environment.NewLine;
            text += "CountPerBundle: " + barBundle.CountPerBundle.ToString() + Environment.NewLine;
            text += "Diameter: " + string.Format("{0:e2}", barBundle.Diameter.Meters) + Environment.NewLine;
            return text;
        }

        public static string Text(ILayer layer)
        {
            string text = string.Empty;
            text += Environment.NewLine;
            text += "Layer: " + Text(layer.BarBundle) + Environment.NewLine;
            return text;
        }

        public static string Text(IList<ILayer> layers)
        {
            string text = string.Empty;
            foreach (ILayer layer in layers)
            {
                text += Text(layer);
            }
            return text;
        }

        public static string Text(IPoint point)
        {
            string text = string.Empty;
            text += "Point: " + string.Format("X: {0:e2}; Y: {1:e2}", point.Y.Meters, point.Y.Meters);
            text += Environment.NewLine;
            return text;
        }

        public static string Text(IList<IPoint> points)
        {
            string text = string.Empty;
            foreach (IPoint point in points)
            {
                text += Text(point);
            }
            return text;
        }

        public static string Text(IWarning warning)
        {
            string text = string.Empty;
            text += "Warning: " + warning.Description;
            text += Environment.NewLine;
            return text;
        }


        public static string Text(IPreload preload)
        {
            string text = string.Empty;
            var preForce = preload as Oasys.AdSec.Reinforcement.Preloads.IPreForce;
            if (preForce != null)
            {
                text += "PreForce: " + string.Format("{0:e2}", preForce.Force.Newtons);
            }
            else
            {
                var preStress = preload as Oasys.AdSec.Reinforcement.Preloads.IPreStress;
                if (preStress != null)
                {
                    text += "PreStress: " + string.Format("{0:e2}", preStress.Stress.NewtonsPerSquareMillimeter);
                }
                else
                {
                    var preStrain = preload as Oasys.AdSec.Reinforcement.Preloads.IPreStrain;
                    if (preStrain != null)
                    {
                        text += "PreStrain: " + string.Format("{0:e2}", preStrain.Strain.Ratio);
                    }
                }
            }
            return text;
        }

        public static string Text(IGroup reinfocementGroup)
        {
            string text = string.Empty;
            if (reinfocementGroup != null)
            {
                text += "Reinfocement Group: " + reinfocementGroup.GetType().ToString() + Environment.NewLine;

                var TemplateGroup = reinfocementGroup as Oasys.AdSec.Reinforcement.Groups.ITemplateGroup;
                if (TemplateGroup != null)
                {
                    text += "Template group: " + Environment.NewLine;
                    text += Text(TemplateGroup.Preload);
                    text += Text(TemplateGroup.Layers);
                }
                else
                {
                    var LongitudinalGroup = reinfocementGroup as Oasys.AdSec.Reinforcement.Groups.ILongitudinalGroup;
                    if (LongitudinalGroup != null)
                    {
                        text += "Longitudinal group: " + Environment.NewLine;
                        text += Text(LongitudinalGroup.Preload);

                        var perimeterGroup = reinfocementGroup as Oasys.AdSec.Reinforcement.Groups.IPerimeterGroup;
                        if (perimeterGroup != null)
                        {
                            text += "Perimeter group: " + Environment.NewLine;
                            text += Text(perimeterGroup.Layers);
                        }

                        var singleBarGroup = reinfocementGroup as Oasys.AdSec.Reinforcement.Groups.ISingleBars;
                        if (singleBarGroup != null)
                        {
                            text += "Single bar group: " + Environment.NewLine;
                            text += Text(singleBarGroup.BarBundle);
                            text += Text(singleBarGroup.Positions);
                        }
                        
                        var lineGroup = reinfocementGroup as Oasys.AdSec.Reinforcement.Groups.ILineGroup;
                        if (lineGroup != null)
                        {
                            text += "Line group: " + Environment.NewLine;
                            text += Text(lineGroup.Layer);
                            text += "First bar: " + Text(lineGroup.FirstBarPosition);
                            text += "Last bar: " + Text(lineGroup.LastBarPosition);
                        }
                    }
                    else
                    {
                        var LinkGroup = reinfocementGroup as Oasys.AdSec.Reinforcement.Groups.ILinkGroup;
                        if (LinkGroup != null)
                        {
                            text += "link group: " + Environment.NewLine;
                            text += Text(LinkGroup.BarBundle) + Environment.NewLine;
                        }
                    }
                }
            }
            return text;
        }

        public static string Text(IList<ISubComponent> subComponents)
        {
            string text = string.Empty;
            if (subComponents.Count > 0)
            {
                text += "Subcompnents: " + Environment.NewLine;
                foreach (ISubComponent subComponent in subComponents)
                {
                    text += Text(subComponent.Offset);
                    text += Text(subComponent.Section);
                }
            }
            return text;
        }

        public static string Text(ISection section)
        {
            string text = string.Empty;
            text += Environment.NewLine;
            text += "Section: " + Environment.NewLine;
            text += Text(section.Profile);
            text += Text(section.ReinforcementGroups) + Environment.NewLine;
            text += Text(section.SubComponents) + Environment.NewLine;
            return text;
        }
    }
}