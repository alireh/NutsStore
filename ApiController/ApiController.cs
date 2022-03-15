using Microsoft.Net.Http.Headers;
using NutsStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using NutsStore.Util;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace NutsStore.ApiController
{
    public class ApiController : Controller
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        public static ILogger<ApiController> logger;
        public IConfiguration Configuration { get; }

        #endregion

        #region Ctor

        [HttpGet]
        [Route("/index")]
        /// <summary>
        /// Api For simple Test connection
        /// </summary>
        /// <returns>Simple test string</returns>
        public string Index()
        {
            return "NutsStore REST API";
        }

        public ApiController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Authentication


        /// <summary>
        /// Api for add one user
        /// </summary>
        /// <param name="user">user infos </param>
        /// <returns>Response Code and message.</returns>
        [HttpPost]
        [Route("/api/authentication/signup")]
        public async Task<ResponseModel> Signup()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var userInfo = await Utility.GetBodyObject<UserInfo>(_httpContextAccessor);
                    userInfo.CreationDate = DateTime.Now.ToString();
                    logger.LogInformation($"Signup {userInfo.Username}");

                    if (string.IsNullOrWhiteSpace(userInfo.Firstname) || (!string.IsNullOrWhiteSpace(userInfo.Email) && !Utility.IsValidEmail(userInfo.Email)) || string.IsNullOrWhiteSpace(userInfo.Lastname) || string.IsNullOrWhiteSpace(userInfo.Username))
                    {
                        HttpContext.Response.StatusCode = 400;
                        return new ResponseModel
                        {
                            message = "bad input",
                            status = 400
                        };
                    }
                    if (SqliteManager.Instance.ExistsUser(userInfo.Username))
                    {
                        HttpContext.Response.StatusCode = 409;
                        return new ResponseModel
                        {
                            message = "already exists",
                            status = 409
                        };
                    }
                    string passwordPolicyMessage = "";
                    if (!Utility.IsValidPassword(userInfo.Password, Constant.PasswordPolicy, ref passwordPolicyMessage))
                    {
                        HttpContext.Response.StatusCode = 400;
                        return new ResponseModel
                        {
                            message = passwordPolicyMessage,
                            status = 400
                        };
                    }

                    userInfo.CreationDate = DateTime.Now.ToString();
                    var id = SqliteManager.Instance.AddUser(userInfo);
                    if (id == -1)
                    {
                        HttpContext.Response.StatusCode = 500;
                        return new ResponseModel
                        {
                            message = "failed",
                            status = 500
                        };
                    }

                    var now = DateTime.Now;
                    SqliteManager.Instance.AddReport(new AuthReportInfo
                    {
                        State = "Signup",
                        ActionDate = $"{now.Year}-{now.Month}-{now.Day} : {now.Hour}:{now.Minute}:{now.Second}",
                        Username = userInfo.Username,
                        UserId = id,
                    });
                    return new ResponseModel
                    {
                        message = "success",
                        status = 200
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"Signup Exception Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "error",
                        status = 500
                    };
                }
            });
        }

        /// <summary>
        /// Api for edit current user
        /// </summary>
        /// <param name="user">user infos </param>
        /// <returns>Response Code and message.</returns>
        [HttpPut]
        [Route("/api/authentication/edit")]
        public async Task<ResponseModel> EditCurrentUser()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var userInfo = await Utility.GetBodyObject<UserInfo>(_httpContextAccessor);
                    logger.LogInformation($"EditCurrentUser {userInfo.Username}");

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeOwnUserInstance(userInfo.Username), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }
                    if (string.IsNullOrWhiteSpace(userInfo.Firstname) || (!string.IsNullOrWhiteSpace(userInfo.Email) && !Utility.IsValidEmail(userInfo.Email)) || string.IsNullOrWhiteSpace(userInfo.Lastname) || string.IsNullOrWhiteSpace(userInfo.Username))
                    {
                        HttpContext.Response.StatusCode = 400;
                        return new ResponseModel
                        {
                            message = "bad input",
                            status = 400
                        };
                    }

                    var user = SqliteManager.Instance.GetUser(userData.Username);
                    userInfo.IsAdmin = user.IsAdmin;
                    userInfo.ModificationDate = DateTime.Now.ToString();
                    if (SqliteManager.Instance.EditCurrentUser(userInfo))
                        return new ResponseModel
                        {
                            message = "success",
                            status = 200
                        };
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"EditCurrentUser Exception Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "error",
                        status = 500
                    };
                }
            });
        }

        /// <summary>
        /// Api for edit specifiec user
        /// </summary>
        /// <param name="user">user infos </param>
        /// <returns>Response Code and message.</returns>
        [HttpPut]
        [Route("/api/authentication/edit/{username}")]
        public async Task<ResponseModel> EditUser([FromRoute] string username)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    logger.LogInformation($"EditUser {username}");

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }
                    var userInfo = await Utility.GetBodyObject<UserInfo>(_httpContextAccessor);
                    if (string.IsNullOrWhiteSpace(userInfo.Firstname) || (!string.IsNullOrWhiteSpace(userInfo.Email) && !Utility.IsValidEmail(userInfo.Email)) || string.IsNullOrWhiteSpace(userInfo.Lastname))
                    {
                        HttpContext.Response.StatusCode = 400;
                        return new ResponseModel
                        {
                            message = "bad input",
                            status = 400
                        };
                    }

                    var user = SqliteManager.Instance.GetUser(userData.Username);
                    if(user.Id == 1)
                    {
                        userInfo.IsAdmin = true;
                    }
                    userInfo.ModificationDate = DateTime.Now.ToString();
                    if (SqliteManager.Instance.EditUser(username, userInfo))
                        return new ResponseModel
                        {
                            message = "success",
                            status = 200
                        };
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"EditUser Exception Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "error",
                        status = 500
                    };
                }
            });
        }

        /// <summary>
        /// Api for reset current user password
        /// </summary>
        /// <returns>Response Code and message.</returns>
        [HttpPut]
        [Route("/api/authentication/resetpass")]
        public async Task<ResponseModel> ResetCurrentUserPassword()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var userInfo = await Utility.GetBodyObject<UserInfo>(_httpContextAccessor);
                    logger.LogInformation($"Reset {userInfo.Username} Password");

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeOwnUserInstance(userInfo.Username), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }
                    string passwordPolicyMessage = "";
                    if (!Utility.IsValidPassword(userInfo.Password, Constant.PasswordPolicy, ref passwordPolicyMessage))
                    {
                        HttpContext.Response.StatusCode = 400;
                        return new ResponseModel
                        {
                            message = passwordPolicyMessage,
                            status = 400
                        };
                    }

                    if (SqliteManager.Instance.ResetPassword(userInfo))
                        return new ResponseModel
                        {
                            message = "success",
                            status = 200
                        };
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"ResetCurrentUserPassword Exception Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "error",
                        status = 500
                    };
                }
            });
        }

        /// <summary>
        /// Api for reset specific user password
        /// </summary>
        /// <returns>Response Code and message.</returns>
        [HttpPut]
        [Route("/api/authentication/resetpass/{username}")]
        public async Task<ResponseModel> ResetPassword([FromRoute] string username)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    logger.LogInformation($"ResetPassword {username}");

                    var userInfo = await Utility.GetBodyObject<UserInfo>(_httpContextAccessor);

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }
                    string passwordPolicyMessage = "";
                    if (!Utility.IsValidPassword(userInfo.Password, Constant.PasswordPolicy, ref passwordPolicyMessage))
                    {
                        HttpContext.Response.StatusCode = 400;
                        return new ResponseModel
                        {
                            message = passwordPolicyMessage,
                            status = 400
                        };
                    }

                    userInfo.Username = username;
                    if (SqliteManager.Instance.ResetPassword(userInfo))
                        return new ResponseModel
                        {
                            message = "success",
                            status = 200
                        };
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"ResetPassword Exception Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "error",
                        status = 500
                    };
                }
            });
        }

        /// <summary>
        /// Returns a user list
        /// </summary>
        /// <param name="userInfo">The user informations </param>
        /// <returns>Response Code, Data and message.</returns>
        [HttpPost]
        [Route("/api/authentication/signin")]
        public async Task<ResponseModel> Signin()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var userInfo = await Utility.GetBodyObject<UserInfo>(_httpContextAccessor);
                    logger.LogInformation($"Signin {userInfo.Username}");

                    var token = SqliteManager.Instance.ExistsUser(userInfo.Username, userInfo.Password);
                    if (token != null)
                    {
                        var now = DateTime.Now;
                        var user = SqliteManager.Instance.GetUser(userInfo.Username);
                        SqliteManager.Instance.AddReport(new AuthReportInfo
                        {
                            State = "Signin",
                            ActionDate = $"{now.Year}-{now.Month}-{now.Day} : {now.Hour}:{now.Minute}:{now.Second}",
                            Username = user.Username,
                            UserId = user.Id,
                        });

                        var userDataObject = new UserData
                        {
                            Token = token,
                            FirstName = user.Firstname,
                            Username = user.Username,
                            LastName = user.Lastname,
                            //profile_pic = userData.profile_pic
                        };
                        HttpContext.Session.Set("UserData", Utility.ObjectToByteArray(userDataObject));
                        return new ResponseModel
                        {
                            content = token,
                            message = "success",
                            status = 200
                        };
                    }
                    HttpContext.Response.StatusCode = 400;
                    return new ResponseModel
                    {
                        content = null,
                        message = "failed",
                        status = 400
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"Signin Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        content = null,
                        message = "failed",
                        status = 500
                    };
                }
            });
        }

        /// <summary>
        /// Signout user
        /// </summary>
        /// <returns>Response Code, Data and message.</returns>
        [HttpGet]
        [Route("/api/authentication/signout")]
        public async Task<ResponseModel> Signout()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var userData = SessionManager.GetSession(_httpContextAccessor, "UserData");
                    logger.LogInformation($"Signout {userData.Username}");
                    if (userData == null)
                    {
                        HttpContext.Response.StatusCode = 401;
                        return new ResponseModel
                        {
                            message = "The user has already signed out",
                            status = 401
                        };
                    }

                    var username = SessionManager.GetSession(_httpContextAccessor, "UserData").Username;
                    var user = SqliteManager.Instance.GetUser(username);

                    var now = DateTime.Now;
                    HttpContext.Session.Remove("UserData");
                    SqliteManager.Instance.AddReport(new AuthReportInfo
                    {
                        State = "Signout",
                        ActionDate = $"{now.Year}-{now.Month}-{now.Day} : {now.Hour}:{now.Minute}:{now.Second}",
                        Username = user.Username,
                        UserId = user.Id,
                    });
                    return new ResponseModel
                    {
                        message = "signout out success",
                        status = 200
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"Signout Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
            });
        }

        /// <summary>
        /// Returns a user list
        /// </summary>
        /// <returns>Response Code, Data and message.</returns>
        [HttpGet]
        [Route("/api/authentication/users")]
        public async Task<ResponseModel> GetUsers()
        {
            return await Task.Run(() =>
            {
                try
                {
                    logger.LogInformation("GetUsers");

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    var users = SqliteManager.Instance.GetUsers();
                    if (users != null && users.Count > 0)
                    {
                        var data = JsonConvert.SerializeObject(users);
                        return new ResponseModel
                        {
                            content = data,
                            message = "success",
                            status = 200
                        };
                    }
                    return new ResponseModel
                    {
                        content = "",
                        message = "empty",
                        status = 200
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"GetUsers Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
            });
        }

        /// <summary>
        /// Returns a current user
        /// </summary>
        /// <returns>Response Code, Data and message.</returns>
        [HttpGet]
        [Route("/api/authentication/user")]
        public async Task<ResponseModel> GetCurrentUser()
        {
            return await Task.Run(() =>
            {
                try
                {
                    logger.LogInformation("GetUsers");

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.EmptyInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    var user = SqliteManager.Instance.GetUser(userData.Username);
                    if (user != null)
                    {
                        var data = JsonConvert.SerializeObject(user);
                        return new ResponseModel
                        {
                            content = data,
                            message = "success",
                            status = 200
                        };
                    }
                    return new ResponseModel
                    {
                        content = "",
                        message = "empty",
                        status = 200
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"GetUsers Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
            });
        }

        /// <summary>
        /// Returns a user
        /// </summary>
        /// <returns>Response Code, Data and message.</returns>
        [HttpGet]
        [Route("/api/authentication/get/{id}")]
        public async Task<ResponseModel> GetUser([FromRoute] int id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    logger.LogInformation($"GetUser by id:{id}");
                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    var user = SqliteManager.Instance.GetUser(id);
                    if (user != null)
                    {
                        var data = JsonConvert.SerializeObject(user);
                        return new ResponseModel
                        {
                            content = data,
                            message = "success",
                            status = 200
                        };
                    }
                    HttpContext.Response.StatusCode = 404;
                    return new ResponseModel
                    {
                        content = null,
                        message = "not exists",
                        status = 404
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"GetUser Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
            });
        }

        /// <summary>
        /// Api for delete specifiec user
        /// </summary>
        /// <returns>Response Code and message.</returns>
        [HttpDelete]
        [Route("/api/authentication/delete/{id}")]
        public async Task<ResponseModel> DeleteUser([FromRoute] int id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    logger.LogInformation($"DeleteUser by id:{id}");

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    if (id == 0)
                    {
                        HttpContext.Response.StatusCode = 400;
                        return new ResponseModel
                        {
                            content = "",
                            status = 400,
                            message = "id is empty"
                        };
                    }
                    var user = SqliteManager.Instance.GetUser(userData.Username);
                    if (id == 1)
                    {
                        HttpContext.Response.StatusCode = 403;
                        return new ResponseModel
                        {
                            content = "",
                            status = 403,
                            message = "can not delete super admin"
                        };
                    }
                    if (SqliteManager.Instance.DeleteUser(id))
                    {
                        return new ResponseModel
                        {
                            status = 200,
                            message = "success"
                        };
                    }
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        content = "",
                        status = 500,
                        message = "failed"
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"DeleteUser Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        content = "",
                        status = 500,
                        message = ex.Message
                    };
                }
            });
        }

        #endregion

        #region Basket

        /// <summary>
        /// Api for add one basket
        /// </summary>
        /// <param name="basketInfo">basket infos </param>
        /// <returns>Response Code and message.</returns>
        [HttpPost]
        [Route("/api/basket/add")]
        public async Task<ResponseModel> AddBasket()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    logger.LogInformation("AddBasket");

                    var basketInfo = await Utility.GetBodyObject<BasketInfo>(_httpContextAccessor);

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.EmptyInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    basketInfo.Number = Guid.NewGuid().ToString();
                    if (!basketInfo.IsValid())
                    {
                        HttpContext.Response.StatusCode = 400;
                        return new ResponseModel
                        {
                            message = "bad input",
                            status = 400
                        };
                    }

                    var username = SessionManager.GetSession(_httpContextAccessor, "UserData").Username;
                    var user = SqliteManager.Instance.GetUser(username);
                    basketInfo.UserId = user.Id;
                    basketInfo.CreationDate = DateTime.Now.ToString();

                    var id = SqliteManager.Instance.AddBasket(basketInfo);
                    if (id == -1)
                    {
                        HttpContext.Response.StatusCode = 500;
                        return new ResponseModel
                        {
                            message = "failed",
                            status = 500
                        };
                    }
                    var now = DateTime.Now;
                    SqliteManager.Instance.AddReport(new AuthReportInfo
                    {
                        State = "Basket",
                        ActionDate = $"{now.Year}-{now.Month}-{now.Day} : {now.Hour}:{now.Minute}:{now.Second}",
                        Username = user.Username,
                        UserId = id,
                    });

                    return new ResponseModel
                    {
                        message = "success",
                        status = 200
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"AddBasket Exception Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "error",
                        status = 500
                    };
                }
            });
        }


        /// <summary>
        /// Api for delete one basket
        /// </summary>
        /// <returns>Response Code and message.</returns>
        [HttpDelete]
        [Route("/api/basket/{id}")]
        public async Task<ResponseModel> DeleteBasket([FromRoute] int id)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    logger.LogInformation($"DeleteBasket by id:{id}");
                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.EmptyInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }
                    if (id == 0)
                    {
                        HttpContext.Response.StatusCode = 400;
                        return new ResponseModel
                        {
                            content = "",
                            status = 400,
                            message = "id is empty"
                        };
                    }
                    if (SqliteManager.Instance.DeleteBasket(id))
                    {
                        return new ResponseModel
                        {
                            status = 200,
                            message = "success"
                        };
                    }
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        content = "",
                        status = 500,
                        message = "failed"
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"DeleteBasket Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        content = "",
                        status = 500,
                        message = ex.Message
                    };
                }
            });
        }

        /// <summary>
        /// Returns a basket list
        /// </summary>
        /// <returns>Response Code, Data and message.</returns>
        [HttpGet]
        [Route("/api/basket/get")]
        public async Task<ResponseModel> GetBaskets()
        {
            return await Task.Run(() =>
            {
                try
                {
                    logger.LogInformation("GetBaskets");

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    var baskets = SqliteManager.Instance.GetBaskets();
                    if (baskets != null && baskets.Count > 0)
                    {
                        var data = JsonConvert.SerializeObject(baskets);
                        return new ResponseModel
                        {
                            content = data,
                            message = "success",
                            status = 200
                        };
                    }

                    return new ResponseModel
                    {
                        content = "",
                        message = "empty",
                        status = 200
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"GetBaskets Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
            });
        }

        /// <summary>
        /// Api for edit one basket
        /// </summary>
        /// <param name="basket">basket info</param>
        /// <returns>Response Code and message.</returns>
        [HttpPut]
        [Route("/api/basket/edit/{id}")]
        public async Task<ResponseModel> EditBasket([FromRoute]int id)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    logger.LogInformation("EditBasket");

                    var basketInfo = await Utility.GetBodyObject<BasketInfo>(_httpContextAccessor);

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.EmptyInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    var user = SqliteManager.Instance.GetUser(userData.Username);
                    basketInfo.UserId = user.Id;
                    basketInfo.ModificationDate = DateTime.Now.ToString();

                    var basket = SqliteManager.Instance.GetBasket(id);
                    if (basket == null)
                    {
                        HttpContext.Response.StatusCode = 404;
                        return new ResponseModel
                        {
                            message = "not exists",
                            status = 404
                        };
                    }

                    basketInfo.Id = id;
                    if (SqliteManager.Instance.EditBasket(basketInfo))
                        return new ResponseModel
                        {
                            message = "success",
                            status = 200
                        };
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"EditBasket Exception Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "error",
                        status = 500
                    };
                }
            });
        }

        #endregion

        #region Product


        /// <summary>
        /// Api for add one product
        /// </summary>
        /// <param name="productInfo">product infos </param>
        /// <returns>Response Code and message.</returns>
        [HttpPost]
        [Route("/api/product/add")]
        public async Task<ResponseModel> AddProduct()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    logger.LogInformation("AddProduct");

                    var productInfo = await Utility.GetBodyObject<ProductInfo>(_httpContextAccessor);

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    productInfo.IsDisplay = true;
                    if (!productInfo.IsValid())
                    {
                        HttpContext.Response.StatusCode = 400;
                        return new ResponseModel
                        {
                            message = "bad input",
                            status = 400
                        };
                    }

                    var user = SqliteManager.Instance.GetUser(userData.Username);
                    productInfo.UserId = user.Id;
                    productInfo.CreationDate = DateTime.Now.ToString();
                    var id = SqliteManager.Instance.AddProduct(productInfo);
                    if (id == -1)
                    {
                        HttpContext.Response.StatusCode = 500;
                        return new ResponseModel
                        {
                            message = "failed",
                            status = 500
                        };
                    }
                    var now = DateTime.Now;
                    SqliteManager.Instance.AddReport(new AuthReportInfo
                    {
                        State = "product",
                        ActionDate = $"{now.Year}-{now.Month}-{now.Day} : {now.Hour}:{now.Minute}:{now.Second}",
                        Username = userData.Username,
                        UserId = id,
                    });

                    return new ResponseModel
                    {
                        message = "success",
                        status = 200
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"AddProduct Exception Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "error",
                        status = 500
                    };
                }
            });
        }

        /// <summary>
        /// Api for edit one product
        /// </summary>
        /// <param name="product">product info</param>
        /// <returns>Response Code and message.</returns>
        [HttpPut]
        [Route("/api/product/edit/{id}")]
        public async Task<ResponseModel> EditProduct([FromRoute] int id)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    logger.LogInformation("EditProduct");

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    var productInfo = await Utility.GetBodyObject<ProductInfo>(_httpContextAccessor);
                    var user = SqliteManager.Instance.GetUser(userData.Username);
                    productInfo.UserId = user.Id;
                    productInfo.ModificationDate = DateTime.Now.ToString();
                    productInfo.Id = id;

                    var product = SqliteManager.Instance.GetProduct(id);
                    if(product == null)
                    {
                        HttpContext.Response.StatusCode = 404;
                        return new ResponseModel
                        {
                            message = "not exists",
                            status = 404
                        };
                    }

                    if (SqliteManager.Instance.EditProduct(productInfo))
                        return new ResponseModel
                        {
                            message = "success",
                            status = 200
                        };
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"EditProduct Exception Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "error",
                        status = 500
                    };
                }
            });
        }

        /// <summary>
        /// Returns a Product list
        /// </summary>
        /// <returns>Response Code, Data and message.</returns>
        [HttpGet]
        [Route("/api/product/get")]
        public async Task<ResponseModel> GetProducts()
        {
            return await Task.Run(() =>
            {
                try
                {
                    logger.LogInformation("GetProducts");


                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    var products = SqliteManager.Instance.GetProducts();
                    if (products != null && products.Count > 0)
                    {
                        var data = JsonConvert.SerializeObject(products);
                        return new ResponseModel
                        {
                            content = data,
                            message = "success",
                            status = 200
                        };
                    }
                    return new ResponseModel
                    {
                        content = "",
                        message = "empty",
                        status = 200
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"GetProducts Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
            });
        }


        /// <summary>
        /// Api for delete one Product
        /// </summary>
        /// <returns>Response Code and message.</returns>
        [HttpDelete]
        [Route("/api/product/{id}")]
        public async Task<ResponseModel> DeleteProduct([FromRoute] int id)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    logger.LogInformation($"DeleteProduct by id:{id}");

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    if (id == 0)
                    {
                        HttpContext.Response.StatusCode = 400;
                        return new ResponseModel
                        {
                            content = "",
                            status = 400,
                            message = "id is empty"
                        };
                    }
                    if (SqliteManager.Instance.DeleteProduct(id))
                    {
                        return new ResponseModel
                        {
                            status = 200,
                            message = "success"
                        };
                    }
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        content = "",
                        status = 500,
                        message = "failed"
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"DeleteProduct Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        content = "",
                        status = 500,
                        message = ex.Message
                    };
                }
            });
        }

        #endregion

        #region Content

        /// <summary>
        /// Returns content
        /// </summary>
        /// <returns>Response Code, Data and message.</returns>
        [HttpGet]
        [Route("/api/content/get")]
        public async Task<ResponseModel> GetContent()
        {
            return await Task.Run(() =>
            {
                try
                {
                    logger.LogInformation("GetContent");

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    var content = SqliteManager.Instance.GetContent();
                    if (content != null)
                    {
                        var data = JsonConvert.SerializeObject(content);
                        return new ResponseModel
                        {
                            content = data,
                            message = "success",
                            status = 200
                        };
                    }
                    return new ResponseModel
                    {
                        content = "",
                        message = "empty",
                        status = 200
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"GetContent Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
            });
        }

        /// <summary>
        /// Api for edit one content
        /// </summary>
        /// <param name="content">content info</param>
        /// <returns>Response Code and message.</returns>
        [HttpPut]
        [Route("/api/content/edit")]
        public async Task<ResponseModel> EditContent()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    logger.LogInformation("EditContent");

                    var contentInfo = await Utility.GetBodyObject<ContentInfo>(_httpContextAccessor);
                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    var user = SqliteManager.Instance.GetUser(userData.Username);
                    if (!user.IsAdmin)
                    {
                        HttpContext.Response.StatusCode = 401;
                        return new ResponseModel
                        {
                            message = "Unauthorized",
                            status = 401
                        };
                    }

                    contentInfo.ModificationDate = DateTime.Now.ToString();
                    if (SqliteManager.Instance.EditContent(contentInfo))
                        return new ResponseModel
                        {
                            message = "success",
                            status = 200
                        };
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"EditContent Exception Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "error",
                        status = 500
                    };
                }
            });
        }

        #endregion

        #region Report

        /// <summary>
        /// Returns a report list
        /// </summary>
        /// <returns>Response Code, Data and message.</returns>
        [HttpGet]
        [Route("/api/report/get")]
        public async Task<ResponseModel> GetReports()
        {
            return await Task.Run(() =>
            {
                try
                {
                    logger.LogInformation("GetReports");

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    var reports = SqliteManager.Instance.GetReports();
                    if (reports != null && reports.Count > 0)
                    {
                        var data = JsonConvert.SerializeObject(reports);
                        return new ResponseModel
                        {
                            content = data,
                            message = "success",
                            status = 200
                        };
                    }
                    return new ResponseModel
                    {
                        content = "",
                        message = "empty",
                        status = 200
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"GetReports Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
            });
        }

        #endregion

        #region Settings

        /// <summary>
        /// Api for edit settings
        /// </summary>
        /// <returns>Response Code and message.</returns>
        [HttpPut]
        [Route("/api/setting/edit")]
        public async Task<ResponseModel> EditSetting()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    logger.LogInformation("EditSetting");
                    var settingInfo = await Utility.GetBodyObject<SettingInfo>(_httpContextAccessor);

                    var userData = new UserData();
                    var authorizeResponse = Utility.CheckAuthorize(_httpContextAccessor, AuthorizeTypeInfo.MustBeAdminInstance(), ref userData);
                    if (null != authorizeResponse)
                    {
                        HttpContext.Response.StatusCode = authorizeResponse.status;
                        return authorizeResponse;
                    }

                    if(Utility.SetStringConfig(Configuration, "PasswordPolicy", settingInfo.PasswordPolicy.ToString()))
                    {
                        Constant.PasswordPolicy = (PasswordPolicy)settingInfo.PasswordPolicy;
                        return new ResponseModel
                        {
                            message = "success",
                            status = 200
                        };
                    }
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "failed",
                        status = 500
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError($"EditSetting Exception Message : {ex.Message}");
                    HttpContext.Response.StatusCode = 500;
                    return new ResponseModel
                    {
                        message = "error",
                        status = 500
                    };
                }
            });
        }

        #endregion
    }
}
