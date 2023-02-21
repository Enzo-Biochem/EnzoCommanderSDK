using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using EnzoCommanderSDK.mef.Interface;
using EnzoCommanderSDK.mef;
using EnzoCommanderSDK.mef.structure;

namespace EnzoCommanderSDK
{
    [Export(typeof(ICommander))]
    public class Core : ICommander
    {


        [ImportMany()]
        IEnumerable<Lazy<IEnzoCommand, IEnzoCommandData>> commands { get; set; }


        string _apiKey { get; set; }


        public void Init(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException(nameof(apiKey));

            _apiKey = apiKey;
        }

        //public Core(string apiKey)
        //{

        //    //if (type != null) catalog.Catalogs.Add(new AssemblyCatalog(typeof(Core).Assembly)); //Import internal plugins..


        //    _apiKey = apiKey;

        //}



        public async Task<bool> Execute(string mapping, bool sendFile = false)
        {

            if (string.IsNullOrEmpty(mapping)) throw new ArgumentNullException(nameof(mapping));

            bool rsl = false;

            var ops = commands.ToList().Find(c => c.Metadata.Mapping.ToUpper().Contains(mapping.ToUpper()));

            string path = $"{Path.GetDirectoryName(Environment.CurrentDirectory)}\\downloads\\" ?? "downloads\\";

            try
            {
                if (sendFile)
                {
                    rsl = await ops.Value.SendFile(_apiKey, path);
                }
                else
                {
                    rsl = await ops.Value.GetFile(_apiKey, path);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Core - Execute()", ex);
            }


            return rsl;
        }

        //public bool GetFile(string mapping)
        //{
        //    if (string.IsNullOrEmpty(mapping)) throw new ArgumentNullException(nameof(mapping));

        //    bool rsl = false;




        //    return rsl;
        //}

    }


    ////[Export(typeof(Commander))]
    //public class CoreTest : Commander
    //{
    //    [ImportingConstructor]
    //    public CoreTest(string apiKey, string mapping, string fileDir, string? fileName = null, EnzoCommand.STAGE stage = EnzoCommand.STAGE.PRE_PROD, bool? sendFile = null) : base(apiKey, mapping, fileDir, fileName, stage, sendFile)
    //    {
    //    }

    //    //public Commander Commander { get => new Commander(_apiKey, _mapping, _fileDir, _fileName); }
    //}
}
