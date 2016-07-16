using System;
using System.Collections.Generic;

namespace OpenMagic.EventStore.AzureBlobStorage
{
    public class BlobMetadata
    {
        internal const string VersionNumberKey = "VersionNumber";
        private readonly IDictionary<string, string> _metadata;

        public BlobMetadata(IDictionary<string, string> metadata)
        {
            _metadata = metadata;
        }

        public int VersionNumber
        {
            get { return GetInteger(VersionNumberKey, 0); }
            set { SetInteger(VersionNumberKey, value); }
        }

        private int GetInteger(string key, int value)
        {
            return Convert.ToInt32(GetString(key, value.ToString()));
        }

        private string GetString(string key, string defaultValue)
        {
            string value;
            return _metadata.TryGetValue(key, out value) ? value : defaultValue;
        }

        private void SetInteger(string key, int value)
        {
            SetString(key, value.ToString());
        }

        private void SetString(string key, string value)
        {
            _metadata[key] = value;
        }
    }
}