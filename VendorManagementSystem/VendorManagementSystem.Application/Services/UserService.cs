using System.Text;
using VendorManagementSystem.Application.Dtos.ModelDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Application.Utilities;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IErrorLoggingService _errorLog;

        public UserService(IUserRepository userRepository, ITokenService tokenService, IEmailService emailService, IUserTokenRepository userTokenRepository ,IErrorLoggingService errorLog)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _userTokenRepository = userTokenRepository;
            _errorLog = errorLog;
        }
        //delete
        public User CreateSuperAdmin(SuperAdminDTO superAdminDto)
        {
            if (superAdminDto == null) throw new VmsException((int)ErrorCodes.NullArgument, $"The Argument of type '{nameof(superAdminDto)}' is Null");
            User user = new()
            {
                Email = superAdminDto.Email,
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(superAdminDto.Password)),
                UserName = superAdminDto.UserName,
                Role = "superadmin",
                Status = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = superAdminDto.Email,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = superAdminDto.Email,
            };
            try
            {
                int userAddition = _userRepository.AddUser(user);
                if (userAddition > 0) return user;
                else throw new VmsException((int)ErrorCodes.DatabaseError, "Unable to add user to Database", new Exception());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        //
        public ApplicationResponseDTO<User> CreateUser(CreateUserDTO userDto, string jwtToken)
        {
            try
            {   
                User? existingUser = _userRepository.GetUserByEmail(userDto.Email);
                if (existingUser != null)
                {
                    _errorLog.LogError($"in the funciton CreateUser, Provided a duplicate email {existingUser}");
                    
                    return new ApplicationResponseDTO<User>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.DuplicateEntryError,
                            Message = new List<string> { $"Email {userDto.Email} already present" }
                        },
                    };
                }
                string currentUser = _tokenService.ExtractUserDetials(jwtToken, "email");
                User user = new()
                {
                    Email = userDto.Email,
                    UserName = userDto.UserName,
                    Role = userDto.Role,
                    Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("Dummy")),
                    Status = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUser,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = currentUser,
                };
                using (var transaction = _userRepository.BeginTransaction())
                {
                    try
                    {
                        TokenDTO tokenDTO = _tokenService.JwtToken(user, "newUser");
                        int userAddition = _userRepository.AddUser(user);
                        int tokenAddition = _userTokenRepository.AddToken(user.Email, tokenDTO.Token);
                        if (userAddition > 0 && tokenAddition > 0)
                        {
                            EmailDetailsDTO emailDetailsDTO = new()
                            {
                                ToAddress = userDto.Email,
                                ToName = userDto.UserName.Replace("$", " "),
                                Body = EmailUtility.GetInvitationBody(userDto.UserName.Replace("$", " "), currentUser, $"{userDto.RedirectUrl}{tokenDTO.Token}"),
                                Subject = "VMS Invitation",

                            };
                            _emailService.SendLoginEmail(emailDetailsDTO);
                            transaction.Commit();
                            return new ApplicationResponseDTO<User>
                            {
                                Data = user,
                            };
                        }
                        else
                        {
                            transaction.Rollback();
                            _errorLog.LogError($"{(int)ErrorCodes.DatabaseError} In Function CreateUser, User didn't aded to db");
                            return new ApplicationResponseDTO<User>
                            {
                                Error = new()
                                {
                                    Code = (int)ErrorCodes.DatabaseError,
                                    Message = new List<string> { "Unable to add user to Database" },
                                },
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                        return new ApplicationResponseDTO<User>
                        {
                            Error = new()
                            {
                                Code = (int)ErrorCodes.InternalError,
                                Message = new List<string> { ex.Message },
                            },
                        };
                    }
                }
            }
            catch(Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<User>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
        }

        public ApplicationResponseDTO<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                var users = _userRepository.GetUsers();
                return new ApplicationResponseDTO<IEnumerable<User>>
                {
                    Data = users,
                };
            }catch(Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<IEnumerable<User>>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
        }

        public ApplicationResponseDTO<TokenDTO> Login(LoginDTO loginDto)
        {
            try
            {
                User? user = _userRepository.GetUserByEmail(loginDto.Email);
                if (user == null)
                {
                    _errorLog.LogError($"in the funciton CreateUser, provided invalid email '{loginDto.Email}'.");
                    return new ApplicationResponseDTO<TokenDTO>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.NullArgument,
                            Message = new List<string> { $"provided invalid email '{loginDto.Email}'"}
                        },
                    };
                }
                
                var encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(loginDto.Password));

                if (loginDto.Email == user.Email && encodedPassword == user.Password)
                {
                    TokenDTO tokenDto = _tokenService.JwtToken(user, "login");
                    return new ApplicationResponseDTO<TokenDTO>
                    {
                        Data = tokenDto,
                    };
                }

                else
                {
                    _errorLog.LogError($"{(int)ErrorCodes.InvalidCredintials}. In function Login, Provided Invalid login credentials");
                    return new ApplicationResponseDTO<TokenDTO>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.InvalidCredintials,
                            Message = new List<string> { "The Given Credintials are Invalid" },
                        }
                    };
                }
            }
            catch(Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<TokenDTO>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                }; 
            }   
        }

        public ApplicationResponseDTO<bool> SetUserPassword(UpdatePasswordDTO updatePasswordDto, string _token)
        {
            var email = updatePasswordDto.Email;
            var newPassword = updatePasswordDto.Password;

            try
            {
                var isTokenValid = _tokenService.ValidateToken(_token);
                var isTokenInDb = _userTokenRepository.GetToken(email);

                if (!isTokenValid || isTokenInDb != _token)
                {
                    Console.WriteLine($"{isTokenInDb} {_token}");
                   return new ApplicationResponseDTO<bool>
                    {
                        Error = new Error
                        {
                            Code = (int)ErrorCodes.InvalidToken,
                            Message = new List<string>()
                            {
                                "The Token is Invalid or Expired",
                            }
                        },
                        Data = false,
                    };
                }

                string tokenEmail = _tokenService.ExtractUserDetials(_token, "email");
                if (tokenEmail == null || tokenEmail != email)
                {
                    return new ApplicationResponseDTO<bool>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.AuthenthicationError,
                            Message = new List<string> { "Token is invalid" }
                        },
                        Data = false,
                    };
                }
                User? user = _userRepository.GetUserByEmail(tokenEmail!);
                if (user == null)
                {
                    _errorLog.LogError($"{(int)ErrorCodes.NotFound} In function SetUserPassword, Email '{email}' not found in the database.");
                    new ApplicationResponseDTO<bool>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.NotFound,
                            Message = new List<string> { $"Email '{email}' not found in the database." }
                        },
                        Data = false,
                    };
                }

                string hashedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(newPassword));
                if(user!.Password == hashedPassword)
                {
                    return new ApplicationResponseDTO<bool>
                    {
                        Error = new Error
                        {
                            Code = (int)ErrorCodes.DuplicateEntryError,
                            Message = new List<string>()
                            {
                                "Provide a new password",
                            }
                        },
                        Data = false,
                    };
                }
               var transaction = _userRepository.BeginTransaction();
                bool res = _userRepository.UpdatePassword(email, hashedPassword);
                _userTokenRepository.DeleteToken(email);
                transaction.Commit();

                return new ApplicationResponseDTO<bool>
                {
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<bool>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
        }

        public ApplicationResponseDTO<bool> SendForgetPasswordEmail(string email, string redirectUrl)
        {
            try
            {
                User? user = _userRepository.GetUserByEmail(email);
                if (user == null)
                {
                    _errorLog.LogError($"{(int)ErrorCodes.NotFound} In function SetUserPassword, Email '{email}' not found in the database.");
                    return new ApplicationResponseDTO<bool>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.NotFound,
                            Message = new List<string> { $"Email '{email}' not found in the database." }
                        },
                        Data = false,
                    };
                }

                using (var transaction = _userRepository.BeginTransaction())
                {
                    try
                    {
                        TokenDTO tokenDTO = _tokenService.JwtToken(user, "resetpassword");
                        _userTokenRepository.AddToken(user.Email, tokenDTO.Token);
                        EmailDetailsDTO emailDetailsDTO = new()
                        {
                            ToAddress = email,
                            ToName = user.UserName.Replace("$", " "),
                            Body = EmailUtility.ForgetPasswordBody(user.UserName.Replace("$", " "), $"{redirectUrl}{tokenDTO.Token}"),
                            Subject = "Reset Password",

                        };

                        _emailService.SendLoginEmail(emailDetailsDTO);
                        transaction.Commit();
                        return new ApplicationResponseDTO<bool>
                        {
                            Data = true,
                        };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                        return new ApplicationResponseDTO<bool>
                        {
                            Error = new()
                            {
                                Code = (int)ErrorCodes.InternalError,
                                Message = new List<string> { ex.Message },
                            },
                        };
                    }
                }
            }
            catch(Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<bool>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
            
        }

        public ApplicationResponseDTO<string> ValidateToken(string token)
        {
            try
            {
                if (_tokenService.ValidateToken(token))
                {
                    string email = _tokenService.ExtractUserDetials(token, "email");
                    var dbToken = _userTokenRepository.GetToken(email);
                   if (dbToken == token)
                    {
                        Console.WriteLine($"{dbToken}");
                        Console.WriteLine($"{token}");
                    }

                    if (dbToken != token)
                    {
                        return new ApplicationResponseDTO<string>
                        {
                            Error = new Error
                            {
                                Code = (int)ErrorCodes.InvalidToken,
                                Message = new List<string>()
                                {
                                    "The token is invalid or Expired",
                                }
                            },
                            Data = string.Empty,
                        };
                    }
                    return new ApplicationResponseDTO<string>
                    {
                        Data = email,
                    };
                }
                else
                {
                    return new ApplicationResponseDTO<string>
                    {
                        Error = new Error
                        {
                            Code = (int)ErrorCodes.InvalidToken,
                            Message = new List<string>()
                                {
                                    "The token is invalid or Expired",
                                }
                        },
                        Data = string.Empty,
                    };
                }
            }
            catch(Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<string>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    },
                };
            }
        }
    }
}
