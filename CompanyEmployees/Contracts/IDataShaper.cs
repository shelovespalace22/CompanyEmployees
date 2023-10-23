using Entities.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDataShaper<T>
    {
        IEnumerable</*ExpandoObject*/ /*Entity*/  ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString);

        /*ExpandoObject*/ /*Entity*/ ShapedEntity ShapeData(T entity, string fieldsString);
    }
}
