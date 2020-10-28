using MongoDB.Bson;
using OpenKey.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenKey.COMMON.Interfaces
{
    public interface IRepositorio<T> where T:BaseDTO
    {
        bool Create(T Entidad);
        List<T> Read { get; }
        bool Update(T EntidadModificada);
        //Cuando se conecte a mongo cambiar el tipo de identidicador para ObjectId
        bool Delete(ObjectId id);
    }
}
