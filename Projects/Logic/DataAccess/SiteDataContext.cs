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

			//var logList = DBExtend.GetLogList(ChangeTrackerData);

            int result = base.SaveChanges();
			//if (result > 0)
			//{
			//    //数据库日志
			//    DBExtend.LogDb(logList);
			//    //数据层缓存
			//    ResetCache(ChangeTrackerData);
			//}

            return result;
        }
		/// <summary>  
		/// 单表操作批量删除  
		/// </summary>  
		/// <typeparam name="T"></typeparam>  
		/// <param name="source"></param>  
		/// <param name="query"></param>  
		/// <returns></returns>  
		//public static int Delete<T>(this System.Data.Linq.Table<T> source, Expression<Func<T, bool>> query)
		//    where T : class
		//{
		//    if (source == null)
		//        throw new ArgumentException("source");
		//    if (query == null)
		//        throw new ArgumentException("query");
		//    //query = t => true;  
		//    //为空DELETE  FROM [dbo].[test] 全删除；个人觉得为空全删除，很不人道，所以还是抛异常  
		//    System.Data.Linq.DataContext db = source.Context;
		//    IQueryable q = source.Where(query).AsQueryable();
		//    DbCommand cmd = db.GetCommand(q);
		//    string sql = cmd.CommandText;
		//    string regex = @"from\s*\[\s*dbo\s*\]\s*\.\s*\[\s*\w+\s*\]\s*(?<tableparam>(as\s*(?<tablepname>(\[\s*\w+\s*\]))))(\.|(\r)|(\n))*";
		//    MatchCollection matches = Regex.Matches(sql, regex, RegexOptions.Multiline | RegexOptions.IgnoreCase);
		//    Debug.Assert(matches != null, "regex match :null");
		//    Debug.Assert(matches.Count == 1, "regex match length :" + matches.Count);
		//    if (matches != null && matches.Count > 0)
		//    {
		//        Match match = matches[0];
		//        sql = ("DELETE  " + match.Value.Replace(match.Groups["tableparam"].Value, "") +
		//        sql.Substring(match.Index + match.Length)).Replace(match.Groups["tablepname"].Value + ".", " ");
		//        List<object> dbparams = new List<object>();
		//        foreach (SqlParameter item in cmd.Parameters)
		//        {
		//            SqlParameter p = new SqlParameter(item.ParameterName, item.SqlDbType, item.Size);
		//            p.Value = item.Value;
		//            dbparams.Add(item.Value);
		//        }
		//        q = null;
		//        cmd = null;
		//        matches = null;
		//        Debug.WriteLine("delete sql :" + sql);
		//        return db.ExecuteCommand(sql, dbparams.ToArray());
		//    }
		//    return 0;
		//}

		//private void ResetCache(object par)
		//{
		//    List<System.Data.Entity.Infrastructure.DbEntityEntry> entryList = (List<System.Data.Entity.Infrastructure.DbEntityEntry>)par;
		//    foreach (var item in entryList)
		//    {
		//        CacheHelp.RemoveCache(item.Entity.GetType().BaseType.Name);
		//    }
		//}

		//public class SiteDataContextInitializer : DropCreateDatabaseAlways<SiteDataContext>
		//{
		//    protected override void Seed(SiteDataContext context)
		//    {
		//        context.SaveChanges();
		//    }
		//}

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

		public DbSet<Company> Companies { get; set; }

		#endregion
	}
}