using Dias.ExcelSteam.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dias.ExcelSteam.DataValidator.MaterailValidation
{
    public class MaterialDtoValidator : AbstractValidator<MaterialDto>
    {
        public MaterialDtoValidator()
        {
            RuleFor(x => x.System)
                .NotEmpty()
                .WithMessage("System cannot be null or empty.");

            RuleFor(x => x.MaterialName)
               .NotEmpty()
               .WithMessage("Material Name cannot be null or empty.");

            RuleFor(x => x.TagNumber)
                .NotEmpty()
                .WithMessage("Tag Number cannot be null or empty.");

            RuleFor(x => x.LocationRFId)
                .NotEmpty()
                .WithMessage("RFID cannot be null or empty.");

            RuleFor(x => x.PurchaseDate)
                .NotEmpty()
                .WithMessage("Purchase Date cannot be null or empty.");

            RuleFor(x => x.ModelNumber)
                .NotEmpty()
                .WithMessage("Model Number cannot be null or empty.");

            RuleFor(x => x.YearOfInstallation)
                .NotEmpty()
                .WithMessage("Installation Date cannot be null or empty.");

            RuleFor(x => x.MaterialStatus)
               .NotEmpty()
               .WithMessage("Material Status cannot be null or empty.");

            RuleFor(x => x.Category.Name)
               .NotEmpty()
               .WithMessage("CategoryName cannot be null or empty.");
           

            RuleFor(x => x.Devision.Name)
                .NotEmpty()
                .WithMessage("Devision Name cannot be null or empty.");

           

            RuleFor(x => x.LocationOfOperation.Name)
                .NotEmpty()
                .WithMessage("Location of Operation Name cannot be null or empty.");

            

            RuleFor(x => x.Manufacturer.Name)
               .NotEmpty()
               .WithMessage("Manufacturer Name cannot be null or empty.");

            RuleFor(x => x.MaterialType.Name)
                .NotEmpty()
                .WithMessage("Type Name cannot be null or empty.");

            

            RuleFor(x => x.Project.ProjectName)
                .NotEmpty()
                .WithMessage("ProjectName cannot be null or empty.");
            

            RuleFor(x => x.Region.Name)
               .NotEmpty()
               .WithMessage("RegionName cannot be null or empty.");


            RuleFor(x => x.Supplier.Name)
                .NotEmpty()
                .WithMessage("Supplier Name cannot be null or empty.");
            
            RuleFor(x => x.WorkPackage.WorkPackageName)
                .NotEmpty()
                .WithMessage("Supplier Name cannot be null or empty.");
        }
    }
}
