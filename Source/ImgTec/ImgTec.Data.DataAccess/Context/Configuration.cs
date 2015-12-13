using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using ImgTec.Data.DataAccess.UnitOfWork;
using ImgTec.Data.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ImgTec.Data.DataAccess.Context
{
    internal sealed class Configuration : DbMigrationsConfiguration<ImgTecDbContext>
    {

        private IDataFacade _dataFacade;
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Contexts\Migrations";
        }

        protected override void Seed(ImgTecDbContext context)
        {
            _dataFacade = new DataFacade(context);
            SeedRoles();
            SeedUsers();
        }

        private void SeedUsers()
        {
            List<User> users = new List<User>
            {
                new User{FirstName = "Adrian", LastName = "Ilewicz", Id = "adrian.ilewicz", Roles = { }},
                new User{FirstName = "James", LastName = "Foley", Id = "james.foley", Roles = { }},
                new User{FirstName = "Adrian", LastName = "Ilewicz", Id = "adrian.ilewicz", Roles = { }},
            };
        }

        private void SeedRoles()
        {
            List<Role> roles = new List<Role>
            {
                new Role{Name = "Admins", Id = Guid.Parse("4e8abb37-cfda-4484-b3b1-573ef99bb2fe")},
                new Role{Name = "Agents", Id = Guid.Parse("93d439bb-e21e-4481-aeca-45be0f7218f0")},
                new Role{Name = "Customers", Id = Guid.Parse("a301a420-1de9-4526-9752-840d2b3a157d")}
            };
        }
    }
}
