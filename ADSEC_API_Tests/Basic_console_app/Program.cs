﻿// See https://aka.ms/new-console-template for more information
using Oasys.AdSec;
using Oasys.AdSec.DesignCode;
using Oasys.AdSec.IO.Serialization;
using Oasys.AdSec.Reinforcement;
using Oasys.AdSec.Reinforcement.Groups;
using Oasys.AdSec.Reinforcement.Layers;
using Oasys.AdSec.StandardMaterials;
using Oasys.Profiles;
using Oasys.Units;
using System.Diagnostics;
using UnitsNet;


var version = IVersion.Api();
Console.WriteLine(version);

var profile = ICircleProfile.Create(Length.FromMillimeters(1000));
var sectionMaterial = Concrete.EN1992.Part1_1.Edition_2004.NationalAnnex.GB.Edition_2014.C20_25;
var section = ISection.Create(profile, sectionMaterial);
section.Cover = ICover.Create(Length.FromMillimeters(50));
var rebarMaterial = Reinforcement.Steel.EN1992.Part1_1.Edition_2004.NationalAnnex.GB.Edition_2014.S500B;

var profile_two = ICircleProfile.Create(Length.FromMillimeters(100));
var steel_meterial = Steel.EN1993.Edition_2005.S235;
var section_two = ISection.Create(profile_two, steel_meterial);
var sub_one = ISubComponent.Create(section_two, IPoint.Create(Length.FromMillimeters(-200), Length.FromMillimeters(0)));
var sub_two = ISubComponent.Create(section_two, IPoint.Create(Length.FromMillimeters(200), Length.FromMillimeters(0)));
section.SubComponents.Add(sub_one);
section.SubComponents.Add(sub_two);

var rebarBundle = IBarBundle.Create(rebarMaterial, Length.FromMillimeters(20));
var rebarlayer = ILayerByBarPitch.Create(rebarBundle, Length.FromMillimeters(150));
var rebarLayer = ICircleGroup.Create(IPoint.Create(Length.FromMillimeters(0), Length.FromMillimeters(0)), Length.FromMillimeters(400), Angle.FromDegrees(20), rebarlayer);
section.ReinforcementGroups.Add(rebarLayer);
Console.WriteLine(AdSec_API_Utils.Textify.Text(section));

var model = IAdSec.Create(EN1992.Part1_1.Edition_2004.NationalAnnex.GB.Edition_2014);
var solution = model.Analyse(section);

var load = ILoad.Create(Force.FromKilonewtons(-1000), Moment.FromKilonewtonMeters(50), Moment.FromKilonewtonMeters(50));
//var ulsResult = solution.Strength.Check(load);
//var slsResult = solution.Serviceability.Check(load);

//Console.WriteLine(AdSec_API_Utils.Textify.Text(ulsResult));
//Console.WriteLine(AdSec_API_Utils.Textify.Text(slsResult));

JsonConverter jsonConverter = new JsonConverter(EN1992.Part1_1.Edition_2004.NationalAnnex.GB.Edition_2014);
string sectionJson = jsonConverter.SectionToJson(section);

//var file = AdSec_API_Utils.FileHelper.GetAdsFileNamewithCurrentTime();
//using (StreamWriter streamWriter = File.AppendText(file))
//{
//    streamWriter.Write(sectionJson);
//}

//using Process myProcess = new Process();
//myProcess.StartInfo.FileName = @"C:\Users\ravikumar.gubbala\Desktop\AdSec Versions\AdSec 10.0.6.8\AdSec.exe";
//myProcess.StartInfo.Arguments = " " + "\"" + file + "\"";
//myProcess.Start();

var parsed = JsonParser.Deserialize(sectionJson);
var section1 = parsed.Sections[0];

Console.WriteLine(AdSec_API_Utils.Textify.Text(section1));

//string filePath = @"C:\Users\ravikumar.gubbala\Desktop\AdSec Test files\REsection.ads";
//string filePath = @"C:\Users\ravikumar.gubbala\Desktop\AdSec Test files\DoubleLayerCircleRft.ads";
//string fileJSONstring = File.ReadAllText(filePath);
//var parsed = JsonParser.Deserialize(fileJSONstring);
//if(parsed != null)
//{
//    var section = parsed.Sections[0];
//    foreach (IGroup reinfocementGroup in section.ReinforcementGroups)
//    {
//        Console.WriteLine(reinfocementGroup.GetType().ToString());
//    }
//}


