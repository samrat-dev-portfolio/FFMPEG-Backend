using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace FFMPEG_Demo.Filter
{
    public class SAuth : ActionFilterAttribute, IAuthenticationFilter
    {
        private string TokenExceptionError { get; set; }
        private bool allowMultiple = false;
        public String UserRole { get; set; }

        private enum UserDataType { UserName, UserRole };
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // excute 1st 
            string authParameter = string.Empty;
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;
            if (authorization == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Authorization Header", request);
                return;
            }
            if (authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid Authorization Scheme", request);
                return;
            }
            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Token", request);
                return;
            }

            ClaimsPrincipal claimsPrincipal = GetPrincipal(authorization.Parameter);
            context.Principal = claimsPrincipal;

            if (context.Principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult(TokenExceptionError, request);
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(UserRole))
                {
                    string _UserRole = TokenUserData(claimsPrincipal, UserDataType.UserRole);
                    string[] roles = UserRole.Split(',');
                    bool isvalid = false;
                    foreach (var role in roles)
                    {
                        if (role == _UserRole)
                        {
                            isvalid = true;
                            break;
                        }
                    }
                    if (!isvalid)
                    {
                        context.ErrorResult = new AuthenticationFailureResult("User not allowed to access", request, HttpStatusCode.Forbidden);
                    }
                }
            }
            //return Task.FromResult(0);
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            // excute 2nd 
            var result = await context.Result.ExecuteAsync(cancellationToken);
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                //result.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic", "realm=localhost"));
            }
            else
            {
                //HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                //context.Result = new AuthenticationFailureResult("Unauthorized", context.Request); ;
            }
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            ClaimsPrincipal principal = null;
            if (!string.IsNullOrEmpty(token))
            {
                string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                    if (jwtToken != null)
                    {
                        var symmetricKey = Convert.FromBase64String(Secret);
                        var validationParameters = new TokenValidationParameters()
                        {
                            RequireExpirationTime = true,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
                            ClockSkew = TimeSpan.Zero
                        };
                        SecurityToken securityToken;
                        principal = handler.ValidateToken(token, validationParameters, out securityToken);
                    }
                }
                catch (Exception ex)
                {
                    TokenExceptionError = TokenException(ex.ToString());
                }
                return principal;
            }
            return principal;
        }

        private string TokenException(string error)
        {
            string ret = "Invalid token!";
            string pattern = @"(Unable to decode the header)|(Unable to decode the payload)|(Signature validation failed)|(The token is expired)";
            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline;
            MatchCollection match = Regex.Matches(error, pattern, options);
            if (match.Count > 0)
            {
                ret = match[0].Value.ToString().Trim();
            }
            return ret;
        }

        private string TokenUserData(ClaimsPrincipal claimsPrincipal, UserDataType userDataType)
        {
            string ret = null;
            if (claimsPrincipal != null)
            {
                var claims = claimsPrincipal.Claims.ToList();
                var userData = claims.Where(c => c.Type == userDataType.ToString())?.Select(c => c.Value).ToArray();
                ret = userData.Count() > 0 ? userData[0] : null;
            }
            return ret;
        }
    }

    public class AuthenticationFailureResult : IHttpActionResult
    {
        public string ReasonPhrase;
        public HttpRequestMessage Request { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
            HttpStatusCode = HttpStatusCode.Unauthorized;
        }
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request, HttpStatusCode httpStatusCode)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
            HttpStatusCode = httpStatusCode;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode);
            responseMessage.RequestMessage = Request;
            responseMessage.ReasonPhrase = ReasonPhrase;
            return responseMessage;
        }
    }

    /* 
     https://www.youtube.com/watch?v=AUbGk5Ab40A

    */
}