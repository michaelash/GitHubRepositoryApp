using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GithubRepositories.Models
{
	public class RepositoryOwner
	{
		[DataMember]
		public string avatar_url { get; set; }
	}
}