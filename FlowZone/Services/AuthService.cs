using FlowZone.shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlowZone.Services
{
	public class AuthService
	{
		public AuthService(CommonService commonService) 
		{ 
			_commonService = commonService;
		}

		private const string AuthKey = "AuthKey";
        private readonly CommonService _commonService;

        public LoggedInUser User { get; private set; }

		public string? Token { get; private set; }

		public void Signin(AuthResponseDto dto)
		{
			var serialized = JsonSerializer.Serialize(dto);
			Preferences.Default.Set(AuthKey, serialized);
            _commonService.SetToken(dto.Token);
            (User, Token) = dto;
		}


        public void Initialize()
        {
            if (Preferences.ContainsKey(AuthKey))
            {
                var serialized = Preferences.Get(AuthKey, string.Empty);
                if (!string.IsNullOrWhiteSpace(serialized))
                {
                    var user = JsonSerializer.Deserialize<AuthResponseDto>(serialized);
                    _commonService.SetToken(user?.Token);
                    (User, Token) = (user?.User, user?.Token);
                }
            }
        }
        public void Signout()
		{
			Preferences.Default.Remove(AuthKey);
            _commonService.SetToken(null);
            (User, Token) = (null, null);

		}

		public bool IsLoggedIn => Preferences.Default.ContainsKey(AuthKey);
	}
}
