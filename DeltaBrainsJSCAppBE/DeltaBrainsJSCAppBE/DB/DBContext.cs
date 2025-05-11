using Microsoft.EntityFrameworkCore;
using System;

namespace DeltaBrainJSC.DB
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


        }



    }
}
