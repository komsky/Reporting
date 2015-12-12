using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ImgTec.Data.Entities
{
    public class Role : IdentityRole, IRole<Guid>
    {
        public Role()
        {
            Id = Guid.NewGuid();
        }

        public Role(string name)
            : this()
        {
            Name = name;
        }

        public Role(string name, Guid id)
        {
            Name = name;
            Id = id;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
