/**
 * 将后端的错误数据转换为前端的错误数据
 * @param errorData 后端的错误数据
 * @param fieldMapping 字段映射
 * @returns 前端的错误数据
 */
export function mapErrorsToFields(errorData: FieldError[], fieldMapping: Record<string, string>) {
  const errors: Record<string, string> = {};
  errorData.forEach((err: FieldError) => {
    // 将后端的字段名转换为前端的字段名
    const fieldName = fieldMapping[err.propertyName] ||
     err.propertyName.charAt(0).toLowerCase() + err.propertyName.slice(1);
    console.log(fieldName,'fieldName')
    errors[fieldName] = err.errorMessage;
  });
  return errors;
}
