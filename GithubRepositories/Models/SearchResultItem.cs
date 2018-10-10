using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubRepositories.Models
{
	public class SearchResultItem
	{
		public int id { get; set; }
		public string node_id { get; set; }
		public string name { get; set; }
		public RepositoryOwner owner { get; set; }
	}
}