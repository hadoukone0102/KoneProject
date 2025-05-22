namespace KoneProject.Authorisations
{
    using KoneProject.Interfaces;
    using System.IdentityModel.Tokens.Jwt;

    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJwtUtils _jwtUtils;


        public JwtMiddleware(RequestDelegate next,
            IJwtUtils jwtUtils)
        {
            _next = next;
            _jwtUtils = jwtUtils;
        }


        //APPELE POUR LES CONTROLLER AVEC ANOTATION AUTHORIZED
        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, token);

            await _next(context);
        }
        //Validaton du token
        private void attachUserToContext(HttpContext context, string token)
        {
            try
            {
                // Create a new instance of JwtSecurityTokenHandler
                var tokenHandler = new JwtSecurityTokenHandler();
                context.Items["roles"] = new List<string>();

                //validate
                if (_jwtUtils.ValidateJwtToken(token))
                {
                    // Read the JWT and parse it into a JwtSecurityToken object
                    var jwtToken = tokenHandler.ReadJwtToken(token);
                    context.Items["userId"] = jwtToken.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                    context.Items["userEmail"] = jwtToken.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                    var roleClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                    if (roleClaim != null)
                    {
                        context.Items["roles"] = roleClaim.Value.Split(',').ToList();
                    }

                }
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}


