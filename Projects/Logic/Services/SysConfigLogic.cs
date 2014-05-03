using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic.DataAccess;
using Component;
using Entity.Entities;
using Logic.Models;

namespace Logic.Services
{
	public class SysConfigLogic : DbAccess
	{
		public SysConfigLogic()
		{
		}

		public SysConfig GetSystemConfig()
		{
			if (!_db.Sys_Configs.Any())
			{
				var sys = new Sys_Config();
				_db.Sys_Configs.Add(sys);
				_db.SaveChanges();
			}
			var sys_config = (from l in _db.Sys_Configs
							 select new SysConfig
							 {
								 AdminEmail = l.AdminEmail,
								 WebsiteName = l.WebsiteName,
								 SmtpUserAccount = l.SmtpUserAccount,
								 SmtpPort = l.SmtpPort,
								 SmtpPassword = l.SmtpPassword,
								 SmtpHost = l.SmtpHost,
								 SmtpEmail = l.SmtpEmail,
								 PageTitle = l.PageTitle,
								 MetaKeywords = l.MetaKeywords,
								 MetaDescription = l.MetaDescription,
								 IsStatic = l.IsStatic,
								 ICP = l.ICP,
								 Copyright = l.Copyright
							 }).FirstOrDefault();

			return sys_config;
		}

		public BaseObject SetSystemConfig(SysConfig param)
		{
			BaseObject obj = new BaseObject();
			if (_db.Sys_Configs.Any())
			{
				var sys_config = _db.Sys_Configs.FirstOrDefault();

				sys_config.AdminEmail = param.AdminEmail;
				sys_config.Copyright = param.Copyright;
				sys_config.ICP = param.ICP;
				sys_config.IsStatic = param.IsStatic;
				sys_config.MetaDescription = param.MetaDescription;
				sys_config.MetaKeywords = param.MetaKeywords;
				sys_config.PageTitle = param.PageTitle;
				sys_config.SmtpEmail = param.SmtpEmail;
				sys_config.SmtpHost = param.SmtpHost;
				sys_config.SmtpPassword = param.SmtpPassword;
				sys_config.SmtpPort = param.SmtpPort;
				sys_config.SmtpUserAccount = param.SmtpUserAccount;
				sys_config.WebsiteName = param.WebsiteName;

				_db.SaveChanges();
				obj.Tag = 1;
			}
			else
			{
				var sys_config = new Sys_Config();
				sys_config.AdminEmail = param.AdminEmail;
				sys_config.Copyright = param.Copyright;
				sys_config.ICP = param.ICP;
				sys_config.IsStatic = param.IsStatic;
				sys_config.MetaDescription = param.MetaDescription;
				sys_config.MetaKeywords = param.MetaKeywords;
				sys_config.PageTitle = param.PageTitle;
				sys_config.SmtpEmail = param.SmtpEmail;
				sys_config.SmtpHost = param.SmtpHost;
				sys_config.SmtpPassword = param.SmtpPassword;
				sys_config.SmtpPort = param.SmtpPort;
				sys_config.SmtpUserAccount = param.SmtpUserAccount;
				sys_config.WebsiteName = param.WebsiteName;

				_db.Sys_Configs.Add(sys_config);

				_db.SaveChanges();
				obj.Tag = 1;
			}

			return obj;
		}
	}
}
