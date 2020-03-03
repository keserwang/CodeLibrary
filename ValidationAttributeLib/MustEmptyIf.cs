using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ValidationAttributeLib
{
    /// <summary>
    /// 若參考欄位的值為指定值，則必不可填
    /// </summary>
    public class MustEmptyIf : ValidationAttribute
    {
        public string RefPropertyName1 { get; set; }
        public object TargetValue1 { get; set; }

        public string RefPropertyName2 { get; set; }
        public object TargetValue2 { get; set; }

        public MustEmptyIf(string refPropertyName1, object targetValue1, string refPropertyName2 = "", object targetValue2 = null)
        {
            this.RefPropertyName1 = refPropertyName1;
            this.TargetValue1 = targetValue1;

            this.RefPropertyName2 = refPropertyName2;
            this.TargetValue2 = targetValue2;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            object instance = validationContext.ObjectInstance;
            
            // 若是刪除，則不需檢查
            if (ValidationUtility.IsDeleting(instance))
            {
                return ValidationResult.Success;
            }

            Type instanceType = instance.GetType();

            bool allPass = true;

            PropertyInfo pptInfo1 = instanceType.GetProperty(RefPropertyName1);
            if (pptInfo1 != null)
            {
                object valueObj1 = pptInfo1.GetValue(instance);
                if (valueObj1 != null && valueObj1.ToString() != TargetValue1.ToString())
                    allPass = false;
            }

            PropertyInfo pptInfo2 = instanceType.GetProperty(RefPropertyName2);
            if (pptInfo2 != null)
            {
                object valueObj2 = pptInfo2.GetValue(instance);
                if (valueObj2 != null && valueObj2.ToString() != TargetValue2.ToString())
                    allPass = false;
            }

            // 必不可填，因為符合所有條件
            if (allPass)
            {
                // 沒有填
                if (null == value || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return ValidationResult.Success;
                }
                // 有填
                else
                {
                    return new ValidationResult("此欄位不可填");
                }
            }

            // 不檢核，因為不符合所有條件
            return ValidationResult.Success;
        }
    }
}
