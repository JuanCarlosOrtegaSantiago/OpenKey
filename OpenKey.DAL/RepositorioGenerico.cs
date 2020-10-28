using MongoDB.Bson;
using MongoDB.Driver;
using OpenKey.COMMON.Entidades;
using OpenKey.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenKey.DAL
{
    public class RepositorioGenerico<T> : IRepositorio<T> where T : BaseDTO
    {

        private MongoClient client;
        private IMongoDatabase db;


        public RepositorioGenerico()
        {
            client = new MongoClient("mongodb://UserOpenKey:OpenKey1234@bdopenkey-shard-00-00.xg6rg.mongodb.net:27017,bdopenkey-shard-00-01.xg6rg.mongodb.net:27017,bdopenkey-shard-00-02.xg6rg.mongodb.net:27017/BdOpenKey?ssl=true&replicaSet=atlas-cd6vj8-shard-0&authSource=admin&retryWrites=true&w=majority");
            db = client.GetDatabase("BdOpenKey");
        }

        private IMongoCollection<T> Collection()
        {
            try
            {
                return db.GetCollection<T>(typeof(T).Name);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<T> Read => Collection().AsQueryable().ToList();

        public bool Create(T Entidad)
        {
            string resul = "";

            Entidad.id = new ObjectId();
            try
            {
                Collection().InsertOne(Entidad);
                resul = "";
                return true;
            }
            catch (Exception ex)
            {
                resul = ex.Message;
                return false;
            }
        }

        public bool Delete(ObjectId id)
        {
            try
            {
                return Collection().DeleteOne(e => e.id == id).DeletedCount == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(T EntidadModificada)
        {
            try
            {
                return Collection().ReplaceOne(e => e.id == EntidadModificada.id, EntidadModificada).ModifiedCount == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
