namespace Microsoft.Deployment.Common.Model
{
    class SSASConnectionElement
    {
        public string Name;
        public string ConnectionString;
        public string ConnectionType;

        public SSASConnectionElement()
        {
            Name = "EntityDataSource";
            ConnectionType = "analysisServicesDatabaseLive";
            ConnectionString = null;
        }

        public SSASConnectionElement(string server, string catalog, string cube) : this()
        {
            InitializeConnectionElement(server, catalog, cube);
        }

        public void InitializeConnectionElement(string server, string catalog, string cube)
        {
            ConnectionString = $"Data Source={server};Initial Catalog=\"{catalog}\";Cube=\"{cube}\"";
        }
    }

    class PbixConnection
    {
        public int Version = 1;
        public SSASConnectionElement[] Connections = new SSASConnectionElement[0];
    }
}