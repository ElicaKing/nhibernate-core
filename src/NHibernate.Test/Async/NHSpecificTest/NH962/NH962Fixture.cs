﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH962
{
	using System.Threading.Tasks;
	[TestFixture]
	public class NH962FixtureAsync : BugTestCase
	{
		[Test]
		public async Task BugAsync()
		{
			Parent parent = new Parent();
			parent.Name = "Test Parent";

			Child child = new Child();
			child.Name = "Test Child";

			child.Parent = parent;
			parent.Children = new HashSet<Child>();
			parent.Children.Add(child);

			using (ISession session = OpenSession())
			using (ITransaction tx = session.BeginTransaction())
			{
				await (session.SaveAsync(child));
				Assert.IsTrue(session.Contains(parent));
				Assert.AreNotEqual(Guid.Empty, parent.Id);
				await (tx.CommitAsync());
			}

			using (ISession session = OpenSession())
			using (ITransaction tx = session.BeginTransaction())
			{
				await (session.DeleteAsync(child));
				await (tx.CommitAsync());
			}
		}
	}
}