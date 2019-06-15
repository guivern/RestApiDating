using RestApiDating.Annotations;

namespace RestApiDating.Dtos
{
    public class UserDto
    {
        [Requerido]
        public string Username {get; set;}
        [Requerido]
        [LongMax(12, MinimumLength = 4)]
        public string Password {get; set;}
    }
}