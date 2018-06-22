﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Algorithm.MVC.Helper
{
    public struct FileData
    {
        #region Static Variable
        public static string InputDirectory { get; set; } =
            @"~/Users/input-files/";
        public static string OutputDirectory { get; set; }
        = @"~/Users/output-files/";
        public static string WexBIMDirectory { get; set; }
        = @"~/Users/wexbim-files/";
        #endregion

        #region Properties
        
        public string FileName { get; set; }

        public string InputName { get; set; }
        public string OutputName { get; set; }
        public string WexBIMArcName { get; set; }
        public string WexBIMStrName { get; set; }
        public string InputPath { get; set; }
        public string OutputPath { get; set; }
        public string WexBIMPathArc { get; set; }
        public string WexBIMPAthStr { get; set; }
        #endregion
        public FileData(string fileName)
        {
            // get File Name To store in DB;
            FileName = fileName;

            //set /data
            InputName = FileName;
            var ext = Path.GetExtension(FileName);
            var withoutExt = Path.GetFileNameWithoutExtension(FileName);
            OutputName = withoutExt + "- Structure" + ext;
            WexBIMArcName = withoutExt + ".WexBIM";
            WexBIMStrName = withoutExt + "- Structure" + ".WexBIM";
            //get File Path to retrive from directories;
            InputPath = HostingEnvironment.MapPath(InputDirectory + InputName);
            OutputPath = HostingEnvironment.MapPath(OutputDirectory + OutputName);
            WexBIMPathArc = HostingEnvironment.MapPath(WexBIMDirectory + WexBIMArcName);
            WexBIMPAthStr = HostingEnvironment.MapPath(WexBIMDirectory + WexBIMStrName);
        }
        public void FromName(string name)
        {
            FileName = name;
            SetData();
        }
        private void SetData()
        {
            InputName = FileName;
            var ext = Path.GetExtension(FileName);
            var withoutExt = Path.GetFileNameWithoutExtension(FileName);
            OutputName = withoutExt + "- Structure" + ext;
            WexBIMArcName = withoutExt + ".WexBIM";
            WexBIMStrName = withoutExt + "- Structure" + ".WexBIM";
            //get File Path to retrive from directories;
            InputPath = HostingEnvironment.MapPath(InputDirectory + InputName);
            OutputPath = HostingEnvironment.MapPath(OutputDirectory + OutputName);
            WexBIMPathArc = HostingEnvironment.MapPath(WexBIMDirectory + WexBIMArcName);
            WexBIMPAthStr = HostingEnvironment.MapPath(WexBIMDirectory + WexBIMStrName);
        }
    }
}