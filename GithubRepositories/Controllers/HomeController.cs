using GithubRepositories.Models;
using GithubRepositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace GithubRepositories.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Bookmarks()
		{
			List<SearchResultItem> books = (List<SearchResultItem>)Session["Bookmarks"];
			return View(books);
		}

		public JsonResult AddBookmark(string name, string image, int id)
		{
			List<SearchResultItem> books = (List<SearchResultItem>)Session["Bookmarks"];
			if (books == null)
				books = new List<SearchResultItem>();
			if (books.Find(c => c.id == id) == null)
			{
				books.Add(new SearchResultItem { id = id, name = name, owner = new RepositoryOwner { avatar_url = image } });
				Session["Bookmarks"] = books;
			}
			return Json("");
		}

		public JsonResult RemoveBookmark(int id)
		{
			List<SearchResultItem> books = (List<SearchResultItem>)Session["Bookmarks"];
			if (books != null)
			{
				SearchResultItem item = books.Find(c => c.id == id);
				books.Remove(item);
				Session["Bookmarks"] = books;
			}
			return Json("");
		}

		private bool ItemInSession(int id)
		{
			bool check = true;
			List<SearchResultItem> books = (List<SearchResultItem>)Session["Bookmarks"];
			if (books != null)
			{
				SearchResultItem item = books.Find(c => c.id == id);
				if (item == null)
					check = false;
			}
			else
				check = false;
			return check;
		}

		public async Task<ActionResult> Index(int? page, string term, string type)
		{
			if (string.IsNullOrEmpty(type) || type == "search")
				page = 1;
			int pageIndex = (page ?? 1);
			int pageSize = 30;
			IEnumerable<string> values;
			SearchResult res = new SearchResult { items = new List<SearchResultItem>() };
			int total = 0;
			int pageNumber = 0;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("User-Agent", "C# App");
				string url = "https://api.github.com/search/repositories?q=" + term;
				if (page != null)
					url += "&page=" + page.Value;
				var uri = new Uri(url);
				var response = await client.GetAsync(uri);
				res = await response.Content.ReadAsAsync<SearchResult>();
				if (response.Headers.TryGetValues("Link", out values))
				{
					string t = @"rel=""last""";
					string p = @"rel=""prev""";
					string link = values.First();
					string[] words = link.Split(',');
					string ks = Array.Find(words, c => c.Contains(t));
					string ps;
					if (ks == null)
					{
						ps = Array.Find(words, c => c.Contains(p));
						Match match = Regex.Match(ps, @"page=(\d+)");
						pageNumber = int.Parse(match.Groups[1].Value) + 1;
					}
					else
					{
						Match match = Regex.Match(ks, @"page=(\d+)");
						pageNumber = int.Parse(match.Groups[1].Value);
					}
					total = pageNumber;
				}
			}
			ViewBag.term = term;
			List<GalleryItemVm> galleryItems = new List<GalleryItemVm>();
			if (res.items != null)
			{
				foreach (SearchResultItem item in res.items)
				{
					galleryItems.Add(new GalleryItemVm
					{
						Id = item.id,
						Name = item.name,
						ImageUrl = item.owner.avatar_url,
						InSession = ItemInSession(item.id)
					});
				}
			}
			var pagelist = new StaticPagedList<GalleryItemVm>(galleryItems, pageIndex, pageSize, pageSize * total);
			return View(pagelist);
		}
	}
}