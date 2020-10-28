using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenKey.COMMON.Entidades
{
    public abstract class BaseDTO
    {
        public ObjectId id { get; set; }
    }
}
