using Dias.ExcelSteam.Dtos;
using FluentValidation;

namespace Dias.ExcelSteam.DataValidator.MaterailValidation
{
    public class MaterialDataValidation(
        IValidator<MaterialDto> validator,
        IExcelValiationErrorService excelValiation)
        : IDataValidator<MaterialDto>
    {
        public async Task<bool> ValidateAsync(Guid id, MaterialDto data)
        {

            if (!ValidateData(data, out var validationFailures))
            {
                LogValidationErrors(id, validationFailures);
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);

        }

        private bool ValidateData(MaterialDto data, out List<string> validationFailures)
        {
            validationFailures = [];

            if (data == null)
            {
                validationFailures.Add("Deserialized object cannot be null.");
                return false;
            }

            CheckRequiredFields(data, validationFailures);
            CheckFluentValidation(data, validationFailures);

            return !validationFailures.Any();
        }
        private void CheckRequiredFields(MaterialDto data, List<string> validationFailures)
        {
            if (data.MaterialType == null) validationFailures.Add("Material Type cannot be null.");
            if (data.Category == null) validationFailures.Add("Category cannot be null.");
            if (data.Project == null) validationFailures.Add("Project cannot be null.");
            if (data.Devision == null) validationFailures.Add("Division cannot be null.");
            if (data.LocationOfOperation == null) validationFailures.Add("Location of Operation cannot be null.");
            if (data.Region == null) validationFailures.Add("Region cannot be null.");
            if (data.Manufacturer == null) validationFailures.Add("Manufacturer cannot be null.");
            if (data.Supplier == null) validationFailures.Add("Supplier cannot be null.");
        }
        private void CheckFluentValidation(MaterialDto data, List<string> validationFailures)
        {
            var validationResult = validator.Validate(data);
            if (!validationResult.IsValid)
            {
                validationFailures.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));
            }
        }
        private void LogValidationErrors(Guid id, List<string> validationFailures)
        {
            var errorMessage = string.Join(", ", validationFailures);
            excelValiation.LogValidationError(id, $"Excel bulk uploading validation error: {errorMessage}");
        }

    }

}
