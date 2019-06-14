using System.ComponentModel.DataAnnotations;

namespace RestApiDating.Annotations
{
    public class Requerido: RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return string.Format("{0} es requerido", name);
        }  
    }
}