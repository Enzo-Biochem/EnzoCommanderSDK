using EnzoCommanderSDK.mef.Interface;
using EnzoCommanderSDK.mef.structure;
using EnzoCommanderSDK.SDK.Handlers;
using EnzoCommanderSDK.SDK.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EnzoCommanderSDK.mef.structure.EnzoCommand;

namespace EnzoCommanderSDK.mef
{
    [Export]
    public class Commander
    {


        Dictionary<string, string> switches = new Dictionary<string, string>()
        {
            {$"-S \"[mapping]\"", "Switch used to send a file to our website, the [mapping] determines which file to send." },
            {$"-G \"[mapping]\"", "Switch used to download a file from our website, the [mapping] determines which file to get." },
            {"-V","Retrieves the version numbers and mapping names available for use." }
        };



        [ImportMany]
        public IEnumerable<Lazy<EnzoCommand, IEnzoCommandData>> commands;

        public STAGE _stage { get; }
        public string _apiKey { get; }
        public string _fileDir { get; }
        public string? _fileName { get; }



        CompositionContainer _container = new CompositionContainer();
        string extRoot = $"extensions\\";

        [ImportingConstructor]
        //public Commander([Import("_apiKey")]string apiKey, [Import("_mapping")]string mapping, [Import("_fileDir")]string fileDir, string? fileName = null, [Import("")]STAGE stage = STAGE.PRE_PROD, bool? sendFile = null)
        public Commander([Import("apiKey")] string apiKey, [Import("fileDir")] string fileDir, [Import("stage")] STAGE stage = STAGE.PRE_PROD, [Import("fileName")] string? fileName = null)
        {


            _apiKey = apiKey;
            _fileDir = fileDir;
            _stage = stage;
            _fileName = fileName;
        }

        public virtual async Task<bool> SendFile(string mapping)
        {
            bool rsl = false;


            var ops = commands.ToList().Find(c => c.Metadata.Mapping.ToUpper().Contains(mapping.ToUpper()));

            if (ops == null)
            {
                throw new Exception($"Commander - SendFile(): {mapping} component was not found!");
            }

            ops.Value.HandshakeFailed += (sender, err) =>
            {
                OnHandshakeFailed.Invoke(sender, err);
            };

            rsl = await ops.Value.SendFileAsync(mapping);


            ops.Value.HandshakeFailed -= (sender, err) => { };


            return rsl;
            //string url = ops.Value.GetURL(sendFile: true);

            //string fileName = !string.IsNullOrEmpty(_fileName) ? _fileName : $"{mapping}.csv";

            //string filePath = $"{_fileDir.TrimEnd('\\')}\\{fileName}";

            //if (!File.Exists(filePath)) { return true; }

            //Stream fileStrm = File.OpenRead(filePath);

            //var form = ops.Value.BuildContent(mapping, fileName, fileStrm);

            ////if it's null the file was found but no content in it.
            //if (form == null) { return true; }

            //using (HttpClient client = new HttpClient())
            //{
            //    var msg = await client.PostAsync(url, form);

            //    switch (msg.StatusCode)
            //    {
            //        case HttpStatusCode.OK:
            //            rsl = true;
            //            break;
            //        case HttpStatusCode.Unauthorized:
            //            throw new Exception("Server says you are unauthorized to send this file!");
            //        case HttpStatusCode.BadRequest:
            //            throw new Exception("Server says you are submitting a bad request!");
            //        case HttpStatusCode.NotFound:
            //            throw new Exception($"Can\'t send the file because the url related to the {mapping} mapping was not found.");
            //        default:
            //            return false;
            //    }



            //}

            //return rsl;
        }

        public virtual async Task<bool> GetFile(string mapping)
        {

            bool rsl = false;

            var ops = commands.ToList().Find(c => c.Metadata.Mapping.ToUpper().Contains(mapping.ToUpper()));

            if (ops == null) throw new Exception($"Commander - SendFile(): {mapping} component was not found!");

            //var onErr = async d (object? s, EnzoCommandEventArgs e) =>
            //{
            //    OnHandshakeFailed?.Invoke(s, e);
            //    return;

            //};

            ops.Value.HandshakeFailed += (sender, err) =>
            {
                OnHandshakeFailed?.Invoke(sender, err);
            };


            //ops.Value.HandshakeFailed += onErr;

            rsl = await ops.Value.GetFileAsync(mapping);

            //ops.Value.HandshakeFailed -= (sender, err) => { };

            return rsl;
            //string url = ops.Value.GetURL(sendFile: false);

            //string fileName = !string.IsNullOrEmpty(_fileName) ? _fileName : $"{mapping}.csv";

            //string filePath = $"{_fileDir.TrimEnd('\\')}\\{fileName}";

            //var form = ops.Value.BuildContent(mapping, fileName);

            //using (HttpClient client = new HttpClient())
            //{
            //    var msg = await client.PostAsync(url, form);

            //    if (msg.StatusCode == System.Net.HttpStatusCode.NotFound) throw new Exception("Customer.GetFile() - This link was not found!");
            //    if (msg.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception("Customer.GetFile() - Bad request!!");
            //    var fileArry = await msg.Content.ReadAsByteArrayAsync();

            //    if (fileArry != null && fileArry.Any())
            //    {
            //        await File.WriteAllBytesAsync(filePath, fileArry);
            //    }

            //    rsl = true;
            //}

            //return rsl;
        }

        public virtual List<string> GetVersion()
        {
            List<string> plugInVersions = new List<string>();


            plugInVersions.Add($"{this.GetType().FullName} \n Version: {this.GetType().Assembly.ImageRuntimeVersion}");

            commands.ToList().ForEach(cmd =>
            {
                plugInVersions.Add(cmd.Value.VersionDetail());
            });


            return plugInVersions;
        }


        public virtual List<string> GetSwitches()
        {
            return switches.Select(s => $"{s.Key} - {s.Value}").ToList();
        }

        public virtual List<string> GetHelp()
        {
            return commands.Select(c => c.Value.Help()).ToList();
        }

        public virtual List<string> GetAvailableMappings()
        {
            return commands.Select(c => c.Metadata.Mapping.ToLower()).ToList();
        }

        public event EventHandler<EnzoCommandEventArgs> OnHandshakeFailed;

    }
}
