using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary2
{
    public class User
    {
        private int Id;
        private string Name;

        public User(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public string Description()
            => $"Id:{this.Id}, Name:{this.Name}";

    }
}
