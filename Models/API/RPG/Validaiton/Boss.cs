using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorServerApp.Models.API.RPG.Validaiton
{
    public class Boss : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            var monster = (Monster)validationContext.ObjectInstance;
            return (monster.Rarity == MonsteRarity.Boss) ? ValidationResult.Success
    :           new ValidationResult("The monster is not a Boss");
        }
    }
}
