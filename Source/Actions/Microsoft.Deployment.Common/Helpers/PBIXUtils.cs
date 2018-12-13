﻿using System;
using System.IO;
using System.IO.Packaging;

using Newtonsoft.Json;

using Microsoft.Deployment.Common.Model;
using System.Text.RegularExpressions;

namespace Microsoft.Deployment.Common.Helpers
{
    public class PBIXUtils : IDisposable
    {
        private string _originalFile;
        private string _modifiedFile;

        private ZipPackage _pbixPackage;

        private ZipPackagePart _mashup;
        private ZipPackagePart _model;
        private ZipPackagePart _connections;

        public PBIXUtils() { }

        public PBIXUtils(string pbixSourceFileName, string pbixTargetFileName)
        {
            _originalFile = pbixSourceFileName;
            _modifiedFile = pbixTargetFileName;

            File.Copy(_originalFile, _modifiedFile, true);

            _pbixPackage = (ZipPackage)ZipPackage.Open(_modifiedFile, FileMode.Open);
            var enumerator = _pbixPackage.GetParts().GetEnumerator();

            if (enumerator == null) return;

            while (enumerator.MoveNext() && (_mashup == null || _model == null || _connections==null))
            {
                var currentPart = enumerator.Current as ZipPackagePart;
                if (currentPart == null) continue;

                if (currentPart.Uri.OriginalString.Contains("/DataMashup"))
                    _mashup = currentPart;
                else if (currentPart.Uri.OriginalString.Contains("/DataModel"))
                    _model = currentPart;
                else if (currentPart.Uri.OriginalString.Contains("/Connections"))
                    _connections = currentPart;
            }

        }

        private string GetPerspectiveFromConnectionString(string ssasConnectionString)
        {
            string result = string.Empty;
            // Could start with a ;
            //     and have few spaces,
            //     then word Cube and maybe some spaces then =
            //     then the perspective name possibily surrounded by "
            //     last a ; or end of string
            Regex r = new Regex(";?\\s*Cube\\s*=\\\"?([\\w,\\s]*)\\\"?;?", RegexOptions.IgnoreCase);
            Match m = r.Match(ssasConnectionString);
            if (m != null && m != Match.Empty && m.Groups.Count >= 2)
            {
                result = m.Groups[1].Value.Trim();
            }
            return result;
        }

        public void ReplaceSSASConnectionString(string server, string catalog, string cube)
        {
            if (_connections == null) return; // Nothing to do

            string connectionJson = null;
            using (MemoryStream m = new MemoryStream())
            {
                _connections.GetStream().CopyTo(m);  // Do not cache GetStream() result
                connectionJson = m.ToArray().GetStringFromUTF8();
            }

            PbixConnection liveConnection = JsonConvert.DeserializeObject<PbixConnection>(connectionJson);
            if (liveConnection.Version != 1)
                throw new Exception("SSAS connection element has an unexpected version");

            if (liveConnection.Connections.Length != 1)
                throw new Exception("Ambiguous connection element change request");

            if (liveConnection.Connections[0] == null)
                throw new Exception("Unexpected null value for connection element");

            liveConnection.Connections[0].InitializeConnectionElement(server, catalog, GetPerspectiveFromConnectionString(liveConnection.Connections[0].ConnectionString));
            connectionJson = JsonConvert.SerializeObject(liveConnection);

            byte[] encodedJsonBytes = connectionJson.GetUTF8Bytes();
            _connections.GetStream().Write(encodedJsonBytes, 0, encodedJsonBytes.Length);
            _connections.GetStream().SetLength(encodedJsonBytes.Length);
            _connections.GetStream().Flush();
            _pbixPackage.Flush();
        }

        public void ReplaceKnownVariableinMashup(string variable, string newValue)
        {
            byte[] mashupBytes;
            using (var memoryStream = new MemoryStream())
            {
                _mashup.GetStream().CopyTo(memoryStream);
                mashupBytes = memoryStream.ToArray();
            }

            QDEFF qdeff = new QDEFF(mashupBytes);
            qdeff.ReplaceKnownVariable(variable, newValue);

            byte[] newContent = qdeff.RecreateContent();

            _mashup.GetStream().Write(newContent, 0, newContent.Length);
            _mashup.GetStream().SetLength(newContent.Length);
            _mashup.GetStream().Flush();
            _pbixPackage.Flush();
        }


        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _pbixPackage?.Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PBIXUtils() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}