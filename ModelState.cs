//get the first error message
var modelError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
TempData.SetStatusMessage(modelError == null ? "" : modelError.ErrorMessage, true);