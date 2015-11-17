(function ($) {
    $.validator.addMethod("mustbetrue", function (value, element, params) {
        return $(element).is(":checked");
    });
    $.validator.unobtrusive.adapters.addBool("mustbetrue");
}(jQuery));