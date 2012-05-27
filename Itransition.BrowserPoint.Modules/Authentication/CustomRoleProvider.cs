namespace Itransition.BrowserPoint.Modules.Authentication
{
    using System.Configuration.Provider;
    using System.Linq;
    using System.Web.Security;
    using Itransition.BrowserPoint.Model;
    using Itransition.BrowserPoint.Model.Entities;

    /// <summary>
	/// Custom role manager for web forms authentication
	/// </summary>
	public class CustomRoleProvider : RoleProvider
	{
		private readonly ModelContext context = new ModelContext();
		#region Overrides of RoleProvider

		/// <summary>
		/// Gets a value indicating whether the specified user is in the specified role for the configured applicationName.
		/// </summary>
		/// <returns>
		/// true if the specified user is in the specified role for the configured applicationName; otherwise, false.
		/// </returns>
		/// <param name="username">The user name to search for.</param>
		/// <param name="roleName">The role to search in.</param>
		public override bool IsUserInRole(string username, string roleName)
		{
			username = username.ToLower();
			roleName = roleName.ToLower();
			return context.Users.Any(t => t.Name == username && t.Roles.Any(k => k.Name == roleName));
		}

		/// <summary>
		/// Gets a list of the roles that a specified user is in for the configured applicationName.
		/// </summary>
		/// <returns>
		/// A string array containing the names of all the roles that the specified user is in for the configured applicationName.
		/// </returns>
		/// <param name="username">The user to return a list of roles for.</param>
		public override string[] GetRolesForUser(string username)
		{
			username = username.ToLower();
			var u = context.Users.SingleOrDefault(t => t.Name == username);
			return u == null ? new string[0] : u.Roles.Select(t => t.Name).ToArray();
		}

		/// <summary>
		/// Adds a new role to the data source for the configured applicationName.
		/// </summary>
		/// <param name="roleName">The name of the role to create.</param>
		public override void CreateRole(string roleName)
		{
			roleName = roleName.ToLower();
			if (!context.Roles.Any(t => t.Name == roleName))
			{
				context.Roles.Add(new Role { Name = roleName });
				context.SaveChanges();
			}
		}

		/// <summary>
		/// Removes a role from the data source for the configured applicationName.
		/// </summary>
		/// <returns>
		/// true if the role was successfully deleted; otherwise, false.
		/// </returns>
		/// <param name="roleName">The name of the role to delete.</param>
		/// <param name="throwOnPopulatedRole">If true, throw an exception if <paramref name="roleName"/> has one or more members and do not delete <paramref name="roleName"/>.</param>
		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			roleName = roleName.ToLower();
			var r = context.Roles.SingleOrDefault(t => t.Name == roleName);
			if (r != null)
			{
				if (throwOnPopulatedRole && r.Users.Count != 0)
					throw new ProviderException("Unable to delete role with users assigned");
				context.Roles.Remove(r);
				context.SaveChanges();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets a value indicating whether the specified role name already exists in the role data source for the configured applicationName.
		/// </summary>
		/// <returns>
		/// true if the role name already exists in the data source for the configured applicationName; otherwise, false.
		/// </returns>
		/// <param name="roleName">The name of the role to search for in the data source.</param>
		public override bool RoleExists(string roleName)
		{
			roleName = roleName.ToLower();
			return context.Roles.Any(t => t.Name == roleName);
		}

		/// <summary>
		/// Adds the specified user names to the specified roles for the configured applicationName.
		/// </summary>
		/// <param name="usernames">A string array of user names to be added to the specified roles. </param>
		/// <param name="roleNames">A string array of the role names to add the specified user names to.</param>
		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			var roles = context.Roles.Where(t => roleNames.Contains(t.Name)).ToArray();
			var users = context.Users.Where(t => usernames.Contains(t.Name)).ToArray();
			foreach (var role in roles)
			{
				foreach (var user in users)
				{
					role.Users.Add(user);
				}
			}
			context.SaveChanges();
		}

		/// <summary>
		/// Removes the specified user names from the specified roles for the configured applicationName.
		/// </summary>
		/// <param name="usernames">A string array of user names to be removed from the specified roles. </param>
		/// <param name="roleNames">A string array of role names to remove the specified user names from.</param>
		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			var roles = context.Roles.Where(t => roleNames.Contains(t.Name)).ToArray();
			var users = context.Users.Where(user => usernames.Contains(user.Name)).ToArray();
			foreach (var role in roles)
			{
				foreach (var user in users)
				{
					role.Users.Remove(user);
				}
			}
			context.SaveChanges();
		}

		/// <summary>
		/// Gets a list of users in the specified role for the configured applicationName.
		/// </summary>
		/// <returns>
		/// A string array containing the names of all the users who are members of the specified role for the configured applicationName.
		/// </returns>
		/// <param name="roleName">The name of the role to get the list of users for.</param>
		public override string[] GetUsersInRole(string roleName)
		{
			roleName = roleName.ToLower();
			var r = context.Roles.SingleOrDefault(t => t.Name == roleName);
			return r == null ? new string[0] : r.Users.Select(t => t.Name).ToArray();
		}

		/// <summary>
		/// Gets a list of all the roles for the configured applicationName.
		/// </summary>
		/// <returns>
		/// A string array containing the names of all the roles stored in the data source for the configured applicationName.
		/// </returns>
		public override string[] GetAllRoles()
		{
			return context.Roles.Select(t => t.Name).ToArray();
		}

		/// <summary>
		/// Gets an array of user names in a role where the user name contains the specified user name to match.
		/// </summary>
		/// <returns>
		/// A string array containing the names of all the users where the user name matches <paramref name="usernameToMatch"/> and the user is a member of the specified role.
		/// </returns>
		/// <param name="roleName">The role to search in.</param>
		/// <param name="usernameToMatch">The user name to search for.</param>
		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			roleName = roleName.ToLower();
			usernameToMatch = usernameToMatch.ToLower();
			var r = context.Roles.SingleOrDefault(t => t.Name == roleName);
			if (r != null)
			{
				var uarr = r.Users.Where(t => t.Name.Contains(usernameToMatch)).Select(t => t.Name).ToArray();
				return uarr;
			}
			return new string[0];
		}

		/// <summary>
		/// Gets or sets the name of the application to store and retrieve role information for.
		/// </summary>
		/// <returns>
		/// The name of the application to store and retrieve role information for.
		/// </returns>
		public override string ApplicationName { get; set; }

		#endregion
	}
}
