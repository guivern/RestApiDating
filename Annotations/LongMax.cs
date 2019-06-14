using System.ComponentModel.DataAnnotations;

namespace RestApiDating.Annotations
{
    public class LongMax : StringLengthAttribute
    {
        public LongMax(int maximumLength) : base(maximumLength)
        {}

        public override string FormatErrorMessage(string name)
        {
            if(MinimumLength == 0)
            {
                return string.Format("{0} no debe tener más de {1} caracteres", name, MaximumLength);
            }

            return string.Format("{0} debe tener más de {1} caracteres y menos de {2} caracteres", name, MinimumLength, MaximumLength);
            
        }
    }
}