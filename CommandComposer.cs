using EnzoCommanderSDK.mef;
using EnzoCommanderSDK.mef.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EnzoCommanderSDK.mef.structure.EnzoCommand;

namespace EnzoCommanderSDK
{
    public class CommandComposer
    {

        string extRoot = $"extensions\\";
        [Export("stage")]
        STAGE _stage { get; }

        [Export("apiKey")]
        string _apiKey { get; }

        [Export("fileDir")]
        string _fileDir { get; }
        //[Export]
        //string _mapping { get; }
        [Export("fileName")]
        string? _fileName { get; }

        [Export("uploadRoot")]
        string? _uploadRoot { get; }
        //[Export]
        //bool? _sendFile { get; }

        CompositionContainer _container = new CompositionContainer();

        [Import]
        public Commander cmd { get; set; }
        //[Import(typeof(ICommander))]
        //public ICommander cmd { get; set; }

        //public CommandComposer(string apiKey, string mapping, string fileName, STAGE stage = STAGE.PRE_PROD, bool? sendFile = null)

        public CommandComposer(string apiKey, string fileDir, string? uploadRoot = null, STAGE stage = STAGE.PRE_PROD, string? fileName = null)
        {

            //cmd = new Commander(apiKey, fileDir, stage);

            _apiKey = apiKey;
            _fileDir = fileDir;
            _fileName = fileName;
            _uploadRoot= uploadRoot;
            //_mapping = mapping;
            _stage = stage;
            //_sendFile = sendFile;

            string extensionsPath = $"{_fileDir}\\{extRoot}";
            if (!Directory.Exists(extensionsPath))
            {
                Directory.CreateDirectory(extensionsPath);
            }

            if (string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException(nameof(apiKey));

            _apiKey = apiKey;

            var catalog = new AggregateCatalog();


            var dirCatalog = new DirectoryCatalog(extensionsPath); //external DLL plugin folder...

            catalog.Catalogs.Add(dirCatalog);
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(CommandComposer).Assembly));

            _container = new CompositionContainer(catalog);
            _container.ComposeParts(this);

            //var t = _container.Catalog?.FirstOrDefault();

            //cmd?.Init(_apiKey);

        }







    }
}
