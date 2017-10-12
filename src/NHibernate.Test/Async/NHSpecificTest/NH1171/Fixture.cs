﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Cfg;
using NHibernate.Dialect;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1171
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			// Firebird has issues with comments containing apostrophes
			return !(dialect is FirebirdDialect);
		}

		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			configuration.SetProperty(Environment.FormatSql, "false");
		}

		[Test]
		public async Task SupportSQLQueryWithCommentsAsync()
		{
			string sql =
				@"
SELECT id 
FROM tablea 
-- Comment with ' number 1 
WHERE Name = :name 
/* Comment with ' number 2 */ 
ORDER BY Name 
";
			using (ISession s = OpenSession())
			{
				var q = s.CreateSQLQuery(sql);
				q.SetString("name", "Evgeny Potashnik");
				await (q.ListAsync());
			}
		}

		[Test]
		public async Task ExecutedContainsCommentsAsync()
		{
			string sql =
				@"
SELECT id 
FROM tablea 
-- Comment with ' number 1 
WHERE Name = :name 
/* Comment with ' number 2 */ 
ORDER BY Name 
";
			using (var ls = new SqlLogSpy())
			{
				using (ISession s = OpenSession())
				{
					var q = s.CreateSQLQuery(sql);
					q.SetString("name", "Evgeny Potashnik");
					await (q.ListAsync());
				}
				string message = ls.GetWholeLog();
				Assert.That(message, Does.Contain("-- Comment with ' number 1"));
				Assert.That(message, Does.Contain("/* Comment with ' number 2 */"));
			}
		}
	}
}