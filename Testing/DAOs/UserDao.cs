using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testing.Models;

namespace Testing.DAOs
{
    public interface IUserDao
    {

        /// <summary>
        /// Get users dao method, to get users data from database by user model properties.
        /// </summary>
        /// <returns></returns>
        List<UserModel> Get(int? ID, string Username, bool? OnlineStatus, DateTime? LastLogin, DateTime? CreatedAt);

        /// <summary>
        /// Insert user dao method, to insert user data in database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool Insert(UserModel user);

        /// <summary>
        /// Update user dao method, to update user data from database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool Update(UserModel user);
        /// <summary>
        /// Delete user dao method, to delete user data from database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int? id);
    }
}
