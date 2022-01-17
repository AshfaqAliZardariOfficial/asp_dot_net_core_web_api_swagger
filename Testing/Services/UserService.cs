using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Testing.DAOs;
using Testing.Models;
using Testing.Utils;

namespace Testing.Services
{
    public class UserService : IUserDao
    {
        public bool Delete(int? id)
        {
            try
            {
                IDictionary<string, object> parameters = ModelDictionaryMapping<UserModel>.GetDictionary(new UserModel { ID = id, IsDelete = true });
                string query = "update users set IsDelete = @IsDelete where id = @ID";
                return DatabaseUtils.Instance.InsertOrUpdateOrDelete(company: null, query: query, parameters: parameters);
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public List<UserModel> Get(int? ID, string Username, bool? OnlineStatus, DateTime? LastLogin, DateTime? CreatedAt)
        {
            try
            {
                IDictionary<string, object> parameters = ModelDictionaryMapping<UserModel>.GetDictionary(new UserModel { ID = ID, Username = Username, OnlineStatus = OnlineStatus, LastLogin = LastLogin, CreatedAt = CreatedAt, IsDelete = false });
                string query = "select Users.ID, Users.Username, Users.OnlineStatus, Users.LastLogin, Users.CreatedAt from Users where (ID IS NULL or ID = COALESCE(@ID, Users.ID)) and (Username IS NULL or Username = COALESCE(@Username, Users.Username)) and (Password IS NULL or Password = Users.Password) and (OnlineStatus IS NULL or OnlineStatus = COALESCE(@OnlineStatus, Users.OnlineStatus)) and (LastLogin IS NULL or LastLogin = COALESCE(@LastLogin, Users.LastLogin)) and (CreatedAt IS NULL or CreatedAt = COALESCE(@CreatedAt, Users.CreatedAt)) and (IsDelete IS NULL or IsDelete = COALESCE(@IsDelete, Users.IsDelete)) Order by ID Desc";
                List<UserModel> users = ModelDictionaryMapping<UserModel>.GetModelList<UserModel>(DatabaseUtils.Instance.GetByModel(company: null, query: query, parameters: parameters));
                return users;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Insert user model data to database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Insert(UserModel user)
        {
            try
            {
                IDictionary<string, object> parameters = ModelDictionaryMapping<UserModel>.GetDictionary(user);
                string query = "insert into users(Username, [Password], CreatedAt, IsDelete) values(@Username, @Password, @CreatedAt, @IsDelete)";
                return DatabaseUtils.Instance.InsertOrUpdateOrDelete(company: null, query: query, parameters: parameters);
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Update(UserModel user)
        {
            try
            {
                IDictionary<string, object> parameters = ModelDictionaryMapping<UserModel>.GetDictionary(user);
                string query = "update users set Username = @Username, [Password] = @Password where id = @ID";
                return DatabaseUtils.Instance.InsertOrUpdateOrDelete(company: null, query: query, parameters: parameters);
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
