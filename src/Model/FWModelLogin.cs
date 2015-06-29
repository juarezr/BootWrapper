using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BootWrapper.BW.Model
{
    public class BWModelLogin : BWBaseModel, IValidatableObject
    {
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Lembrar Senha")]
        public bool RememberPassword { get; set; }

        public bool StatusLogin { get; set; }
        public string UrlRedirect { get; set; }    
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrEmpty(this.Login))
            {
                results.Add(new ValidationResult("Campo Login é obrigatório!", new string[] { "Login" }));
                return results;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                results.Add(new ValidationResult("Campo senha é obrigatório!", new string[] { "Senha" }));
                return results;
            }

            return results;
        }
    }
}