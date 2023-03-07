using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_System.Entity
{
    public abstract class BaseEntity
    {
        private int? Id;
        public int GetId() => (int)Id;
        public void SetId(int? id) => Id = id;
    }
}
