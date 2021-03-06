﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nest;
using Nest.TestData;
using Nest.TestData.Domain;
using NUnit.Framework;


namespace Nest.Tests.Search
{
	[TestFixture]
	public class OpenCloseTests : BaseElasticSearchTests
	{
		[Test]
		public void CloseAndOpenIndex()
		{
			var r = this.ConnectedClient.CloseIndex(Test.Default.DefaultIndex);
			Assert.True(r.OK);
			Assert.True(r.Acknowledged);
			Assert.True(r.IsValid);
			r = this.ConnectedClient.OpenIndex(Test.Default.DefaultIndex);
			Assert.True(r.OK);
			Assert.True(r.Acknowledged);
			Assert.True(r.IsValid);
		}
		[Test]
		public void CloseAndOpenIndexTyped()
		{
			var r = this.ConnectedClient.CloseIndex<ElasticSearchProject>();
			Assert.True(r.OK);
			Assert.True(r.Acknowledged);
			Assert.True(r.IsValid);
			r = this.ConnectedClient.OpenIndex<ElasticSearchProject>();
			Assert.True(r.OK);
			Assert.True(r.Acknowledged);
			Assert.True(r.IsValid);
		}
		[Test]
		public void CloseAndSearchAndOpenIndex()
		{
			var r = this.ConnectedClient.CloseIndex(Test.Default.DefaultIndex);
			Assert.True(r.OK);
			Assert.True(r.Acknowledged);
			Assert.True(r.IsValid);
			var results = this.ConnectedClient.Search<ElasticSearchProject>(
				@" { ""query"" : {
						    ""match_all"" : { }
				} }"
			);
			Assert.False(results.IsValid);
			Assert.IsNotNull(results.ConnectionStatus.Error);
			Assert.True(results.ConnectionStatus.Error.HttpStatusCode == System.Net.HttpStatusCode.Forbidden, results.ConnectionStatus.Error.HttpStatusCode.ToString());
			Assert.True(results.ConnectionStatus.Error.ExceptionMessage.Contains("ClusterBlockException"));
			Assert.True(results.ConnectionStatus.Error.ExceptionMessage.Contains("index closed"));
			r = this.ConnectedClient.OpenIndex(Test.Default.DefaultIndex);
			Assert.True(r.OK);
			Assert.True(r.Acknowledged);
			Assert.True(r.IsValid);
		}
	}
}