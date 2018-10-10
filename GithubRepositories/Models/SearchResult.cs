using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace GithubRepositories.Models
{
	[Serializable]
	[DataContract]
	public class SearchResult
	{
		[DataMember]
		public string total_count { get; set; }
		[DataMember]
		public List<SearchResultItem> items { get; set; }
	}
}