using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web;

namespace Component.Component
{
	public class ImageHandler : IHttpHandler
	{
		/// <summary>
		/// 图片处理
		/// </summary>
		#region IHttpHandler Members

		public bool IsReusable
		{
			get { return true; }
		}
		public enum ScaleType
		{
			/// <summary>
			/// WhiteSpace : keeps wxh ratio and provides a white space
			/// </summary>
			WhiteSpace = 0,
			/// <summary>
			/// Cropped:keeps wxh ratio and crops overflow
			/// </summary>
			Cropped = 1,
			/// <summary>
			/// Stretch: stretches the image
			/// </summary>
			Stretch = 2
		}

		string CopyRightFile = "";
		public void ProcessRequest(HttpContext context)
		{
			//在此写入您的处理程序实现。
			string url = context.Request.Url.ToString().ToLower(); //获取当前请求的URL

			Regex reg = new Regex("/images/([^-][0-9/]*)-([^-][0-9]*)-([^-][0-9]*)-([^-][0-2]*).jpg");

			Regex regReal = new Regex("/images/([^-.]*).jpg");

			string showUrl = "";

			if (reg.IsMatch(url))
			{
				string realUrl = "";//处理的图片路径
				string cacheImgUrl = "";
				int width = 0;
				int height = 0;
				ScaleType scaleType = ScaleType.WhiteSpace;

				GroupCollection gc = reg.Match(url).Groups;
				realUrl = gc[1].Value + ".jpg";
				cacheImgUrl = gc[1].Value + "-" + gc[2].Value + "-" + gc[3].Value + "-" + gc[4].Value + ".jpg";
				width = Convert.ToInt32(gc[2].Value);
				height = Convert.ToInt32(gc[3].Value);
				scaleType = (ScaleType)Enum.Parse(typeof(ScaleType), gc[4].Value);
				realUrl = Config.ImageRelDirectory + realUrl.Replace("/", @"\"); //最后输出路径；
				string cacheUrl = Config.ImageCacheDirectory + cacheImgUrl.Replace("/", @"\");
				if (!File.Exists(realUrl))
				{
					realUrl = Config.ImageSysRelDirectory + "NoImage.jpg";
				}
				if (!File.Exists(cacheUrl))
				{
					//生成缓存图片
					CreateCacheImage(width, height, realUrl, scaleType, false, cacheUrl);
				}
				showUrl = cacheUrl;
				//如果生产缓存失败，则显示真实图片地址
				if (!File.Exists(showUrl))
				{
					showUrl = realUrl;
				}

				//context.Response.AddHeader("content-type", "image/jpeg;charset=UTF-8");
				//context.Response.ContentType = "image/jpeg";
				//context.Response.WriteFile(cacheUrl);
			}
			else if (regReal.IsMatch(url))
			{
				//这个必须改进.... 让原图也有生成缓存，原图不给直接浏览
				GroupCollection gc = regReal.Match(url).Groups;
				string realUrl = Config.ImageRelDirectory + gc[1].Value + ".jpg";

				if (!File.Exists(realUrl))
				{
					realUrl = Config.ImageSysRelDirectory + "NoImage.jpg";
				}

				showUrl = realUrl;

				//context.Response.AddHeader("content-type", "image/jpeg;charset=UTF-8");
				//context.Response.ContentType = "image/jpeg";
				//context.Response.WriteFile(realUrl);
			}
			else
			{
				string realUrl = Config.ImageSysRelDirectory + "NoImage.jpg";
				if (!File.Exists(realUrl))
				{
					return;
				}
				showUrl = realUrl;
				//context.Response.AddHeader("content-type", "image/jpeg;charset=UTF-8");
				//context.Response.ContentType = "image/jpeg";
				//context.Response.WriteFile(realUrl);
			}

			context.Response.AddHeader("content-type", "image/jpeg;charset=UTF-8");
			context.Response.ContentType = "image/jpeg";
			context.Response.WriteFile(showUrl);
		}
		#endregion



		private void CreateCacheImage(int width, int height, string pic, ScaleType scaleType, bool IsWatermark, string cacheUrl)
		{
			Bitmap img = new Bitmap(width, height);
			System.Drawing.Image image = null;
			try
			{
				image = System.Drawing.Image.FromFile(pic);
			}
			catch (System.IO.FileNotFoundException ee)
			{
				throw ee;
			}
			float Ax, Ay, Bx, By;
			Ax = width;
			Ay = height;
			Bx = image.Width;
			By = image.Height;
			float baseLength = 0;
			int Dx = 0, Dy = 0, Ex = 0, Ey = 0;
			switch (scaleType)
			{
				case ScaleType.WhiteSpace:
					if (Ax / Bx > Ay / By)
					{
						baseLength = Bx * (Ay / By);
						Dx = (int)(Ax - baseLength) / 2;
						Ax = (int)baseLength;
					}
					else
					{
						baseLength = By * (Ax / Bx);
						Dy = (int)(Ay - baseLength) / 2;
						Ay = (int)baseLength;
					}
					Ex = Ey = 0;
					break;
				case ScaleType.Cropped:
					if (Ax / Bx > Ay / By)
					{
						// x main
						baseLength = Ay / (Ax / Bx);
						Ey = (int)(By - baseLength) / 2;
						By = (int)baseLength;
					}
					else
					{
						// y main
						baseLength = Ax / (Ay / By);
						Ex = (int)(Bx - baseLength) / 2;
						Bx = (int)baseLength;
					}
					break;
				case ScaleType.Stretch:
					break;
				default:
					return;
			}
			Graphics g = Graphics.FromImage(img);
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			g.Clear(System.Drawing.Color.White);
			g.DrawImage(image,
			new System.Drawing.Rectangle(Dx, Dy, (int)Ax, (int)Ay),
			new System.Drawing.Rectangle(Ex, Ey, (int)Bx, (int)By),
			System.Drawing.GraphicsUnit.Pixel);

			if (IsWatermark)
			{
				string strCopyRightFile = CopyRightFile;
				System.Drawing.Image CopyMyImage = System.Drawing.Image.FromFile(strCopyRightFile);
				int IntX = 0;
				int IntY = 0;
				IntX = width - CopyMyImage.Width;
				IntY = height - CopyMyImage.Height;
				g.DrawImage(CopyMyImage, IntX, IntY, CopyMyImage.Width, CopyMyImage.Height);
			}
			try
			{
				string directory = Path.GetDirectoryName(cacheUrl);
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
				img.Save(cacheUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				img.Dispose();
				image.Dispose();
				g.Dispose();
			}
		}


	}

	/// <summary>
	/// 对图片路径，提供操作。也有对应的js方法
	/// </summary>
	public static class ImageHelp
	{
		public static string GetImageUrl(string url)
		{
			return Config.ImageWebDir + url;
			//return "" + url;
		}
		public static string GetImageUrl(string url, int width, int height)
		{
			var urlArray = url.Split('.');
			if (urlArray.Length <= 1)
			{
				return "";
			}
			return Config.ImageWebDir + urlArray[0] + ".jpg";
			//return "" + urlArray[0] + ".jpg";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="imgUrl"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="scaleType"></param>
		/// <returns></returns>
		public static string FormatImageUrl(string imgUrl, string width = "80", string height = "80", string style = "")
		{
			if (!string.IsNullOrEmpty(imgUrl))
			{
				if (imgUrl.IndexOf("http://") >= 0)
				{
					return imgUrl;
				}
				var array = imgUrl.Split('.');
                var result =
                    "<img src='" + System.Configuration.ConfigurationSettings.AppSettings["ImageRelDirectory"].ToString() 
                    + array[0] + ".jpg' width='" + width + "' height='" + height + "' style='"+ style +"'/>";
				return result;
			}
			else
			{
                return System.Configuration.ConfigurationSettings.AppSettings["ImageRelDirectory"].ToString() + "NoImage.jpg";
			}
		}

	}
}
