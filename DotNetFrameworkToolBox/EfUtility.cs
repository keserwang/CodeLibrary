using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace DotNetFrameworkToolBox
{
    public class EfUtility
    {
        public static EntityConnection CreateEntityConnectionForOracle(string connectionStringName, string modelName = null, string schemaName = null)
        {
            string connString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            EntityConnectionStringBuilder ecsBuilder = new EntityConnectionStringBuilder(connString);

            if (modelName == null)
            {
                try
                {
                    // For example:
                    // Metadata = "res://*/Boaud.BoaudModel.csdl|res://*/Boaud.BoaudModel.ssdl|res://*/Boaud.BoaudModel.msl";
                    // modelName = "Boaud.BoaudModel";
                    string[] metaArray = ecsBuilder.Metadata.Split('|');
                    modelName = Path.GetFileNameWithoutExtension(metaArray[0]);
                }
                catch
                {
                    return null;
                }
            }

            if (schemaName == null)
            {
                try
                {
                    OracleConnectionStringBuilder ocsBuilder = new OracleConnectionStringBuilder(ecsBuilder.ProviderConnectionString);
                    schemaName = ocsBuilder.UserID;
                }
                catch
                {
                    return null;
                }
            }

            #region Conceptual
            XmlReader conceptualXmlReader = XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream(modelName + ".csdl"));
            XmlReader[] conceptualXmlReaderArray = new XmlReader[] { conceptualXmlReader };
            EdmItemCollection conceptualCollection = new EdmItemCollection(conceptualXmlReaderArray);
            #endregion

            #region Store
            XmlReader storeXmlReader = XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream(modelName + ".ssdl"));
            XElement storeXElement = XElement.Load(storeXmlReader);
            XNamespace storeXNamespace = "http://schemas.microsoft.com/ado/2009/11/edm/ssdl";
            foreach (XElement entitySet in storeXElement.Descendants(storeXNamespace + "EntitySet"))
            {
                XAttribute schemaAttribute = entitySet.Attributes("Schema").FirstOrDefault();
                if (schemaAttribute != null)
                {
                    schemaAttribute.SetValue(schemaName);
                }
            }

            StoreItemCollection storeCollection = new StoreItemCollection(
                new XmlReader[] { storeXElement.CreateReader() }
            );
            #endregion

            #region Mapping
            XmlReader mappingXmlReader = XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream(modelName + ".msl"));
            XmlReader[] mappingXmlReaderArray = new XmlReader[] { mappingXmlReader };
            StorageMappingItemCollection mappingCollection = new StorageMappingItemCollection(
                conceptualCollection, storeCollection, mappingXmlReaderArray
            );
            #endregion

            MetadataWorkspace workspace = new MetadataWorkspace(() => conceptualCollection, () => storeCollection, () => mappingCollection);

            DbConnection connection = DbProviderFactories.GetFactory(ecsBuilder.Provider).CreateConnection();
            connection.ConnectionString = ecsBuilder.ProviderConnectionString;

            return new EntityConnection(workspace, connection);
        }
    }
}
