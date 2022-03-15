using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using NutsStore.Models;
using Microsoft.Extensions.Logging;

namespace NutsStore.Util
{
    public class SqliteManager
    {
        #region Fields

        public static ILogger<SqliteManager> logger;
        private static volatile SqliteManager instance;
        private static object syncRoot = new Object();

        private static SQLiteConnection _sqlCon;
        private SQLiteCommand _sqlCmd;
        private SQLiteDataAdapter DB;

        private readonly DataSet _userDs = new DataSet();
        private DataTable _userDt = new DataTable();
        private const string _userTable = "USERS";
        private const string _userColumns = "USERNAME, FIRSTNAME, LASTNAME, PHONE_NUMBER, ADDRESS, CREATION_DATE, MODIFICATION_DATE, EMAIL, PASSWORD, AGE, GENDER, IS_ADMIN";

        private readonly DataSet _reportDs = new DataSet();
        private DataTable _reportDt = new DataTable();
        private const string _reportTable = "REPORT";
        private const string _reportColumns = "STATE, ACTION_DATE, USERNAME, USER_ID";

        private readonly DataSet _basketDs = new DataSet();
        private DataTable _basketDt = new DataTable();
        private const string _basketTable = "BASKET";
        private const string _basketColumns = "USER_ID, NUMBER, BASKET_JSON, ADDRESS, IS_PAID_DIRECTLY, CREATION_DATE, MODIFICATION_DATE, DESCRIPTION, STATUS";

        private readonly DataSet _contentDs = new DataSet();
        private DataTable _contentDt = new DataTable();
        private const string _contentTable = "CONTENT";
        private const string _contentColumns = "TITLE1, TITLE2, TITLE3, TITLE4, TITLE5, TITLE6, TITLE7, TITLE8, TITLE9, TITLE10, TITLE11, TITLE12, CONTENT1, CONTENT1, CONTENT3, CONTENT4, CONTENT5, CONTENT6, CONTENT7, CONTENT8, CONTENT9, CONTENT10, CONTENT11, CONTENT12 , CREATION_DATE, MODIFICATION_DATE ";

        private readonly DataSet _productDs = new DataSet();
        private DataTable _productDt = new DataTable();
        private const string _productTable = "PRODUCT";
        private const string _productColumns = "USER_ID, TITLE, CAPACITY, PRICE, CATEGORY, DESCRIPTION, IS_DISPLAY, CREATION_DATE, MODIFICATION_DATE , IMAGE";

        #endregion

        #region Ctor

        public static SqliteManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            InitDb();
                        }
                    }
                }

                return instance;
            }
        }

        public static void InitDb()
        {
            var connectionString = Directory.GetCurrentDirectory() + "\\SqliteDB.db";
            instance = new SqliteManager();
            if (!File.Exists(connectionString))
            {
                var dir = Path.GetDirectoryName(connectionString);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                SQLiteConnection.CreateFile(connectionString);

                using (var sqlite = new SQLiteConnection(string.Format("Data Source={0}", connectionString)))
                {
                    sqlite.Open();
                    string sql = $"CREATE TABLE {_userTable} ( [ID] INTEGER  PRIMARY KEY NOT NULL" +
                                                            ", [USERNAME] TEXT NOT NULL" +
                                                            ", [FIRSTNAME] TEXT NOT NULL" +
                                                            ", [LASTNAME] TEXT NOT NULL" +
                                                            ", [PHONE_NUMBER] TEXT NOT NULL" +
                                                            ", [ADDRESS] TEXT NOT NULL" +
                                                            ", [CREATION_DATE] TEXT NOT NULL" +
                                                            ", [MODIFICATION_DATE] TEXT NULL" +
                                                            ", [EMAIL] TEXT NULL" +
                                                            ", [PASSWORD] TEXT NOT NULL" +
                                                            ", [AGE] INTEGER NOT NULL" +
                                                            ", [GENDER] INTEGER NOT NULL" +
                                                            ", [IS_ADMIN] INTEGER NOT NULL);";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();

                    var query = string.Format("INSERT INTO {0}(ID, {1}) VALUES({2} , {3});", _userTable, _userColumns, 1, instance.ConvertToQueryValue(
                        new UserInfo(Constant.AdminInitialUsername,
                            "admin",
                            "admin",
                            "",
                            "address",
                            DateTime.Now.ToString(),
                            "",
                            "",
                            Constant.AdminInitialPassword,
                            0,
                            true,
                            true)));
                    command = new SQLiteCommand(query, sqlite);
                    command.ExecuteNonQuery();
                }

                using (var sqlite = new SQLiteConnection(string.Format("Data Source={0}", connectionString)))
                {
                    sqlite.Open();
                    string sql = $"CREATE TABLE {_basketTable} ( [ID] INTEGER  PRIMARY KEY NOT NULL" +
                                                            ",   [USER_ID] INTEGER NOT NULL" +
                                                            ",   [NUMBER] TEXT NOT NULL" +
                                                            ",   [BASKET_JSON] TEXT NOT NULL" +
                                                            ",   [ADDRESS] TEXT NULL" +
                                                            ",   [IS_PAID_DIRECTLY] INTEGER NOT NULL" +
                                                            ",   [CREATION_DATE] TEXT NOT NULL" +
                                                            ",   [MODIFICATION_DATE] TEXT NULL" +
                                                            ",   [DESCRIPTION] TEXT NULL" +
                                                            ",   [STATUS] INTEGER NOT NULL);";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();
                }

                using (var sqlite = new SQLiteConnection(string.Format("Data Source={0}", connectionString)))
                {
                    sqlite.Open();

                    string sql = $"CREATE TABLE {_contentTable} ( [ID] INTEGER  PRIMARY KEY NOT NULL" +
                                                               ", [TITLE1] TEXT NULL" +
                                                               ", [TITLE2] TEXT NULL" +
                                                               ", [TITLE3] TEXT NULL" +
                                                               ", [TITLE4] TEXT NULL" +
                                                               ", [TITLE5] TEXT NULL" +
                                                               ", [TITLE6] TEXT NULL" +
                                                               ", [TITLE7] TEXT NULL" +
                                                               ", [TITLE8] TEXT NULL" +
                                                               ", [TITLE9] TEXT NULL" +
                                                               ", [TITLE10] TEXT NULL" +
                                                               ", [TITLE11] TEXT NULL" +
                                                               ", [TITLE12] TEXT NULL" +
                                                               ", [CONTENT1] TEXT NULL" +
                                                               ", [CONTENT2] TEXT NULL" +
                                                               ", [CONTENT3] TEXT NULL" +
                                                               ", [CONTENT4] TEXT NULL" +
                                                               ", [CONTENT5] TEXT NULL" +
                                                               ", [CONTENT6] TEXT NULL" +
                                                               ", [CONTENT7] TEXT NULL" +
                                                               ", [CONTENT8] TEXT NULL" +
                                                               ", [CONTENT9] TEXT NULL" +
                                                               ", [CONTENT10] TEXT NULL" +
                                                               ", [CONTENT11] TEXT NULL" +
                                                               ", [CONTENT12] TEXT NULL" +
                                                               ", [CREATION_DATE] TEXT NOT NULL" +
                                                               ", [MODIFICATION_DATE] TEXT NULL);";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();


                    var query = string.Format("INSERT INTO {0}(ID, {1}) VALUES({2} , {3});", _contentTable, _contentColumns, 1, instance.ConvertToQueryValue(new ContentInfo("Title1", "Title2", "Title3", "Title4", "Title5", "Title6", "Title7", "Title8", "Title9", "Title10", "Title11", "Title12", "Content1", "Content2", "Content3", "Content4", "Content5", "Content6", "Content7", "Content8", "Content9", "Content10", "Content11", "Content12", DateTime.Now.ToString(), "")));
                    command = new SQLiteCommand(query, sqlite);
                    command.ExecuteNonQuery();
                }

                using (var sqlite = new SQLiteConnection(string.Format("Data Source={0}", connectionString)))
                {
                    sqlite.Open();
                    string sql = $"CREATE TABLE {_productTable} ( [ID] INTEGER  PRIMARY KEY NOT NULL" +
                                                               ", [USER_ID] INTEGER NOT NULL" +
                                                               ", [TITLE] TEXT NOT NULL" +
                                                               ", [CAPACITY] INTEGER NOT NULL" +
                                                               ", [PRICE] INTEGER NOT NULL" +
                                                               ", [CATEGORY] INTEGER NOT NULL" +
                                                               ", [DESCRIPTION] TEXT NOT NULL" +
                                                               ", [IS_DISPLAY] INTEGER NOT NULL" +
                                                               ", [CREATION_DATE] TEXT NOT NULL" +
                                                               ", [MODIFICATION_DATE] TEXT NULL" +
                                                               ", [IMAGE] TEXT NOT NULL);";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();
                }

                using (var sqlite = new SQLiteConnection(string.Format("Data Source={0}", connectionString)))
                {
                    sqlite.Open();
                    string sql = $"CREATE TABLE {_reportTable} ( [ID] INTEGER  PRIMARY KEY NOT NULL" +
                                                              ", [STATE] TEXT NOT NULL" +
                                                              ", [ACTION_DATE] TEXT NOT NULL" +
                                                              ", [USERNAME] TEXT NOT NULL" +
                                                              ", [USER_ID] INTEGER NOT NULL);";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();
                }
            }
            SetConnection(connectionString);
        }

        private static void SetConnection(string connectionStringModel)
        {
            var con = string.Format("DATA SOURCE=" + connectionStringModel + ";VERSION=3;");
            _sqlCon = new SQLiteConnection(con);
        }

        #endregion

        #region User

        public bool DeleteUser(double id)
        {
            try
            {
                logger?.LogInformation("DeleteUser");
                string txtSqlQuery = String.Format("DELETE FROM {0} WHERE ID = {1}", _userTable, id);
                return ExecuteQuery(txtSqlQuery);
            }
            catch (Exception ex)
            {
                _sqlCon.Close();
                logger?.LogError($"DeleteUser Exception Message : {ex.Message}");
                return false;
            }
        }

        public int AddUser(UserInfo userInfo)
        {
            try
            {
                logger?.LogInformation("AddUser");
                var cnt = GetUserCount();
                if (cnt != -1)
                {
                    var id = cnt + 1;
                    var query = string.Format("INSERT INTO {0}(ID, {1}) VALUES({2} , {3});", _userTable, _userColumns, id, ConvertToQueryValue(userInfo));
                    if (ExecuteQuery(query))
                        return id;
                    return -1;
                }
            }
            catch (Exception ex)
            {
                logger?.LogError($"AddUser Exception Message : {ex.Message}");
                _sqlCon.Close();
                return -1;
            }
            return -1;
        }

        public int GetUserCount()
        {
            try
            {
                logger?.LogInformation("GetUserCount");
                var count = ExecuteScalar(string.Format("SELECT COUNT(*) FROM {0}", _userTable));
                return count;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetUserCount Exception Message : {ex.Message}");
                _sqlCon.Close();
                return -1;
            }
        }

        public string ExistsUser(string username, string password)
        {
            try
            {
                logger?.LogInformation("GetUserCount");
                var hashPass = SecurePasswordHasher.Hash(password);
                var count = ExecuteScalar($"SELECT COUNT(*) FROM {_userTable} WHERE USERNAME = '{username}' ");
                var exists = count > 0;
                if (exists)
                {
                    var fetchedPass = ExecuteScalarTxt($"SELECT PASSWORD FROM {_userTable} WHERE USERNAME = '{username}' ");
                    var isMatchePass = SecurePasswordHasher.Verify(password, fetchedPass);
                    if (isMatchePass)
                    {
                        //string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        var usernameToken = SecurePasswordHasher.Hash(username);
                        var passToken = SecurePasswordHasher.Hash(password);
                        var token = $"{usernameToken}__token__{passToken}";
                        var user = GetUser(username);
                        return token;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetUserCount Exception Message : {ex.Message}");
                _sqlCon.Close();
                return null;
            }
        }

        public bool ExistsUser(string username)
        {
            try
            {
                logger?.LogInformation("GetUserCount");
                var count = ExecuteScalar($"SELECT COUNT(*) FROM {_userTable} WHERE USERNAME = '{username}' ");
                return count > 0;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetUserCount Exception Message : {ex.Message}");
                _sqlCon.Close();
                return false;
            }
        }

        public bool EditCurrentUser(UserInfo userInfo)
        {
            try
            {
                logger?.LogInformation("EditUser");
                var gender = userInfo.Gender == true ? 1 : 0;
                string txtSqlQuery = $"UPDATE {_userTable} SET FIRSTNAME = \"{userInfo.Firstname}\", LASTNAME = \"{userInfo.Lastname}\", PHONE_NUMBER = \"{userInfo.PhoneNumber}\", ADDRESS = \"{userInfo.Address}\", MODIFICATION_DATE = \"{userInfo.ModificationDate}\", EMAIL =\"{userInfo.Email}\", AGE =\"{userInfo.Age}\", GENDER = \"{gender}\" WHERE ID = \"{userInfo.Id}\" ";
                return ExecuteQuery(txtSqlQuery);
            }
            catch (Exception ex)
            {
                logger?.LogError($"EditUser Exception Message : {ex.Message}");
                _sqlCon.Close();
                return false;
            }
        }

        public bool EditUser(string username, UserInfo userInfo)
        {
            try
            {
                logger?.LogInformation("EditUser");
                var gender = userInfo.Gender == true ? 1 : 0;
                var isAdmin = userInfo.IsAdmin ? 1 : 0;
                string txtSqlQuery = $"UPDATE {_userTable} SET FIRSTNAME = \"{userInfo.Firstname}\", LASTNAME = \"{userInfo.Lastname}\", PHONE_NUMBER = \"{userInfo.PhoneNumber}\", ADDRESS = \"{userInfo.Address}\", MODIFICATION_DATE = \"{userInfo.ModificationDate}\", EMAIL =\"{userInfo.Email}\", AGE =\"{userInfo.Age}\", GENDER = \"{gender}\", IS_ADMIN = \"{isAdmin}\" WHERE USERNAME = \"{username}\" ";
                return ExecuteQuery(txtSqlQuery);
            }
            catch (Exception ex)
            {
                logger?.LogError($"EditUser Exception Message : {ex.Message}");
                _sqlCon.Close();
                return false;
            }
        }

        public UserInfo GetUser(int id)
        {
            try
            {
                logger?.LogInformation("GetUser");
                _sqlCon.Open();

                _sqlCmd = _sqlCon.CreateCommand();
                string commandText = $"select id,{_userColumns} from {_userTable} where id = \"{id}\" ";
                DB = new SQLiteDataAdapter(commandText, _sqlCon);
                _userDs.Reset();
                DB.Fill(_userDs);
                _userDt = _userDs.Tables[0];
                _sqlCon.Close();


                var item = _userDt.Rows[0];
                bool? gender = item[11].ToString() == "1";

                var userInfo = new UserInfo(item[1].ToString(),
                                            item[2].ToString(),
                                            item[3].ToString(),
                                            item[4].ToString(),
                                            item[5].ToString(),
                                            item[6].ToString(),
                                            item[7].ToString(),
                                            item[8].ToString(),
                                            item[9].ToString(),
                                            int.Parse(item[10].ToString()),
                                            gender
                                            , false);

                userInfo.Id = id;

                return userInfo;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetUser Exception Message : {ex.Message}");
                _sqlCon.Close();
                return null;
            }
        }

        public UserInfo GetUser(string username)
        {
            try
            {
                logger?.LogInformation("GetUser");
                _sqlCon.Open();

                _sqlCmd = _sqlCon.CreateCommand();
                string commandText = $"SELECT ID,{_userColumns} FROM {_userTable} WHERE USERNAME = '{username}'";
                DB = new SQLiteDataAdapter(commandText, _sqlCon);
                _userDs.Reset();
                DB.Fill(_userDs);
                _userDt = _userDs.Tables[0];
                _sqlCon.Close();


                var item = _userDt.Rows[0];
                bool? gender = item[11].ToString() == "1";
                bool isAdmin = item[12].ToString() == "1";

                var userInfo = new UserInfo(item[1].ToString(),
                                            item[2].ToString(),
                                            item[3].ToString(),
                                            item[4].ToString(),
                                            item[5].ToString(),
                                            item[6].ToString(),
                                            item[7].ToString(),
                                            item[8].ToString(),
                                            item[9].ToString(),
                                            int.Parse(item[10].ToString()),
                                            gender
                                            , isAdmin);

                userInfo.Id = int.Parse(item[0].ToString());

                return userInfo;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetUser Exception Message : {ex.Message}");
                _sqlCon.Close();
                return null;
            }
        }

        public List<UserInfo> GetUsers()
        {
            try
            {
                logger?.LogInformation("GetUsers");
                var result = new List<UserInfo>();
                _sqlCon.Open();

                _sqlCmd = _sqlCon.CreateCommand();
                string commandText = $"SELECT ID,{_userColumns} FROM {_userTable}";
                DB = new SQLiteDataAdapter(commandText, _sqlCon);
                _userDs.Reset();
                DB.Fill(_userDs);
                _userDt = _userDs.Tables[0];
                _sqlCon.Close();
                for (int i = 0; i < _userDt.Rows.Count; i++)
                {
                    var item = _userDt.Rows[i];
                    bool? gender = item[11].ToString() == "1";
                    bool isAdmin = item[12].ToString() == "1";

                    var id = int.Parse(item[0].ToString());
                    var userInfo = new UserInfo(item[1].ToString(),
                                                item[2].ToString(),
                                                item[3].ToString(),
                                                item[4].ToString(),
                                                item[5].ToString(),
                                                item[6].ToString(),
                                                item[7].ToString(),
                                                item[8].ToString(),
                                                item[9].ToString(),
                                                int.Parse(item[10].ToString()),
                                                gender,
                                                isAdmin);

                    userInfo.Id = id;
                    result.Add(userInfo);
                }
                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetUsers Exception Message : {ex.Message}");
                _sqlCon.Close();
                return null;
            }
        }

        public bool ResetPassword(UserInfo userInfo)
        {
            try
            {
                logger?.LogInformation("EditUser");
                var hashPass = SecurePasswordHasher.Hash(userInfo.Password);
                string txtSqlQuery = $"UPDATE {_userTable} SET PASSWORD = \"{hashPass}\" WHERE USERNAME = \"{userInfo.Username}\" ";
                return ExecuteQuery(txtSqlQuery);
            }
            catch (Exception ex)
            {
                logger?.LogError($"EditUser Exception Message : {ex.Message}");
                _sqlCon.Close();
                return false;
            }
        }

        private string ConvertToQueryValue(UserInfo userInfo)
        {
            try
            {
                logger?.LogInformation("ConvertToQueryValue");
                var gender = userInfo.Gender == null ? -1 : (userInfo.Gender == true ? 1 : 0);
                var isAdmin = userInfo.IsAdmin == true ? 1 : 0;
                var hashPass = SecurePasswordHasher.Hash(userInfo.Password);
                return $"'{userInfo.Username}','{userInfo.Firstname}','{userInfo.Lastname}','{userInfo.PhoneNumber}','{userInfo.Address}','{userInfo.CreationDate}','{userInfo.ModificationDate}','{userInfo.Email}','{hashPass}','{userInfo.Age}','{gender}','{isAdmin}'";
            }
            catch (Exception ex)
            {
                logger?.LogError($"ConvertToQueryValue Exception Message : {ex.Message}");
            }
            return null;
        }

        #endregion

        #region Basket

        public bool DeleteBasket(int id)
        {
            try
            {
                logger?.LogInformation("DeleteBasket");
                string txtSqlQuery = $"DELETE FROM {_basketTable} WHERE ID = {id}";
                return ExecuteQuery(txtSqlQuery);
            }
            catch (Exception ex)
            {
                _sqlCon.Close();
                logger?.LogError($"DeleteBasket Exception Message : {ex.Message}");
                return false;
            }
        }

        public int AddBasket(BasketInfo basketInfo)
        {
            try
            {
                logger?.LogInformation("AddBasket");
                var cnt = GetBasketCount();
                if (cnt != -1)
                {
                    var id = cnt + 1;
                    var query = $"INSERT INTO {_basketTable} (ID, {_basketColumns}) VALUES({id} , {ConvertToQueryValue(basketInfo)});";
                    if (ExecuteQuery(query))
                        return id;
                    return -1;
                }
            }
            catch (Exception ex)
            {
                logger?.LogError($"AddBasket Exception Message : {ex.Message}");
                _sqlCon.Close();
                return -1;
            }
            return -1;
        }

        public int GetBasketCount()
        {
            try
            {
                logger?.LogInformation("GetBasketCount");
                var count = ExecuteScalar(string.Format("SELECT COUNT(*) FROM {0}", _basketTable));
                return count;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetBasketCount Exception Message : {ex.Message}");
                _sqlCon.Close();
                return -1;
            }
        }

        public List<BasketInfo> GetBaskets()
        {
            try
            {
                logger?.LogInformation("GetBaskets");
                var result = new List<BasketInfo>();
                _sqlCon.Open();

                _sqlCmd = _sqlCon.CreateCommand();
                string commandText = $"SELECT ID,{_basketColumns} FROM {_basketTable}";
                DB = new SQLiteDataAdapter(commandText, _sqlCon);
                _basketDs.Reset();
                DB.Fill(_basketDs);
                _basketDt = _basketDs.Tables[0];
                _sqlCon.Close();
                for (int i = 0; i < _basketDt.Rows.Count; i++)
                {
                    var item = _basketDt.Rows[i];
                    var id = int.Parse(item[0].ToString());
                    var isSentToUserAddres = item[5].ToString() == "1";

                    var basketInfo = new BasketInfo(int.Parse(item[1].ToString()),
                                                    item[2].ToString(),
                                                    item[3].ToString(),
                                                    item[4].ToString(),
                                                    isSentToUserAddres,
                                                    item[6].ToString(),
                                                    item[7].ToString(),
                                                    item[8].ToString(),
                                                    int.Parse(item[9].ToString()));

                    basketInfo.Id = id;
                    result.Add(basketInfo);
                }
                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetBaskets Exception Message : {ex.Message}");
                _sqlCon.Close();
                return null;
            }
        }

        public BasketInfo GetBasket(int id)
        {
            try
            {
                logger?.LogInformation("GetBasket");
                _sqlCon.Open();

                _sqlCmd = _sqlCon.CreateCommand();
                string commandText = $"select id,{_basketColumns} from {_basketTable} where id = \"{id}\" ";
                DB = new SQLiteDataAdapter(commandText, _sqlCon);
                _basketDs.Reset();
                DB.Fill(_basketDs);
                _basketDt = _basketDs.Tables[0];
                _sqlCon.Close();


                var item = _basketDt.Rows[0];
                bool isSentToUserAddress = item[5].ToString() == "1";

                var basketInfo = new BasketInfo(int.Parse(item[1].ToString()),
                                            item[2].ToString(),
                                            item[3].ToString(),
                                            item[4].ToString(),
                                            isSentToUserAddress,
                                            item[6].ToString(),
                                            item[7].ToString(),
                                            item[8].ToString(),
                                            int.Parse(item[9].ToString()));

                basketInfo.Id = id;
                return basketInfo;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetBasket Exception Message : {ex.Message}");
                _sqlCon.Close();
                return null;
            }
        }

        public bool EditBasket(BasketInfo basketInfo)
        {
            try
            {
                logger?.LogInformation("EditBasket");
                string txtSqlQuery = $"UPDATE {_basketTable} SET USER_ID =\"{basketInfo.UserId}\", NUMBER =\"{basketInfo.Number}\", BASKET_JSON = \"{basketInfo.BasketJSON}\", ADDRESS = \"{basketInfo.Address}\", MODIFICATION_DATE = \"{basketInfo.ModificationDate}\", DESCRIPTION = \"{basketInfo.Description}\", STATUS = \"{basketInfo.Status}\" WHERE ID = \"{basketInfo.Id}\" ";
                return ExecuteQuery(txtSqlQuery);
            }
            catch (Exception ex)
            {
                logger?.LogError($"EditBasket Exception Message : {ex.Message}");
                _sqlCon.Close();
                return false;
            }
        }

        private string ConvertToQueryValue(BasketInfo basketInfo)
        {
            try
            {
                logger?.LogInformation("ConvertToQueryValue");
                var isSentToUserAddress = basketInfo.IsSentToUserAddress == true ? 1 : 0;
                return $"'{basketInfo.UserId}','{basketInfo.Number}','{basketInfo.BasketJSON}','{basketInfo.Address}','{isSentToUserAddress}','{basketInfo.CreationDate}','{basketInfo.ModificationDate}','{basketInfo.Description}','{basketInfo.Status}'";
            }
            catch (Exception ex)
            {
                logger?.LogError($"ConvertToQueryValue Exception Message : {ex.Message}");
            }
            return null;
        }

        #endregion

        #region Content

        public ContentInfo GetContent()
        {
            try
            {
                logger?.LogInformation("GetContent");

                _sqlCmd = _sqlCon.CreateCommand();
                string commandText = $"SELECT ID,{_contentColumns} FROM {_contentTable}";
                DB = new SQLiteDataAdapter(commandText, _sqlCon);
                _contentDs.Reset();
                DB.Fill(_contentDs);
                _contentDt = _contentDs.Tables[0];
                _sqlCon.Close();

                var item = _contentDt.Rows[0];
                var id = int.Parse(item[0].ToString());
                var contentInfo = new ContentInfo(item[1].ToString(),
                                                  item[2].ToString(),
                                                  item[3].ToString(),
                                                  item[4].ToString(),
                                                  item[5].ToString(),
                                                  item[6].ToString(),
                                                  item[7].ToString(),
                                                  item[8].ToString(),
                                                  item[9].ToString(),
                                                  item[10].ToString(),
                                                  item[11].ToString(),
                                                  item[12].ToString(),
                                                  item[13].ToString(),
                                                  item[14].ToString(),
                                                  item[15].ToString(),
                                                  item[16].ToString(),
                                                  item[17].ToString(),
                                                  item[18].ToString(),
                                                  item[19].ToString(),
                                                  item[20].ToString(),
                                                  item[21].ToString(),
                                                  item[22].ToString(),
                                                  item[23].ToString(),
                                                  item[24].ToString(),
                                                  item[25].ToString(),
                                                  item[26].ToString());

                contentInfo.Id = id;
                return contentInfo;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetContent Exception Message : {ex.Message}");
                _sqlCon.Close();
                return null;
            }
        }

        public bool EditContent(ContentInfo contentInfo)
        {
            try
            {
                logger?.LogInformation("EditContent");
                string txtSqlQuery = $"UPDATE {_contentTable} SET TITLE1 =\"{contentInfo.Title1}\",  TITLE2 =\"{contentInfo.Title2}\",  TITLE3 =\"{contentInfo.Title3}\",  TITLE4 =\"{contentInfo.Title4}\",  TITLE5 =\"{contentInfo.Title5}\",   TITLE6 =\"{contentInfo.Title6}\",   TITLE7 =\"{contentInfo.Title7}\",   TITLE8 =\"{contentInfo.Title8}\",   TITLE9 =\"{contentInfo.Title9}\",   TITLE10 =\"{contentInfo.Title10}\",   TITLE11 =\"{contentInfo.Title11}\", TITLE12 =\"{contentInfo.Title12}\" , CONTENT1 =\"{contentInfo.Content1}\",  CONTENT2 =\"{contentInfo.Content2}\",  CONTENT3 =\"{contentInfo.Content3}\",  CONTENT4 =\"{contentInfo.Content4}\",  CONTENT5 =\"{contentInfo.Content5}\",   CONTENT6 =\"{contentInfo.Content6}\",   CONTENT7 =\"{contentInfo.Content7}\",   CONTENT8 =\"{contentInfo.Content8}\",   CONTENT9 =\"{contentInfo.Content9}\",   CONTENT10 =\"{contentInfo.Content10}\",   CONTENT11 =\"{contentInfo.Content11}\", CONTENT12 =\"{contentInfo.Content12}\" , Creation_Date =\"{contentInfo.CreationDate}\" , MODIFICATION_DATE =\"{contentInfo.ModificationDate}\" ";
                return ExecuteQuery(txtSqlQuery);
            }
            catch (Exception ex)
            {
                logger?.LogError($"EditContent Exception Message : {ex.Message}");
                _sqlCon.Close();
                return false;
            }
        }

        private string ConvertToQueryValue(ContentInfo contentInfo)
        {
            try
            {
                logger?.LogInformation("ConvertToQueryValue");
                return $"'{contentInfo.Title1}', '{contentInfo.Title2}', '{contentInfo.Title3}', '{contentInfo.Title4}', '{contentInfo.Title5}', '{contentInfo.Title6}', '{contentInfo.Title7}', '{contentInfo.Title8}', '{contentInfo.Title9}', '{contentInfo.Title10}', '{contentInfo.Title11}', '{contentInfo.Title12}','{contentInfo.Content1}', '{contentInfo.Content2}', '{contentInfo.Content3}', '{contentInfo.Content4}', '{contentInfo.Content5}', '{contentInfo.Content6}', '{contentInfo.Content7}', '{contentInfo.Content8}', '{contentInfo.Content9}', '{contentInfo.Content10}', '{contentInfo.Content11}', '{contentInfo.Content12}', '{contentInfo.CreationDate}', '{contentInfo.ModificationDate}'";
            }
            catch (Exception ex)
            {
                logger?.LogError($"ConvertToQueryValue Exception Message : {ex.Message}");
            }
            return null;
        }

        #endregion

        #region Product

        public int AddProduct(ProductInfo productInfo)
        {
            try
            {
                logger?.LogInformation("AddProduct");
                var cnt = GetProductCount();
                if (cnt != -1)
                {
                    var id = cnt + 1;
                    var query = string.Format("INSERT INTO {0}(ID, {1}) VALUES({2} , {3});", _productTable, _productColumns, id, ConvertToQueryValue(productInfo));
                    if (ExecuteQuery(query))
                        return id;
                    return -1;
                }
            }
            catch (Exception ex)
            {
                logger?.LogError($"AddProduct Exception Message : {ex.Message}");
                _sqlCon.Close();
                return -1;
            }
            return -1;
        }

        public bool DeleteProduct(double id)
        {
            try
            {
                logger?.LogInformation("DeleteProduct");
                string txtSqlQuery = String.Format("DELETE FROM {0} WHERE ID = {1}", _productTable, id);
                return ExecuteQuery(txtSqlQuery);
            }
            catch (Exception ex)
            {
                _sqlCon.Close();
                logger?.LogError($"DeleteProduct Exception Message : {ex.Message}");
                return false;
            }
        }

        public int GetProductCount()
        {
            try
            {
                logger?.LogInformation("GetProductCount");
                var count = ExecuteScalar(string.Format("SELECT COUNT(*) FROM {0}", _productTable));
                return count;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetProductCount Exception Message : {ex.Message}");
                _sqlCon.Close();
                return -1;
            }
        }

        public List<ProductInfo> GetProducts()
        {
            try
            {
                logger?.LogInformation("GetProducts");
                var result = new List<ProductInfo>();
                _sqlCon.Open();

                _sqlCmd = _sqlCon.CreateCommand();
                string commandText = $"SELECT ID,{_productColumns} FROM {_productTable}";
                DB = new SQLiteDataAdapter(commandText, _sqlCon);
                _productDs.Reset();
                DB.Fill(_productDs);
                _productDt = _productDs.Tables[0];
                _sqlCon.Close();
                for (int i = 0; i < _productDt.Rows.Count; i++)
                {
                    var item = _productDt.Rows[i];
                    bool IS_DISPLAY = item[7].ToString() == "1";
                    string base64 = item[10].ToString();

                    var id = int.Parse(item[0].ToString());
                    var productInfo = new ProductInfo(int.Parse(item[1].ToString()),
                                                      item[2].ToString(),
                                                      int.Parse(item[3].ToString()),
                                                      int.Parse(item[4].ToString()),
                                                      int.Parse(item[5].ToString()),
                                                      item[6].ToString(),
                                                      IS_DISPLAY,
                                                      item[8].ToString(),
                                                      item[9].ToString(),
                                                      base64);

                    productInfo.Id = id;
                    result.Add(productInfo);
                }
                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetProducts Exception Message : {ex.Message}");
                _sqlCon.Close();
                return null;
            }
        }

        public List<ProductInfo> GetProduct(int id)
        {
            try
            {
                logger?.LogInformation("GetProducts");
                var result = new List<ProductInfo>();
                _sqlCon.Open();

                _sqlCmd = _sqlCon.CreateCommand();
                string commandText = $"SELECT ID,{_productColumns} FROM {_productTable} where id = \"{id}\" ";
                DB = new SQLiteDataAdapter(commandText, _sqlCon);
                _productDs.Reset();
                DB.Fill(_productDs);
                _productDt = _productDs.Tables[0];
                _sqlCon.Close();

                var item = _productDt.Rows[0];
                bool IS_DISPLAY = item[7].ToString() == "1";
                string base64 = item[10].ToString();

                var productInfo = new ProductInfo(int.Parse(item[1].ToString()),
                                                  item[2].ToString(),
                                                  int.Parse(item[3].ToString()),
                                                  int.Parse(item[4].ToString()),
                                                  int.Parse(item[5].ToString()),
                                                  item[6].ToString(),
                                                  IS_DISPLAY,
                                                  item[8].ToString(),
                                                  item[9].ToString(),
                                                  base64);

                productInfo.Id = id;
                result.Add(productInfo);

                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetProducts Exception Message : {ex.Message}");
                _sqlCon.Close();
                return null;
            }
        }

        public bool EditProduct(ProductInfo productInfo)
        {
            try
            {
                logger?.LogInformation("EditProduct");

                var isDisplay = productInfo.IsDisplay ? "1" : "0";
                string txtSqlQuery = $"UPDATE {_productTable} SET USER_ID =\"{productInfo.UserId}\",  TITLE =\"{productInfo.Title}\", CAPACITY = \"{productInfo.Capacity}\", PRICE = \"{productInfo.Price}\", CATEGORY = \"{productInfo.Category}\", DESCRIPTION = \"{productInfo.Description}\", IS_DISPLAY = \"{isDisplay}\", MODIFICATION_DATE = \"{productInfo.ModificationDate}\", image = \"{productInfo.Image}\" WHERE ID = \"{productInfo.Id}\" ";
                return ExecuteQuery(txtSqlQuery);
            }
            catch (Exception ex)
            {
                logger?.LogError($"EditProduct Exception Message : {ex.Message}");
                _sqlCon.Close();
                return false;
            }
        }

        private string ConvertToQueryValue(ProductInfo productInfo)
        {
            try
            {
                logger?.LogInformation("ConvertToQueryValue");
                var isDisplay = productInfo.IsDisplay == true ? 1 : 0;
                return $"'{productInfo.UserId}','{productInfo.Title}','{productInfo.Capacity}','{productInfo.Price}','{productInfo.Category}','{productInfo.Description}','{isDisplay}','{productInfo.CreationDate}','{productInfo.ModificationDate}','{productInfo.Image}'";
            }
            catch (Exception ex)
            {
                logger?.LogError($"ConvertToQueryValue Exception Message : {ex.Message}");
            }
            return null;
        }

        #endregion

        #region Report

        public bool AddReport(AuthReportInfo authReportInfo)
        {
            try
            {
                logger?.LogInformation("AddReport");
                var cnt = GetReportCount();
                if (cnt != -1)
                {
                    var id = cnt + 1;
                    var query = string.Format("INSERT INTO {0}(ID, {1}) VALUES({2} , {3});", _reportTable, _reportColumns, id, ConvertToQueryValue(authReportInfo));
                    return ExecuteQuery(query);
                }
                return false;
            }
            catch (Exception ex)
            {
                logger?.LogError($"AddReport Exception Message : {ex.Message}");
                _sqlCon.Close();
                return false;
            }
        }

        public int GetReportCount()
        {
            try
            {
                logger?.LogInformation("GetReportCount");
                var count = ExecuteScalar(string.Format("SELECT COUNT(*) FROM {0}", _reportTable));
                return count;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetReportCount Exception Message : {ex.Message}");
                _sqlCon.Close();
                return -1;
            }
        }

        public List<AuthReportInfo> GetReports()
        {
            try
            {
                logger?.LogInformation("GetReports");
                var result = new List<AuthReportInfo>();
                _sqlCon.Open();

                _sqlCmd = _sqlCon.CreateCommand();
                string commandText = $"SELECT ID,{_reportColumns} FROM {_reportTable}";
                DB = new SQLiteDataAdapter(commandText, _sqlCon);
                _reportDs.Reset();
                DB.Fill(_reportDs);
                _reportDt = _reportDs.Tables[0];
                _sqlCon.Close();
                for (int i = 0; i < _reportDt.Rows.Count; i++)
                {
                    var item = _reportDt.Rows[i];

                    var id = int.Parse(item[0].ToString());
                    var AuthReportInfo = new AuthReportInfo
                    {
                        State = item[1].ToString(),
                        ActionDate = item[2].ToString(),
                        Username = item[3].ToString(),
                        UserId = int.Parse(item[4].ToString()),
                    };

                    AuthReportInfo.Id = id;
                    result.Add(AuthReportInfo);
                }
                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError($"GetReports Exception Message : {ex.Message}");
                _sqlCon.Close();
                return null;
            }
        }

        private string ConvertToQueryValue(AuthReportInfo AuthReportInfo)
        {
            try
            {
                logger?.LogInformation("ConvertToQueryValue");
                return $"'{AuthReportInfo.State}','{AuthReportInfo.ActionDate}','{AuthReportInfo.Username}','{AuthReportInfo.UserId}'";
            }
            catch (Exception ex)
            {
                logger?.LogError($"ConvertToQueryValue Exception Message : {ex.Message}");
            }
            return null;
        }

        #endregion

        #region Sqlite

        private bool ExecuteQuery(string txtQuery)
        {
            try
            {
                logger?.LogInformation("ExecuteQuery");
                _sqlCon.Open();

                _sqlCmd = _sqlCon.CreateCommand();
                _sqlCmd.CommandText = txtQuery;

                _sqlCmd.ExecuteNonQuery();
                _sqlCon.Close();
                return true;
            }
            catch (Exception ex)
            {
                _sqlCon.Close();
                logger?.LogError($"ExecuteQuery Exception Message : {ex.Message}");
                return false;
            }
        }

        private int ExecuteScalar(string txtQuery)
        {
            try
            {
                logger?.LogInformation("ExecuteScalar");
                _sqlCon.Open();
                _sqlCmd = new SQLiteCommand(txtQuery, _sqlCon);
                var result = _sqlCmd.ExecuteScalar();
                _sqlCon.Close();
                return int.Parse(result.ToString());
            }
            catch (Exception ex)
            {
                _sqlCon.Close();
                logger?.LogError($"ExecuteScalar Exception Message : {ex.Message}");
            }
            return -1;
        }

        private string ExecuteScalarTxt(string txtQuery)
        {
            try
            {
                logger?.LogInformation("ExecuteScalar");
                _sqlCon.Open();
                _sqlCmd = new SQLiteCommand(txtQuery, _sqlCon);
                var result = _sqlCmd.ExecuteScalar();
                _sqlCon.Close();
                return result.ToString();
            }
            catch (Exception ex)
            {
                _sqlCon.Close();
                logger?.LogError($"ExecuteScalar Exception Message : {ex.Message}");
            }
            return "";
        }

        #endregion
    }
}
