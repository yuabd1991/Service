using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using Logic.Models;
using System.Text.RegularExpressions;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Logic.DataAccess
{
	public class SiteContext : DbContext
	{
		public SiteContext()
            : base()
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public DbConnection Connection
        {
            get
            {
                var objectContext = ((IObjectContextAdapter)this).ObjectContext;
                DbConnection con = objectContext.Connection;
                return con;
            }
        }

		protected override void Dispose(bool disposing)
		{
			if (Connection.State == ConnectionState.Open)
			{
				Connection.Close();
			}

			base.Dispose(disposing);
		}

		public override int SaveChanges()
        {
            var validErrs = this.GetValidationErrors().ToList();
            if (validErrs.Count() > 0)
            {
                string msg = validErrs[0].ValidationErrors.Select(m => m.ErrorMessage).FirstOrDefault();
                throw (new Exception(msg));
            }
            var ChangeTrackerData = base.ChangeTracker.Entries().ToList();

            int result = base.SaveChanges();

            return result;
        }

		#region DbSet

		public DbSet<Menu> Menus { get; set; }

		public DbSet<Page> Pages { get; set; }

		public DbSet<Article> Articles { get; set; }

		public DbSet<Comment> Comments { get; set; }

		public DbSet<ArticleTag> ArticleTags { get; set; }

		public DbSet<Column> Columns { get; set; }

		public DbSet<ContentTemplate> ContentTemplates { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<UserRole> UserRoles { get; set; }

		public DbSet<UserRoleJoin> UserRoleJoins { get; set; }

		public DbSet<UserRolePermission> UserRolePermissions { get; set; }

		public DbSet<Links> Links { get; set; }

		public DbSet<LinkCategory> LinkCategories { get; set; }

		public DbSet<ArticleImage> ArticleImages { get; set; }

        public DbSet<Document> Documents { get; set; }

		public DbSet<Sys_Config> Sys_Configs { get; set; }

		public DbSet<RecycleBin> RecycleBins { get; set; }

		public DbSet<Picture> Pictures { get; set; }

		public DbSet<Industry> Industries { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseType> CourseTypes { get; set; }
        /// <summary>
        /// 字典
        /// </summary>
        public DbSet<Dictionary> Dictionaries { get; set; }

		#endregion
	}
}