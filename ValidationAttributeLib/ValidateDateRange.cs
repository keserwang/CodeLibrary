using System;
using System.ComponentModel.DataAnnotations;

namespace ValidationAttributeLib
{
    /// <summary>
    /// 檢核本日期是否大於等於指定欄位的日期。
    /// 通常用於檢核結束日期是否大於等於開始日期。
    /// </summary>
    public class ValidateDateRange : ValidationAttribute
    {
        public string BeginDatePropertyName { get; set; }

        public ValidateDateRange(string beginDatePropertyName)
        {
            this.BeginDatePropertyName = beginDatePropertyName;
        }

        protected override ValidationResult IsValid(object endDateObj, ValidationContext validationContext)
        {
            if (endDateObj == null)
                return ValidationResult.Success;

            object instance = validationContext.ObjectInstance;

            // 若是刪除，則不需檢查
            if (ValidationUtility.IsDeleting(instance))
            {
                return ValidationResult.Success;
            }

            Type type = instance.GetType();
            var property = type.GetProperty(this.BeginDatePropertyName);
            if (property == null) return ValidationResult.Success;
            var beginDateObj = property.GetValue(instance);

            DateTime beginDate, endDate;
            if (beginDateObj == null || !DateTime.TryParse(beginDateObj.ToString(), out beginDate))
            {
                return ValidationResult.Success;
            }
            if (endDateObj == null || !DateTime.TryParse(endDateObj.ToString(), out endDate))
            {
                return ValidationResult.Success;
            }

            // 開始時間小於等於結束時間
            if (beginDate <= endDate)
                return ValidationResult.Success;
            else
            {
                // MetadataTypeAttribute is not supported in System.ComponentModel.Annotations 4.7.0. Wait for future update.
                //string beginDateDisplayName = ValidationUtility.GetDisplayName(instance, BeginDatePropertyName);
                string beginDateDisplayName = "開始時間";

                return new ValidationResult(validationContext.DisplayName + "必須大於或等於" + beginDateDisplayName);
            }
        }
    }
}
