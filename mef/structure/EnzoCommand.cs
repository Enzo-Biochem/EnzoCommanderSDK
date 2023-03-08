using CsvHelper;
using EnzoCommanderSDK.SDK.Handlers;
using EnzoCommanderSDK.SDK.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EnzoCommanderSDK.mef.structure.EnzoCommand;

namespace EnzoCommanderSDK.mef.structure
{
    public abstract class EnzoCommand
    {

        static class _PLACE_HOLDERS
        {
            public const string ACTION = "[action]";
            public const string STAGE = "[stage]";
        }

        static class _ACTION_OPTIONS
        {
            public const string EXPORT = "export";
            public const string IMPORT = "import";
        }

        static class _STAGE_OPTIONS
        {
            public const string DEV = "https://enzo-develop.go-vip.net";
            public const string PRE_PROD = "https://enzo-preprod.go-vip.net";
            public const string PROD = "https://www.enzo.com";
        }

        public enum STAGE
        {
            UNKNOWN,
            DEV,
            PRE_PROD,
            PROD
        }

        const string _URL_TEMPLATE = "[stage]/wp-json/enzo-navision/v1/[action]-csv";

        STAGE _stage { get; }
        public string _apiKey { get; }
        public string _filePath { get; }
        public string? _fileName { get; }

        //[ImportingConstructor]
        //public EnzoCommand(string apiKey, string mapping, string fileName, STAGE stage = STAGE.PRE_PROD, bool? sendFile = null) : base()
        public EnzoCommand([Import("apiKey")] string apiKey, [Import("fileDir")] string fileDir, [Import("stage")] STAGE stage = STAGE.PRE_PROD, [Import("fileName")] string? fileName = null)//(string apiKey, string fileDir, STAGE stage = STAGE.PRE_PROD)
        {
            _apiKey = apiKey;
            _fileName = fileName;
            //_mapping = mapping;
            _filePath = fileDir;
            _stage = stage;
            //_sendFile = sendFile;
            return;
        }


        //public EnzoCommand() { }

        public virtual async Task<bool> GetFileAsync(string mapping, string? fileName = null)
        {
            bool rsl = false;

            string url = this.GetURL(sendFile: false);

            fileName = !string.IsNullOrEmpty(_fileName) ? _fileName : $"{mapping}.csv";

            string filePath = $"{_filePath.TrimEnd('\\')}\\downloads\\";

            FilePathExists(filePath);

            filePath = $"{filePath}{fileName}";

            var form = BuildContent(mapping, fileName); //BuildContent(mapping, fileName);

            using (HttpClient client = new HttpClient())
            {
                try
                {

                    var msg = await client.PostAsync(url, form);

                    //if (msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    //{
                    //    OnHandshakeFailed(await ErrorFactory(msg));

                    //    //if (!string.IsNullOrEmpty(errRsp))
                    //    //{
                    //    //    OnHandshakeFailed();
                    //    //}
                    //    //else
                    //    //{
                    //    //    OnHandshakeFailed(new ErrorResponse
                    //    //    {
                    //    //        code = "Unkown",
                    //    //        message = "This error was generated because the server had no further details on this error!",
                    //    //    });
                    //    //}

                    //    //OnUploadFailed()

                    //    throw new Exception($"{this.GetType().FullName}.GetFile() - This link was not found!");
                    //}
                    ////if (msg.StatusCode == System.Net.HttpStatusCode.NotFound) 
                    //if (msg.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    //{
                    //    OnHandshakeFailed(await ErrorFactory(msg));
                    //    throw new Exception($"{this.GetType().FullName}.GetFile() - Bad request!!");
                    //}


                    switch (msg.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            var fileArry = await msg.Content.ReadAsByteArrayAsync();

                            if (fileArry != null && fileArry.Any())
                            {
                                await File.WriteAllBytesAsync(filePath, fileArry);
                            }

                            rsl = true;
                            break;
                        default:
                            var err = new EnzoCommandEventArgs
                            {
                                Error = await ErrorFactory(msg),
                                Sender = $"{this.GetType().Name} - GetFileAssync()",
                            };



                            HandshakeFailed.Invoke(this, err);
                            //OnHandshakeFailed(this, await ErrorFactory(msg));
                            //throw new Exception($"{this.GetType().FullName}.GetFile() - Website response: {msg.ReasonPhrase} ");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("EnzoCommand - GetFileAsync()", ex);
                }



            }

            return rsl;
        }

        public virtual async Task<bool> SendFileAsync(string mapping)
        {
            bool rsl = false;

            string url = this.GetURL(sendFile: true);

            //Get the file:
            //if (string.IsNullOrEmpty(_fileName)) throw new NullReferenceException($"{this.GetType().FullName} - SendFileAsync(): file name cannot be null!");
            if (string.IsNullOrEmpty(_filePath)) throw new NullReferenceException($"{this.GetType().FullName} - SendFileAsync(): file path cannot be null!");

            //string filePath = $"{_filePath.TrimEnd('\\')}\\{_fileName}";

            //get all files being uploaded:
            var files = GetFileNames(mapping);

            foreach (var file in files)
            {
                Stream fileStrm = File.OpenRead(file);

                string fileName = _fileName ?? mapping;

                string[] parsedFile = file.Split('\\');

                fileName = parsedFile[parsedFile.Length - 1];


                var form = BuildContent(mapping, fileName, fileStrm: fileStrm);

                //if it's null the file was found but no content in it.
                if (form == null) { return true; }

                using (HttpClient client = new HttpClient())
                {
                    var msg = await client.PostAsync(url, form);

                    switch (msg.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            //Archive the file:
                            string archivePath = $"{_filePath.TrimEnd('\\')}\\uploads\\archived\\";

                            FilePathExists(archivePath);

                            File.Move(file, $"{archivePath}{fileName}");


                            rsl = true;
                            break;
                        case HttpStatusCode.Unauthorized:
                            HandshakeFailed?.Invoke(this, new EnzoCommandEventArgs
                            {
                                Error = await ErrorFactory(msg),
                                Sender = $"{this.GetType().Name} - SendFileAsync()"
                            });
                            break;
                        //throw new Exception("Server says you are unauthorized to send this file!");
                        case HttpStatusCode.BadRequest:
                            HandshakeFailed?.Invoke(this, new EnzoCommandEventArgs
                            {
                                Error = await ErrorFactory(msg),
                                Sender = $"{this.GetType().Name} - SendFileAsync()"
                            });
                            break;
                        //throw new Exception("Server says you are submitting a bad request!");
                        case HttpStatusCode.NotFound:
                            HandshakeFailed?.Invoke(this, new EnzoCommandEventArgs
                            {
                                Error = await ErrorFactory(msg),
                                Sender = $"{this.GetType().Name} - SendFileAsync()"
                            });
                            break;
                        //throw new Exception($"Can\'t send the file because the url related to the {mapping} mapping was not found.");
                        default:
                            return false;
                    }



                }
            }


            //if (!File.Exists(filePath)) throw new FileNotFoundException($"{this.GetType().FullName} - SendFileAsync(): {filePath} was not found!");


            return rsl;
        }

        /// <summary>
        /// Sends object of type List<T> directly to its HTTP destination without first creating a file.
        /// </summary>
        /// <typeparam name="T">Object of desired type</typeparam>
        /// <param name="obj"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<bool> SendFileAsync<T>(List<T> obj, string mapping) where T : class, new()
        {
            bool rsl = false;

            string url = this.GetURL(sendFile: true);

            using (StreamWriter ms = new StreamWriter(new MemoryStream()))
            {

                try
                {
                    using (var csv = new CsvWriter(ms, CultureInfo.InvariantCulture))
                    {

                        await csv.WriteRecordsAsync(obj);

                        var content = BuildContent(mapping, mapping, fileStrm: ms.BaseStream);

                        using (HttpClient client = new HttpClient())
                        {
                            var rsp = await client.PostAsync(url, content);


                            switch (rsp.StatusCode)
                            {
                                case HttpStatusCode.OK:

                                    return true;
                                default:
                                    OnHandshakeFailed(this, new EnzoCommandEventArgs
                                    {
                                        Error = await ErrorFactory(rsp)
                                    });

                                    return false;
                            }


                        }

                    }
                }
                catch (Exception ex)
                {
                    OnHandshakeFailed(this, new EnzoCommandEventArgs
                    {
                        Error = new ErrorResponse
                        {
                            code = ex.Message,
                            data = new Data
                            {
                                details = new[]{
                                    ex.Data
                                }
                            }
                        }
                    });

                    throw new Exception($"{this.GetType().Name} - SendAsync()", ex);
                }
                finally
                {
                    ms.Close();
                }


            }



            return rsl;
        }

        public virtual string VersionDetail()
        {

            return $"{this.GetType().FullName} \n Version: {this.GetType().Assembly.ImageRuntimeVersion}";
        }

        //public virtual HttpContent? BuildContent(string mapping, string fileName, Stream? fileStrm = null)
        //{
        //    HttpContent content = null;

        //    if (fileStrm != null)
        //    {
        //        //Send file, we need multipart:


        //        if (string.IsNullOrEmpty(_apiKey)) throw new Exception($"{nameof(_apiKey)} cannot be null!");
        //        if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName));

        //        bool rsl = false;

        //        //FilePathExists(_filePath);

        //        //if (!File.Exists(fileName)) return null; //Nothing to send...

        //        ////var strm = File.OpenRead(fileName);

        //        //if (strm.Length == 0) return null;

        //        content = new MultipartFormDataContent()
        //        {
        //            {new StringContent("api_key"), _apiKey },
        //            {new StringContent("mapping"), mapping },
        //            {new StreamContent (fileStrm),"file",$"{fileName}.csv" }
        //        };
        //    }
        //    else
        //    {
        //        //Get file just need FormUrlEncode:
        //        Dictionary<string, string> parameters = new Dictionary<string, string>{
        //                                                                                {"api_key",_apiKey},
        //                                                                                {"mapping", mapping },
        //                                                                                {"file", fileName }
        //                                                                               };

        //        content = new FormUrlEncodedContent(parameters);
        //    }

        //    return content;
        //}




        //public virtual HttpContent BuildContent(string apiKey, string mapping, string fileName, IDictionary<string, string>? param, Stream? fileArry)
        public virtual HttpContent BuildContent(string mapping, string fileName, IDictionary<string, string>? param = null, Stream? fileStrm = null)
        {

            if (param == null) param = new Dictionary<string, string>();

            if (!param.ContainsKey("api_key")) param["api_key"] = _apiKey;
            if (!param.ContainsKey("mapping")) param["mapping"] = mapping;

            HttpContent form = null;

            List<HttpContent> contents = new List<HttpContent>();

            if (fileStrm == null || fileStrm.Length == 0)
            {
                //form.Add(new StringContent("file"), fileName);
                param["file"] = fileName;
                form = new FormUrlEncodedContent(param);
            }
            else
            {
                var tmp = new MultipartFormDataContent();

                foreach (var kvp in param)
                {
                    var str = new StringContent(kvp.Key);

                    tmp.Add(new StringContent(kvp.Value), kvp.Key);
                }

                tmp.Add(new StreamContent(fileStrm), "file", fileName);

                form = tmp;
            }


            //foreach (var kvp in param)
            //{
            //    var str = new StringContent(kvp.Key);

            //    form.Add(new StringContent(kvp.Value), kvp.Key);
            //}

            //if (fileArry == null || fileArry.Length == 0)
            //{
            //    form.Add(new StringContent("file"), fileName);
            //}
            //else
            //{
            //    form.Add(new StreamContent(fileArry), "file", fileName);
            //}

            return form;
        }

        public virtual string GetURL(bool? sendFile = null)
        {

            string stg = "";

            switch (_stage)
            {
                case STAGE.PRE_PROD:
                    stg = _STAGE_OPTIONS.PRE_PROD;

                    break;
                case STAGE.PROD:
                    stg = _STAGE_OPTIONS.PROD;
                    break;
                case STAGE.DEV:
                    stg = _STAGE_OPTIONS.DEV;
                    break;
                default:
                    throw new InvalidOperationException("EnzoCommand - GetURL(): Unkown stage (DEV | PROD | PREPROD)! ");
            }

            string action = sendFile.HasValue && sendFile.Value ? _ACTION_OPTIONS.IMPORT : _ACTION_OPTIONS.EXPORT;



            return $"{_URL_TEMPLATE.Replace(_PLACE_HOLDERS.STAGE, stg).Replace(_PLACE_HOLDERS.ACTION, action)}";
        }
        public void FilePathExists(string filePath)
        {

            if (!Directory.Exists(filePath))
            {
                var t = Directory.CreateDirectory(filePath);
            }

            return;
        }

        public List<string> GetFileNames(string mapping)
        {
            List<string> files = new List<string>();

            string absolutePath = $"{_filePath.TrimEnd('\\')}\\uploads\\";

            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }
            else
            {
                Directory.GetFiles(absolutePath, $"*{mapping}_*.csv").ToList().ForEach(f => files.Add(f));

            }


            return files;
        }

        public abstract string Help();


        #region Events

        /// <summary>
        /// Event is raised whenever a send or get file fails.
        /// </summary>
        public event EventHandler<EnzoCommandEventArgs> HandshakeFailed;

        protected virtual void OnHandshakeFailed(object sender, EnzoCommandEventArgs rsp)
        {
            HandshakeFailed?.Invoke(this, rsp);
        }

        public async Task<ErrorResponse> ErrorFactory(HttpResponseMessage msg)
        {
            StreamReader rdr = new StreamReader(await msg.Content.ReadAsStreamAsync());

            string errRsp = await rdr.ReadToEndAsync();


            return JsonConvert.DeserializeObject<ErrorResponse>(errRsp) ?? new ErrorResponse
            {
                code = "Unkown",
                message = "This error was generated because the server had no further details on this error!",
            };
        }

        #endregion
    }

}

//internal class EnzoCommandExportFactory
//{


//    STAGE _stage { get; }
//    public string _apiKey { get; }
//    public string _mapping { get; }
//    public string _fileName { get; }
//    public bool? _sendFile { get; }

//    [ImportingConstructor]
//    public EnzoCommandExportFactory(string apiKey, string mapping, string fileName, STAGE stage = STAGE.PRE_PROD, bool? sendFile = null)
//    {
//        _apiKey = apiKey;
//        _fileName = fileName;
//        _mapping = mapping;
//        _stage = stage;
//        _sendFile = sendFile;
//    }

//}
//}
