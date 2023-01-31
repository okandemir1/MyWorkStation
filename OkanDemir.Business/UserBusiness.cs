using AutoMapper;
using FluentValidation;
using OkanDemir.Business.Encyription;
using OkanDemir.Business.Mapper;
using OkanDemir.Data.Repository;
using OkanDemir.Dto;
using OkanDemir.Dto.Validation;
using OkanDemir.Model;

namespace OkanDemir.Business
{
    public class UserBusiness
    {
        private readonly IRepository<User> _userRepository;

        public UserBusiness(IRepository<User> _userRepository)
        {
            this._userRepository = _userRepository;
        }

        public DbOperationResult<UserDto> Login(LoginRequestDto loginModel)
        {
            var validCheck = new LoginValidation().Validate(loginModel);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult<UserDto>(false, "Eksik veya hatalı veri girişi", null, errors);
            }

            Cipher cipher = new Cipher(loginModel.Key);
            try
            {
                var password = cipher.Encrypt(loginModel.Password);

                var user = _userRepository.ListQueryableNoTracking
                    .FirstOrDefault(x => x.Username == loginModel.Username && x.Password == password && x.IsActive && !x.IsDeleted);

                if(user == null)
                    return new DbOperationResult<UserDto>(false, "Kullanıcı hesabı bulunamadı. Kullanıcı adını, Şifreni veya Key'i yanlış giriyor olabilirsin", null);

                if (user.IsBanned)
                    return new DbOperationResult<UserDto>(false, "Hesabınız engellenmiş ne yapmış olabilirsin acaba :) destek bölümünden iletişime geçersen iyi olur", null);


                var userDto = ObjectMapper.Mapper.Map<UserDto>(user);
                userDto.Fullname = cipher.Decrypt(userDto.Fullname);

                return new DbOperationResult<UserDto>(true, "", userDto);
            }
            catch (Exception ex)
            {
                return new DbOperationResult<UserDto>(false, ex.Message, null);
            }
        }

        public DbOperationResult<UserDto> Register(RegisterRequestDto reqModel)
        {
            var validCheck = new RegisterValidation().Validate(reqModel);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult<UserDto>(false, "Eksik veya hatalı veri girişi", null, errors);
            }

            Cipher cipher = new Cipher(reqModel.Key);
            try
            {
                IDictionary<int, string> existUser = HasExistUser(reqModel.Username, reqModel.Mail);

                if (existUser.Count > 0)
                {
                    var existErrors = new List<string>();
                    existUser.ToList().ForEach(x => existErrors.Add(x.Value));
                    return new DbOperationResult<UserDto>(false, "Kayıtlı veri mevcut", null, existErrors);
                }

                reqModel.Phone = "9" + reqModel.Phone.Replace("(", "")
                    .Replace(" ", "")
                    .Replace(")", "")
                    .Trim();

                reqModel.Fullname = cipher.Encrypt(reqModel.Fullname);
                reqModel.Password = cipher.Encrypt(reqModel.Password);
                reqModel.Phone = cipher.Encrypt(reqModel.Phone);
                reqModel.IdNo = cipher.Encrypt(reqModel.IdNo);
                reqModel.Address = cipher.Encrypt(reqModel.Address);
                reqModel.IsActive = true;
                reqModel.IsDeleted = false;
                reqModel.IsBanned = false;
                reqModel.Token = "";
                reqModel.IpAddress = reqModel.IpAddress ?? "";
                reqModel.RoleId = 1;

                var userModel = ObjectMapper.Mapper.Map<User>(reqModel);

                var insert = _userRepository.Insert(userModel);
                if(insert == null)
                    return new DbOperationResult<UserDto>(false, "Üyelik işlemi gerçekleştirilemedi", null);

                var userDto = ObjectMapper.Mapper.Map<UserDto>(insert);
                userDto.Fullname = cipher.Decrypt(reqModel.Fullname);

                return new DbOperationResult<UserDto>(true, "", userDto);
            }
            catch (Exception ex)
            {
                return new DbOperationResult<UserDto>(false, ex.Message, null);
            }
        }

        public IDictionary<int, string> HasExistUser(string username, string mail)
        {
            IDictionary<int, string> response = new Dictionary<int, string>();

            var usernameExist = _userRepository.ListQueryableNoTracking.FirstOrDefault(x => x.Username == username && x.IsActive && !x.IsDeleted);
            if (usernameExist != null)
                response.Add(1, "Bu kullanıcı adı daha önce kullanılmış");

            var mailExist = _userRepository.ListQueryableNoTracking.FirstOrDefault(x => x.Mail == mail && x.IsActive && !x.IsDeleted);
            if (mailExist != null)
                response.Add(2, "Bu e-posta adresi daha önce kullanılmış");

            return response;
        }
    }
}
