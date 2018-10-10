using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubRepositories.ViewModels
{
	public class GalleryItemVm
	{
		public int Id { get; set; }
		public string ImageUrl { get; set; }
		public bool InSession { get; set; }
		public string Name { get; set; }
	}
}