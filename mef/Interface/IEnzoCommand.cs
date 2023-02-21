using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnzoCommanderSDK.mef.Interface
{

    //public interface IEnzoCommand
    //{
    //    bool SendFile(string apiKey);
    //    bool GetFile(string apiKey);

    //}
    public interface IEnzoCommand
    {
        string _SEND_URL();
        string _GET_URL();
        Task<bool> SendFile(string apiKey, string filePath);
        Task<bool> GetFile(string apiKey, string filePath);

        /// <summary>
        /// Creates Form content object that contains needed fields when sending a download request.
        /// </summary>
        /// <param name="apiKey">Site's API key.</param>
        /// <param name="mapping">Site's integration mapping.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        FormUrlEncodedContent FormFactory(string apiKey, string mapping, string fileName);

        /// <summary>
        /// Creates a Multipart form used when a file is going to be sent along with form fields.
        /// </summary>
        /// <param name="apiKey">Site's API key.</param>
        /// <param name="mapping">Site's integration mapping.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="file">The file stream.</param>
        /// <returns></returns>
        MultipartFormDataContent FormFactory(string apiKey, string mapping, string fileName, Stream file);
        void FilePathExists(string filePath);
    }

    public interface IEnzoCommandData
    {
        string Mapping { get; }
    }
}
